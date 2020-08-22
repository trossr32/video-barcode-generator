using System;
using System.IO;
using System.Linq;

namespace VideoBarcodeGenerator.Core.Models.BarcodeGenerator
{
    public class VideoLibraryItem
    {
        public VideoLibraryItem(VideoCollection processed)
        {
            Processed = processed;

            StandardBarcodeCreated = processed.BarcodeConfigs.Any(p =>
            {
                try
                {
                    return File.Exists(p.Barcode_Standard?.FullOutputFile);
                }
                catch (Exception)
                {
                    return false;
                }
            });

            OnePixelBarcodeCreated = processed.BarcodeConfigs.Any(p =>
            {
                try
                {
                    return File.Exists(p.Barcode_1px?.FullOutputFile);
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        public VideoCollection Processed { get; set; }

        public bool ProcessedStandardFrameCountInFileSystemMatchesDuration { get; set; }
        public bool ProcessedOnePixelFrameCountInFileSystemMatchesDuration { get; set; }
        public bool StandardBarcodeCreated { get; set; }
        public bool OnePixelBarcodeCreated { get; set; }

        public bool IsSelected { get; set; }
    }
}