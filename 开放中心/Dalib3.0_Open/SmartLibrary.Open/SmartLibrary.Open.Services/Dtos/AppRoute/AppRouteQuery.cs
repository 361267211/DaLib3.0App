using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// App路由表请求参数
    /// </summary>
   public  class AppRouteQuery : TableQueryBase
    {
        /// <summary>
        /// 租户标识
        /// </summary>
        public string TenantCode { get; set; }
    }
}
