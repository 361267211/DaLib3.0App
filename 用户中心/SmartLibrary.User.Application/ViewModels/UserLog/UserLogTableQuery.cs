/*********************************************************
* 名    称：UserLogTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户日志查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户日志查询
    /// </summary>
    public class UserLogTableQuery : TableQueryBase
    {
        /// <summary>
        /// 行为开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 行为结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }
    }
}
