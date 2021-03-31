namespace Utilities.Extensions
{
    public static class ByteExt
    {
        /// <summary>
        /// 取得位元
        /// </summary>
        /// <param name="b">this</param>
        /// <param name="bitNumber">位置</param>
        /// <returns></returns>
        public static bool GetBit(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber - 1)) != 0;
        }
    }
}
