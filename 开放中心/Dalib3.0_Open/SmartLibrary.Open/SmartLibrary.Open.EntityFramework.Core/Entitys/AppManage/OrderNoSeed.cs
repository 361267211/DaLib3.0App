using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 订单单据号种子记录
    /// </summary>
    public class OrderNoSeed:Entity<Guid>
    {
        /// <summary>
        /// 种子值
        /// </summary>
        public int SeedValue { get; set; }
        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime SeedDate { get; set; }
    }
}
