/*********************************************************
* 名    称：UserBorrowListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户借阅记录
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户借阅记录
    /// </summary>
    public class UserBorrowListItemOutput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 检索号
        /// </summary>
        public string SearchNo { get; set; }
        /// <summary>
        /// 馆藏地
        /// </summary>
        public string CollectPlace { get; set; }
        /// <summary>
        /// 续借申请
        /// </summary>
        public int RenewApply { get; set; }
        /// <summary>
        /// 续借次数
        /// </summary>
        public int RenewCount { get; set; }
        /// <summary>
        /// 借阅时间
        /// </summary>
        public DateTime BorrowTime { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime ShowReturnTime { get; set; }
    }
}
