/*********************************************************
 * 名    称：AppBranchEntryPoint
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：应用分支入口
 *
 * 更新历史：
 *
 * *******************************************************/

using System;
using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys
{

    /// <summary>
    /// 应用分支入口
    /// </summary>
    public class AppBranchEntryPoint : Entity<Guid>
    {

        /// <summary>
        /// 应用标识
        /// </summary>
        [StringLength(48), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 应用分支标识
        /// </summary>
        [StringLength(48)]
        public string AppBranchId { get; set; }

        /// <summary>
        /// 入口名称
        /// </summary>
        [StringLength(20), Required]
        public string Name { get; set; }

        /// <summary>
        /// 入口编码
        /// </summary>
        [StringLength(20), Required]
        public string Code { get; set; }

        /// <summary>
        /// 使用场景 1-前台  2-后台
        /// </summary>
        [Required]
        public int UseScene { get; set; }

        /// <summary>
        /// 入口业务类型 参照  AppDictioanry.BusinessType
        /// </summary>
        [StringLength(50)]
        public string BusinessType { get; set; }

        /// <summary>
        /// 访问路径
        /// </summary>
        [StringLength(100), Required]
        public string VisitUrl { get; set; }

        /// <summary>
        /// 是否内置入口
        /// </summary>
        [Required]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 是否默认入口
        /// </summary>
        [Required]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 应用事件
        /// </summary>
        public ICollection<AppEvent> AppEvents { get; set; }

    }
}