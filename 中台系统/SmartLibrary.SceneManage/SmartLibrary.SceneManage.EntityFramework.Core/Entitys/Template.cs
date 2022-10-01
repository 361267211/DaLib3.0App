/*********************************************************
 * 名    称：Template
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 19:27:45
 * 描    述：场景模板
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.SceneManage.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 场景模板
    /// </summary>
    public class Template : Entity<Guid>
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        [StringLength(50), Required]
        public string Name { get; set; }

        /// <summary>
        /// 模板路由
        /// </summary>
        [StringLength(100), Required]
        public string Router { get; set; }

        /// <summary>
        /// 屏数
        /// </summary>
        [Required]
        public int ScreenCount { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        [Required]
        public int ColumnCount { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        [Required]
        public bool IsLock { get; set; }

        /// <summary>
        /// 宽高比
        /// </summary>
        [StringLength(48)]
        public string AspectRatio { get; set; }

        /// <summary>
        /// 适用布局标识
        /// </summary>
        [StringLength(48)]
        public string LayoutId { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        [StringLength(100)]
        public string Cover { get; set; }

        /// <summary>
        /// 模板类型 1-场景 2-头部 3-底部
        /// </summary>
        [StringLength(48), Required]
        public int Type { get; set; }

        /// <summary>
        /// 模板宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        [StringLength(48)]
        public string BackgroundColor { get; set; }

        /// <summary>
        /// 默认头部模板标识
        /// </summary>
        [StringLength(48)]
        public string DefaultHeaderTemplateId { get; set; }

        /// <summary>
        /// 默认底部模板标识
        /// </summary>
        [StringLength(48)]
        public string DefaultFooterTemplateId { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
