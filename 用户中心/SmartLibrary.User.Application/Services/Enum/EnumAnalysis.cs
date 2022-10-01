/*********************************************************
* 名    称：EnumAnalysis.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：分析报表相关枚举定义
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Services.Enum
{
    public enum AnalysisTimeType
    {
        最近7天 = 0,
        最近30天 = 1,
        最近90天 = 2,
        最近1年 = 3,
    }

    public enum IntervalType
    {
        按天 = 0,
        按周 = 1,
        按月 = 2,
    }

    public enum BasicAnalysisType
    {
        访问量 = 0,
        借阅量 = 1,
        入馆量 = 2,
        检索量 = 3,
        点击量 = 4,
        下载量 = 5,
    }

    public enum ResourceRankType
    {
        数据库点击 = 0,
        图书借阅 = 1,
        图书预约 = 2,
        图书续借 = 3,
        借阅学科 = 4,
        期刊订阅 = 5,
    }
    public enum ReaderRankType
    {
        数据库点击 = 0,
        图书借阅 = 1,
        图书预约 = 2,
        图书续借 = 3,
        借阅学科 = 4,
        期刊订阅 = 5,
    }
}
