using System;
using System.IO;
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

        public static VideoCollection BuildColourList(VideoCollection videoCollection)
        {
            Directory.CreateDirectory(videoCollection.Config.ImageDirectory);

            for (int i = 1; i < videoCollection.Config.Duration; i++)
            {
                var image = $"frame.{i}.jpg";

                videoCollection.Data.Images.Add(new VideoImage
                {
                    Frame = i,
                    Name = image
                });

                videoCollection.Data.Colours.Add(new VideoColour
                {
                    Frame = i,
                    Hex = ImageProcessor.GetAverageHtmlColourFromImageStreamUsingScale(i, videoCollection.Config.FullPath, Path.Combine(videoCollection.Config.ImageDirectory, image))
                });
            }

            return videoCollection;
        }

        public static VideoCollection BuildColourListAsync(VideoCollection videoCollection, IProgress<ProgressWrapper> progress)
        {
            Directory.CreateDirectory(videoCollection.Config.ImageDirectory);

            for (int i = 1; i < videoCollection.Config.Duration; i++)
            {
                var image = $"frame.{i}.jpg";

                videoCollection.Data.Images.Add(new VideoImage
                {
                    Frame = i,
                    Name = image
                });

                videoCollection.Data.Colours.Add(new VideoColour
                {
                    Frame = i,
                    Hex = ImageProcessor.GetAverageHtmlColourFromImageStreamUsingScale(i, videoCollection.Config.FullPath, Path.Combine(videoCollection.Config.ImageDirectory, image))
                });

                progress.Report(new ProgressWrapper(videoCollection.Config.Duration, i, ProcessType.BuildColourList));
            }

            return videoCollection;
        }
    }
}