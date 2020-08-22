using System;
using VideoBarcodeGenerator.Core.Enums;

namespace VideoBarcodeGenerator.Core.Models
{
    public class ProgressWrapper
    {
        public ProgressWrapper(int totalProgress, int currentProgress, ProcessType processType)
        {
            Frames = currentProgress;

            switch (processType)
            {
                case ProcessType.BuildColourList:
                    Progress = (int) Math.Round(currentProgress / (double) totalProgress * 100);
                    Status = $"Frames: {currentProgress} of {totalProgress}";
                    break;

                case ProcessType.ZipArchive:
                    Progress = currentProgress;
                    Status = currentProgress == 0 ? "Initiating zip of frame images..." : $"Zipping frame images: {currentProgress}%";
                    break;

                case ProcessType.DeleteFrameImages:
                    Progress = 100;
                    Status = "Deleting frame images directory...";
                    break;

                case ProcessType.RenderImage:
                    Progress = (int)Math.Round(currentProgress / (double)totalProgress * 100);
                    Status = $"Rendering image frame: {currentProgress} of {totalProgress}";
                    break;

                case ProcessType.RenderImageCompressedToOnePixelWide:
                    Progress = (int)Math.Round(currentProgress / (double)totalProgress * 100);
                    Status = $"Rendering 1px image frame: {currentProgress} of {totalProgress}";
                    break;

                case ProcessType.WriteVideoFile:
                    Progress = 100;
                    Status = "Writing video file json...";
                    break;
            }
        }

        public int Progress { get; set; }
        public int Frames { get; set; }
        public string Status { get; set; }
    }
}