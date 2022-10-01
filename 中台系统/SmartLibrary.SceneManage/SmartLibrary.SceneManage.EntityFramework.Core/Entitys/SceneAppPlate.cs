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
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLibrary.SceneManage.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 场景内应用
    /// </summary>
    public class SceneAppPlate: Entity<Guid>
    {

        /// <summary>
        /// 场景内应用标识
        /// </summary>
        [StringLength(48), Required]
        public string SceneAppId { get; set; }

        /// <summary>
        /// 场景标识
        /// </summary>
        [StringLength(48), Required]
        public string SceneId { get; set; }


        /// <summary>
        /// 应用栏目标识
        /// </summary>
        [StringLength(200), Required]
        public string AppPlateId { get; set; }

        /// <summary>
        /// 数据条数
        /// </summary>
        [Required]
        public int TopCount { get; set; }

        /// <summary>
        /// 排序方式 1-创建时间倒序  2-访问量倒序
        /// </summary>
        [Required]
        public string SortType { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        [Required]
        public int OrderIndex { get; set; }
    }
}
