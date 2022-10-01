using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public string Id {  get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 应用权限集合
        /// </summary>
        public List<AppAuth> AppAuths { get; set; }
    }

    /// <summary>
    /// 应用权限
    /// </summary>
    public class AppAuth
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 管理员类型
        /// 1：管理员，2：操作员，3：浏览者
        /// </summary>
        public int ManagerType { get; set; }
    }
}
