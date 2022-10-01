using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 应用到期数据
    /// </summary>
    public class AppExpireDto
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 到日时间
        /// </summary>
        public string ExpireDate { get; set; }

        /// <summary>
        /// 是否授权
        /// </summary>
        public bool IsMyApp { get; set; }

    }
}
