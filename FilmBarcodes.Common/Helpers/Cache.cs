using System;
using System.IO;
using FilmBarcodes.Common.Models.Settings;
using NLog;

namespace FilmBarcodes.Common.Helpers
{
    public static class Cache
    {
        public static void ClearMagickImageCache(SettingsWrapper settings, Logger logger)
        {
            try
            {
                ClearMagickImageCache(settings.BarcodeManager.MagickImageTempDir);

                Directory.CreateDirectory(settings.BarcodeManager.MagickImageTempDir);
            }
            catch (Exception e)
            {
                logger.Error(e, "Failed attempting to clear ImageMagick cache");
            }
        }

        public static void ClearMagickImageCache(string dir)
        {
            Directory.Delete(dir, true);
        }
    }
}