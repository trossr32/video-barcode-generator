using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models;
using FilmBarcodes.Common.Models.BarcodeManager;
using ImageMagick;

namespace FilmBarcodes.Common
{
    public class ImageProcessor
    {
        public static string GetAverageHtmlColourFromImageStreamUsingScale(int frameTime, string file, string frameImage)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new NReco.VideoConverter.FFMpegConverter().GetVideoThumbnail(file, ms, frameTime);
                
                Image.FromStream(ms).Save(frameImage, ImageFormat.Jpeg);

                ms.Seek(0, SeekOrigin.Begin);

                using (MagickImage image = new MagickImage(ms, new MagickReadSettings{Format = MagickFormat.Jpeg}))
                {
                    image.Crop(new MagickGeometry(new Percentage(70), new Percentage(70)), Gravity.Center);
                    image.Scale(1, 1);

                    MagickColor color = image.GetPixels().First().ToColor();

                    return ColorTranslator.ToHtml(Color.FromArgb(color.ToColor().ToArgb()));
                }
            }
        }

        public static void RenderImage(VideoFile file)
        {
            var imageFile = $"{file.FilenameWithoutExtension}.jpg";
            
            var bmp = new Bitmap(file.OutputWidth, file.OutputHeight);

            using (Graphics graph = Graphics.FromImage(bmp))
            {
                double frame = 0;
                double frameJump = file.Colours.Count / (double)file.OutputWidth;

                for (var i = 0; i < file.OutputWidth; i++)
                {
                    frame = frame + frameJump;

                    Rectangle imageSize = new Rectangle(i, 0, i, file.OutputHeight);

                    graph.FillRectangle(new SolidBrush(ColorTranslator.FromHtml(file.Colours.First(c => c.Frame == Convert.ToInt32(Math.Round(frame))).Hex)), imageSize);
                }
            }

            bmp.Save(Path.Combine(file.FullOutputDirectory, imageFile), ImageFormat.Jpeg);
        }

        public static void RenderImageAsync(VideoFile file, IProgress<ProgressWrapper> progress)
        {
            var imageFile = $"{file.FilenameWithoutExtension}.jpg";

            var bmp = new Bitmap(file.OutputWidth, file.OutputHeight);

            using (Graphics graph = Graphics.FromImage(bmp))
            {
                double frame = 0;
                double frameJump = file.Colours.Count / (double)file.OutputWidth;

                for (var i = 0; i < file.OutputWidth; i++)
                {
                    frame = frame + frameJump;

                    Rectangle imageSize = new Rectangle(i, 0, i, file.OutputHeight);

                    graph.FillRectangle(new SolidBrush(ColorTranslator.FromHtml(file.Colours.First(c => c.Frame == Convert.ToInt32(Math.Round(frame))).Hex)), imageSize);
                }
            }

            bmp.Save(Path.Combine(file.FullOutputDirectory, imageFile), ImageFormat.Jpeg);

            progress.Report(new ProgressWrapper(file.Duration, file.Duration, ProcessType.RenderImage));
        }
    }
}