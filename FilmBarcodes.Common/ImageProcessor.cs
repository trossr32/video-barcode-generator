using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models;
using FilmBarcodes.Common.Models.BarcodeManager;
using ImageMagick;
using NLog;

namespace FilmBarcodes.Common
{
    public static class ImageProcessor
    {
        private static Logger _logger;

        public static string GetAverageHtmlColourFromImageStreamUsingScale(int frameTime, string file, VideoCollection videoCollection)
        {
            MagickNET.SetTempDirectory(@"D:\MagickTemp");

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

        public static string GetAverageHtmlColourFromImageUsingScale(int frame, VideoCollection videoCollection)
        {
            MagickNET.SetTempDirectory(@"D:\MagickTemp");

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
                    
                    graph.FillRectangle(new SolidBrush(ColorTranslator.FromHtml(videoCollection.Data.Colours[i].Hex)), imageSize);

                    progress.Report(new ProgressWrapper(file.OutputWidth, i + 1, ProcessType.RenderImage));
                }
            }

            bmp.Save(file.Barcode_Standard.FullOutputFile, ImageFormat.Jpeg);
        }

        public static void BuildAndRenderImageCompressedToOnePixelWideImageAsync(VideoCollection videoCollection, BarcodeConfig file, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        {
            MagickNET.SetTempDirectory(@"D:\MagickTemp");

            Directory.CreateDirectory(videoCollection.Config.OnePixelImageDirectory);

            if (File.Exists(file.Barcode_1px.FullOutputFile))
            {
                _logger = LogManager.GetCurrentClassLogger();
                _logger.Warn($"Image {file.Barcode_1px.FullOutputFile} already exists, skipping image creation");

                return;
            }

            // get any image files now to save querying the file system for every image
            string[] imageFiles = Directory.GetFiles(videoCollection.Config.OnePixelImageDirectory);

            using (MagickImageCollection images = new MagickImageCollection())
            {
                for (var i = 0; i < videoCollection.Data.Images.Count; i++)
                {
                    var frame = videoCollection.Data.Images[i];

                    cancellationToken.ThrowIfCancellationRequested();

                    if (imageFiles.All(f => f != frame.Name))
                        using (MagickImage image = new MagickImage(Path.Combine(videoCollection.Config.ImageDirectory, frame.Name)))
                        {
                            // Resize the image to a fixed size without maintaining the aspect ratio (normally an image will be resized to fit inside the specified size)
                            image.Resize(new MagickGeometry(1, file.OutputHeight) {IgnoreAspectRatio = true});
                            image.Page = new MagickGeometry(i * 1, 0, 0, 0);

                            image.Write(Path.Combine(videoCollection.Config.OnePixelImageDirectory, frame.Name));
                        }

                    images.Add(new MagickImage(Path.Combine(videoCollection.Config.OnePixelImageDirectory, frame.Name)));

                    progress.Report(new ProgressWrapper(videoCollection.Data.Images.Count, frame.Frame, ProcessType.RenderImageCompressedToOnePixelWide));
                }

                using (IMagickImage result = images.Mosaic())
                {
                    result.Write(file.Barcode_1px.FullOutputFile);
                }
            }
        }
    }
}