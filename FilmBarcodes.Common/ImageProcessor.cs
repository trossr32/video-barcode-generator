using System;
using System.Collections.Generic;
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
            using (MagickImage image = new MagickImage(Path.Combine(videoCollection.Config.ImageDirectory, $"frame.{frame}.jpg")))
            {
                image.Crop(new MagickGeometry(new Percentage(70), new Percentage(70)), Gravity.Center);
                image.Scale(1, 1);

                MagickColor color = image.GetPixels().First().ToColor();

                return ColorTranslator.ToHtml(Color.FromArgb(color.ToColor().ToArgb()));
            }
        }

        public static void RenderImageAsync(VideoCollection videoCollection, VideoFile file, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        {
            if (File.Exists(file.StandardImage.FullOutputFile))
            {
                _logger = LogManager.GetCurrentClassLogger();
                _logger.Warn($"Image {file.StandardImage.FullOutputFile} already exists, skipping image creation");

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

                    var copyFrame = Convert.ToInt32(Math.Round(frame));

                    file.Data.Colours.Add(videoCollection.Data.Colours.First(c => c.Frame == copyFrame));
                    file.Data.Images.Add(videoCollection.Data.Images.First(c => c.Frame == copyFrame));

                    graph.FillRectangle(new SolidBrush(ColorTranslator.FromHtml(file.Data.Colours.Last().Hex)), imageSize);

                    progress.Report(new ProgressWrapper(file.OutputWidth, i + 1, ProcessType.RenderImage));
                }
            }

            bmp.Save(file.StandardImage.FullOutputFile, ImageFormat.Jpeg);
        }

        //public static void GetImageCompressedToOnePixelWide(int frame, VideoCollection videoCollection)
        //{
        //    var file = $"frame.{frame}.jpg";

        //    ProcessImageCompressedToOnePixelWide(file, videoCollection);
        //}

        //public static void BuildImageCompressedToOnePixelWide(VideoCollection videoCollection, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        //{
        //    foreach (var frame in videoCollection.Data.Images)
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();

        //        ProcessImageCompressedToOnePixelWide(frame.Name, videoCollection);

        //        progress.Report(new ProgressWrapper(videoCollection.Data.Images.Count, frame.Frame, ProcessType.BuildOnePixelImage));
        //    }
        //}

        //private static void ProcessImageCompressedToOnePixelWide(string file, VideoCollection videoCollection)
        //{
        //    using (MagickImage image = new MagickImage(Path.Combine(videoCollection.Config.ImageDirectory, file)))
        //    {
        //        // Resize the image to a fixed size without maintaining the aspect ratio (normally an image will be resized to fit inside the specified size)
        //        image.Resize(new MagickGeometry(1, image.Height) { IgnoreAspectRatio = true });

        //        image.Write(Path.Combine(videoCollection.Config.OnePixelImageDirectory, file));
        //    }
        //}

        public static void BuildAndRenderImageCompressedToOnePixelWideImageAsync(VideoCollection videoCollection, VideoFile file, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(videoCollection.Config.OnePixelImageDirectory);

            if (File.Exists(file.CompressedOnePixelImage.FullOutputFile))
            {
                _logger = LogManager.GetCurrentClassLogger();
                _logger.Warn($"Image {file.CompressedOnePixelImage.FullOutputFile} already exists, skipping image creation");

                return;
            }
            
            using (MagickImageCollection images = new MagickImageCollection())
            {
                for (var i = 0; i < videoCollection.Data.Images.Count; i++)
                {
                    var frame = videoCollection.Data.Images[i];

                    cancellationToken.ThrowIfCancellationRequested();

                    using (MagickImage image = new MagickImage(Path.Combine(videoCollection.Config.ImageDirectory, frame.Name)))
                    {
                        // Resize the image to a fixed size without maintaining the aspect ratio (normally an image will be resized to fit inside the specified size)
                        image.Resize(new MagickGeometry(1, file.OutputHeight) { IgnoreAspectRatio = true });
                        image.Page = new MagickGeometry(i * 1, 0, 0, 0);

                        image.Write(Path.Combine(videoCollection.Config.OnePixelImageDirectory, frame.Name));

                        images.Add(image.Clone());
                    }

                    progress.Report(new ProgressWrapper(videoCollection.Data.Images.Count, frame.Frame, ProcessType.RenderImageCompressedToOnePixelWide));
                }

                using (IMagickImage result = images.Mosaic())
                {
                    result.Write(file.CompressedOnePixelImage.FullOutputFile);
                }
            }
        }
    }
}