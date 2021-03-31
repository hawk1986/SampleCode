using System;
using System.Collections.Concurrent;
using SampleCode.Models;

namespace SampleCode.Manager
{
    public static class LoginLockManager
    {
        /// <summary>
        /// 登入資訊
        /// </summary>
        static readonly ConcurrentDictionary<string, LoginLockInfo> lockInfo = new ConcurrentDictionary<string, LoginLockInfo>();

        /// <summary>
        /// 是否鎖定帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool IsLockAccount(string account)
        {
            //判斷是否在15分鐘內連續登入失敗三次
            return lockInfo.TryGetValue(account, out LoginLockInfo info) && info.FailedCount > 3 && (DateTime.Now - info.RecordTime).TotalMinutes < 15;
        }

        /// <summary>
        /// 登入失敗
        /// </summary>
        /// <param name="account"></param>
        /// <param name="ip"></param>
        public static void LoginFailed(string account, string ip)
        {
            //紀錄登入失敗
            if (!lockInfo.TryGetValue(account, out LoginLockInfo info))
            {
                info = new LoginLockInfo(account, ip);
                lockInfo.TryAdd(account, info);
            }
            //判斷上次登入失敗是否在15分鐘內
            if ((DateTime.Now - info.RecordTime).TotalMinutes > 15)
                info.RecordTime = DateTime.Now;
            info.FailedCount++;
        }

        /// <summary>
        /// 登入成功
        /// </summary>
        /// <param name="account"></param>
        public static void LoginSuccess(string account)
        {
            lockInfo.TryRemove(account, out LoginLockInfo info);
        }
    }
}