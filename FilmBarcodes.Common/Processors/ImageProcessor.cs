using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Helpers;
using FilmBarcodes.Common.Models;
using FilmBarcodes.Common.Models.BarcodeManager;
using FilmBarcodes.Common.Models.Settings;
using ImageMagick;
using NLog;

namespace FilmBarcodes.Common.Processors
{
    public static class ImageProcessor
    {
        private static Logger _logger;

        private static string SetTempDir(SettingsWrapper settings)
        {
            var dir = Path.Combine(settings.BarcodeManager.MagickImageTempDir, Guid.NewGuid().ToString());

            Directory.CreateDirectory(dir);

            MagickNET.SetTempDirectory(dir);

            return dir;
        }
        
        public static string GetAverageHtmlColourFromImageStreamUsingScale(int frameTime, string file, VideoCollection videoCollection, SettingsWrapper settings)
        {
            MagickNET.SetTempDirectory(settings.BarcodeManager.MagickImageTempDir); 

            using (MemoryStream ms = new MemoryStream())
            {
                new NReco.VideoConverter.FFMpegConverter().GetVideoThumbnail(videoCollection.Config.FullPath, ms, frameTime);
                
                if (ms.Length == 0)
                    return null;

                Image.FromStream(ms).Save(Path.Combine(videoCollection.Config.ImageDirectory, file), ImageFormat.Jpeg);

                ms.Seek(0, SeekOrigin.Begin);

                using (MagickImage image = new MagickImage(ms, new MagickReadSettings { Format = MagickFormat.Jpeg }))
                {
                    image.Crop(new MagickGeometry(new Percentage(70), new Percentage(70)), Gravity.Center);
                    image.Scale(1, 1);

                    MagickColor color = image.GetPixels().First().ToColor();

                    return ColorTranslator.ToHtml(Color.FromArgb(color.ToColor().ToArgb()));
                }
            }
        }

        public static string GetAverageHtmlColourFromImageUsingScale(int frame, VideoCollection videoCollection, SettingsWrapper settings)
        {
            MagickNET.SetTempDirectory(settings.BarcodeManager.MagickImageTempDir);

            using (MagickImage image = new MagickImage(Path.Combine(videoCollection.Config.ImageDirectory, $"frame.{frame}.jpg")))
            {
                image.Crop(new MagickGeometry(new Percentage(70), new Percentage(70)), Gravity.Center);
                image.Scale(1, 1);

                MagickColor color = image.GetPixels().First().ToColor();

                return ColorTranslator.ToHtml(Color.FromArgb(color.ToColor().ToArgb()));
            }
        }

        public static void RenderImageAsync(VideoCollection videoCollection, BarcodeConfig file, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        {
            if (File.Exists(file.Barcode_Standard.FullOutputFile))
            {
                _logger = LogManager.GetCurrentClassLogger();
                _logger.Warn($"Image {file.Barcode_Standard.FullOutputFile} already exists, skipping image creation");

                return;
            }

            var bmp = new Bitmap(file.OutputWidth, file.OutputHeight);

            using (Graphics graph = Graphics.FromImage(bmp))
            {
                double frame = 0;
                double frameJump = videoCollection.Data.Colours.Count / (double)file.OutputWidth;

                for (var i = 0; i < file.OutputWidth; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    frame = frame + frameJump;

                    Rectangle imageSize = new Rectangle(i, 0, i, file.OutputHeight);
                    
                    graph.FillRectangle(new SolidBrush(ColorTranslator.FromHtml(videoCollection.Data.Colours.First(c => c.Frame == Convert.ToInt32(frame)).Hex)), imageSize);

                    progress.Report(new ProgressWrapper(file.OutputWidth, i + 1, ProcessType.RenderImage));
                }
            }

            bmp.Save(file.Barcode_Standard.FullOutputFile, ImageFormat.Jpeg);
        }

        public static void BuildAndRenderImageCompressedToOnePixelWideImageAsync(VideoCollection videoCollection, BarcodeConfig file, SettingsWrapper settings, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        {
            MagickNET.SetTempDirectory(settings.BarcodeManager.MagickImageTempDir);

            const int partDivider = 1000;

            Directory.CreateDirectory(videoCollection.Config.OnePixelImageDirectory);

            if (File.Exists(file.Barcode_1px.FullOutputFile))
            {
                _logger = LogManager.GetCurrentClassLogger();
                _logger.Warn($"Image {file.Barcode_1px.FullOutputFile} already exists, skipping image creation");

                return;
            }

            // get any image files now to save querying the file system for every image
            string[] imageFiles = Directory.GetFiles(videoCollection.Config.OnePixelImageDirectory);

            List<string> imageParts = new List<string>();
            
            using (MagickImageCollection images = new MagickImageCollection())
            {
                int geo = 0;

                for (var i = 0; i < videoCollection.Data.Images.Count; i++)
                {
                    var frame = videoCollection.Data.Images[i];

                    cancellationToken.ThrowIfCancellationRequested();

                    if (imageFiles.All(f => f != frame.Name))
                        using (MagickImage image = new MagickImage(Path.Combine(videoCollection.Config.ImageDirectory, frame.Name)))
                        {
                            // Resize the image to a fixed size without maintaining the aspect ratio (normally an image will be resized to fit inside the specified size)
                            image.Resize(new MagickGeometry(1, file.OutputHeight) {IgnoreAspectRatio = true});
                            image.Page = new MagickGeometry(geo, 0, 0, 0);

                            image.Write(Path.Combine(videoCollection.Config.OnePixelImageDirectory, frame.Name));

                            images.Add(image.Clone());
                        }
                    else
                        using (MagickImage image = new MagickImage(Path.Combine(videoCollection.Config.OnePixelImageDirectory, frame.Name)))
                        {
                            images.Add(image.Clone());
                        }

                    geo++;

                    // split out and write partitioned images every 1000 frames to prevent ImageMagick's 'too many open files' fuckery
                    if (i % partDivider == 0 && i != 0)
                    {
                        var part = Path.Combine(settings.BarcodeManager.MagickImageTempDir, $"{Guid.NewGuid()}.jpg");

                        using (IMagickImage result = images.Mosaic())
                        {
                            result.Write(part);
                        }

                        images.Clear();

                        imageParts.Add(part);

                        geo = 0;
                    }

                    progress.Report(new ProgressWrapper(videoCollection.Data.Images.Count, frame.Frame, ProcessType.RenderImageCompressedToOnePixelWide));
                }

                // create the last part
                var lastPart = Path.Combine(settings.BarcodeManager.MagickImageTempDir, $"{Guid.NewGuid()}.jpg");

                using (IMagickImage result = images.Mosaic())
                {
                    result.Write(lastPart);
                }

                images.Clear();

                imageParts.Add(lastPart);

                // add all the parts to the image collection
                geo = 0;

                foreach (var imagePart in imageParts)
                {
                    using (MagickImage image = new MagickImage(imagePart))
                    {
                        image.Page = new MagickGeometry(geo, 0, 0, 0);

                        images.Add(image.Clone());
                    }

                    geo = geo + partDivider;

                    File.Delete(imagePart);
                }

                // lets do some fucking barcode magic
                using (IMagickImage result = images.Mosaic())
                {
                    result.Write(file.Barcode_1px.FullOutputFile);
                }
            }

            //Cache.ClearMagickImageCache(tempDir);
        }
    }
}