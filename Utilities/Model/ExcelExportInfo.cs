using System.Data;

namespace Utilities.Model
{
    public class ExcelExportInfo
    {
        /// <summary>
        /// 試算表名稱
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 頁面標題
        /// </summary>
        public string PageTitle { get; set; }

        /// <summary>
        /// 資料
        /// </summary>
        public DataTable Data { get; set; }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="title"></param>
        /// <param name="data"></param>
        public ExcelExportInfo(string sheet, string title, DataTable data)
        {
            SheetName = sheet;
            PageTitle = title;
            Data = data;
        }
    }
}
