using SmartLibrary.AppCenter.Application.Dtos.Application;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.Application
{
    /// <summary>
    /// 应用服务
    /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> InsertRole(RoleDto dto);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateRole(RoleDto dto);

        /// <summary>
        /// 删除角色(批量)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteRole(List<string> ids);

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RoleDto> GetRole(Guid id);

        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<RoleListDto>> GetRoleList(int pageIndex, int pageSize);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        Task<List<RoleListDto>> GetAllRole();

        /// <summary>
        /// 获取应用更新日志
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<AppLogDto>> GetAppLogs(int pageIndex, int pageSize);

        /// <summary>
        /// 获取日志更新详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AppLogDetailDto> GetAppLogDetail(string id);

        /// <summary>
        /// 分页获取即将到期应用集合
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<AppExpireDto>> GetAppExpire(int pageIndex, int pageSize);

        /// <summary>
        /// 分页获取应用检索列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="appType"></param>
        /// <param name="purchaseType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<AppListDto>> GetAppSearchList(string key, string appType, string purchaseType, int pageIndex, int pageSize);

        /// <summary>
        /// 获取应用详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onlyAppInfo">是否只要应用信息</param>
        /// <returns></returns>
        Task<AppDetailDto> GetAppDetail(string id, bool onlyAppInfo = false);

        /// <summary>
        /// 分页获取推荐付费应用
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<AppPayDto>> GetPayAppList(string appType, int pageIndex, int pageSize);

        /// <summary>
        /// 应用信息管理，获取付费推荐应用
        /// </summary>
        /// <returns></returns>
        Task<List<AppPayDto>> GetPayAppForIndex();

        /// <summary>
        /// 分页获取订单
        /// </summary>
        /// <param name="key"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<OrderInfoDto>> GetOrderList(string key, string status, int pageIndex, int pageSize);

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> CancelOrder(string id);


        /// <summary>
        /// 应用续订/延期/免费试用/预约采购
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AppAction(AppActionRequest dto);

        /// <summary>
        /// 应用启用和停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<string> AppChangeStatus(AppStatusChangeDto dto);

        /// <summary>
        /// 应用改名
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AppReName(AppReNameDto dto);

        /// <summary>
        /// 分页获取管理员应用权限列表
        /// </summary>
        /// <param name="key">姓名，卡号，登录名</param>
        /// <param name="status">账号状态</param>
        /// <param name="type">账号类型</param>
        /// <param name="role">授权角色</param>
        /// <param name="expire">截止有效期</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<ManagerInfoDto>> GetManagerList(string key, string status, string type, string role, string expire, int pageIndex, int pageSize);

        /// <summary>
        /// 获取管理员详情
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<ManagerInfoDetailDto> GetManagerInfoDetail(string userKey);

        /// <summary>
        /// 修改管理员授权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> GrantManager(ManagerInfoDetailUpdateDto dto);

        /// <summary>
        /// 获取所有应用，用户权限设置使用
        /// </summary>
        /// <returns></returns>
        Task<List<AppListDto>> GetAllApp();

        /// <summary>
        /// 获取所有三方应用
        /// </summary>
        /// <returns></returns>
        Task<List<AppListDto>> GetAllThirdApp();

        /// <summary>
        /// 导航栏目设置选择应用
        /// </summary>
        /// <returns></returns>
        Task<List<AppListDto>> GetNavigationApp();

        /// <summary>
        /// 获取用户应用授权列表-按应用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="authType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<AuthAppDto>> GetAppUserByApp(string key, int authType, int pageIndex, int pageSize);

        /// <summary>
        /// 获取用户应用授权列表-按用户
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<AuthUserDto>> GetAppUserByUser(string key, int pageIndex, int pageSize);

        /// <summary>
        /// 用户应用授权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateAppUser(AppAuthUpdateDto dto);

        /// <summary>
        /// 分页获取导航栏目列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<NavgationItemDto>> GetNavgationItems(int pageIndex, int pageSize);

        /// <summary>
        /// 删除导航栏目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteNavgation(Guid id);

        /// <summary>
        /// 获取导航栏目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<NavigationItemInfoDto> GetNavgationDetail(Guid id);

        /// <summary>
        /// 修改导航栏目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateNavigationItem(NavigationItemUpdateDto dto);

        /// <summary>
        /// 分页获取三方应用列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PagedList<ThirdAppInfoDto>> GetThirdAppList(string key, int pageIndex, int pageSize);

        /// <summary>
        /// 删除三方应用
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteThirdApp(List<string> ids);

        /// <summary>
        /// 获取三方应用详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ThirdAppDetailDto> GetThirdAppDetail(string id);

        /// <summary>
        /// 新增/修改三方应用信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateThirdAppInfo(ThirdAppInfoUpdateDto dto);

        /// <summary>
        /// 通过appCode获取应用详情
        /// </summary>
        /// <param name="routeCode"></param>
        /// <returns></returns>
        Task<AppDetailDto> GetAppDetailByCode(string routeCode);
    }
}
