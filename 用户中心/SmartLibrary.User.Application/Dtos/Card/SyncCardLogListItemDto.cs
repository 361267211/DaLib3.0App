/*********************************************************
* 名    称：SyncCardLogListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：读者卡同步日志记录
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.Card
{
    /// <summary>
    /// 同步读者卡日志
    /// </summary>
    public class SyncCardLogListItemDto
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 同步开始时间
        /// </summary>
        public DateTime? SyncStartTime { get; set; }
        /// <summary>
        /// 同步截止时间
        /// </summary>
        public DateTime? SyncEndTime { get; set; }
        /// <summary>
        /// 同步类型
        /// </summary>
        public int SyncType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Context { get; set; }
    }
}
