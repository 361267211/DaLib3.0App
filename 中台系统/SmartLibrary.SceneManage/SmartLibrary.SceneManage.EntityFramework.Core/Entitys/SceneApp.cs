/*********************************************************
 * 名    称：SceneApp
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 15:47:05
 * 描    述：场景内应用
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
    /// 场景内应用
    /// </summary>
    public class SceneApp: Entity<Guid>
    {

        /// <summary>
        /// 场景标识
        /// </summary>
        [StringLength(48), Required]
        public string SceneId { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        [StringLength(48), Required]
        public string AppId { get; set; }

        /// <summary>
        /// 父级场景应用标识（用于个人中心）
        /// </summary>
        [StringLength(48)]
        public string ParentSceneAppId { get; set; }


        /// <summary>
        /// 应用组件(模板)标识
        /// </summary>
        [StringLength(48), Required]
        public string AppWidgetId { get; set; }


        /// <summary>
        /// 分屏Id
        /// </summary>
        [StringLength(48), Required]
        public string ScreenId { get; set; }

        /// <summary>
        /// X轴位置
        /// </summary>
        [Required]
        public int XIndex { get; set; }

        /// <summary>
        /// Y轴位置
        /// </summary>
        [Required]
        public int YIndex { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        [Required]
        public int Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        [Required]
        public int Height { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
