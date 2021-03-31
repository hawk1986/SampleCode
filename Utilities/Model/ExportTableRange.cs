using System.Data;

namespace Utilities.Model
{
    public class ExportTableRange
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 欄
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// 是否新增列
        /// </summary>
        public bool IsInsertRow { get; set; }

        /// <summary>
        /// 資料內容
        /// </summary>
        public DataTable Table { get; set; }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="insertRow"></param>
        /// <param name="dt"></param>
        public ExportTableRange(int row, int col, bool insertRow, DataTable dt)
        {
            Row = row;
            Column = col;
            IsInsertRow = insertRow;
            Table = dt;

        }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="n"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="insertRow"></param>
        /// <param name="dt"></param>
        public ExportTableRange(int n, int row, int col, bool insertRow, DataTable dt) : this(row, col, insertRow, dt)
        {
            Index = n;
        }
    }
}