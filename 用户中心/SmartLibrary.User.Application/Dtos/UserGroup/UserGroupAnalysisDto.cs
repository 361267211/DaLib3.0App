/*********************************************************
* 名    称：UserGroupAnalysisDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组数据分析展示
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Dtos.UserGroup
{
    /// <summary>
    /// 用户组数据分析展示信息
    /// </summary>
    public class UserGroupVisitBriefDto
    {
        /// <summary>
        /// 今日访问次
        /// </summary>
        public int TodayTotalCount { get; set; }
        public int PreTotalCount { get; set; }
        /// <summary>
        /// 今日访问人数
        /// </summary>
        public int TodayUserCount { get; set; }
        public int PreUserCount { get; set; }
        /// <summary>
        /// 人均访问次数
        /// </summary>
        public decimal GroupAvgCount { get; set; }
        public decimal UserAvgCount { get; set; }
    }

    public class ChartDataItemDto
    {
        public string GroupName { get; set; }
        public string Label { get; set; }
        public decimal Value { get; set; }
        public int Sort { get; set; }
    }

    public class HotEventListItemDto
    {
        public string EventName { get; set; }
        public int GroupEventCount { get; set; }
        public int TotalEventCount { get; set; }
        public decimal EventPercent { get; set; }

        public int GroupUserCount { get; set; }
        public int TotalUserCount { get; set; }
        public decimal UserPercent { get; set; }

        public decimal PercentDiff { get; set; }
    }

    public class ResourceRankListItemDto
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class ReaderRankListItemDto
    {
        public string Name { get; set; }
        public string College { get; set; }
        public int Count { get; set; }
    }
}
