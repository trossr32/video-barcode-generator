using System;

namespace FilmBarcodes.Common.Models.CafePress
{
    public class SettingsWrapper
    {
        //private const string ApiUrl = "http://open-api.cafepress.com/";
        //private const string AppKey = "c3cwruiq405i2or26foviseoyrhivlaxaxbi";
        //private const string Email = "rob@200sx.org.uk";
        //private const string Password = "zmszj6OP27cA";

        public SettingsWrapper()
        {
            BarcodeManager = new BarcodeManager
            {
                OutputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            };

            CafePress = new CafePress
            {
                ApiUrl = "http://open-api.cafepress.com/",
                AppKey = "c3cwruiq405i2or26foviseoyrhivlaxaxbi",
                Credentials = new Credentials
                {
                    Email = "rob@200sx.org.uk",
                    Password = "zmszj6OP27cA"
                }
            };
        }
        
        public BarcodeManager BarcodeManager { get; set; }

        public CafePress CafePress { get; set; }
    }

    public class BarcodeManager
    {
        private string _outputDirectory;

        public string OutputDirectory {
            get => string.IsNullOrEmpty(_outputDirectory)
                ? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                : _outputDirectory;
            set => _outputDirectory = value;
        }
    }

    public class CafePress
    {
        public string ApiUrl { get; set; }
        public string AppKey { get; set; }

        public Credentials Credentials { get; set; }
        public AuthenticationToken AuthenticationToken { get; set; }
    }

    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticationToken
    {
        public string Token { get; set; }
        public DateTime Created { get; set; }
    }
}