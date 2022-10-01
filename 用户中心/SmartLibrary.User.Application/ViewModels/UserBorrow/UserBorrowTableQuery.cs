/*********************************************************
* 名    称：UserBorrowTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户借阅记录查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户借阅记录查询
    /// </summary>
    public class UserBorrowTableQuery : TableQueryBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 馆藏地
        /// </summary>
        public string CollectionPlace { get; set; }
        /// <summary>
        /// 借阅开始时间
        /// </summary>
        public DateTime? BorrowStartTime { get; set; }
        /// <summary>
        /// 借阅截止时间
        /// </summary>
        public DateTime? BorrowEndTime { get; set; }
        /// <summary>
        /// 归还开始时间
        /// </summary>
        public DateTime? ReturnStartTime { get; set; }
        /// <summary>
        /// 归还截止时间
        /// </summary>
        public DateTime? ReturnEndTime { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 检索号
        /// </summary>
        public string SearchNo { get; set; }
    }
}
