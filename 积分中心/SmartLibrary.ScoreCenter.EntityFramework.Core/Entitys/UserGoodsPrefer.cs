using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 用户商品收藏记录
    /// </summary>
    public class UserGoodsPrefer : BaseEntity<int>
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UserKey { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public Guid GoodsID { get; set; }
    }
}
