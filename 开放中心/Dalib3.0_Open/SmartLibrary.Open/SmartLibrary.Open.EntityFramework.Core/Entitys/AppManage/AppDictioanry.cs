/*********************************************************
 * 名    称：AppDictioanry
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：应用字典
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
    /// 应用字典
    /// </summary>
    public class AppDictioanry : Entity<Guid>
    {
        /// <summary>
        /// 字典类型
        /// </summary>
        [StringLength(20), Required]
        public string DictType { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        [StringLength(20), Required]
        public string Name { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>
        [StringLength(50), Required]
        public string Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(100), Required]
        public string Desc { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

    }
}