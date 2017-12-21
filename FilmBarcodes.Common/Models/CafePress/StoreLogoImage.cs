using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "storeLogoImage")]
    public class StoreLogoImage
    {
        [XmlAttribute(AttributeName = "imageSrc")]
        public string ImageSrc { get; set; }
    }
}