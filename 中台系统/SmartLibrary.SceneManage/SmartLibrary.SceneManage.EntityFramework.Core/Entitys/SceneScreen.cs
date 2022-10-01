/*********************************************************
 * 名    称：SceneApp
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 15:47:05
 * 描    述：场景分屏
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
    /// 场景分屏
    /// </summary>
    public class SceneScreen: Entity<Guid>
    {

        /// <summary>
        /// 场景标识
        /// </summary>
        [StringLength(48), Required]
        public string SceneId { get; set; }

        /// <summary>
        /// 分屏名称
        /// </summary>
        [StringLength(48), Required]
        public string ScreenName { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Required]
        public int OrderIndex { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
