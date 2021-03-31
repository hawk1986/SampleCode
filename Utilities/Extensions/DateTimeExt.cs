using System;
using System.Globalization;

namespace Utilities.Extensions
{
    public static class DateTimeExt
    {
        /// <summary>
        /// 將西元日期轉為民國日期，不支援民國前
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToFullTaiwanDate(this DateTime datetime, string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                format = "yyyy 年 MM 月 dd 日";
            }
            var info = new CultureInfo("zh-TW");
            var calendar = new TaiwanCalendar();
            info.DateTimeFormat.Calendar = calendar;
            var result = datetime.ToString(format, info);

            return result;
        }
    }
}
