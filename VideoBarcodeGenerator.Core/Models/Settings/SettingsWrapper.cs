using System.Collections.Generic;

namespace VideoBarcodeGenerator.Core.Models.Settings
{
    public class SettingsWrapper
    {
        private readonly string DefaultOutputDirectory = @"Q:\Film Barcodes";

        public SettingsWrapper()
        {
            CoreSettings = new CoreSettings
            {
                OutputDirectory = DefaultOutputDirectory
            };
        }
        
        public CoreSettings CoreSettings { get; set; }
    }

    public class CoreSettings
    {
        public string OutputDirectory { get; set; } = @"Q:\Film Barcodes";

        public string DefaultSource { get; set; } = @"M:\movies";

        public List<string> AcceptedVideoFiles => new List<string> { "avi", "divx", "m4v", "mkv", "m2ts", "mp4", "mpg", "wmv" };

        public int NumberOfConcurrentTasks => 3;
        public int NumberOfTasksToRunBetweenCacheClear => 5;

        public string MagickImageTempDir => @"C:\temp\VideoBarcodeGenerator.temp";
    }
}