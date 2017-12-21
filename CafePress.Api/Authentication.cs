using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Helpers;
using FilmBarcodes.Common.Models.CafePress;

namespace CafePress.Api
{
    public class Authentication
    {
        private SettingsWrapper _settingsWrapper;
        private ApiMethods ApiMethods { get; set; }

        public Authentication(SettingsWrapper settingsWrapper)
        {
            _settingsWrapper = settingsWrapper;
        }

        //public void Authenticate()
        //{
        //    if (!IsValidToken())
        //        GetUserToken();

        //    if (!IsValidToken())
        //        throw new ArgumentNullException($"SettingsWrapper.AuthenticationToken.Token", "The authentication token is null or empty, the API call to CafePress must have failed.");
        //}

        //private bool IsValidToken()
        //{
        //    if (string.IsNullOrEmpty(SettingsWrapper?.AuthenticationToken?.Token))
        //        return false;

        //    try
        //    {
        //        var resp = ApiMethods.CallApiString("authentication.isValid.cp", new Dictionary<string, string> {{"testUserToken", SettingsWrapper.AuthenticationToken.Token}});

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public SettingsWrapper GetUserToken()
        {
            var parameters = new Dictionary<string, string>
            {
                {"email", _settingsWrapper.CafePress.Credentials.Email},
                {"password", _settingsWrapper.CafePress.Credentials.Password}
            };

            try
            {
                using (WebClient wc = new WebClient())
                {
                    var url = $"{_settingsWrapper.CafePress.ApiUrl}authentication.getUserToken.cp?v=3&appKey={_settingsWrapper.CafePress.AppKey}{parameters.StringifyParametersDictionary()}";

                    var resp = wc.DownloadString(url);

                    using (StringReader reader = new StringReader(resp))
                    {
                        _settingsWrapper.CafePress.AuthenticationToken = new AuthenticationToken
                        {
                            Token = ((Value)new XmlSerializer(typeof(Value)).Deserialize(reader)).Text,
                            Created = DateTime.Now
                        };
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }

            Settings.SetSettings(_settingsWrapper);

            return _settingsWrapper;
        }
    }
}