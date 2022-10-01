using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 用户权限设置-按应用
    /// </summary>
    public class AuthAppDto
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
        /// 授权的读者
        /// </summary>
        public string AuthUsers { get; set; }

        /// <summary>
        /// 授权读者ID集合-按类型
        /// </summary>
        public List<string> AuthUserIdsByType { get; set; }

        /// <summary>
        /// 授权读者ID集合-按分组
        /// </summary>
        public List<string> AuthUserIdsByGroup { get; set; }
    }
}
