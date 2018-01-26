using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageMagick;
using NReco.VideoInfo;
using NUnit.Framework;

namespace FilmBarcodes.Common.Tests
{
    [TestFixture]
    public class ImageProcessorTests
    {
        [Test]
        public void Mosaic()
        {
            var dir = "C:\\Users\\Rob\\Pictures\\mx5";
            string[] ilist = Directory.GetFiles(dir);

            using (MagickImageCollection images = new MagickImageCollection())
            {
                for (var i = 0; i < ilist.Length; i++)
                {
                    var frame = ilist[i];
                    using (MagickImage image = new MagickImage(frame))
                    {
                        // Resize the image to a fixed size without maintaining the aspect ratio (normally an image will be resized to fit inside the specified size)
                        image.Resize(new MagickGeometry(100, 600) { IgnoreAspectRatio = true });
                        image.Page = new MagickGeometry(i * 100, 0, 0, 0);

                        image.Write(Path.Combine(dir, $"resized_{frame.Replace($"{dir}\\", "")}"));

                        images.Add(image.Clone());
                    }
                }

                using (IMagickImage result = images.Mosaic())
                {
                    result.Write(Path.Combine(dir, "mosaic.jpg"));
                }
            }
        }

        [Test]
        public void MosaicExample()
        {
            using (MagickImageCollection collection = new MagickImageCollection())
            {
                collection.Add(new MagickImage(new MagickColor("purple"), 1024, 1024));
                MagickImage frame = new MagickImage("logo:") {Page = new MagickGeometry(300, 100, 0, 0)};
                collection.Add(frame);

                using (IMagickImage moscaic = collection.Mosaic())
                {
                    moscaic.Write(@"C:\Users\Rob\Pictures\mx5\Mosaic.png");
                }
            }
        }

        [Test]
        public void CaptureFrames()
        {
            var ffProbe = new FFProbe();

            var info = ffProbe.GetMediaInfo(@"M:\game videos\Super Mario Bros (1985) (Full Game Run).mp4");


            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

            var thumbSettings = new NReco.VideoConverter.ConvertSettings()
            {
                VideoFrameRate = 1,
                VideoFrameCount = 1, // extract exactly 1 frame
                Seek = 30, // frame position in seconds
                CustomOutputArgs = "" // any ffmpeg arguments that goes before output param
            };

            using (MemoryStream ms = new MemoryStream())
            {
                ffMpeg.ConvertMedia(@"M:\game videos\Super Mario Bros (1985) (Full Game Run).mp4", null, ms, "mjpeg", thumbSettings);

                Image.FromStream(ms).Save(Path.Combine(@"M:\game videos", "test.jpg"), ImageFormat.Jpeg);
            }
        }
    }
}