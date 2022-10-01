/*********************************************************
 * 名    称：Order
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：授权订单
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
    /// 授权订单
    /// </summary>
    public class Order : Entity<Guid>
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [StringLength(20), Required]
        public string No { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        [StringLength(48), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>

        [StringLength(100), Required]
        public string AppName { get; set; }

        /// <summary>
        /// 客户标识
        /// </summary>
        [StringLength(48), Required]
        public string CustomerId { get; set; }

        /// <summary>
        /// 授权类型 1、正式授权 2、试用授权
        /// </summary>
        [Required]
        public int AuthType { get; set; }

        /// <summary>
        /// 开通类型 1、首次授权 2、续费授权 3、试用延期
        /// </summary>
        [Required]
        public int OpenType { get; set; }

        /// <summary>
        /// 状态 0、待审核 1、正常 2、过期 
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 途径 1、中央授权 2、客户申请
        /// </summary>
        [Required]
        public int Way { get; set; }

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
        /// 客户联系人
        /// </summary>
        [StringLength(50)]
        public string ContactMan { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(20)]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        [StringLength(50)]
        public string ApplyMan { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

    }
}