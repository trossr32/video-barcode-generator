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

        [SetUp]
        public void Init()
        {
            SettingsWrapper = Settings.GetSettings();

            StoreMethods = new StoreMethods(SettingsWrapper);
        }

        [Test]
        public void ListTest()
        {
            var stores = StoreMethods.ListStores();
            var x = stores;

            //Assert.AreEqual("&a=a1&b=b1", parms);
        }

        [Test]
        public void FindTest()
        {
            try
            {
                var designs = StoreMethods.FindByStoreId("");
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Invalid response from API call: No design with id 0 was found");
            }

            //Assert.AreEqual("&a=a1&b=b1", parms);
        }

        [Test]
        public void CountTest()
        {
            var count = StoreMethods.CountStoreSections("");
            
            Assert.IsTrue(count >= 0);
        }
    }
}