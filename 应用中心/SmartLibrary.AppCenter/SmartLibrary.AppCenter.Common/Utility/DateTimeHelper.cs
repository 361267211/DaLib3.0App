using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Common.Utility
{
    /// <summary>
    /// 时间日期帮助类
    /// </summary>
    public class DateTimeHelper
    {
        public static int[] ToResult(string d1, string d2, DiffResultFormat drf)
        {
            try
            {
                var isSuccess1 = DateTime.TryParse(d1, out DateTime date1);
                var isSuccess2 = DateTime.TryParse(d2, out DateTime date2);
                if (!isSuccess1)
                {
                    date1 = DateTime.MaxValue;
                }
                return ToResult(date1, date2, drf);
            }
            catch
            {
                throw new Exception("DateTimeHelper.ToResult时间字符串参数不正确!");
            }
        }

        /// <summary>
        /// 计算日期间隔
        /// </summary>
        /// <param name="d1">要参与计算的其中一个日期</param>
        /// <param name="d2">要参与计算的另一个日期</param>
        /// <param name="drf">决定返回值形式的枚举</param>
        /// <returns>一个代表年月日的int数组，具体数组长度与枚举参数drf有关</returns>
        public static int[] ToResult(DateTime d1, DateTime d2, DiffResultFormat drf)
        {
            #region 数据初始化
            DateTime max;
            DateTime min;
            int year;
            int month;
            int tempYear, tempMonth;
            if (d1 > d2)
            {
                max = d1;
                min = d2;
            }
            else
            {
                max = d2;
                min = d1;
            }
            tempYear = max.Year;
            tempMonth = max.Month;
            if (max.Month < min.Month)
            {
                tempYear--;
                tempMonth = tempMonth + 12;
            }
            year = tempYear - min.Year;
            month = tempMonth - min.Month;
            #endregion
            #region 按条件计算
            if (drf == DiffResultFormat.dd)
            {
                TimeSpan ts = max - min;
                return new int[] { ts.Days };
            }
            if (drf == DiffResultFormat.mm)
            {
                return new int[] { month + year * 12 };
            }
            if (drf == DiffResultFormat.yy)
            {
                return new int[] { year };
            }
            return new int[] { year, month };
            #endregion
        }
    }


    /// <summary>
    /// 关于返回值形式的枚举
    /// </summary>
    public enum DiffResultFormat
    {
        /// <summary>
        /// 年数和月数
        /// </summary>
        yymm,
        /// <summary>
        /// 年数
        /// </summary>
        yy,
        /// <summary>
        /// 月数
        /// </summary>
        mm,
        /// <summary>
        /// 天数
        /// </summary>
        dd,
    }
}
