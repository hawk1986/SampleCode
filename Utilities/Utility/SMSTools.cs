using System.IO;
using System.Net;

namespace Utilities.Utility
{
    public class SMSTools
    {
        private readonly string baseUrl = Tools.GetConfigValue("SMSUrl", "http://103.250.30.4/SendSMS/sendmsg.php");

        private readonly string uname = Tools.GetConfigValue("SMSUser", "gorigo");

        private readonly string pass = Tools.GetConfigValue("SMSPassword", "s$4Zr)7U");

        private readonly string send = Tools.GetConfigValue("SMSSend", "GORIGO");

        public SMSTools()
        {

        }

        public bool Send(string dest, string msg)
        {
            var url = string.Format("{0}?uname={1}&pass={2}&send={3}&dest={4}&msg={5}", baseUrl, uname, pass, send, dest, msg);
            var result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (var response = request.GetResponse())
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    var responseString = sr.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(responseString))
                    {
                        result = true;
                    }
                }
            }
            catch { return false; }
            return result;
        }
    }
}
