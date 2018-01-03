using System.Collections.Generic;
using FilmBarcodes.Common.Models.Settings;

namespace CafePress.Api
{
    public class ImageMethods
    {
        private SettingsWrapper SettingsWrapper { get; set; }
        private ApiMethods ApiMethods { get; set; }

        public ImageMethods(SettingsWrapper settingsWrapper)
        {
            SettingsWrapper = settingsWrapper;

            ApiMethods = new ApiMethods(SettingsWrapper);
        }

        /// <summary>
        /// vv
        /// </summary>
        public void Upload(string folder, List<string> images)
        {
            var parameters = new Dictionary<string, string> {{"folder", folder}};

            for (var i = 0; i < images.Count; i++)
            {
                parameters.Add($"cpFile{i+1}", images[i]);
            }

            ApiMethods.CallApiString("image.upload.cp", parameters, true);
        }
    }
}