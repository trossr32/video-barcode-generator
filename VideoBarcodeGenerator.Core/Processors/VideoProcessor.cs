using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using NReco.VideoInfo;
using VideoBarcodeGenerator.Core.Enums;
using VideoBarcodeGenerator.Core.Models;
using VideoBarcodeGenerator.Core.Models.BarcodeGenerator;
using VideoBarcodeGenerator.Core.Models.Settings;

namespace VideoBarcodeGenerator.Core.Processors
{
    public static class VideoProcessor
    {
        public static MediaInfo GetVideoInfo(string file)
        {
            return new FFProbe().GetMediaInfo(file);
        }

        public static VideoCollection BuildColourListAsync(VideoCollection videoCollection, BarcodeConfig videoFile, SettingsWrapper settings, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(videoCollection.Config.ImageDirectory);

            //initialise colour and image lists otherwise they just keep growing
            videoCollection.Data.Images = new List<VideoImage>();
            videoCollection.Data.Colours = new List<VideoColour>();

            // get any image files now to save querying the file system for every image
            List<string> imageFiles = Directory.GetFiles(videoCollection.Config.ImageDirectory).Select(f => f.Split('\\').Last()).ToList();

            for (int i = 1; i <= videoCollection.Config.Duration; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var image = $"frame.{i}.jpg";

                var hex = videoFile.UseExistingFrameImages && imageFiles.Any(f => f == image)
                    ? ImageProcessor.GetAverageHtmlColourFromImageUsingScale(i, videoCollection, settings)
                    : ImageProcessor.GetAverageHtmlColourFromImageStreamUsingScale(i, image, videoCollection, settings);

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