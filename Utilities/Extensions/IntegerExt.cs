using System.Collections.Generic;

namespace Utilities.Extensions
{
    public static class IntegerExt
    {
        /// <summary>
        /// 位元是否啟用
        /// </summary>
        /// <param name="b"></param>
        /// <param name="addSelf"></param>
        /// <returns></returns>
        public static List<int> IsBitEnable(this int b, bool addSelf = false)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < 16; ++i)
            {
                int testValue = 1 << i;
                if ((b & testValue) != 0)
                    result.Add(testValue);
            }

            if (addSelf && !result.Contains(b))
                result.Add(b);
            return result;
        }
    }
}