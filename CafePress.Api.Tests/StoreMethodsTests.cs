using System;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;
using NUnit.Framework;

namespace CafePress.Api.Tests
{
    [TestFixture]
    public class StoreMethodsTests
    {
        public SettingsWrapper SettingsWrapper;
        public StoreMethods StoreMethods;
        public string StoreId;

        [SetUp]
        public void Init()
        {
            SettingsWrapper = Settings.GetSettings();

            StoreMethods = new StoreMethods(SettingsWrapper);

            StoreId = Guid.NewGuid().ToString();
        }

        [Test, Order(1)]
        public void IsStoreIdTaken_Test()
        {
            var isStoreIdTaken = StoreMethods.IsStoreIdTaken(StoreId);

            Assert.IsFalse(isStoreIdTaken);
        }

        [Test, Order(2)]
        public void CreateTest()
        {
            Store store = new Store
            {
                Id = StoreId,
                Name = "Test Store",
                Title = "Test store",
                Description = "Test store",
                //StoreNo = "34115349",
                //MemberNo = "23763971"
                IsPremiumStore = "True",
                IsSelfBuy = "false",
                //HomePage = "" 
                DefaultProductImageId = "0",
                DefaultProductMarkup = "0.2",
                DefaultProductMarkupIsAbsolute = "False",
                LogoType = " ",
                //LogoLink = "",
                LogoWrapText = "0",
                //Keywords = "",
                //MetaDescription = "",
                IsPrivate = "true",
                DisplayBio = "false",
                //LastUpdated = "2018-01-02T06:52:27.54",
                HasOptedOutOfMarketplace = "true",
                Disabled = "false",
                //PaidTill = "2018-02-01T00:00:00",
                //UpgradeDate = "2018-01-02T06:52:00",
                HeadTag = "<style>body{background-color:#ffffff;}# cpWrapper{padding:0;background-image:url('http://content3.cpcache.com/si/themes/simple_c/simple_pattern.png');border-bottom: 1px solid #1A1A1A;width:1000px;margin:auto;}# shopContainer{width:960px;background-color:#ffffff;margin:0px auto;}# shopBorder{border:0;background-color:#dfdede;border-top:0;}#shopWrapper{background-color:#ffffff;border:0;padding:30px;}#shopHeader{background-color:#ffffff;}.bodyText a{text-decoration:none;}.center{margin:auto;width:50%;}.smallsidebartext a{font-size: 12px;text-decoration:none;}# sidebarContent {padding: 85px 5px 5px;position: relative;}#sidebarContent table {width:auto;}#sidebarContentTD {min-width:200px;}#shopCollection{font-family:Georgia,\"Times New Roman\",Times,serif;font-size:15px;font-weight:bold;font-color:#006c67;left: 7px;position: absolute;top: 60px;}.promoBox{margin-top:50px;}.sidebarbg {background-color:#ffffff;border-right: 1px dashed #b5b7b7;}#shopName{font-family:Georgia,Times,serif;font-size:64px;color:#636363;padding:30px 10px;}#colorBar{height:16px;}#topNav{background-color:#ffffff;font-family:\"Times New Roman\",Times,serif;font-size:16px;font-weight:bold;color:#ffffff;text-align:center;width:910px;margin:auto auto 25px;border-top: 1px solid #636363;border-bottom: 1px solid #636363;padding:0 10px;}#topNav ul, #topNav ul li{margin:0;padding:0;list-style:none;float:left;}#topNav ul{width:910px;}#topNav ul li a{display:block;padding:15px 27px;line-height:100%;color:#636363;text-decoration:none;}#searchFormContainer{width:206px;float:right;left: -16px;position: absolute;top: 0;}#submitSearch{float:right;}#searchTerm{float:left;width:125px;height:16px;padding:3px;}</style>",
                //BodyTag = "",
                Header = "<div id=\"shopContainer\"><div id=\"shopHeader\"><div id=\"shopName\"><cpstore:name></div></div> <div id=\"topNav\"><ul><li><a href=\"http://www.cafepress.com/<cpstore:id>\">Home</a></li><li><a href=\"http://www.cafepress.com/<cpstore:id>/s__t-shirts-clothing\">T-Shirts & amp; Clothing</a></li><li><a href=\"http://www.cafepress.com/<cpstore:id>/s__mugs\"> Drinkware </a></li><li><a href=\"http://www.cafepress.com/<cpstore:id>/s__art-posters\">Posters &amp; Art</a></li><li><a href=\"http://www.cafepress.com/<cpstore:id>/s__stickers-flair\">Stickers & amp; Flair</a></li><li><a href=\"http://www.cafepress.com/<cpstore:id>/s__home-decor\" class=\"last\">Home Decor</a></li> </ul> <div class=\"clear\">&nbsp;</div></div><div id=\"shopWrapper\">",
                Footer = "</div><!--closing shop wrapper --></div><!-- closing shop container -->",
                ShowFrontPageListing = "true",
                ShowSubscribeForm = "false",
                ExtraSideBarContent = "<div id=\"searchFormContainer\"><form action=\"/cp/storesearchresults.aspx\" id=\"searchForm\" name=\"searchForm\"><input type=\"text\" name=\"searchterm\" id=\"searchTerm\" /><input type=\"hidden\" name=\"s\" id=\"s\" value=\"<cpstore:id>\" /><input type=\"submit\" id=\"submitSearch\" value=\"Search\" /></form></div><div id=\"shopCollection\"> Shop by Collection</div><div class=\"promoBox\"><iframe src=\"http://www.cafepress.com/content/si/promo/125x125.html\" mce_src=\"http://www.cafepress.com/content/si/promo/125x125.html\" marginwidth=\"0\" marginheight=\"0\" frameborder=\"0\" height=\"125\"scrolling=\"no\" width=\"125\"></iframe></div>",
                ShowSideBar = "true",
                ShowNestedSections = "false",
                FontFace = "Verdana,Arial,Geneva,Helvetica,sans-serif",
                TextColor = "636363",
                LinkColor = "a1a1a1",
                ActiveLinkColor = "a1a1a1",
                VisitedLinkColor = "a1a1a1",
                //BackgroundColor ="",
                //CheckoutBgColor ="",
                //AlertMessageColor ="",
                HeadlineColor = "000000",
                //ThBgColor = "",
                ThFgColor = "000000",
                //TableFgColor = "",
                //TableBgColor = "",
                //TableFg2Color = "",
                //TableBg2Color = "",
                //ImageBorderColor = "",
                ImageBorderWidth = "0",
                //SidebarTextColor = "",
                //SidebarBgColor = "",
                //ProdImageColor = "",
                DefaultSectionNo = "0",
                //BillingOption = "0",
                StoreTypeNo = "1",
                FixedPricingCommissionPercentage = "0",
                //StoreLogoImage = new StoreLogoImage {ImageSrc = "http:///nocache/9/34115349."}
            };

            StoreMethods.SaveStore(store);

            //Assert.AreEqual("&a=a1&b=b1", parms);
        }

        [Test, Order(3)]
        public void ListTest()
        {
            var stores = StoreMethods.ListStores();
            var x = stores;

            //Assert.AreEqual("&a=a1&b=b1", parms);
        }

        //[Test, Order(4)]
        //public void FindTest()
        //{
        //    try
        //    {
        //        var designs = StoreMethods.FindByStoreId("");
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.AreEqual(e.Message, "Invalid response from API call: No design with id 0 was found");
        //    }

        //    //Assert.AreEqual("&a=a1&b=b1", parms);
        //}

        //[Test, Order(5)]
        //public void CountTest()
        //{
        //    var count = StoreMethods.CountStoreSections("");
            
        //    Assert.IsTrue(count >= 0);
        //}
    }
}