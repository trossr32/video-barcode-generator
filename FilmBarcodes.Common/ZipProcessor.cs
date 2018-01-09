using System;
using System.IO.Compression;
using System.Threading;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models;
using FilmBarcodes.Common.Models.BarcodeManager;

namespace FilmBarcodes.Common
{
    public static class ZipProcessor
    {
        public static void ZipDirectoryAsync(VideoConfig config, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        {
            progress.Report(new ProgressWrapper(0, 0, ProcessType.ZipArchive));
            
            ZipFile.CreateFromDirectory(config.ImageDirectory, config.ZipFile, CompressionLevel.Fastest, true);
        }

        //public static void ZipDirectoryAsync(string dir, IProgress<ProgressWrapper> progress, CancellationToken cancellationToken)
        //{
        //    progress.Report(new ProgressWrapper(0, 0, ProcessType.ZipArchive));

        //    string sevenZipPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");

        //    SevenZipBase.SetLibraryPath(sevenZipPath);

        //    var directoryName = dir.Split('\\').Last();
        //    var directory = dir.Replace(directoryName, "");

        //    var file = new SevenZipCompressor
        //    {
        //        ArchiveFormat = OutArchiveFormat.SevenZip,
        //        CompressionMode = CompressionMode.Create,
        //        TempFolderPath = Path.GetTempPath()
        //    };

        //    file.Compressing += (sender, args) =>
        //    {
        //        cancellationToken.ThrowIfCancellationRequested();

        //        progress.Report(new ProgressWrapper(0, args.PercentDone, ProcessType.ZipArchive));
        //    };

        //    file.CompressDirectory(dir, Path.Combine(directory, $"{directoryName}.7z"));
        //}

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