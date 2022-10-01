/*********************************************************
* 名    称：ExportUserListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：导出读者列表输出
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 导出读者列表输出
    /// </summary>
    public class ExportUserListItemOutput:UserListItemOutput
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string CardBarCode { get; set; }
        /// <summary>
        /// 物理码
        /// </summary>
        public string CardPhysicNo { get; set; }
        /// <summary>
        /// 认证号
        /// </summary>
        public string CardIdentityNo { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        public DateTime CardIssueDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime CardExpireDate { get; set; }
        /// <summary>
        /// 卡押金
        /// </summary>
        public decimal CardDeposit { get; set; }
        /// <summary>
        /// 是否主卡
        /// </summary>
        public bool CardIsPrincipal { get; set; }
    }
}
