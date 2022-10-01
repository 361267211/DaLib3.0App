using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.BaseInfo
{
    /// <summary>
    /// code和URL对应关系
    /// </summary>
    public class UrlInfo
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// routecode
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// url
        /// </summary>
        public string Path { get; set; }
    }
}
