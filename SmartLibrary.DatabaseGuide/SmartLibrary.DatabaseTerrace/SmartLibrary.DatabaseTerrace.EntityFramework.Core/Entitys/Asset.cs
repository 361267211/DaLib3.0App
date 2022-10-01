/*********************************************************
 * 名    称：Asset
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：数据库模型Assrt示例。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 测试 EF、多租户、多租户迁移用的模型Asset
    /// </summary>
    public class Asset : Entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(50), Required]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(50)]
        public string Content { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public double? Language { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public double? Type { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        [StringLength(50)]
        public string EnglihName { get; set; }
        /// <summary>
        /// 模板
        /// </summary>
        [StringLength(50)]
        public string Plate { get; set; }
    }
}
