using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 导航栏目详情
    /// </summary>
    public class NavigationItemInfoDto
    {
        /// <summary>
        /// 导航栏目ID
        /// </summary>
        public string NavId { get; set; }

        /// <summary>
        /// 导航栏目名称
        /// </summary>
        public string NavName { get; set; }

        /// <summary>
        /// 是否个人应用优先
        /// </summary>
        public bool IsPrivateFirst { get; set; }

        /// <summary>
        /// 关联应用列表
        /// </summary>
        public List<NavigationItemInfoAppInfo> AppInfos { get; set; }
    }

    /// <summary>
    /// 导航栏目关联应用
    /// </summary>
    public class NavigationItemInfoAppInfo
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
        /// 开发商
        /// </summary>
        public string Developer { get; set; }

        /// <summary>
        /// 应用类型
        /// </summary>
        public string AppType { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>
        public int OrderIndex { get; set; }
    }
}
