/*********************************************************
 * 名    称：MicroApplicationService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/2 13:44:56
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.Open.Common.Dtos;
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.Services.Dtos.Application;
using SmartLibrary.Open.Services.Dtos.Infomation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public interface IApplicationService
    {
        Task<PagedList<AppSearchViewModel>> AppSearch(AppSearchQuery queryFilter);
        Task<Guid> Create(ApplicationDto appDto);
        Task<bool> Delete(Guid appID);
        bool DisableApplication(List<Guid> appIds);
        bool EnableApplication(List<Guid> appIds);
        Task<AppViewModel> GetById(Guid appId);
        Task<AppInitModel> GetDictionary();
        Task<PagedList<AppCustomerTableViewModel>> QueryAppCustomerTableData(AppCustomerTableQuery queryFilter);
        Task<PagedList<AppTableViewModel>> QueryTableData(AppTableQuery queryFilter);
        Task<PagedList<AppDynamicInfoViewModel>> QueryVersionRecordData(VersionTableQuery queryFilter);
        Task<Guid> Update(ApplicationDto appDto);
        Task<AppSearchViewModel> GetAppDetail(string appId);
        Task<List<SysDictModel>> GetDictionaryByType(string type);
        Task<PagedList<AppDynamicInfoViewModel>> GetAppLog(AppLogQuery queryFilter);
        Task<AppDynamicInfoViewModel> GetAppLogDetail(string id);
        Task<PagedList<AppListViewModel>> GetAppList(AppSearchQuery queryFilter);
        Task<PagedList<PayAppListItemDto>> GetPayAppList(PayAppTableQuery queryFilter);
        void AddTestData();
        void AddCustomerAndAppUse();

        void AddAppEntrance();
        void AddAppWidget();
        void AddAppTestData();

        /// <summary>
        /// 添加重大需要的应用,全部跳转到2.2版本
        /// </summary>
        void AddTestAppsForCqu();

        /// <summary>
        /// 添加快应用中心应用
        /// </summary>
        void AddQuickAppForCqu();

        /// <summary>
        /// 添加应用更新日志
        /// </summary>
        void AddTestAppLogs();
    }
}