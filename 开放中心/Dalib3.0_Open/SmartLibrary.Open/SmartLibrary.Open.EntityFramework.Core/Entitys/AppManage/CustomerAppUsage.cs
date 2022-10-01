/*********************************************************
 * 名    称：CustomerAppUsage
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：应用使用情况
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys
{

    /// <summary>
    /// 应用使用情况
    /// </summary>
    public class CustomerAppUsage : Entity<Guid>
    {
        /// <summary>
        /// 客户名称
        /// </summary>
        [StringLength(100), Required]
        public string CustomerId { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        [StringLength(100), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 应用分支标识
        /// </summary>
        [StringLength(100), Required]
        public string AppBranchId { get; set; }

        /// <summary>
        /// 状态 0、待审核 1、正常 2、过期
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 授权类型 1、正式授权 2、试用授权
        /// </summary>
        [Required]
        public int AuthType { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        [Required]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 借书日期
        /// </summary>
        [Required]
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

    }
}