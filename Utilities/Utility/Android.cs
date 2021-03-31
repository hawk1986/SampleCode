using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace Utilities.Utility
{
    public static class Android
    {
        public static string PushNotification(PushMessage message)
        {
            var result = string.Empty;
            var JsonString = JsonConvert.SerializeObject(message);
            var JsonBytes = Encoding.UTF8.GetBytes(JsonString);

            var pushUrl = Tools.GetConfigValue("PushUrl", "https://android.googleapis.com/gcm/send");
            var apiKey = Tools.GetConfigValue("ApiKey", "AIzaSyDScyq9Wz7iN-47vwpQgh7HFI6W0B8c5aY");
            var request = WebRequest.Create(pushUrl);
            request.Method = "POST";
            request.Headers[HttpRequestHeader.Authorization] = "key=" + apiKey;
            request.ContentType = @"application/json";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentLength = JsonBytes.Length;
            var stream = request.GetRequestStream();
            stream.Write(JsonBytes, 0, JsonBytes.Length);
            stream.Close();

            try
            {
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {
                result = "Push Request Error!";
            }

            return result;
        }
    }
}
