using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 导航栏目设置
    /// </summary>
    public class NavigationItemUpdateDto
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
        public List<NavigationItemUpdateAppInfo> AppInfos { get; set; }
    }


    /// <summary>
    /// 导航栏目关联应用
    /// </summary>
    public class NavigationItemUpdateAppInfo
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>
        public int OrderIndex { get; set; }
    }
}
