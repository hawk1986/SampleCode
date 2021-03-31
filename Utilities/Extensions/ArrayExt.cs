using System;

namespace Utilities.Extensions
{
    public static class ArrayExt
    {
        /// <summary>
        /// 檢查陣列是否為空，如果陣列為 null 則會拋出 ArgumentNullException
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this T[] instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance), "這個陣列實體不能為 null.");
            }

            return instance.Length == 0;
        }

        /// <summary>
        /// 檢查陣列是否為空，或是 null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] instance)
        {
            return (instance == null) || (instance.Length == 0);
        }
    }
}
