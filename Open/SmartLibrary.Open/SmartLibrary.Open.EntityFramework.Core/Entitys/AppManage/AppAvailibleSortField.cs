/*********************************************************
 * 名    称：AppAvailibleSortField
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：应用模板可用排序字段
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
    /// 积分事件注册
    /// </summary>
    public class AppAvailibleSortField : Entity<Guid>
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        [StringLength(48), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 排序字段名称
        /// </summary>
        [StringLength(100), Required]
        public string SortFieldName { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [StringLength(100), Required]
        public string SortFieldValue { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

    }
}