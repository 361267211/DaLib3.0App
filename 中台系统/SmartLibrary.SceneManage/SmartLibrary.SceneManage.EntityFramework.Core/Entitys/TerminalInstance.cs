/*********************************************************
 * 名    称：TerminalInstance
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 19:27:45
 * 描    述：终端实例
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
    /// 终端实例
    /// </summary>
    public class TerminalInstance : Entity<Guid>
    {
        /// <summary>
        /// 终端实例名称
        /// </summary>
        [StringLength(50), Required]
        public string Name { get; set; }

        /// <summary>
        /// 终端类型 1-PC端、2-APP端、3-小程序端、4-自适应移动端、5-显示屏
        /// </summary>
        [Required]
        public int TerminalType { get; set; }

        /// <summary>
        /// 关键词，多个逗号分隔
        /// </summary>
        [StringLength(100)]
        public string KeyWords { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// Logo的Url
        /// </summary>
        [StringLength(100)]
        public string Logo { get; set; }

        /// <summary>
        /// 图标的Url
        /// </summary>
        [StringLength(100), Required]
        public string Icon { get; set; }

        /// <summary>
        /// 访问路径
        /// </summary>
        [StringLength(50), Required]
        public string VisitUrl { get; set; }

        /// <summary>
        /// 状态  0-下线  1-正常
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 是否系统默认实例 0-否  1-是
        /// </summary>
        [Required]
        public bool IsSystemInstance { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
