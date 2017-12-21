using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "size")]
    public class Size
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "fullName")]
        public string FullName { get; set; }
        [XmlAttribute(AttributeName = "shortName")]
        public string ShortName { get; set; }
        [XmlAttribute(AttributeName = "default")]
        public string Default { get; set; }
        [XmlAttribute(AttributeName = "priceDifference")]
        public string PriceDifference { get; set; }
        [XmlAttribute(AttributeName = "displaySellPrice")]
        public string DisplaySellPrice { get; set; }
        [XmlAttribute(AttributeName = "displayDiscountPrice")]
        public string DisplayDiscountPrice { get; set; }
    }
}