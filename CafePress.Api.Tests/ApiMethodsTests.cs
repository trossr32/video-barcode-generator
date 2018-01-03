using System.Collections.Generic;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Helpers;
using FilmBarcodes.Common.Models.Settings;
using NUnit.Framework;

namespace CafePress.Api.Tests
{
    [TestFixture]
    public class ApiMethodsTests
    {
        private SettingsWrapper _settingsWrapper;

        [SetUp]
        public void Init()
        {
            _settingsWrapper = Settings.GetSettings();
        }

        [Test]
        public void StringifyParametersDictionary_Populated()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string> { { "a", "a1" }, { "b", "b1" } };

            Assert.AreEqual("&a=a1&b=b1", parameters.StringifyParametersDictionary());
        }

        [Test]
        public void StringifyParametersDictionary_Null()
        {
            Dictionary<string, string> parameters = null;

            Assert.AreEqual("", parameters.StringifyParametersDictionary());
        }

        [Test]
        public void StringifyParametersDictionary_Empty()
        {
            Assert.AreEqual("", new Dictionary<string, string>().StringifyParametersDictionary());
        }
    }
}