using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace VideoBarcodeGenerator.Core.Models.BarcodeGenerator
{
    public class VideoLibraryItem
    {
        public VideoLibraryItem(VideoCollection processed)
        {
            Processed = processed;

            var barcodeConfig = processed.BarcodeConfigs.FirstOrDefault();

            if (barcodeConfig == null)
                return;

            StandardBarcodeCreated = File.Exists(barcodeConfig.Barcode_Standard?.FullOutputFile);
            OnePixelBarcodeCreated = File.Exists(barcodeConfig.Barcode_1px?.FullOutputFile);

            ImagePath = StandardBarcodeCreated
                ? Path.GetDirectoryName(barcodeConfig.Barcode_Standard.FullOutputFile)
                : Path.GetDirectoryName(barcodeConfig.Barcode_1px.FullOutputFile);

            if (DateTime.TryParseExact(ImagePath.Split(' ').Last(), "yyyyMMdd_HHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime runDate))
                RunDate = runDate.ToString("f");
        }

        public VideoCollection Processed { get; set; }

        public bool StandardBarcodeCreated { get; set; }
        public bool OnePixelBarcodeCreated { get; set; }
        public string ImagePath { get; set; }
        public string RunDate { get; set; }

        public bool IsSelected { get; set; }
    }
}