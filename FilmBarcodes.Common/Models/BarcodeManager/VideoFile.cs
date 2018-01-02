using System.IO;
using System.Linq;

namespace FilmBarcodes.Common.Models.BarcodeManager
{
    public class VideoFile
    {
        public VideoFile(int duration, string file)
        {
            OutputWidth = duration;

            FileName = string.IsNullOrEmpty(file) 
                ? "" 
                : file.Split('\\').Last();

            FilenameWithoutExtension = string.IsNullOrEmpty(file) 
                ? "" 
                : Path.GetFileNameWithoutExtension(file);


        }

        public int OutputWidth { get; set; }
        public int OutputHeight { get; set; } = 600;
        public int OutputWidthAsCanvasRatio => OutputHeight * 7;

        public string FileName { get; set; }
        public string FilenameWithoutExtension { get; set; }

        public string OutputFilename => $"{FilenameWithoutExtension}_{OutputWidth}_{OutputHeight}.jpg";
        public string FullOutputFile { get; set; }

        public VideoData Data { get; set; } = new VideoData();
    }
}