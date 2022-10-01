/*********************************************************
* 名    称：ScoreManualListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分奖惩列表
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 积分奖惩列表
    /// </summary>
    public class ScoreManualListItemDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 事件描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 影响用户
        /// </summary>
        public int UserCount { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatorTime { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}
