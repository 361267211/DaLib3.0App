using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos
{
    public class CustomerAppTableViewModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid CustomerID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid AppID { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string DevName { get; set; }
        /// <summary>
        /// 授权类型
        /// </summary>
        public int AuthType { get; set; }
        /// <summary>
        /// 授权类型名称
        /// </summary>
        public string AuthTypeDisp { get; set; }
        /// <summary>
        /// 有效结束期
        /// </summary>
        public DateTime EndDate { get; set; }

    }
}
