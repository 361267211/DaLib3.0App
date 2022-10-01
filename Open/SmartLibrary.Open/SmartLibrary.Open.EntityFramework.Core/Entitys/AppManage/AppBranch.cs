/*********************************************************
 * 名    称：AppBranch
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：应用分支
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
    /// 应用分支
    /// </summary>
    public class AppBranch : Entity<Guid>
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        [StringLength(48), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 分支名称
        /// </summary>
        [StringLength(20), Required]
        public string Name { get; set; }

        /// <summary>
        /// 分支图标
        /// </summary>
        [StringLength(50), Required]
        public string Icon { get; set; }

        /// <summary>
        /// 部署环境标识
        /// </summary>
        [StringLength(48), Required]
        public string DeployeeId { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [StringLength(20), Required]
        public string Version { get; set; }

        /// <summary>
        /// 是否主分支
        /// </summary>
        [Required]
        public bool IsMaster { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500), Required]
        public string Remark { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

    }
}