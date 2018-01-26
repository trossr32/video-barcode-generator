using System;
using System.IO;
using System.Linq;
using FilmBarcodes.Common.Processors;
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
                    
                    if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FilePath))
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
        public string FileName => FullPath.Split('\\').Last();
        public string FilenameWithoutExtension => Path.GetFileNameWithoutExtension(FileName);
        public string FilePath => FullPath.Replace(FileName, "");

        public string OutputDirectory { get; set; }
        public string FullOutputDirectory { get; set; }
        public string ImageDirectory => Path.Combine(FullOutputDirectory, "frame images");
        public string OnePixelImageDirectory => Path.Combine(FullOutputDirectory, "one pixel frame images");
        public string ZipFile => Path.Combine(FullOutputDirectory, "frame images.zip");

        public bool IsValid { get; set; }

        public int SecondsFromStart { get; set; } = 0;
        public int SecondsFromEnd { get; set; } = 0;
    }
}