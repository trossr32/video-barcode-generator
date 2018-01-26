namespace FilmBarcodes.Common.Models.BarcodeManager
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

        public int OutputWidth { get; set; }
        public int OutputHeight { get; set; } = 600;
        public int OutputWidthAsCanvasRatio => OutputHeight * 7;

        public OutputImage Barcode_Standard { get; set; }
        public OutputImage Barcode_1px { get; set; }

        /// <summary>
        ///   Use this field if the frame images were created successfully but the following steps failed (zip, build & write json)
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
    }
}