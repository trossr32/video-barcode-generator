using System.IO;
using FilmBarcodes.Common.Models.BarcodeManager;
using NReco.VideoInfo;

namespace FilmBarcodes.Common
{
    public class VideoProcessor
    {
        public static MediaInfo GetVideoInfo(string file)
        {
            return new FFProbe().GetMediaInfo(file);
        }

        public static VideoFile BuildColourList(VideoFile file)
        {
            Directory.CreateDirectory(file.ImageDirectory);

            for (int i = 1; i < file.Duration; i++)
            {
                var image = $"{file.FilenameWithoutExtension}.{i}.jpg";

                file.Images.Add(new FrameImage
                {
                    Frame = i,
                    Name = image
                });

                file.Colours.Add(new Colours
                {
                    Frame = i,
                    Hex = ImageProcessor.GetAverageHtmlColourFromImageStreamUsingScale(i, file.FullPath, Path.Combine(file.ImageDirectory, image))
                });
            }

            return file;
        }
    }
}