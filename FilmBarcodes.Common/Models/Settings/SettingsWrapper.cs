using System;
using System.Collections.Generic;

namespace FilmBarcodes.Common.Models.Settings
{
    public class SettingsWrapper
    {
        private readonly string DefaultOutputDirectory = @"Q:\Film Barcodes";

        public SettingsWrapper()
        {
            BarcodeManager = new BarcodeManager
            {
                OutputDirectory = DefaultOutputDirectory
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
        public string OutputDirectory { get; set; } = @"Q:\Film Barcodes";

        public string DefaultSource { get; set; } = @"M:\movies";

        public List<string> AcceptedVideoFiles => new List<string> { "avi", "divx", "m4v", "mkv", "m2ts", "mp4", "mpg", "wmv" };

        public int NumberOfConcurrentTasks => 1;
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