using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FilmBarcodes.Common.Enums;

namespace FilmBarcodes.Common.Models.BarcodeManagerOld
{
    public class VideoFile
    {
        public VideoFile(int duration, string file)
        {
            OutputWidth = duration;

            VideoFileName = string.IsNullOrEmpty(file) 
                ? "" 
                : file.Split('\\').Last();

            VideoFilenameWithoutExtension = string.IsNullOrEmpty(file) 
                ? "" 
                : Path.GetFileNameWithoutExtension(file);

            OutputImages = new List<OutputImage>();

            Enum.GetValues(typeof(OutputImageType))
                .Cast<OutputImageType>()
                .ToList()
                .ForEach(imageType =>
                {
                    OutputImages.Add(new OutputImage(imageType, this));
                });

            CreateOnePixelBarcode = true;
        }

        public int OutputWidth { get; set; }
        public int OutputHeight { get; set; } = 600;
        public int OutputWidthAsCanvasRatio => OutputHeight * 7;

        public string VideoFileName { get; set; }
        public string VideoFilenameWithoutExtension { get; set; }

        public List<OutputImage> OutputImages {get; set;}

        public OutputImage StandardImage => OutputImages.FirstOrDefault(o => !o.OutputFilename.Contains("_1px_"));
        public OutputImage CompressedOnePixelImage => OutputImages.FirstOrDefault(o => o.OutputFilename.Contains("_1px_"));

        /// <summary>
        ///   Use this field if the frame images were created successfully but the following steps failed (zip, build & write json)
        /// </summary>
        public bool UseExistingFrameImages { get; set; }
        public bool CreateOnePixelBarcode { get; set; }
        public bool CreateZipAndDeleteFrameImages { get; set; }

        public VideoData Data { get; set; } = new VideoData();

        public void SetOutputImagesFullDirectory(VideoCollection videoCollection)
        {
            foreach (var outputImage in OutputImages)
            {
                outputImage.SetFullOutputFile(videoCollection);
            }
        }
    }
}