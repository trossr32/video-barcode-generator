using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "help")]
    public class Help
    {
        [XmlElement(ElementName = "exception-message")]
        public string Exceptionmessage { get; set; }
        [XmlElement(ElementName = "documentation")]
        public string Documentation { get; set; }
    }
}