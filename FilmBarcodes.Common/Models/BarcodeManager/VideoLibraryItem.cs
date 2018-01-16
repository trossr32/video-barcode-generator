﻿using System;
using System.IO;
using System.Linq;

namespace FilmBarcodes.Common.Models.BarcodeManager
{
    public class VideoLibraryItem
    {
        public VideoLibraryItem(VideoCollection processed, string source)
        {
            Processed = processed;
            Source = source;

            StandardBarcodeCreated = processed.BarcodeConfigs.Any(p =>
            {
                try
                {
                    return File.Exists(p.Barcode_Standard.FullOutputFile);
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

            //ProcessedStandardFrameCountInFileSystemMatchesDuration = processed.Config.Duration == Directory.GetFiles(processed.Config.ImageDirectory).Length;

            //ProcessedOnePixelFrameCountInFileSystemMatchesDuration = processed.Config.Duration == Directory.GetFiles(processed.Config.OnePixelImageDirectory).Length;
        }

        public VideoCollection Processed { get; set; }
        public string Source { get; set; }

        public int Frames { get; set; }

        public bool ProcessedStandardFrameCountInFileSystemMatchesDuration { get; set; }
        public bool ProcessedOnePixelFrameCountInFileSystemMatchesDuration { get; set; }
        public bool StandardBarcodeCreated { get; set; }
        public bool OnePixelBarcodeCreated { get; set; }

        public bool IsSelected { get; set; }
        public bool IsAllItemsSelected { get; set; }
    }
}