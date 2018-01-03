using System;
using FilmBarcodes.Common.Enums;

namespace FilmBarcodes.Common.Models
{
    public class ProgressWrapper
    {
        public ProgressWrapper(int totalFrames, int framesProcessed, ProcessType processType)
        {
            Frames = framesProcessed;

            switch (processType)
            {
                case ProcessType.BuildColourList:
                    Progress = (int) Math.Round(framesProcessed / (double) totalFrames * 88);
                    Status = $"Frames: {framesProcessed} of {totalFrames}";
                    break;
                case ProcessType.ZipArchive:
                    Progress = framesProcessed / 10 + 88;
                    Status = $"Zipping frame images: {framesProcessed}%";
                    break;
                case ProcessType.RenderImage:
                    Progress = 99;
                    Status = "Rendering image...";
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