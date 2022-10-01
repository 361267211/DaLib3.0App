/*********************************************************
 * 名    称：AppCenterSettings
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/15 18:03:03
 * 描    述：应用中心设置
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 应用中心设置
    /// </summary>
    public class AppCenterSettings : Entity<Guid>
    {
        /// <summary>
        /// 设置名称
        /// </summary>
        [StringLength(50), Required]
        public string ItemName { get; set; }

        /// <summary>
        /// 设置标识
        /// 保证值唯一
        /// </summary>
        [StringLength(50), Required]
        public string ItemKey { get; set; }

        /// <summary>
        /// 设置值
        /// </summary>
        [StringLength(100), Required]
        public string ItemValue { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
