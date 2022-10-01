/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys
{
    /// 数据库导航栏目
    public class DatabaseColumn: Entity<Guid>
    {


        /// <summary>
        /// 栏目名称
        /// </summary>
        [Column("ColumnName")]
        [StringLength(maximumLength: 50, MinimumLength = 0), Required]
        public string ColumnName { get; set; }

        /// <summary>
        /// 匹配数量
        /// </summary>
        [Column("MatchCount")]
        public int MatchCount { get; set; } 

        /// <summary>
        /// 排序规则
        /// </summary>
        [Column("OrderRule")]
        [Required]
        public int OrderRule { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        [Column("DeleteFlag")]
        public bool DeleteFlag { get; set; }

    }
}
