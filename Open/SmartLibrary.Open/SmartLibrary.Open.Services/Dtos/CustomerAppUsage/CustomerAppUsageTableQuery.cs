using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;

namespace SmartLibrary.Open.Services.Dtos
{
    public class CustomerAppUsageTableQuery : TableQueryBase
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public Guid? CustomerID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid? AppID { get; set; }
    }
}
