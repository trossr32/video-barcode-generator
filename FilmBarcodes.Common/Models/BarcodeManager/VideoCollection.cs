using System;
using System.Collections.Generic;
using System.IO;
using FilmBarcodes.Common.Enums;
using Newtonsoft.Json;

namespace FilmBarcodes.Common.Models.BarcodeManager
{
    public class VideoCollection
    {
        public VideoCollection()
        {
            Config = new VideoConfig();
            Data = new VideoData();
            VideoFiles = new List<VideoFile>();
        }

        public VideoCollection(string file, Settings.BarcodeManager settings)
        {
            Config = new VideoConfig(file, settings);

            Data = new VideoData();
            
            VideoFiles = new List<VideoFile>();
        }

        public VideoConfig Config { get; set; }
        public VideoData Data { get; set; }
        public List<VideoFile> VideoFiles { get; set; }
        

        public void Write()
        {
            File.WriteAllText(Path.Combine(Config.FullOutputDirectory, "videocollection.json"), JsonConvert.SerializeObject(this));
        }

        public void WriteAsync(IProgress<ProgressWrapper> progress)
        {
            File.WriteAllText(Path.Combine(Config.FullOutputDirectory, "videocollection.json"), JsonConvert.SerializeObject(this));

            progress.Report(new ProgressWrapper(Config.Duration, Config.Duration, ProcessType.WriteVideoFile));
        }
    }
}