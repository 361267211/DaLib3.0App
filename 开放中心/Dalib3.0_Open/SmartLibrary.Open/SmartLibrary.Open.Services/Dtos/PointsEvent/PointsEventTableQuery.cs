using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用积分事件查询
    /// </summary>
    public class PointsEventTableQuery : TableQueryBase
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid AppID { get; set; }
    }
}
