/*********************************************************
* 名    称：UserPointsTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户积分查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户积分查询
    /// </summary>
    public class UserPointsTableQuery : TableQueryBase
    {
        public Guid UserID { get; set; }
        /// <summary>
        /// 变更方式
        /// </summary>
        public int? ChangeType { get; set; }
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
