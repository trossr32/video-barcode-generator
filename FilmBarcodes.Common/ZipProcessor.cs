using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models;
using SevenZip;

namespace FilmBarcodes.Common
{
    public static class ZipProcessor
    {
        public static void ZipDirectory(string dir)
        {
            string sevenZipPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");

            SevenZipBase.SetLibraryPath(sevenZipPath);

            var directoryName = dir.Split('\\').Last();

            var file = new SevenZipCompressor
            {
                ArchiveFormat = OutArchiveFormat.SevenZip,
                CompressionMode = CompressionMode.Create,
                TempFolderPath = Path.GetTempPath()
            };

            //file.Compressing += (sender, args) => { Console.Write($"\rCompressing {directoryName} : {args.PercentDone}%"); };
            //file.CompressionFinished += (sender, args) => { Console.WriteLine(""); };

            file.CompressDirectory(dir, Path.Combine(dir, $"{directoryName}.7z"));
        }

        public static void ZipDirectoryAsync(string dir, IProgress<ProgressWrapper> progress)
        {
            string sevenZipPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");

            SevenZipBase.SetLibraryPath(sevenZipPath);

            var directoryName = dir.Split('\\').Last();
            var directory = dir.Replace(directoryName, "");

            var file = new SevenZipCompressor
            {
                ArchiveFormat = OutArchiveFormat.SevenZip,
                CompressionMode = CompressionMode.Create,
                TempFolderPath = Path.GetTempPath()
            };

            file.Compressing += (sender, args) => { progress.Report(new ProgressWrapper(0, args.PercentDone, ProcessType.ZipArchive)); };
            //file.CompressionFinished += (sender, args) => { };

            file.CompressDirectory(dir, Path.Combine(directory, $"{directoryName}.7z"));
        }

        public static void UnZipArchivesInDirectory(string dir)
        {
            string sevenZipPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");

            SevenZipBase.SetLibraryPath(sevenZipPath);

            foreach (var archive in Directory.GetFiles(dir, "*.7z", SearchOption.TopDirectoryOnly))
            {
                var file = new SevenZipExtractor(archive);

                file.Extracting += (sender, args) => { Console.Write($"\rExtracting : {args.PercentDone}%"); };
                file.ExtractionFinished += (sender, args) => { Console.WriteLine(""); };

                file.ExtractArchive(dir);
            }
        }
    }
}