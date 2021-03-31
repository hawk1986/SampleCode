using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Utilities.Utility
{
    public class WowzaApi
    {
        readonly string _apiUrlBase;
        readonly string _rtspUrl;
        readonly int _timeout = 5;

        /// <summary>
        /// WowzaApi Ctor
        /// </summary>
        public WowzaApi()
        {
            _apiUrlBase = Tools.GetConfigValue("StreamServerApiBase", "http://180.179.207.45:8087/v2/servers/_defaultServer_/vhosts/_defaultVHost_/applications");
            _rtspUrl = Tools.GetConfigValue("RTSPUrl", "rtsp://180.179.207.45:1935");
        }

        /// <summary>
        /// WowzaApi Ctor
        /// </summary>
        /// <param name="baseUrl"></param>
        public WowzaApi(string baseUrl, string rtspUrl)
        {
            _apiUrlBase = baseUrl;
            _rtspUrl = rtspUrl;
        }

        /// <summary>
        /// Create a Application
        /// </summary>
        /// <param name="organizationCode"></param>
        public bool CreateApplication(string organizationCode)
        {
            var result = false;
            organizationCode = organizationCode.Trim().ToLower();
            var restURI = _apiUrlBase + "/" + organizationCode;
            var request = (HttpWebRequest)WebRequest.Create(restURI);
            request.Accept = "application/json; charset=utf-8";
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "POST";
            request.Timeout = _timeout * 1000;
            try
            {
                var jsonObject = new
                {
                    restURI = restURI,
                    name = organizationCode,
                    appType = "Live",
                    clientStreamReadAccess = "*",
                    clientStreamWriteAccess = "*",
                    description = "",
                    streamConfig = new
                    {
                        restURI = restURI + "/streamconfiguration",
                        streamType = "live",
                        createStorageDir = true,
                        storageDirExists = true,
                        storageDir = "${com.wowza.wms.context.VHostConfigHome}/content/" + organizationCode
                    },
                    securityConfig = new
                    {
                        restURI = restURI + "/security",
                        secureTokenVersion = 0,
                        clientStreamWriteAccess = "*",
                        publishRequirePassword = true,
                        publishPasswordFile = "",
                        publishRTMPSecureURL = "",
                        publishIPBlackList = "",
                        publishIPWhiteList = "",
                        publishBlockDuplicateStreamNames = false,
                        publishValidEncoders = "",
                        publishAuthenticationMethod = "digest",
                        playMaximumConnections = 0,
                        playRequireSecureConnection = false,
                        secureTokenSharedSecret = "",
                        secureTokenUseTEAForRTMP = false,
                        secureTokenIncludeClientIPInHash = false,
                        secureTokenHashAlgorithm = "",
                        secureTokenQueryParametersPrefix = "",
                        secureTokenOriginSharedSecret = "",
                        playIPBlackList = "",
                        playIPWhiteList = "",
                        playAuthenticationMethod = "none"
                    }
                };
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var json = JsonConvert.SerializeObject(jsonObject);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var responseString = reader.ReadToEnd();
                var responseData = JsonConvert.DeserializeObject<ApiResponse>(responseString);
                if (null != responseData &&
                    responseData.Success)
                {
                    result = true;
                }
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                log(ex.Message);
                throw;
            }

            return result;
        }

        public bool CreateStreamFile(string organizationCode, string orderNo)
        {
            var result = false;
            organizationCode = organizationCode.Trim().ToLower();
            orderNo = orderNo.Trim().ToLower();
            var restURI = _apiUrlBase + "/" + organizationCode + "/streamfiles";
            var request = (HttpWebRequest)WebRequest.Create(string.Concat(restURI, "/", orderNo));
            request.Accept = "application/json; charset=utf-8";
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "POST";
            request.Timeout = _timeout * 1000;
            try
            {
                var id = "connectAppName=" + organizationCode + "&appInstance=_definst_&mediaCasterType=rtp";
                var jsonObject = new
                {
                    restURI = restURI,
                    streamFiles = new
                    {
                        id = id,
                        href = restURI + "/" + id
                    }
                };
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var json = JsonConvert.SerializeObject(jsonObject);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var responseString = reader.ReadToEnd();
                var responseData = JsonConvert.DeserializeObject<ApiResponse>(responseString);
                if (null != responseData &&
                    responseData.Success)
                {
                    reader.Close();
                    response.Close();
                    // update stream file
                    var date = new DateTime(1970, 1, 1, 0, 0, 0);
                    var timeBase = date.Ticks;
                    var startLong = (DateTime.UtcNow.Ticks - timeBase) / 10000;

                    var updateUrl = string.Concat(restURI, "/", orderNo, "/adv");
                    request = (HttpWebRequest)WebRequest.Create(updateUrl);
                    request.Accept = "application/json; charset=utf-8";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Method = "PUT";
                    var updateData = new
                    {
                        restURI = updateUrl,
                        version = startLong.ToString(),
                        advancedSettings = new[] {
                            new
                            {
                                enabled = true,
                                canRemove = true,
                                name = "uri",
                                value = _rtspUrl,
                                defaultValue = "",
                                type = "String",
                                sectionName = "Common",
                                documented = true
                            },
                            new
                            {
                                enabled = false,
                                canRemove = true,
                                name = "streamTimeout",
                                value = "",
                                defaultValue = "12000",
                                type = "Integer",
                                sectionName = "Common",
                                documented = true
                            },
                            new
                            {
                                enabled = false,
                                canRemove = true,
                                name = "reconnectWaitTime",
                                value = "",
                                defaultValue = "3000",
                                type = "Integer",
                                sectionName = "Common",
                                documented = true
                            }
                        }
                    };
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        var json = JsonConvert.SerializeObject(updateData);

                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    response = request.GetResponse();
                    reader = new StreamReader(response.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    responseData = JsonConvert.DeserializeObject<ApiResponse>(responseString);
                    if (null != responseData &&
                        responseData.Success)
                    {
                        result = true;
                    }
                    reader.Close();
                    response.Close();
                }
                else
                {
                    reader.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                log(ex.Message);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Delete Application
        /// </summary>
        /// <param name="organizationCode"></param>
        /// <returns></returns>
        public bool DeleteApplication(string organizationCode)
        {
            var result = false;
            organizationCode = organizationCode.Trim().ToLower();
            var request = (HttpWebRequest)WebRequest.Create(string.Concat(_apiUrlBase, "/", organizationCode));
            request.Accept = "application/json; charset=utf-8";
            request.Method = "DELETE";
            request.Timeout = _timeout * 1000;
            try
            {
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var responseString = reader.ReadToEnd();
                var responseData = JsonConvert.DeserializeObject<ApiResponse>(responseString);
                if (null != responseData && responseData.Success)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log(ex.Message);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Delete Stream File
        /// </summary>
        /// <param name="organizationCode"></param>
        /// <returns></returns>
        public bool DeleteStreamFile(string organizationCode, string orderNo)
        {
            var result = false;
            organizationCode = organizationCode.Trim().ToLower();
            orderNo = orderNo.Trim().ToLower();
            var request = (HttpWebRequest)WebRequest.Create(string.Concat(_apiUrlBase, "/", organizationCode, "/streamfiles/", orderNo));
            request.Accept = "application/json; charset=utf-8";
            request.Method = "DELETE";
            request.Timeout = _timeout * 1000;
            try
            {
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var responseString = reader.ReadToEnd();
                var responseData = JsonConvert.DeserializeObject<ApiResponse>(responseString);
                if (null != responseData && responseData.Success)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log(ex.Message);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Check Application is exist
        /// </summary>
        /// <param name="organizationCode"></param>
        /// <returns></returns>
        public bool IsApplicationExist(string organizationCode)
        {
            var result = false;
            organizationCode = organizationCode.Trim().ToLower();
            var request = (HttpWebRequest)WebRequest.Create(_apiUrlBase);
            request.Accept = "application/json; charset=utf-8";
            request.Method = "GET";
            request.Timeout = _timeout * 1000;
            try
            {
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var responseString = reader.ReadToEnd();
                var responseData = JsonConvert.DeserializeObject<WowzaApiApplication>(responseString);
                if (null != responseData &&
                    null != responseData.Applications &&
                    responseData.Applications.Any(x => x.ID.Trim().ToLower() == organizationCode))
                {
                    result = true;
                }
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                log(ex.Message);
                throw;
            }

            return result;
        }

        /// <summary>
        /// Check Stream File in Application
        /// </summary>
        /// <param name="organizationCode"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool IsStreamFileExist(string organizationCode, string orderNo)
        {
            var result = false;
            organizationCode = organizationCode.Trim().ToLower();
            orderNo = orderNo.Trim().ToLower();
            var url = string.Concat(_apiUrlBase, "/", organizationCode, "/streamfiles");
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "application/json; charset=utf-8";
            request.Method = "GET";
            request.Timeout = _timeout * 1000;
            try
            {
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                var responseString = reader.ReadToEnd();
                var responseData = JsonConvert.DeserializeObject<WowzaApiStreamFile>(responseString);
                if (null != responseData &&
                    null != responseData.StreamFiles &&
                    responseData.StreamFiles.Any(x => x.ID.Trim().ToLower() == orderNo))
                {
                    result = true;
                }
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                log(ex.Message);
                throw;
            }

            return result;
        }

        private void log(string msg)
        {
            var temp = Path.Combine(HttpRuntime.AppDomainAppPath, "Content\\Uploads\\Log");
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }
            var date = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
            File.AppendAllText(Path.Combine(temp, "wowzalog.txt"), date + ": " + msg + Environment.NewLine);
        }
    }
}
