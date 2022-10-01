/*********************************************************
 * 名    称：ThemeColor
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 19:27:45
 * 描    述：主题色
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
    /// 主题色
    /// </summary>
    public class ThemeColor: Entity<Guid>
    {
        /// <summary>
        /// 主题色名称
        /// </summary>
        [StringLength(50), Required]
        public string Name { get; set; }

        /// <summary>
        /// 颜色名称或自定义色号
        /// </summary>
        [StringLength(32)]
        public string Color { get; set; }


        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public int DeleteFlag { get; set; }
    }
}
