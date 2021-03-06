﻿using System;
using System.IO.Compression;
using System.Threading;
using VideoBarcodeGenerator.Core.Enums;
using VideoBarcodeGenerator.Core.Models;
using VideoBarcodeGenerator.Core.Models.BarcodeGenerator;

namespace VideoBarcodeGenerator.Core.Processors
{
    public static class ZipProcessor
    {
        public static void ZipDirectoryAsync(VideoConfig config, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        {
            progress.Report(new ProgressWrapper(0, 0, ProcessType.ZipArchive));

            ZipFile.CreateFromDirectory(config.ImageDirectory, config.ZipFile, CompressionLevel.Fastest, true);
        }

        public static void UnZipArchivesInDirectory(string dir)
        {
            //string sevenZipPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");

            //SevenZipBase.SetLibraryPath(sevenZipPath);

            //foreach (var archive in Directory.GetFiles(dir, "*.7z", SearchOption.TopDirectoryOnly))
            //{
            //    var file = new SevenZipExtractor(archive);

            //    file.Extracting += (sender, args) => { Console.Write($"\rExtracting : {args.PercentDone}%"); };
            //    file.ExtractionFinished += (sender, args) => { Console.WriteLine(""); };

            //    file.ExtractArchive(dir);
            //}
        }
    }
}