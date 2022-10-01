/*********************************************************
* 名    称：ExportUserListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户数据导出
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.User
{
    /// <summary>
    /// 用户数据导出
    /// </summary>
    public class ExportUserListItemDto:UserListItemDto
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
        /// 卡类型名称
        /// </summary>
        public string CardTypeName { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        public DateTime? CardIssueDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? CardExpireDate { get; set; }
        /// <summary>
        /// 卡押金
        /// </summary>
        public decimal? CardDeposit { get; set; }
        /// <summary>
        /// 是否主卡
        /// </summary>
        public bool? CardIsPrincipal { get; set; }
    }

    /// <summary>
    /// 用户数据导出简要信息
    /// </summary>
    public class UserExportBriefDto
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 分页条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
    }
}
