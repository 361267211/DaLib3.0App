using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// App表格数据查询接口
    /// </summary>
   public  class AppSearchQuery : TableQueryBase
    {
        /// <summary>
        /// 应用类型字典值
        /// </summary>
        public string ServiceType { get; set; }
        /// <summary>
        /// 使用场景
        /// </summary>
        public string UseScene { get; set; }
        /// <summary>
        /// 采购类型
        /// </summary>
        public int? PurchaseType { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public string CustomerId { get; set; }
    }
}
