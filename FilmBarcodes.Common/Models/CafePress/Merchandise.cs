using System.Collections.Generic;
using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "merchandises")]
    public class Merchandises
    {
        public List<Merchandise> MerchandiseList { get; set; }
    }

    [XmlRoot(ElementName = "merchandise")]
    public class Merchandise
    {
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "color")]
        public List<Color> Color { get; set; }
        [XmlElement(ElementName = "size")]
        public List<Size> Size { get; set; }
        [XmlElement(ElementName = "stockAvailabilityOverride")]
        public List<StockAvailabilityOverride> StockAvailabilityOverride { get; set; }
        [XmlElement(ElementName = "mediaRegion")]
        public List<MediaRegion> MediaRegion { get; set; }
        [XmlElement(ElementName = "perspective")]
        public List<Perspective> Perspective { get; set; }
        [XmlElement(ElementName = "miscNotes")]
        public string MiscNotes { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "defaultCaption")]
        public string DefaultCaption { get; set; }
        [XmlAttribute(AttributeName = "printMethod")]
        public string PrintMethod { get; set; }
        [XmlAttribute(AttributeName = "defaultOrientation")]
        public string DefaultOrientation { get; set; }
        [XmlAttribute(AttributeName = "defaultPerspective")]
        public string DefaultPerspective { get; set; }
        [XmlAttribute(AttributeName = "basePrice")]
        public string BasePrice { get; set; }
        [XmlAttribute(AttributeName = "marketplacePrice")]
        public string MarketplacePrice { get; set; }
        [XmlAttribute(AttributeName = "sellPrice")]
        public string SellPrice { get; set; }
        [XmlAttribute(AttributeName = "discountAmount")]
        public string DiscountAmount { get; set; }
        [XmlAttribute(AttributeName = "currencyName")]
        public string CurrencyName { get; set; }
        [XmlAttribute(AttributeName = "currencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "currencySymbol")]
        public string CurrencySymbol { get; set; }
        [XmlAttribute(AttributeName = "markdown")]
        public string Markdown { get; set; }
        [XmlAttribute(AttributeName = "isOnSale")]
        public string IsOnSale { get; set; }
        [XmlAttribute(AttributeName = "shortCaption")]
        public string ShortCaption { get; set; }
        [XmlAttribute(AttributeName = "shortDescription")]
        public string ShortDescription { get; set; }
        [XmlAttribute(AttributeName = "stockAvailability")]
        public string StockAvailability { get; set; }
        [XmlAttribute(AttributeName = "stockAvailabilityId")]
        public string StockAvailabilityId { get; set; }
        [XmlAttribute(AttributeName = "allowAddToCart")]
        public string AllowAddToCart { get; set; }
        [XmlAttribute(AttributeName = "availability")]
        public string Availability { get; set; }
        [XmlAttribute(AttributeName = "availabilityId")]
        public string AvailabilityId { get; set; }
        [XmlAttribute(AttributeName = "wildcardBlankProductUrl")]
        public string WildcardBlankProductUrl { get; set; }
        [XmlAttribute(AttributeName = "defaultBlankProductUrl")]
        public string DefaultBlankProductUrl { get; set; }
        [XmlAttribute(AttributeName = "categoryId")]
        public string CategoryId { get; set; }
        [XmlAttribute(AttributeName = "defaultMediaRegion")]
        public string DefaultMediaRegion { get; set; }
    }

    [XmlRoot(ElementName = "stockAvailabilityOverride")]
    public class StockAvailabilityOverride
    {
        [XmlAttribute(AttributeName = "colorNumber")]
        public string ColorNumber { get; set; }
        [XmlAttribute(AttributeName = "sizeNumber")]
        public string SizeNumber { get; set; }
        [XmlAttribute(AttributeName = "productTypeNumber")]
        public string ProductTypeNumber { get; set; }
        [XmlAttribute(AttributeName = "stockAvailabilityNumber")]
        public string StockAvailabilityNumber { get; set; }
        [XmlAttribute(AttributeName = "stockAvailabilityMessage")]
        public string StockAvailabilityMessage { get; set; }
        [XmlAttribute(AttributeName = "allowAddToCart")]
        public string AllowAddToCart { get; set; }
    }

    [XmlRoot(ElementName = "mediaRegion")]
    public class MediaRegion
    {
        [XmlAttribute(AttributeName = "defaultAlignment")]
        public string DefaultAlignment { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "displayName")]
        public string DisplayName { get; set; }
        [XmlAttribute(AttributeName = "dpi")]
        public string Dpi { get; set; }
        [XmlAttribute(AttributeName = "hasFullBleed")]
        public string HasFullBleed { get; set; }
        [XmlAttribute(AttributeName = "minHeight")]
        public string MinHeight { get; set; }
        [XmlAttribute(AttributeName = "minWidth")]
        public string MinWidth { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
