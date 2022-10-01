/*********************************************************
* 名    称：UserPointsListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户积分列表数据
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户积分列表数据
    /// </summary>
    public class UserPointsListItemOutput
    {
        /// <summary>
        /// 积分
        /// </summary>
        public string Points { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime ChangeTime { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }
    }
}
