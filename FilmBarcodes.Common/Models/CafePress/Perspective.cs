using System.Collections.Generic;
using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "perspective")]
    public class Perspective
    {
        [XmlElement(ElementName = "showsMediaRegion")]
        public List<string> ShowsMediaRegion { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "pixelWidth")]
        public string PixelWidth { get; set; }
        [XmlAttribute(AttributeName = "pixelHeight")]
        public string PixelHeight { get; set; }
        [XmlAttribute(AttributeName = "sortOrder")]
        public string SortOrder { get; set; }
        [XmlAttribute(AttributeName = "isEditable")]
        public string IsEditable { get; set; }
        [XmlAttribute(AttributeName = "defaultMediaRegion")]
        public string DefaultMediaRegion { get; set; }
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }
        [XmlAttribute(AttributeName = "wildcardProductUri")]
        public string WildcardProductUri { get; set; }
    }
}