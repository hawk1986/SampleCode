using System;
using System.Collections.Generic;

namespace Utilities.Extensions
{
    public static class CollectionExt
    {
        /// <summary>
        /// 將 IEnumerable 中的物件加入到 ICollection 中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="collection">要加入 ICollection 的 IEnumerable 物件</param>
        public static void AddRange<T>(this ICollection<T> instance, IEnumerable<T> collection)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance), "這個 ICollection 實體不能為 null.");
            }

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection), "這個 IEnumerable 實體不能為 null.");
            }

            foreach (T item in collection)
            {
                instance.Add(item);
            }
        }

        /// <summary>
        /// 檢查 ICollection 是否為空，如果 ICollection 為 null 則會拋出 ArgumentNullException
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this ICollection<T> instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance), "這個 ICollection 實體不能為 null.");
            }

            return instance.Count == 0;
        }

        /// <summary>
        /// 檢查 ICollection 是否為空，或是 null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> instance)
        {
            return (instance == null) || (instance.Count == 0);
        }
    }
}
