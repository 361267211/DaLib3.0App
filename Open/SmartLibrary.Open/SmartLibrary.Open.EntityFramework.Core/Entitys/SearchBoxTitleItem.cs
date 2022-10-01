using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 检索
    /// </summary>
   public class SearchBoxTitleItem : Entity<Guid>
    {  /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        [Comment("删除标识")]
        public bool DeleteFlag { get; set; }
        [StringLength(50), Required,Comment("标题")]
        public string Title { get; set; }
        [StringLength(10), Required,Comment("标识符")]
        public string Symbol { get; set; }
        [Comment("对应文献类型值，0表示全部"),Required]
        public int Value { get; set; }
    }
}
