using System;
using System.IO;
using NLog;
using VideoBarcodeGenerator.Core.Models.Settings;

namespace VideoBarcodeGenerator.Core.Helpers
{
    public static class Cache
    {
        /// <summary>
        /// Delete (recursively) and recreate the image cache directory
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="logger"></param>
        public static void ClearMagickImageCache(SettingsWrapper settings, Logger logger)
        {
            try
            {
                Directory.Delete(settings.CoreSettings.MagickImageTempDir, true);

                Directory.CreateDirectory(settings.CoreSettings.MagickImageTempDir);
            }
            catch (Exception e)
            {
                logger?.Error(e, "Failed attempting to clear ImageMagick cache");
            }
        }
    }
}