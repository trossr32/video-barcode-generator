using System.Collections.Generic;
using System.Xml.Serialization;

namespace FilmBarcodes.Common.Models.CafePress
{

    [XmlRoot(ElementName = "stores")]
    public class Stores
    {
        [XmlElement(ElementName = "store")]
        public List<Store> StoreList { get; set; }
    }

    [XmlRoot(ElementName = "store")]
    public class Store
    {
        [XmlElement(ElementName = "storeLogoImage")]
        public StoreLogoImage StoreLogoImage { get; set; }

        [XmlElement(ElementName = "descriptionHtml")]
        public string DescriptionHtml { get; set; }
        
        /// <summary>
        /// This will be the name assigned to the cafepress uri, e.g. http://www.cafepress.co.uk/__id__
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "storeNo")]
        public string StoreNo { get; set; }

        [XmlAttribute(AttributeName = "memberNo")]
        public string MemberNo { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "isPremiumStore")]
        public string IsPremiumStore { get; set; }

        [XmlAttribute(AttributeName = "isSelfBuy")]
        public string IsSelfBuy { get; set; }

        [XmlAttribute(AttributeName = "homePage")]
        public string HomePage { get; set; }

        [XmlAttribute(AttributeName = "defaultProductImageId")]
        public string DefaultProductImageId { get; set; }

        [XmlAttribute(AttributeName = "defaultProductMarkup")]
        public string DefaultProductMarkup { get; set; }

        [XmlAttribute(AttributeName = "defaultProductMarkupIsAbsolute")]
        public string DefaultProductMarkupIsAbsolute { get; set; }

        [XmlAttribute(AttributeName = "logoType")]
        public string LogoType { get; set; }

        [XmlAttribute(AttributeName = "logoLink")]
        public string LogoLink { get; set; }

        [XmlAttribute(AttributeName = "logoWrapText")]
        public string LogoWrapText { get; set; }

        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }

        [XmlAttribute(AttributeName = "keywords")]
        public string Keywords { get; set; }

        [XmlAttribute(AttributeName = "metaDescription")]
        public string MetaDescription { get; set; }

        [XmlAttribute(AttributeName = "isPrivate")]
        public string IsPrivate { get; set; }

        [XmlAttribute(AttributeName = "displayBio")]
        public string DisplayBio { get; set; }

        [XmlAttribute(AttributeName = "lastUpdated")]
        public string LastUpdated { get; set; }

        [XmlAttribute(AttributeName = "hasOptedOutOfMarketplace")]
        public string HasOptedOutOfMarketplace { get; set; }

        [XmlAttribute(AttributeName = "disabled")]
        public string Disabled { get; set; }

        [XmlAttribute(AttributeName = "paidTill")]
        public string PaidTill { get; set; }

        [XmlAttribute(AttributeName = "upgradeDate")]
        public string UpgradeDate { get; set; }

        [XmlAttribute(AttributeName = "headTag")]
        public string HeadTag { get; set; }

        [XmlAttribute(AttributeName = "bodyTag")]
        public string BodyTag { get; set; }

        [XmlAttribute(AttributeName = "header")]
        public string Header { get; set; }

        [XmlAttribute(AttributeName = "footer")]
        public string Footer { get; set; }

        [XmlAttribute(AttributeName = "showFrontPageListing")]
        public string ShowFrontPageListing { get; set; }

        [XmlAttribute(AttributeName = "showSubscribeForm")]
        public string ShowSubscribeForm { get; set; }

        [XmlAttribute(AttributeName = "extraSideBarContent")]
        public string ExtraSideBarContent { get; set; }

        [XmlAttribute(AttributeName = "showSideBar")]
        public string ShowSideBar { get; set; }

        [XmlAttribute(AttributeName = "showNestedSections")]
        public string ShowNestedSections { get; set; }

        [XmlAttribute(AttributeName = "fontFace")]
        public string FontFace { get; set; }

        [XmlAttribute(AttributeName = "textColor")]
        public string TextColor { get; set; }

        [XmlAttribute(AttributeName = "linkColor")]
        public string LinkColor { get; set; }

        [XmlAttribute(AttributeName = "activeLinkColor")]
        public string ActiveLinkColor { get; set; }

        [XmlAttribute(AttributeName = "visitedLinkColor")]
        public string VisitedLinkColor { get; set; }

        [XmlAttribute(AttributeName = "backgroundColor")]
        public string BackgroundColor { get; set; }

        [XmlAttribute(AttributeName = "checkoutBgColor")]
        public string CheckoutBgColor { get; set; }

        [XmlAttribute(AttributeName = "alertMessageColor")]
        public string AlertMessageColor { get; set; }

        [XmlAttribute(AttributeName = "headlineColor")]
        public string HeadlineColor { get; set; }

        [XmlAttribute(AttributeName = "thBgColor")]
        public string ThBgColor { get; set; }

        [XmlAttribute(AttributeName = "thFgColor")]
        public string ThFgColor { get; set; }

        [XmlAttribute(AttributeName = "tableFgColor")]
        public string TableFgColor { get; set; }

        [XmlAttribute(AttributeName = "tableBgColor")]
        public string TableBgColor { get; set; }

        [XmlAttribute(AttributeName = "tableFg2Color")]
        public string TableFg2Color { get; set; }

        [XmlAttribute(AttributeName = "tableBg2Color")]
        public string TableBg2Color { get; set; }

        [XmlAttribute(AttributeName = "imageBorderColor")]
        public string ImageBorderColor { get; set; }

        [XmlAttribute(AttributeName = "imageBorderWidth")]
        public string ImageBorderWidth { get; set; }

        [XmlAttribute(AttributeName = "sidebarTextColor")]
        public string SidebarTextColor { get; set; }

        [XmlAttribute(AttributeName = "sidebarBgColor")]
        public string SidebarBgColor { get; set; }

        [XmlAttribute(AttributeName = "prodImageColor")]
        public string ProdImageColor { get; set; }

        [XmlAttribute(AttributeName = "defaultSectionNo")]
        public string DefaultSectionNo { get; set; }

        [XmlAttribute(AttributeName = "billingOption")]
        public string BillingOption { get; set; }

        [XmlAttribute(AttributeName = "storeTypeNo")]
        public string StoreTypeNo { get; set; }

        [XmlAttribute(AttributeName = "fixedPricingCommissionPercentage")]
        public string FixedPricingCommissionPercentage { get; set; }
    }
}