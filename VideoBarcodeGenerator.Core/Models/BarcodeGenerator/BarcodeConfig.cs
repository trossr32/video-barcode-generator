using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;

namespace VideoBarcodeGenerator.Core.Models.BarcodeGenerator
{
    public class BarcodeConfig
    {
        public BarcodeConfig(int duration, string filenameWithoutExtension)
        {
            OutputWidth = duration;

            Barcode_Standard = new OutputImage { OutputFilename = $"{filenameWithoutExtension}_{OutputWidth}_{OutputHeight}.jpg" };
            Barcode_1px = new OutputImage { OutputFilename = $"{filenameWithoutExtension}_1px_{OutputWidth}_{OutputHeight}.jpg" };

            CreateOnePixelBarcode = true;
        }

        public BarcodeConfig()
        {
            CreateOnePixelBarcode = true;
        }

        [JsonIgnore]
        public ICommand OpenImageDirectoryCommand => new DelegateCommand(OpenImageDirectory);

        public int OutputWidth { get; set; }
        public int OutputHeight { get; set; } = 600;
        public int OutputWidthAsCanvasRatio => OutputHeight * 7;

        public OutputImage Barcode_Standard { get; set; }
        public OutputImage Barcode_1px { get; set; }

        /// <summary>
        /// Use this field if the frame images were created successfully but the following steps failed (zip, build & write json)
        /// </summary>
        public bool UseExistingFrameImages { get; set; }
        public bool CreateOnePixelBarcode { get; set; }
        public bool CreateZipAndDeleteFrameImages { get; set; }

        public void SetOutputImagesFullDirectory(VideoCollection videoCollection)
        {
            Barcode_Standard = new OutputImage { OutputFilename = $"{videoCollection.Config.FilenameWithoutExtension}_{OutputWidth}_{OutputHeight}.jpg" };
            Barcode_1px = new OutputImage { OutputFilename = $"{videoCollection.Config.FilenameWithoutExtension}_1px_{OutputWidth}_{OutputHeight}.jpg" };

            Barcode_Standard.SetFullOutputFile(videoCollection);
            Barcode_1px.SetFullOutputFile(videoCollection);
        }

        /// <summary>
        /// If previously generated barcode images are found then this can be called to open
        /// Windows Explorer at the containing directory
        /// </summary>
        private void OpenImageDirectory()
        {
            var path = Path.GetDirectoryName(Barcode_Standard?.FullOutputFile) ?? Path.GetDirectoryName(Barcode_1px?.FullOutputFile);

            if (string.IsNullOrEmpty(path))
                return;

            Process.Start(path);
        }
    }
}