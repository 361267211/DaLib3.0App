/*********************************************************
 * 名    称：Customer
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：客户信息
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
    /// 客户信息
    /// </summary>
    public class Customer : Entity<Guid>
    {
        /// <summary>
        /// 中心站同步标识
        /// </summary>
        [StringLength(100), Required]
        public string SyncKey { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [StringLength(50), Required]
        public string Name { get; set; }

        /// <summary>
        /// 租户编码
        /// </summary>
        [StringLength(20), Required]
        public string Owner { get; set; }

        /// <summary>
        /// 平台版本
        /// </summary>
        [StringLength(20), Required]
        public string PlatformVersion { get; set; }

        /// <summary>
        /// 请求Token所需账号
        /// </summary>
        [StringLength(50), Required]
        public string Key { get; set; }

        /// <summary>
        /// 请求Token所需密码
        /// </summary>
        [StringLength(100), Required]
        public string Secret { get; set; }

        /// <summary>
        /// 门户地址
        /// </summary>
        [StringLength(100), Required]
        public string PortalUrl { get; set; }
        /// <summary>
        /// 后台地址
        /// </summary>
        [StringLength(100), Required]
        public string ManageUrl { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        [StringLength(100), Required]
        public string FileUrl { get; set; }
        /// <summary>
        /// 前台登录地址
        /// </summary>
        [StringLength(100), Required]
        public string LoginUrl { get; set; }
        /// <summary>
        /// 后台登录地址
        /// </summary>
        [StringLength(100), Required]
        public string MgrLoginUrl { get; set; }

        /// <summary>
        /// 机构Logo
        /// </summary>
        [StringLength(100), Required]
        public string LogoUrl { get; set; }

        /// <summary>
        /// 机构简版Logo
        /// </summary>
        [StringLength(100), Required]
        public string SimpleLogoUrl { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }


    }
}