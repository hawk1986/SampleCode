namespace Utilities.Model
{
    public class ColumnInfo
    {
        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 標題名稱
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 預設值
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// 寬度
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="width"></param>
        public ColumnInfo(string fieldName, string columnName, object defaultValue, int? width = null)
        {
            FieldName = fieldName;
            ColumnName = columnName;
            DefaultValue = defaultValue;
            Width = width;
        }
    }
}