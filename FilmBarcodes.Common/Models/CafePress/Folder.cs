using System.Collections.Generic;
using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "folder")]
    public class Folder
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "folders")]
    public class Folders
    {
        [XmlElement(ElementName = "folder")]
        public List<Folder> FolderList { get; set; }
    }
}