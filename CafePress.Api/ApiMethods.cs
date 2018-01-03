﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Helpers;
using FilmBarcodes.Common.Models.CafePress;
using FilmBarcodes.Common.Models.Settings;
using NLog;

namespace CafePress.Api
{
    public class ApiMethods
    {
        private Logger _logger;
        private SettingsWrapper _settingsWrapper;

        public ApiMethods(SettingsWrapper settingsWrapper)
        {
            _settingsWrapper = settingsWrapper;

            _logger = LogManager.GetCurrentClassLogger();
        }

        public string CallApiString(string method, Dictionary<string, string> parameters = null, bool requiresUserToken = false)
        {
            return DoRequest(ApiCallType.DownloadString, method, parameters, requiresUserToken);
        }

        public void Post(string method, Dictionary<string, string> parameters = null, string xml = "", bool requiresUserToken = false, string xmlKey = "value")
        {
            DoRequest(ApiCallType.UploadString, method, parameters, requiresUserToken, xmlKey, xml);
        }

        private string DoRequest(ApiCallType apiCallType, string method, Dictionary<string, string> parameters, bool requiresUserToken, string xmlKey = "", string xml = "")
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();

            if (requiresUserToken)
                parameters.Add("userToken", _settingsWrapper?.CafePress?.AuthenticationToken?.Token);

            if (apiCallType == ApiCallType.UploadString)
                parameters.Add(xmlKey, xml);

            Help help = null;
            string resp = null;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    switch (apiCallType)
                    {
                        case ApiCallType.DownloadString:
                            resp = wc.DownloadString(BuildUrl(parameters, method));
                            break;
                        case ApiCallType.UploadString:
                            resp = wc.UploadString(BuildUrl(parameters, method), "");
                            break;
                    }
                    
                    help = CheckResponse(resp);
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError && e.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.BadRequest)
                {
                    resp = new StreamReader(e.Response.GetResponseStream() ?? throw e).ReadToEnd();

                    help = CheckResponse(resp);
                }
                else
                {
                    _logger.Error(e, "");

                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "");

                throw;
            }

            if (help != null)
            {
                if (help.Exceptionmessage.ToLower().Contains("token") && help.Exceptionmessage.ToLower().Contains("invalid"))
                {
                    _settingsWrapper = new Authentication(_settingsWrapper).GetUserToken();

                    parameters["userToken"] = _settingsWrapper.CafePress.AuthenticationToken.Token;

                    using (WebClient wc = new WebClient())
                    {
                        switch (apiCallType)
                        {
                            case ApiCallType.DownloadString:
                                resp = wc.DownloadString(BuildUrl(parameters, method));
                                break;
                            case ApiCallType.UploadString:
                                resp = wc.UploadString(BuildUrl(parameters, method), "");
                                break;
                        }
                    }

                    help = CheckResponse(resp);

                    if (help != null)
                    {
                        var e = new Exception($"Unable to authenticate user, response form API is: {help.Exceptionmessage}");

                        _logger.Error(e);

                        throw e;
                    }
                }
                else
                {
                    var e = new Exception($"Invalid response from API call: {help.Exceptionmessage}");

                    _logger.Error(e);

                    throw e;
                }
            }

            return resp;
        }

        private Help CheckResponse(string resp)
        {
            Help help = null;

            try
            {
                using (StringReader reader = new StringReader(resp))
                {
                    help = (Help)new XmlSerializer(typeof(Help)).Deserialize(reader);
                }
            }
            catch (Exception)
            {
                return help;
            }

            return help;
        }

        private string BuildUrl(Dictionary<string, string> parameters, string method)
        {
            var uri = $"{_settingsWrapper.CafePress.ApiUrl}{method}?v=3&appKey={_settingsWrapper.CafePress.AppKey}{parameters.StringifyParametersDictionary()}";

            _logger.Debug($"calling CafePress API: {uri}");

            return uri;
        }
    }
}