using System;
using System.IO;
using System.Threading;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models;
using FilmBarcodes.Common.Models.BarcodeManager;
using NReco.VideoInfo;

namespace FilmBarcodes.Common
{
    public static class VideoProcessor
    {
        public static MediaInfo GetVideoInfo(string file)
        {
            return new FFProbe().GetMediaInfo(file);
        }

        public static VideoCollection BuildColourListAsync(VideoCollection videoCollection, VideoFile videoFile, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(videoCollection.Config.ImageDirectory);

            for (int i = 1; i <= videoCollection.Config.Duration; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var image = $"frame.{i}.jpg";

                videoCollection.Data.Images.Add(new VideoImage
                {
                    Frame = i,
                    Name = image
                });

                var hex = videoFile.UseExistingFrameImages && File.Exists(Path.Combine(videoCollection.Config.ImageDirectory, image))
                    ? ImageProcessor.GetAverageHtmlColourFromImageUsingScale(i, videoCollection)
                    : ImageProcessor.GetAverageHtmlColourFromImageStreamUsingScale(i, image, videoCollection);

                videoCollection.Data.Colours.Add(new VideoColour
                {
                    Frame = i,
                    Hex = hex
                });

                progress.Report(new ProgressWrapper(videoCollection.Config.Duration, i, ProcessType.BuildColourList));
            }

            return videoCollection;
        }
    }
}