using System.Collections.Generic;

namespace VideoBarcodeGenerator.Core.Models.BarcodeGenerator
{
    public class VideoData
    {
        public List<VideoColour> Colours { get; set; } = new List<VideoColour>();
        public List<VideoImage> Images { get; set; } = new List<VideoImage>();
    }
}