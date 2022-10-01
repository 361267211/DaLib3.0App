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
   public  class AppTableQuery:TableQueryBase
    {
        /// <summary>
        /// 应用类型字典值
        /// </summary>
        public string ServiceType { get; set; }
        /// <summary>
        /// 适用终端
        /// </summary>
        public int? Terminal { get; set; }
        /// <summary>
        /// 服务状态
        /// </summary>
        public int? Status { get; set; }
    }
}
