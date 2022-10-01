/*********************************************************
 * 名    称：AppDynamic
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：应用动态
 *
 * 更新历史：
 *
 * *******************************************************/


using System;
using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys
{

    /// <summary>
    /// 应用动态
    /// </summary>
    public class AppDynamic : Entity<Guid>
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        [StringLength(48), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 应用分支标识
        /// </summary>
        [StringLength(48), Required]
        public string AppBranchId { get; set; }

        /// <summary>
        /// 信息类型 1-应用动态 2-活动信息 3-使用教程
        /// </summary>
        [Required]
        public int InfoType { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [StringLength(20), Required]
        public string Version { get; set; }

        /// <summary>
        /// 消息标识
        /// </summary>
        [StringLength(48), Required]
        public string InfoId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

    }
}