using System.Collections.Generic;
using System.IO;
using System.Linq;
using FilmBarcodes.Common.Models.BarcodeManager;
using FilmBarcodes.Common.Models.Settings;
using FilmBarcodes.Common.Processors;
using Newtonsoft.Json;
using NReco.VideoInfo;
using NUnit.Framework;

namespace FilmBarcodes.Common.Tests
{
    [TestFixture]
    public class DataFix
    {
        private SettingsWrapper _settings;

        [SetUp]
        public void Init()
        {
            _settings = Settings.GetSettings();
        }

        [Test]
        public void FixJson()
        {
            // need to fix duration to be the math.floor value, so re-grab all videos and get their durations, and update the json
            foreach (var directory in Directory.GetDirectories(_settings.BarcodeManager.OutputDirectory).Where(x => x.ToLower().Contains("nemo")))
            {
                var file = Path.Combine(directory, "videocollection.json");

                if (!File.Exists(file))
                    continue;

                //Models.BarcodeManagerOld.VideoCollection videoCollection = JsonConvert.DeserializeObject<Models.BarcodeManagerOld.VideoCollection>(File.ReadAllText(file));
                VideoCollection videoCollection = JsonConvert.DeserializeObject<VideoCollection>(File.ReadAllText(file));

                MediaInfo fileInfo = VideoProcessor.GetVideoInfo(videoCollection.Config.FullPath);

                VideoConfig vc = new VideoConfig(videoCollection.Config.FullPath, _settings.BarcodeManager);

                

                //if (videoCollection.VideoFiles.Count == 0)
                //    continue;

                //VideoCollection target = new VideoCollection
                //{
                //    Config = new VideoConfig
                //    {
                //        DurationTimeSpan = videoCollection.Config.DurationTimeSpan,
                //        FullPath = videoCollection.Config.FullPath,
                //        FullOutputDirectory = videoCollection.Config.FullOutputDirectory,
                //        IsValid = videoCollection.Config.IsValid,
                //        OutputDirectory = videoCollection.Config.OutputDirectory,
                //        SecondsFromEnd = videoCollection.Config.SecondsFromEnd,
                //        SecondsFromStart = videoCollection.Config.SecondsFromStart
                //    }
                //};

                //foreach (var colour in videoCollection.Data.Colours)
                //{
                //    target.Data.Colours.Add(new VideoColour { Frame = colour.Frame, Hex = colour.Hex });
                //}

                //foreach (var image in videoCollection.Data.Images)
                //{
                //    target.Data.Images.Add(new VideoImage { Frame = image.Frame, Name = image.Name });
                //}

                //target.BarcodeConfigs = new List<BarcodeConfig>();

                //foreach (var videoFile in videoCollection.VideoFiles)
                //{
                //    var barcodeconfig = new BarcodeConfig
                //    {
                //        CreateOnePixelBarcode = videoFile.CreateOnePixelBarcode,
                //        CreateZipAndDeleteFrameImages = videoFile.CreateZipAndDeleteFrameImages,
                //        OutputHeight = videoFile.OutputHeight,
                //        OutputWidth = videoFile.OutputWidth,
                //        UseExistingFrameImages = videoFile.UseExistingFrameImages
                //    };

                //    foreach (var outputImage in videoFile.OutputImages)
                //    {
                //        if (outputImage.OutputFilename.Contains("_1px_"))
                //            barcodeconfig.Barcode_1px = new OutputImage
                //            {
                //                OutputFilename = outputImage.OutputFilename,
                //                FullOutputFile = outputImage.FullOutputFile
                //            };
                //        else
                //            barcodeconfig.Barcode_Standard = new OutputImage
                //            {
                //                OutputFilename = outputImage.OutputFilename,
                //                FullOutputFile = outputImage.FullOutputFile
                //            };
                //    }

                //    target.BarcodeConfigs.Add(barcodeconfig);
                //}

                //target.Write();

                //if (Directory.Exists(videoCollection.Config.OnePixelImageDirectory))
                //    Directory.Move(videoCollection.Config.OnePixelImageDirectory, target.Config.OnePixelImageDirectory);
            }
        }
    }
}