using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.LogAnalysis.Application.Dtos
{
    /// <summary>
    /// 重大首页那些
    /// </summary>
    public class IndexStatisticsDto
    {
        /// <summary>
        /// 周入馆 用户访问数
        /// </summary>
        public int WeeklyVisitUserCount { get; set; }
        /// <summary>
        /// 上周入馆访问用户数
        /// </summary>
        public int PreviousWeeklyVisitUserCount { get; set; }
        /// <summary>
        /// 周访问量
        /// </summary>
        public int WeeklyVisitCount { get; set; }
        /// <summary>
        /// 上周访问量
        /// </summary>
        public int PreviousWeeklyVisitCount { get; set; }
        /// <summary>
        /// 学院使用排行
        /// </summary>
        public NameValueItem<string,int>[] DepartmentRankList { get; set; }
        /// <summary>
        /// 周 资源更新量
        /// </summary>
        public int WeeklyResourceIncreaseCount { get; set; }
        /// <summary>
        /// 资源总量
        /// </summary>
        public int TotalResourceCount { get; set; }
        /// <summary>
        /// 资源学科分布
        /// </summary>
        public NameValueItem<string, int>[] ResourceDomainRank { get; set; }
    }



    public class NameValueItem<TName,TValue>
    {
        public TName Name { get; set; }
        public TValue Value { get; set; }
    }
}
