using SmartLibrary.Open.Common.AssemblyBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos.Application
{
    /// <summary>
    /// 付费应用推荐查询参数
    /// </summary>
    public class PayAppTableQuery : TableQueryBase
    {
        /// <summary>
        /// 服务类型
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// token中的orgCode
        /// </summary>
        public string Owner { get; set; }
    }
}
