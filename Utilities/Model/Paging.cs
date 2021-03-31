using System.Collections.Generic;

namespace Utilities.Model
{
    public class Paging<T> where T : class
    {
        /// <summary>
        /// 總筆數
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageLimit { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 資料
        /// </summary>
        public List<T> Rows { get; set; }
    }
}