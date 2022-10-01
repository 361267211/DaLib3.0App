/*********************************************************
 * 名    称：SceneUser
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 15:47:05
 * 描    述：场景用户
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
    /// 场景用户
    /// </summary>
    public class SceneUser: Entity<Guid>
    {

        /// <summary>
        /// 场景标识
        /// </summary>
        [StringLength(32), Required]
        public string SceneId { get; set; }

        /// <summary>
        /// 用户群体标识
        /// </summary>
        [StringLength(32), Required]
        public string UserSetId { get; set; }

        
        /// <summary>
        /// 用户群体类型。1-学院，1-类型，3-分组
        /// </summary>
        [Required]
        public int UserSetType { get; set; }


        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public int DeleteFlag { get; set; }
    }
}
