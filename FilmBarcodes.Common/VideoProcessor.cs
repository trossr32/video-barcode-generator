using System;
using System.IO;
using System.Linq;
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

        public static VideoCollection BuildColourListAsync(VideoCollection videoCollection, BarcodeConfig videoFile, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(videoCollection.Config.ImageDirectory);

            // get any image files now to save querying the file system for every image
            string[] imageFiles = Directory.GetFiles(videoCollection.Config.ImageDirectory);

            for (int i = 1; i <= videoCollection.Config.Duration; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var image = $"frame.{i}.jpg";

                var hex = videoFile.UseExistingFrameImages && imageFiles.Any(f => f == image)
                    ? ImageProcessor.GetAverageHtmlColourFromImageUsingScale(i, videoCollection)
                    : ImageProcessor.GetAverageHtmlColourFromImageStreamUsingScale(i, image, videoCollection);

                // nreco can fail on the last frame, unsure why at the moment. Maybe duration & frame count mismatch?
                if (hex != null)
                {
                    videoCollection.Data.Images.Add(new VideoImage { Frame = i, Name = image});
                    videoCollection.Data.Colours.Add(new VideoColour { Frame = i, Hex = hex});
                }

                progress.Report(new ProgressWrapper(videoCollection.Config.Duration, i, ProcessType.BuildColourList));
            }

            return videoCollection;
        }
    }
}