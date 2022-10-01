/*********************************************************
* 名    称：ScoreConsumeTaskDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分消费任务
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 积分消费任务
    /// </summary>
    public class ScoreConsumeTaskDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 应用编码
        /// </summary>
        public string AppCode { get; set; }
        /// <summary>
        /// 触发事件编码
        /// </summary>
        public string EventCode { get; set; }
        /// <summary>
        /// 消耗积分
        /// </summary>
        public int ConsumeScore { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
