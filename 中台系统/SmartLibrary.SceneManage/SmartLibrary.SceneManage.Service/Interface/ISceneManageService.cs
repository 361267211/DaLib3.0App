/*********************************************************
 * 名    称：SceneManageService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/21 9:46:10
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.Service.Dtos;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Service.Service
{
    public interface ISceneManageService
    {
        Task<Guid> CreateScene(SceneDto sceneDto);
        Task<bool> DeleteScene(string sceneId);
        Task<bool> ChangeSceneStatus(string sceneId, int newStatus);
        Task<List<AppListViewModel>> GetAppListByServiceType(string appServiceType, int terminalType);
        Task<List<SysDictModel<string>>> GetAppPlateListByAppId(string appId);
        Task<List<AppPlateViewModel>> GetAppPlateListBySceneId(string sceneId);
        Task<List<AppWidgetListViewModel>> GetAppWidgetListByAppId(string appId, int sceneType);
        Task<DictionaryViewModel> GetDictionary();
        Task<List<SysDictModel<string>>> GetDictionaryByType(int dicType);
        Task<AppWidgetListViewModel> GetPersonalAppWidgetByAppId(string appId);
        Task<List<AppListViewModel>> GetPersonalAppList(int terminalType);
        Task<SceneDto> GetPersonalSceneDetail(string userKey);
        Task<SceneDto> GetSceneDetail(string sceneId);
        Task<PagedList<SceneListViewModel>> GetSceneListByTerminalId(SceneListQuery queryFilter);
        Task<List<SceneOverviewViewModel>> GetSceneOverview(SceneOverviewQuery queryFilter);
        Task<string> GetSceneUrlById(string sceneId);
        Task<List<SysDictModel<int>>> GetSceneUseageByColumnId(List<string> columnIdList);
        Task<TemplateListViewModel> GetTemplateDetail(string templateId);
        Task<PagedList<TemplateListViewModel>> GetTemplateList(TemplateListQuery queryFilter);
        Task<Guid> UpdateScene(SceneDto sceneDto);
        Task<Guid> SavePersonalScene(SceneDto sceneDto);
        Task<DefaultHeaderFooterViewModel> GetDefaultTemplateList();
        Task<bool> SetPersonalDefaultScene(string sceneId, int isDefault, string userKey);
        Task<DefaultSceneViewModel> GetPersonalDefaultScene(string userKey);
    }
}