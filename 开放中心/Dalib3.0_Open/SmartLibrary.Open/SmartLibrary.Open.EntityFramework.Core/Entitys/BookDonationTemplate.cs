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
    public  class BookDonationTemplate:Entity<Guid>
    {
        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        [Comment("删除标识")]
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(100), Required, Comment("名称")]
        public string Name { get; set; }
 
        /// <summary>
        /// 备注
        /// </summary>
        [Comment("备注")]
        public string Mark { get; set; }
        /// <summary>
        /// 唯一标识符，需保持全局唯一性
        /// </summary>
        [StringLength(50), Required, Comment("唯一标识符，需保持全局唯一性")]
        public string Symbol { get; set; }

         


        [StringLength(1000), Required, Comment("表示该解析器的链接")]


        public string DllLink { get; set; }
    }
}
