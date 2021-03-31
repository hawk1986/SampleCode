using System.Collections.Generic;

namespace Utilities.Extensions
{
    public static class ShortExt
    {
        /// <summary>
        /// 位元是否啟用
        /// </summary>
        /// <param name="b"></param>
        /// <param name="addSelf"></param>
        /// <returns></returns>
        public static List<short> IsBitEnable(this short b, bool addSelf = false)
        {
            List<short> result = new List<short>();
            for (int i = 0; i < 16; ++i)
            {
                int testValue = 1 << i;
                if ((b & testValue) != 0)
                    result.Add((short)testValue);
            }

            if (addSelf && !result.Contains(b))
                result.Add(b);
            return result;
        }

        /// <summary>
        /// 時間轉換成字串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string TimeMinute2String(this short time)
        {
            return string.Concat((time / 60).ToString("D2"), ":", (time % 60).ToString("D2"));
        }
    }
}