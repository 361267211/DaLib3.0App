using SmartLibrary.AppCenter.EntityFramework.Core.Dto.Permission;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.Permission
{
    public interface ISysMenuService
    {
        /// <summary>
        /// 获取本应用中所有的权限列表
        /// </summary>
        /// <returns></returns>
        Task<PermissionNode> GetAllPermissionTree();

        /// <summary>
        /// 获取用户权限树
        /// </summary>
        /// <returns></returns>
        Task<PermissionNode> GetUserPermissionTree();

        /// <summary>
        /// 获取用户API调用权限列表
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetUserPermissionList();
    }
}
