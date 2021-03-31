namespace Utilities.Model
{
    public class ColumnSpanInfo
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 欄位索引
        /// </summary>
        public int ColunmIndex { get; set; }

        /// <summary>
        /// 跨欄合併的數量
        /// </summary>
        public int SpanCount { get; set; }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public ColumnSpanInfo(string name, int index, int count)
        {
            Name = name;
            ColunmIndex = index;
            SpanCount = count;
        }
    }
}