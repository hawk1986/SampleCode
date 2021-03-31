using System.Configuration;

namespace Utilities.Utility
{
    public static class Tools
    {
        /// <summary>
        /// 依鍵值 key 將 config 參數取出並轉型為 bool，失敗則取用 defaultValue
        /// </summary>
        /// <param name="key">鍵值</param>
        /// <param name="defaultValue">預設值</param>
        /// <returns></returns>
        public static bool GetConfigValue(string key, bool defaultValue)
        {
            bool result;
            string configValue = ConfigurationManager.AppSettings[key];

            return
                !string.IsNullOrEmpty(configValue) &&
                bool.TryParse(configValue, out result) ?
                result : defaultValue;
        }

        /// <summary>
        /// 依鍵值 key 將 config 參數取出並轉型為 int，失敗則取用 defaultValue
        /// </summary>
        /// <param name="key">鍵值</param>
        /// <param name="defaultValue">預設值</param>
        /// <returns></returns>
        public static int GetConfigValue(string key, int defaultValue)
        {
            int result;
            string configValue = ConfigurationManager.AppSettings[key];

            return
                !string.IsNullOrEmpty(configValue) &&
                int.TryParse(configValue, out result) ?
                result : defaultValue;
        }

        /// <summary>
        /// 依鍵值 key 將 config 參數取出，失敗則取用 defaultValue
        /// </summary>
        /// <param name="key">鍵值</param>
        /// <param name="defaultValue">預設值</param>
        /// <returns></returns>
        public static string GetConfigValue(string key, string defaultValue)
        {
            string configValue = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }

        /// <summary>
        /// 依鍵值 key 將 Connection String 參數取出，失敗則為 string.Empty
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[key];
            return connectionStringSettings != null ? connectionStringSettings.ConnectionString : string.Empty;
        }

        /// <summary>
        /// 轉換字串到byte陣列
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static byte[] ConvertStringToByteArray(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return null;
            string[] sAry = ids.Split(',');
            byte[] nAry = new byte[sAry.Length];
            //執行轉換，若有任一數無法轉換則傳回 null
            for (int i = 0; i < sAry.Length; i++)
                if (!byte.TryParse(sAry[i], out nAry[i])) return null;
            return nAry;
        }

        /// <summary>
        /// 轉換字串到int陣列
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static int[] ConvertStringToIntArray(string ids)
        {
            if (string.IsNullOrEmpty(ids)) return null;
            string[] sAry = ids.Split(',');
            int[] nAry = new int[sAry.Length];
            //執行轉換，若有任一數無法轉換則傳回 null
            for (int i = 0; i < sAry.Length; i++)
                if (!int.TryParse(sAry[i], out nAry[i])) return null;
            return nAry;
        }
    }
}