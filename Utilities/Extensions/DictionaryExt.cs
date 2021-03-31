using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Routing;

namespace Utilities.Extensions
{
    public static class DictionaryExt
    {
        /// <summary>
        /// 根據 key 取得 IDictionary 內的物件。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="key">鍵值</param>
        /// <returns></returns>
        public static T Get<T>(this IDictionary<string, object> instance, string key)
        {
            return instance.ContainsKey(key) ? (T)instance[key] : default(T);
        }

        /// <summary>
        /// 合併兩個 IDictionary 物件，key 存在會被取代。
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="from">IDictionary 物件</param>
        public static void Merge(this IDictionary<string, object> instance, IDictionary<string, object> from)
        {
            instance.Merge(from, true);
        }

        /// <summary>
        /// 合併兩個 IDictionary 物件，key 存在，由 replaceExisting 決定是否取代。
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="from">IDictionary 物件</param>
        /// <param name="replaceExisting">是否取代原有 key 的值</param>
        public static void Merge(this IDictionary<string, object> instance, IDictionary<string, object> from, bool replaceExisting)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance), "這個 IDictionary 目的實體不能為 null.");
            }

            if (from == null)
            {
                throw new ArgumentNullException(nameof(from), "這個 IDictionary 來源實體不能為 null.");
            }

            foreach (KeyValuePair<string, object> pair in from)
            {
                if (!replaceExisting && instance.ContainsKey(pair.Key))
                {
                    continue;
                }
                instance[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// 將物件合併至 IDictionary 物件，key 存在會被取代。
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="values">待合併物件</param>
        public static void Merge(this IDictionary<string, object> instance, object values)
        {
            instance.Merge(values, true);
        }

        /// <summary>
        /// 將物件合併至 IDictionary 物件，key 存在，由 replaceExisting 決定是否取代。
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="values">待合併物件</param>
        /// <param name="replaceExisting">是否取代原有 key 的值</param>
        public static void Merge(this IDictionary<string, object> instance, object values, bool replaceExisting)
        {
            instance.Merge(new RouteValueDictionary(values), replaceExisting);
        }

        /// <summary>
        /// 將物件 value 加入至 IDictionary 物件，key 存在，由 replaceExisting 決定是否取代。
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="key">鍵值</param>
        /// <param name="value">待合併物件</param>
        /// <param name="replaceExisting">是否取代原有 key 的值</param>
        public static void Merge(this IDictionary<string, object> instance, string key, object value, bool replaceExisting)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance), "這個 IDictionary 目的實體不能為 null.");
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "這個物件實體不能為 null.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "鍵值不能為 null 或 empty.");
            }

            if (replaceExisting || !instance.ContainsKey(key))
            {
                instance[key] = value;
            }
        }

        /// <summary>
        /// 根據 key 設定 IDictionary 內的物件。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="key">鍵值</param>
        /// <param name="value">待合併物件</param>
        public static void Set<T>(this IDictionary<string, object> instance, string key, T value)
        {
            instance[key] = value;
        }

        /// <summary>
        /// 將字典清單轉換成JSON字串
        /// </summary>
        /// <typeparam name="TKey">TKey</typeparam>
        /// <typeparam name="TValue">TValue</typeparam>
        /// <param name="source">source</param>
        /// <returns></returns>
        public static string ToJSON<TKey, TValue>(this Dictionary<TKey, TValue> source)
        {
            if (source.Count == 0)
            {
                return "[]";
            }
            StringBuilder sb = new StringBuilder(512);
            sb.Append("[");
            foreach (var item in source)
            {
                sb.Append("{\"value\":\"").Append(item.Key).Append("\",\"text\":\"").Append(item.Value).Append("\"},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// 將字典清單轉換成JSON陣列
        /// </summary>
        /// <param name="source">source</param>
        /// <returns></returns>
        public static string ToArrayString(this Dictionary<string, int> source)
        {
            //[["01", 10], ["02", 8], ["03", 4], ["04", 13], ["05", 17], ["06", 9]]
            StringBuilder sb = new StringBuilder(512);
            sb.Append("[");
            if (source.Count > 0)
            {
                foreach (var item in source)
                {
                    sb.Append("[\"");
                    sb.Append(item.Key);
                    sb.Append("\",");
                    sb.Append(item.Value);
                    sb.Append("],");
                }
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// 將字典清單轉換成JSON陣列
        /// </summary>
        /// <param name="source">source</param>
        /// <returns></returns>
        public static string ToArrayString(this Dictionary<string, float> source)
        {
            //[["01", 10], ["02", 8], ["03", 4], ["04", 13], ["05", 17], ["06", 9]]
            StringBuilder sb = new StringBuilder(512);
            sb.Append("[");
            if (source.Count > 0)
            {
                foreach (var item in source)
                {
                    sb.Append("[\"");
                    sb.Append(item.Key);
                    sb.Append("\",");
                    sb.Append(item.Value.ToString("F2"));
                    sb.Append("],");
                }
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// 將字典清單轉換成JSON陣列
        /// </summary>
        /// <param name="source">source</param>
        /// <returns></returns>
        public static string ToArrayString(this Dictionary<string, double> source)
        {
            //[["01", 10], ["02", 8], ["03", 4], ["04", 13], ["05", 17], ["06", 9]]
            StringBuilder sb = new StringBuilder(512);
            sb.Append("[");
            if (source.Count > 0)
            {
                foreach (var item in source)
                {
                    sb.Append("[\"");
                    sb.Append(item.Key);
                    sb.Append("\",");
                    sb.Append(item.Value.ToString("F2"));
                    sb.Append("],");
                }
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");

            return sb.ToString();
        }
    }
}
