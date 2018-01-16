using System.IO;
using FilmBarcodes.Common.Enums;

namespace FilmBarcodes.Common.Models.BarcodeManagerOld
{
    public class OutputImage
    {
        public OutputImage(OutputImageType outputImageType, VideoFile videoFile)
        {
            if (videoFile == null)
                return;

            OutputImageType = outputImageType;

            switch (outputImageType)
            {
                case OutputImageType.Standard:
                    OutputFilename = $"{videoFile.VideoFilenameWithoutExtension}_{videoFile.OutputWidth}_{videoFile.OutputHeight}.jpg";
                    break;
                case OutputImageType.CompressedOnePixel:
                    OutputFilename = $"{videoFile.VideoFilenameWithoutExtension}_1px_{videoFile.OutputWidth}_{videoFile.OutputHeight}.jpg";
                    break;
            }
        }

        public void SetFullOutputFile(VideoCollection videoCollection)
        {
            FullOutputFile = Path.Combine(videoCollection.Config.FullOutputDirectory, OutputFilename);
        }

        public OutputImageType OutputImageType { get; set; }
        public string OutputFilename { get; set; }
        public string FullOutputFile { get; set; }
    }
}