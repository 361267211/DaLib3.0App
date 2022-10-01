/*********************************************************
* 名    称：IUserPermissionService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：用户权限服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.ScoreCenter.Application.Dtos;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Interface
{
    /// <summary>
    /// 用户权限服务
    /// </summary>
    public interface IUserPermissionService
    {
        /// <summary>
        /// 获取读者信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<AppUserInfo> GetUserInfo(string userKey);
        /// <summary>
        /// 获取馆员权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<AppUserPermission> GetUserPermission(string userKey);
        /// <summary>
        /// 获取读者权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<AppReaderPermission> GetReaderPermission(string userKey);
    }
}
