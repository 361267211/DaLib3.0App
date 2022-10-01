/*********************************************************
* 名    称：*.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分获取任务列表数据
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分获取列表任务
    /// </summary>
    public class ScoreObtainListItemOutput
    {
        /// <summary>
        /// 标识
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
        /// 获取积分
        /// </summary>
        public int ObtainScore { get; set; }
        /// <summary>
        /// 触发周期,0:固定1:每日2:每周3:每月4:每年
        /// </summary>
        public int TriggerTerm { get; set; }
        /// <summary>
        /// 周期内可重复触发次数
        /// </summary>
        public int TriggerTime { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public int ValidTerm { get; set; }
        /// <summary>
        /// 任务截止日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
