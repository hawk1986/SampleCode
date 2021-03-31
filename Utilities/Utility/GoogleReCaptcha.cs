using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

namespace Utilities.Utility
{
    public class GoogleReCaptcha
    {
        /// <summary>
        /// 結果
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 驗證
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool GetCaptchaResponse(string message)
        {
            string url = Tools.GetConfigValue("ReCaptchaUrl", string.Empty);
            string key = Tools.GetConfigValue("ReCaptchaServerKey", string.Empty);
            if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
                return false;

            try
            {
                using (var client = new HttpClient())
                {
                    var pars = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("secret", key),
                        new KeyValuePair<string, string>("response", message),
                    });

                    var result = client.PostAsync(url, pars).Result;
                    var content = result.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<GoogleReCaptcha>(content);
                    return data.Success;
                }
            }
            catch { return false; }
        }
    }
}