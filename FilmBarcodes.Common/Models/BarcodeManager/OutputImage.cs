using System.IO;

namespace FilmBarcodes.Common.Models.BarcodeManager
{
    public class OutputImage
    {
        public string OutputFilename { get; set; }
        public string FullOutputFile { get; set; }

        public void SetFullOutputFile(VideoCollection videoCollection)
        {
            FullOutputFile = Path.Combine(videoCollection.Config.FullOutputDirectory, OutputFilename);
        }
    }
}