/*********************************************************
 * 名    称：Layout
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 19:27:45
 * 描    述：场景布局
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
    /// 场景布局
    /// </summary>
    public class Layout:Entity<Guid>
    {
        /// <summary>
        /// 布局名称
        /// </summary>
        [StringLength(50), Required]
        public string Name { get; set; }        

        /// <summary>
        /// 适用终端类型 1-PC端、2-APP端、3-小程序端、4-自适应移动端、5-显示屏
        /// </summary>
        [Required]
        public int TerminalType { get; set; }


        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
