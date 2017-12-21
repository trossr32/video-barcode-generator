using System;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Models.CafePress;
using NUnit.Framework;

namespace CafePress.Api.Tests
{
    [TestFixture]
    public class DesignMethodsTests
    {
        public SettingsWrapper SettingsWrapper;

        [SetUp]
        public void Init()
        {
            SettingsWrapper = Settings.GetSettings();
        }

        [Test]
        public void ListTest()
        {
            var designs = new DesignMethods(SettingsWrapper).List();
            var x = designs;

            //Assert.AreEqual("&a=a1&b=b1", parms);
        }

        [Test]
        public void FindTest()
        {
            try
            {
                var designs = new DesignMethods(SettingsWrapper).Find(0);
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
            var count = new DesignMethods(SettingsWrapper).Count();
            
            Assert.IsTrue(count >= 0);
        }
    }
}