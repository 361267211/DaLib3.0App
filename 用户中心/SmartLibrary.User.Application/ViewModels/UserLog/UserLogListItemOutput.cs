/*********************************************************
* 名    称：UserLogListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者操作日志列表数据
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者操作日志列表数据
    /// </summary>
    public class UserLogListItemOutput
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime EventTime { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// 日志来源
        /// </summary>
        public string LogFrom { get; set; }
        /// <summary>
        /// 日志描述
        /// </summary>
        public string LogDesc { get; set; }
    }
}
