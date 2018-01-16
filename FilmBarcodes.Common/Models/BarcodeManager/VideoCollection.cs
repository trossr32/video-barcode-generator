﻿using System;
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
            BarcodeConfigs = new List<BarcodeConfig>();
            Data = new VideoData();
        }

        public VideoCollection(string file, Settings.BarcodeManager settings)
        {
            Config = new VideoConfig(file, settings);
            BarcodeConfigs = new List<BarcodeConfig>();
            Data = new VideoData();
        }

        public VideoConfig Config { get; set; }
        public List<BarcodeConfig> BarcodeConfigs { get; set; }
        public VideoData Data { get; set; }
        

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