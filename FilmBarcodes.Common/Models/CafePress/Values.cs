using System.Collections.Generic;
using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "values")]
    public class Values
    {
        [XmlElement(ElementName = "value")]
        public List<string> Value { get; set; }
    }
}