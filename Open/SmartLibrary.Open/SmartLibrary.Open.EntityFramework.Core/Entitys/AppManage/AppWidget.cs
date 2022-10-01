/*********************************************************
 * 名    称：AppWidget
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/1 14:40:07
 * 描    述：应用组件
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
    /// 应用组件
    /// </summary>
    public class AppWidget : Entity<Guid>
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        [StringLength(100), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 组件名称
        /// </summary>
        [StringLength(100), Required]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(2000), Required]
        public string Desc { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        [StringLength(100), Required]
        public string Target { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        [StringLength(100), Required]
        public string Cover { get; set; }

        /// <summary>
        /// 可用配置 1-栏目 2-数据条数 3-排序字段
        /// 逗号分隔
        /// </summary>
        [StringLength(20)]
        public string AvailableConfig { get; set; }

        /// <summary>
        /// 最大数据条数
        /// </summary>
        public int MaxTopCount { get; set; }

        /// <summary>
        /// 数据条数间隔
        /// </summary>
        public int TopCountInterval { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 默认宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 默认高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 特殊场景标识 ，1-通用 2-个人中心
        /// </summary>
        public int SceneType { get; set; }

    }
}