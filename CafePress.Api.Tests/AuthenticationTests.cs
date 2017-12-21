using FilmBarcodes.Common;
using NUnit.Framework;

namespace CafePress.Api.Tests
{
    [TestFixture]
    public class AuthenticationTests
    {
        [Test]
        public void AuthenticationTest()
        {
            var settingsWrapper = Settings.GetSettings();

            var auth = new Authentication(settingsWrapper);

            settingsWrapper = auth.GetUserToken();
            
            Assert.IsNotNull(settingsWrapper?.CafePress?.AuthenticationToken?.Token);
        }
    }
}