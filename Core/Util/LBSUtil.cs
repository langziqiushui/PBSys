using System;
using System.Collections.Generic;
using System.Text;

namespace YX.Core
{
    /// <summary>
    /// 辅助类。
    /// </summary>
    public static class LBSUtil
    {
        /// <summary>
        /// 地球半径。
        /// </summary>
        private const double EARTH_RADIUS = 6378.137;

        /// <summary>
        /// 计算PI。
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// 计算两坐标之间的距离(单位：KM)。
        /// </summary>
        /// <param name="lat1">坐标1经度</param>
        /// <param name="lng1">坐标1纬度</param>
        /// <param name="lat2">坐标2经度</param>
        /// <param name="lng2">坐标2纬度</param>
        /// <returns></returns>
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }       
    }
}
