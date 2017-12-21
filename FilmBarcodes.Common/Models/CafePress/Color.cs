using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "color")]
    public class Color
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "default")]
        public string Default { get; set; }
        [XmlAttribute(AttributeName = "allowed")]
        public string Allowed { get; set; }
        [XmlAttribute(AttributeName = "colorSwatchUrl")]
        public string ColorSwatchUrl { get; set; }
    }
}