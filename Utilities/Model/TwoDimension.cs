namespace Utilities.Model
{
    public class TwoDimension<T>
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
        /// 值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public TwoDimension(int row, int col, T value)
        {
            Row = row;
            Column = col;
            Value = value;
        }

        /// <summary>
        /// 建構函數
        /// </summary>
        /// <param name="n"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public TwoDimension(int n, int row, int col, T value) : this(row, col, value)
        {
            Index = n;
        }
    }
}