using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FilmBarcodes.Common.Enums;
using Newtonsoft.Json;
using NReco.VideoInfo;

namespace FilmBarcodes.Common.Models.BarcodeManager
{
    public class VideoFile
    {
        public VideoFile(string file)
        {
           try
            {
                if (!AcceptedVideoFiles.Contains(Path.GetExtension(file).TrimStart('.')))
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
                        OutputWidth = Duration;

                        OutputDirectory = FilenameWithoutExtension;

                        IsValid = true;
                    }
                }
            }
            catch (Exception e)
            {
                IsValid = false;
            }
        }

        [JsonIgnore]
        public List<string> AcceptedVideoFiles = new List<string> { "avi", "divx", "m4v", "mkv", "m2ts", "mp4", "mpg", "wmv" };

        public TimeSpan DurationTimeSpan { get; set; }
        public int Duration => Convert.ToInt32(Math.Round(DurationTimeSpan.TotalSeconds, 0));
        public string DurationText => DurationTimeSpan.ToString("g");

        public string FullPath { get; set; }
        public string FileName => string.IsNullOrEmpty(FullPath) ? "" : FullPath.Split('\\').Last();
        public string FilenameWithoutExtension => string.IsNullOrEmpty(FullPath) ? "" : Path.GetFileNameWithoutExtension(FullPath);
        public string FilePath => string.IsNullOrEmpty(FullPath) ? "" : FullPath.Replace(FileName, "");
        public string OutputDirectory { get; set; }
        public string FullOutputDirectory { get; set; }
        public string ImageDirectory => Path.Combine(FullOutputDirectory, "frame images");

        public int OutputWidth { get; set; }
        public int OutputHeight { get; set; } = 600;
        public int OutputWidthAsCanvasRatio => OutputHeight * 7;

        public bool ForCanvas { get; set; }
        public bool CreateWebPage { get; set; }

        public bool IsValid { get; set; }

        public List<Colours> Colours { get; set; } = new List<Colours>();
        public List<FrameImage> Images { get; set; } = new List<FrameImage>();

        public void Write()
        {
            File.WriteAllText(Path.Combine(FullOutputDirectory, "videofile.json"), JsonConvert.SerializeObject(this));
        }

        public void WriteAsync(IProgress<ProgressWrapper> progress)
        {
            File.WriteAllText(Path.Combine(FullOutputDirectory, "videofile.json"), JsonConvert.SerializeObject(this));

            progress.Report(new ProgressWrapper(Duration, Duration, ProcessType.WriteVideoFile));
        }
    }

    public class Colours
    {
        public int Frame { get; set; }
        public string Hex { get; set; }
    }

    public class FrameImage
    {
        public int Frame { get; set; }
        public string Name { get; set; }
    }
}