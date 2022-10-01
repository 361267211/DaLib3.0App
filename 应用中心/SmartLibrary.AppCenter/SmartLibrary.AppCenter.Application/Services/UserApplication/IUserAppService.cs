using SmartLibrary.AppCenter.Application.Dtos.Application;
using SmartLibrary.AppCenter.Application.Dtos.BaseInfo;
using SmartLibrary.AppCenter.Application.Dtos.UserApplication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.UserApplication
{
    /// <summary>
    /// 个人应用中心
    /// </summary>
    public interface IUserAppService
    {
        /// <summary>
        /// 收藏应用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> CollectionApp(string id);

        /// <summary>
        /// 删除已经收藏的应用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteMyApp(string id);

        /// <summary>
        /// 获取应用详情
        /// </summary>
        /// <param name="id">应用ID</param>
        /// <returns></returns>
        Task<AppDetailDto> GetAppDetail(string id);

        /// <summary>
        /// 获取我的应用(已经收藏的应用)
        /// </summary>
        /// <returns></returns>
        Task<List<MyCollectionAppDto>> GetMyCollectionApps();

        /// <summary>
        /// 获取推荐应用
        /// </summary>
        /// <returns></returns>
        Task<List<RecommendAppDto>> GetRecommendApps();

        /// <summary>
        /// 获取推荐应用，带应用中心跳转地址
        /// </summary>
        /// <returns></returns>
        Task<RecommendAppMoreDto> GetRecommendAppMore();

        /// <summary>
        /// 获取所有应用
        /// </summary>
        /// <returns></returns>
        Task<List<AllAppDto>> GetAllApps();

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<UserInfo> GetCurrentUserInfo(string userKey);

        /// <summary>
        /// 判断用户对指定应用是否有使用权限(前台)
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="appRouteCode"></param>
        /// <returns></returns>
        Task<UserAppPermissionReply> GetUserAppPermission(string userKey, string appRouteCode);

        /// <summary>
        /// 根据userkey获取指定应用的权限类型
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="appRouteCode"></param>
        /// <returns></returns>
        Task<UserAppPermissionTypeReply> GetUserAppPermissionType(string userKey, string appRouteCode);

        /// <summary>
        /// 获取顶部菜单
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<List<AppMenuListDto>> GetMgrTopMenu(string userKey);

        /// <summary>
        /// 根据routecode获取当前应用的名称和版本号
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        Task<CurrentAppInfo> GetCurrentAppInfo(string appCode);
    }
}
