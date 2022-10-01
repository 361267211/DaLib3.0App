using Furion.DatabaseAccessor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.AppCenter.Application.Dtos.Application;
using SmartLibrary.AppCenter.Application.Services.Application;
using SmartLibrary.AppCenter.Common.BaseService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.AppServices
{
    /// <summary>
    /// 后台应用接口
    /// </summary>
    [Authorize(Policy = "back")]
    public class ApplicationAppService : BaseAppService
    {
        /// <summary>
        /// 应用服务
        /// </summary>
        private readonly IApplicationService _ApplicationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationService"></param>
        public ApplicationAppService(IApplicationService applicationService)
        {
            _ApplicationService = applicationService;
        }

        /// <summary>
        /// 获取所有应用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AppListDto>> GetAllApp()
        {
            return await _ApplicationService.GetAllApp();
        }

        /// <summary>
        /// 导航栏目选择应用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AppListDto>> GetNavigationApp()
        {
            return await _ApplicationService.GetNavigationApp();
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async Task<bool> InsertRole([FromBody] RoleDto dto)
        {
            return await _ApplicationService.InsertRole(dto);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> UpdateRole([FromBody] RoleDto dto)
        {
            return await _ApplicationService.UpdateRole(dto);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> DeleteRole([FromBody] List<string> guids)
        {
            return await _ApplicationService.DeleteRole(guids);
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<RoleDto> GetRoleDetail(Guid id)
        {
            return await _ApplicationService.GetRole(id);
        }

        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<RoleListDto>> GetRoleLists(int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetRoleList(pageIndex, pageSize);
        }

        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<RoleListDto>> GetAllRole()
        {
            return await _ApplicationService.GetAllRole();
        }


        /// <summary>
        /// 分页获取应用更新日志
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<AppLogDto>> GetAppLogs(int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetAppLogs(pageIndex, pageSize);
        }

        /// <summary>
        /// 获取日志更新详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AppLogDetailDto> GetAppLogDetail(string id)
        {
            return await _ApplicationService.GetAppLogDetail(id);
        }

        /// <summary>
        /// 分页获取应用到期列表，包含获取总数
        /// </summary>
        [HttpGet]
        public async Task<PagedList<AppExpireDto>> GetAppExpire(int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetAppExpire(pageIndex, pageSize);
        }

        /// <summary>
        /// 应用检索和应用列表
        /// </summary>
        /// <param name="key">检索词</param>
        /// <param name="appType">应用类型</param>
        /// <param name="purchaseType">采购类型</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<AppListDto>> GetAppSearchList(string key, string appType, string purchaseType, int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetAppSearchList(key, appType, purchaseType, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取应用详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AppDetailDto> GetAppDetail(string id)
        {
            return await _ApplicationService.GetAppDetail(id);
        }

        /// <summary>
        /// 分页获取推荐付费应用
        /// </summary>
        /// <param name="appType">应用类型</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<AppPayDto>> GetPayAppList(string appType, int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetPayAppList(appType, pageIndex, pageSize);
        }

        /// <summary>
        /// 应用信息管理，获取付费推荐应用
        /// </summary>
        [HttpGet]
        public async Task<List<AppPayDto>> GetPayAppForIndex()
        {
            return await _ApplicationService.GetPayAppForIndex();
        }

        /// <summary>
        /// 分页获取订单列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<OrderInfoDto>> GetOrderList(string key, string status, int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetOrderList(key, status, pageIndex, pageSize);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> CancelOrder(string id)
        {
            return await _ApplicationService.CancelOrder(id);
        }

        /// <summary>
        /// 应用续订/延期/免费试用/预约采购
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AppAction([FromBody] AppActionRequest request)
        {
            return await _ApplicationService.AppAction(request);
        }

        /// <summary>
        /// 应用 启用和停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AppChangeStatus([FromBody] AppStatusChangeDto dto)
        {
            return await _ApplicationService.AppChangeStatus(dto);
        }

        /// <summary>
        /// 应用改名
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AppReName([FromBody] AppReNameDto dto)
        {
            return await _ApplicationService.AppReName(dto);
        }

        /// <summary>
        /// 分页获取管理员权限列表
        /// </summary>
        /// <param name="key">姓名，卡号</param>
        /// <param name="status">账号状态</param>
        /// <param name="type">账号类型</param>
        /// <param name="role">角色</param>
        /// <param name="expire">到期时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<ManagerInfoDto>> GetManagerList(string key, string status, string type, string role, string expire, int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetManagerList(key, status, type, role, expire, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取管理员详情
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ManagerInfoDetailDto> GetManagerInfoDetail(string userKey)
        {
            return await _ApplicationService.GetManagerInfoDetail(userKey);
        }

        /// <summary>
        /// 修改管理员授权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> GrantManager([FromBody] ManagerInfoDetailUpdateDto dto)
        {
            return await _ApplicationService.GrantManager(dto);
        }


        /// <summary>
        /// 用户权限设置-按应用-应用授权列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="authType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<AuthAppDto>> GetAppUserByApp(string key, int authType, int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetAppUserByApp(key, authType, pageIndex, pageSize);
        }

        /// <summary>
        /// 用户权限设置-按读者-读者授权列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<AuthUserDto>> GetAppUserByUser(string key, int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetAppUserByUser(key, pageIndex, pageSize);
        }

        /// <summary>
        /// 用户-应用设置权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async Task<bool> UpdateAppUser([FromBody] AppAuthUpdateDto dto)
        {
            return await _ApplicationService.UpdateAppUser(dto);
        }

        /// <summary>
        /// 分页获取导航栏目
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<NavgationItemDto>> GetNavgationItems(int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetNavgationItems(pageIndex, pageSize);
        }

        /// <summary>
        /// 删除导航栏目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> DeleteNavgation(Guid id)
        {
            return await _ApplicationService.DeleteNavgation(id);
        }

        /// <summary>
        /// 获取导航栏目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<NavigationItemInfoDto> GetNavgationDetail(Guid id)
        {
            return await _ApplicationService.GetNavgationDetail(id);
        }

        /// <summary>
        /// 修改导航栏目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async Task<bool> UpdateNavigationItem([FromBody] NavigationItemUpdateDto dto)
        {
            return await _ApplicationService.UpdateNavigationItem(dto);
        }

        /// <summary>
        /// 分页获取三方应用列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<ThirdAppInfoDto>> GetThirdAppList(string key, int pageIndex, int pageSize)
        {
            return await _ApplicationService.GetThirdAppList(key, pageIndex, pageSize);
        }

        /// <summary>
        /// 删除三方应用(批量)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> DeleteThirdApp(List<string> ids)
        {
            return await _ApplicationService.DeleteThirdApp(ids);
        }

        /// <summary>
        /// 获取三方应用详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ThirdAppDetailDto> GetThirdAppDetail(string id)
        {
            return await _ApplicationService.GetThirdAppDetail(id);
        }

        /// <summary>
        /// 新增/修改三方应用信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async Task<bool> UpdateThirdAppInfo([FromBody] ThirdAppInfoUpdateDto dto)
        {
            return await _ApplicationService.UpdateThirdAppInfo(dto);
        }
    }
}
