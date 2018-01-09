using System;
using System.IO;
using System.Linq;
using NReco.VideoInfo;

namespace FilmBarcodes.Common.Models.BarcodeManager
{
    public class VideoConfig
    {
        public VideoConfig()
        {

        }

        public VideoConfig(string file, Settings.BarcodeManager settings)
        {
            try
            {
                if (!settings.AcceptedVideoFiles.Contains(Path.GetExtension(file).TrimStart('.')))
                    IsValid = false;
                else
                {
                    MediaInfo fileInfo = VideoProcessor.GetVideoInfo(file);

                    FullPath = file;
                    DurationTimeSpan = fileInfo.Duration;

                    var fileName = file.Split('\\').Last();

                    FilePath = string.IsNullOrEmpty(file) ? "" : file.Replace(fileName, "");

                    if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(FilePath))
                        IsValid = false;
                    else
                    {
                        OutputDirectory = Path.GetFileNameWithoutExtension(file);

                        FullOutputDirectory = Path.Combine(settings.OutputDirectory, OutputDirectory);

                        IsValid = true;
                    }
                }
            }
            catch (Exception e)
            {
                IsValid = false;
            }
        }

        public TimeSpan DurationTimeSpan { get; set; }
        public int Duration => Convert.ToInt32(Math.Floor(DurationTimeSpan.TotalSeconds));
        public string DurationText => DurationTimeSpan.ToString("g");

        public string FullPath { get; set; }
        public string FilePath { get; set; }
        public string OutputDirectory { get; set; }
        public string FullOutputDirectory { get; set; }
        public string ImageDirectory => Path.Combine(FullOutputDirectory, "frame images");
        public string ZipFile => Path.Combine(FullOutputDirectory, "frame images.zip");

        public bool IsValid { get; set; }

        public int SecondsFromStart { get; set; } = 0;
        public int SecondsFromEnd { get; set; } = 0;
    }
}