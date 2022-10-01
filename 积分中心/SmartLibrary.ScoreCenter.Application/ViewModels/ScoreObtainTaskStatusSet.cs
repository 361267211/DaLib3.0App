/*********************************************************
* 名    称：ScoreObtainTaskStatusSet.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分获取任务状态
* 更新历史：
*
* *******************************************************/
using System;
namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分获取任务状态
    /// </summary>
    public class ScoreObtainTaskStatusSet
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
    }
}
