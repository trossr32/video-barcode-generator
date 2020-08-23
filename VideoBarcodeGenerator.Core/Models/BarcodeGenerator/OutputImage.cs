using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;

namespace VideoBarcodeGenerator.Core.Models.BarcodeGenerator
{
    public class OutputImage
    {
        public string OutputFilename { get; set; }
        public string FullOutputFile { get; set; }

        public void SetFullOutputFile(VideoCollection videoCollection)
        {
            FullOutputFile = Path.Combine(videoCollection.Config.FullOutputDirectory, OutputFilename);
        }

        [JsonIgnore]
        public ICommand OpenImageDirectoryCommand => new DelegateCommand(OpenImageDirectory);

        /// <summary>
        /// If previously generated barcode images are found then this can be called to open
        /// Windows Explorer at the containing directory
        /// </summary>
        private void OpenImageDirectory()
        {
            var path = Path.GetDirectoryName(FullOutputFile);

            if (string.IsNullOrEmpty(path))
                return;

            Process.Start(path);
        }
    }
}