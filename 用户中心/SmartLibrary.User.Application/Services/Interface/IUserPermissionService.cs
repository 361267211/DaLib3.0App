/*********************************************************
* 名    称：IUserPermissionService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户权限管理服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 用户权限服务
    /// </summary>
    public interface IUserPermissionService
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<AppUserInfo> GetUserInfo(string userKey);
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<AppUserPermission> GetUserPermission(string userKey);
        /// <summary>
        /// 获取读者前台权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<AppReaderPermission> GetReaderPermission(string userKey);
        /// <summary>
        /// 检查读者修改信息权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<bool> CheckModifyReaderInfo(string userKey);
        /// <summary>
        /// 检查读者领卡权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<bool> CheckCardClaimPermit(string userKey);
    }
}
