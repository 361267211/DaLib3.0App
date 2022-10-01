using SmartLibrary.Identity.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services.Interface
{
    /// <summary>
    /// 用户权限服务
    /// </summary>
    public interface IUserPermissionService
    {
        Task<AppUserInfo> GetUserInfo(string userKey);
        Task<AppUserPermission> GetUserPermission(string userKey);
    }
}
