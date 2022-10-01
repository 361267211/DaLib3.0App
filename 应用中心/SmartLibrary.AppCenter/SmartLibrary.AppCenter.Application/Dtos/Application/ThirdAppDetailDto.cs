using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 三方应用详细信息
    /// </summary>
    public class ThirdAppDetailDto
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用类型，多选
        /// </summary>
        public List<string> AppType { get; set; }

        /// <summary>
        /// 开发者
        /// </summary>
        public string Developer { get; set; }

        /// <summary>
        /// 开发者联系方式
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 应用介绍
        /// </summary>
        public string AppIntroduction { get; set; }

        /// <summary>
        /// 应用说明
        /// </summary>
        public string AppExplain { get; set; }

        /// <summary>
        /// 支持终端，多选
        /// </summary>
        public List<string> Terminal { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 前台访问地址
        /// </summary>
        public string FrontUrl { get; set; }

        /// <summary>
        /// 读者授权列表
        /// </summary>
        public List<ThirdAppDetailAuthInfo> AuthInfos { get; set; }
    }

    /// <summary>
    /// 三方应用授权信息
    /// </summary>
    public class ThirdAppDetailAuthInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户集合类型 1=用户类型，2=用户分组
        /// </summary>
        public int UserSetType { get; set; }
        /// <summary>
        /// 用户类型/分组ID
        /// </summary>
        public string UserSetId { get; set; }
        /// <summary>
        /// 用户群/类型名称
        /// </summary>
        public string UserSetName { get; set; }
    }
}
