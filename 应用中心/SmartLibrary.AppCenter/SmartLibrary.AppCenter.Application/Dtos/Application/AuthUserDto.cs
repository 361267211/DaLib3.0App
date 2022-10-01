using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 用户权限设置-按用户
    /// </summary>
    public class AuthUserDto
    {
        /// <summary>
        /// 用户类型-用户分组ID
        /// </summary>
        public string UserSetId { get; set; }

        /// <summary>
        /// 用户类型/分组 名称
        /// </summary>
        public string UserSetName { get; set; }

        /// <summary>
        /// 用户集合类型 1：用户类型，2：用户分组
        /// </summary>
        public int UserSetType { get; set; }

        /// <summary>
        /// 授权应用
        /// </summary>
        public string AuthApps { get; set; }

        /// <summary>
        /// 已授权应用ID集合
        /// </summary>
        public List<string> AuthAppIds { get; set; }
    }
}
