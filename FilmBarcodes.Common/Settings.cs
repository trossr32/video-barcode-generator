using System;
using System.IO;
using FilmBarcodes.Common.Models.CafePress;
using Newtonsoft.Json;

namespace FilmBarcodes.Common
{
    public class Settings
    {
        private static readonly string SettingsDir = Path.Combine(Environment.SpecialFolder.LocalApplicationData.ToString(), "Film Barcodes");
        private static readonly string SettingsFile = Path.Combine(SettingsDir, "settings.json");

        public static SettingsWrapper GetSettings()
        {
            var settings = new InMemoryCache().GetOrSet("Settings", GetSettingsFromFileOrApi);

            return settings;
        }

        private static SettingsWrapper GetSettingsFromFileOrApi()
        {
            Directory.CreateDirectory(SettingsDir);

            if (File.Exists(SettingsFile))
            {
                try
                {
                    return JsonConvert.DeserializeObject<SettingsWrapper>(File.ReadAllText(SettingsFile));
                }
                catch (Exception)
                {
                    // on failure we want to continue as the settings file format may have changed
                }
            }

            var settings = new SettingsWrapper();

            SetSettings(settings);

            return settings;
        }

        public static void SetSettings(SettingsWrapper settings)
        {
            Directory.CreateDirectory(SettingsDir);

            File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(settings));
        }
    }
}