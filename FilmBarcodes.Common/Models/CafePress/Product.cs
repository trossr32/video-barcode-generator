using System.Collections.Generic;
using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "products")]
    public class Products
    {
        public List<Product> ProductList { get; set; }
    }

    [XmlRoot(ElementName = "product")]
    public class Product
    {
        [XmlElement(ElementName = "color")]
        public List<Color> Color { get; set; }
        [XmlElement(ElementName = "size")]
        public List<Size> Size { get; set; }
        [XmlElement(ElementName = "perspective")]
        public List<Perspective> Perspective { get; set; }
        [XmlElement(ElementName = "mediaConfiguration")]
        public List<MediaConfiguration> MediaConfiguration { get; set; }
        [XmlElement(ElementName = "productImage")]
        public List<ProductImage> ProductImage { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "merchandiseId")]
        public string MerchandiseId { get; set; }
        [XmlAttribute(AttributeName = "sellPrice")]
        public string SellPrice { get; set; }
        [XmlAttribute(AttributeName = "marketplacePrice")]
        public string MarketplacePrice { get; set; }
        [XmlAttribute(AttributeName = "currencyName")]
        public string CurrencyName { get; set; }
        [XmlAttribute(AttributeName = "currencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "currencySymbol")]
        public string CurrencySymbol { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "memberId")]
        public string MemberId { get; set; }
        [XmlAttribute(AttributeName = "storeId")]
        public string StoreId { get; set; }
        [XmlAttribute(AttributeName = "sectionId")]
        public string SectionId { get; set; }
        [XmlAttribute(AttributeName = "defaultOrientation")]
        public string DefaultOrientation { get; set; }
        [XmlAttribute(AttributeName = "defaultPerspective")]
        public string DefaultPerspective { get; set; }
        [XmlAttribute(AttributeName = "basePrice")]
        public string BasePrice { get; set; }
        [XmlAttribute(AttributeName = "shortCaption")]
        public string ShortCaption { get; set; }
        [XmlAttribute(AttributeName = "defaultCaption")]
        public string DefaultCaption { get; set; }
        [XmlAttribute(AttributeName = "shortDescription")]
        public string ShortDescription { get; set; }
        [XmlAttribute(AttributeName = "categoryId")]
        public string CategoryId { get; set; }
        [XmlAttribute(AttributeName = "categoryCaption")]
        public string CategoryCaption { get; set; }
        [XmlAttribute(AttributeName = "stockAvailability")]
        public string StockAvailability { get; set; }
        [XmlAttribute(AttributeName = "addToCart")]
        public string AddToCart { get; set; }
        [XmlAttribute(AttributeName = "stockAvailabilityStatusId")]
        public string StockAvailabilityStatusId { get; set; }
        [XmlAttribute(AttributeName = "merchandiseAvailability")]
        public string MerchandiseAvailability { get; set; }
        [XmlAttribute(AttributeName = "merchandiseAvailabilityStatusId")]
        public string MerchandiseAvailabilityStatusId { get; set; }
        [XmlAttribute(AttributeName = "defaultProductUri")]
        public string DefaultProductUri { get; set; }
        [XmlAttribute(AttributeName = "additionalPhotoCount")]
        public string AdditionalPhotoCount { get; set; }
        [XmlAttribute(AttributeName = "isCalendar")]
        public string IsCalendar { get; set; }
        [XmlAttribute(AttributeName = "isBook")]
        public string IsBook { get; set; }
        [XmlAttribute(AttributeName = "isCD")]
        public string IsCd { get; set; }
        [XmlAttribute(AttributeName = "isAudioCD")]
        public string IsAudioCd { get; set; }
        [XmlAttribute(AttributeName = "isPoster")]
        public string IsPoster { get; set; }
        [XmlAttribute(AttributeName = "legacyHeightValue")]
        public string LegacyHeightValue { get; set; }
        [XmlAttribute(AttributeName = "legacyWidthValue")]
        public string LegacyWidthValue { get; set; }
        [XmlAttribute(AttributeName = "sortPriority")]
        public string SortPriority { get; set; }
        [XmlAttribute(AttributeName = "isSellable")]
        public string IsSellable { get; set; }
        [XmlAttribute(AttributeName = "isFulfillment")]
        public string IsFulfillment { get; set; }
    }

    [XmlRoot(ElementName = "mediaConfiguration")]
    public class MediaConfiguration
    {
        [XmlAttribute(AttributeName = "dpi")]
        public string Dpi { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "designId")]
        public string DesignId { get; set; }
        [XmlAttribute(AttributeName = "alignment")]
        public string Alignment { get; set; }
        [XmlAttribute(AttributeName = "isEditable")]
        public string IsEditable { get; set; }
        [XmlAttribute(AttributeName = "perspectives")]
        public string Perspectives { get; set; }
    }

    [XmlRoot(ElementName = "productImage")]
    public class ProductImage
    {
        [XmlAttribute(AttributeName = "colorId")]
        public string ColorId { get; set; }
        [XmlAttribute(AttributeName = "perspectiveName")]
        public string PerspectiveName { get; set; }
        [XmlAttribute(AttributeName = "imageSize")]
        public string ImageSize { get; set; }
        [XmlAttribute(AttributeName = "productUrl")]
        public string ProductUrl { get; set; }
    }
}