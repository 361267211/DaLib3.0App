using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.BaseInfo
{
    /// <summary>
    /// 当前应用信息
    /// </summary>
    public class CurrentAppInfo
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }
    }
}
