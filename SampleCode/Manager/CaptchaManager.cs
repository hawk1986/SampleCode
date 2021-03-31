using System;
using System.Collections.Concurrent;
using System.Linq;
using SampleCode.Models;

namespace SampleCode.Manager
{
    public static class CaptchaManager
    {
        /// <summary>
        /// 驗證碼清單
        /// </summary>
        readonly static ConcurrentDictionary<Guid, CaptchaData> captchas = new ConcurrentDictionary<Guid, CaptchaData>();

        /// <summary>
        /// 新增驗證碼
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        public static void InsertCaptcha(Guid id, string code)
        {
            captchas.TryAdd(id, new CaptchaData(id, code));
        }

        /// <summary>
        /// 移除驗證碼
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool RemoveCaptcha(Guid id, string code)
        {
            CaptchaData tmp;
            //先清除過期的驗證碼
            var removes = captchas.Values.Where(x => x.ExpiredTime < DateTime.Now);
            if (removes.Any())
            {
                foreach (var item in removes)
                    captchas.TryRemove(item.ID, out tmp);
            }

            //判斷驗證碼是否正確
            if (captchas.TryGetValue(id, out tmp) && tmp.Data == code)
            {
                //移除驗證碼
                captchas.TryRemove(id, out tmp);
                return true;
            }
            return false;
        }
    }
}