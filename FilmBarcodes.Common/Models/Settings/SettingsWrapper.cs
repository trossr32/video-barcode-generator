using System;
using System.Collections.Generic;

namespace FilmBarcodes.Common.Models.Settings
{
    public class SettingsWrapper
    {
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

        public string OutputDirectory
        {
            get => string.IsNullOrEmpty(_outputDirectory)
                ? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                : _outputDirectory;
            set => _outputDirectory = value;
        }

        public List<string> AcceptedVideoFiles => new List<string> { "avi", "divx", "m4v", "mkv", "m2ts", "mp4", "mpg", "wmv" };
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