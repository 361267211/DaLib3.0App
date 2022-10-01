using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.UserApplication
{
    /// <summary>
    /// 我的收藏应用
    /// </summary>
    public class MyCollectionAppDto
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string RouteCode { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 前台访问地址
        /// </summary>
        public string FrontUrl { get; set; }
    }
}
