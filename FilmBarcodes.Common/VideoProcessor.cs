using System;
using System.IO;
using System.Threading.Tasks;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models;
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

        public static VideoFile BuildColourListAsync(VideoFile file, IProgress<ProgressWrapper> progress)
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

                progress.Report(new ProgressWrapper(file.Duration, i, ProcessType.BuildColourList));
            }

            return file;
        }
    }
}