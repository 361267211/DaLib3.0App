/*********************************************************
* 名    称：ReaderScoreObtainTaskDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者积分获取任务
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 读者积分获取任务
    /// </summary>
    public class ReaderScoreObtainTaskDto
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid TaskID { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string TaskDesc { get; set; }
        /// <summary>
        /// 获取积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 宣传图地址
        /// </summary>
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// 跳转地址
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 触发周期
        /// </summary>
        public int TriggerTerm { get; set; }
        /// <summary>
        /// 周期内总共可触发次数
        /// </summary>
        public int TermTriggerTime { get; set; }

        /// <summary>
        /// 周期内已经触发次数
        /// </summary>
        public int TermHasTriggerTime { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool HasCompleted
        {
            get
            {
                return TermHasTriggerTime == TermTriggerTime;
            }
        }
    }
    /// <summary>
    /// 路由地址信息
    /// </summary>
    public class AppRouteUrlDto
    {
        /// <summary>
        /// 应用编码
        /// </summary>
        public string AppCode { get; set; }
        /// <summary>
        /// 前台地址
        /// </summary>
        public string FrontUrl { get; set; }
        /// <summary>
        /// 后台地址
        /// </summary>
        public string BackUrl { get; set; }
    }
}
