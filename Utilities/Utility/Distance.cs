using System;

namespace Utilities.Utility
{
    public static class Distance
    {
        /// <summary>
        /// 根據經緯度計算距離，單位公尺
        /// </summary>
        /// <param name="latitude1">緯度1</param>
        /// <param name="longitude1">經度1</param>
        /// <param name="latitude2">緯度2</param>
        /// <param name="longitude2">經度2</param>
        /// <returns></returns>
        public static double GetGreatCircleDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var radiusLatitude1 = latitude1 * Math.PI / 180.0;
            var radiusLatitude2 = latitude2 * Math.PI / 180.0;

            var a = radiusLatitude1 - radiusLatitude2;
            var b = (longitude1 * Math.PI / 180.0) - (longitude2 * Math.PI / 180.0);

            var s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radiusLatitude1) * Math.Cos(radiusLatitude2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * 6378137.0;
            s = Math.Round(s * 10000) / 10000.0;

            return s;
        }
    }
}
