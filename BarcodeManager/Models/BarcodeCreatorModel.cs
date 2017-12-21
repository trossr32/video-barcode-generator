using FilmBarcodes.Common.Models.BarcodeManager;

namespace BarcodeManager.Models
{
    public class BarcodeCreatorModel
    {
        public VideoFile ProcessNewFile(string file)
        {
            return new VideoFile(file);
        }
    }
}