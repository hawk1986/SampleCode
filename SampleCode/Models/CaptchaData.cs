using System;

namespace SampleCode.Models
{
    public class CaptchaData
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// ExpiredTime
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public CaptchaData(Guid id, string data)
        {
            ID = id;
            Data = data;
            ExpiredTime = DateTime.Now.AddMinutes(30);
        }
    }
}