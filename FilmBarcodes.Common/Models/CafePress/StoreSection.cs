using System.Collections.Generic;
using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{
    [XmlRoot(ElementName = "storeSections")]
    public class StoreSections
    {
        public List<StoreSection> StoreSectionList { get; set; }
    }

    [XmlRoot(ElementName = "storeSection")]
    public class StoreSection
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "memberId")]
        public string MemberId { get; set; }
        [XmlAttribute(AttributeName = "storeId")]
        public string StoreId { get; set; }
        [XmlAttribute(AttributeName = "parentSectionId")]
        public string ParentSectionId { get; set; }
        [XmlAttribute(AttributeName = "caption")]
        public string Caption { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "visible")]
        public string Visible { get; set; }
        [XmlAttribute(AttributeName = "active")]
        public string Active { get; set; }
        [XmlAttribute(AttributeName = "defaultMarkupProfile")]
        public string DefaultMarkupProfile { get; set; }
        [XmlAttribute(AttributeName = "defaultProductMarkup")]
        public string DefaultProductMarkup { get; set; }
        [XmlAttribute(AttributeName = "sectionImageId")]
        public string SectionImageId { get; set; }
        [XmlAttribute(AttributeName = "sectionImageWidth")]
        public string SectionImageWidth { get; set; }
        [XmlAttribute(AttributeName = "sectionImageHeight")]
        public string SectionImageHeight { get; set; }
        [XmlAttribute(AttributeName = "sortPriority")]
        public string SortPriority { get; set; }
        [XmlAttribute(AttributeName = "itemsAcross")]
        public string ItemsAcross { get; set; }
        [XmlAttribute(AttributeName = "categoryId")]
        public string CategoryId { get; set; }
        [XmlAttribute(AttributeName = "imageType")]
        public string ImageType { get; set; }
        [XmlAttribute(AttributeName = "defaultProductDescription")]
        public string DefaultProductDescription { get; set; }
        [XmlAttribute(AttributeName = "defaultProductImageId")]
        public string DefaultProductImageId { get; set; }
        [XmlAttribute(AttributeName = "defaultProductName")]
        public string DefaultProductName { get; set; }
        [XmlAttribute(AttributeName = "teaser")]
        public string Teaser { get; set; }
    }
}