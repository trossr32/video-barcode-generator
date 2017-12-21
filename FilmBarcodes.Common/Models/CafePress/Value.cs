using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "value")]
    public class Value
    {
        [XmlText]
        public string Text { get; set; }
    }
}