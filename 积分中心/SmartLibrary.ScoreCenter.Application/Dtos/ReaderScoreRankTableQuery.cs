/*********************************************************
* 名    称：ReaderScoreRankTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者积分排行数据
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 读者积分排行数据
    /// </summary>
    public class ReaderScoreRankTableQuery : TableQueryBase
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
