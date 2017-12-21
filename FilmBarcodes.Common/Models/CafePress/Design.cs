using System.Collections.Generic;
using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "designs")]
    public class Designs
    {
        public List<Design> DesignList { get; set; }
    }

    [XmlRoot(ElementName = "design")]
    public class Design
    {
        [XmlElement(ElementName = "category")]
        public string Category { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "active")]
        public string Active { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "parentId")]
        public string ParentId { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "caption")]
        public string Caption { get; set; }
        [XmlAttribute(AttributeName = "creator")]
        public string Creator { get; set; }
        [XmlAttribute(AttributeName = "folderId")]
        public string FolderId { get; set; }
    }
}