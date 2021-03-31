using System;

namespace SampleCode.Models
{
    public class LoginLockInfo
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 失敗次數
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// 紀錄時間
        /// </summary>
        public DateTime RecordTime { get; set; }

        /// <summary>
        /// IP位址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="account"></param>
        /// <param name="ip"></param>
        public LoginLockInfo(string account, string ip)
        {
            Account = account;
            FailedCount = 0;
            RecordTime = DateTime.Now;
            IPAddress = ip;
        }
    }
}