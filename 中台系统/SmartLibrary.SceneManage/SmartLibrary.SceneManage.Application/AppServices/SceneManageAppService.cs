/*********************************************************
 * 名    称：SceneManage
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/21 9:06:57
 * 描    述：场景管理聚合服务
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.DatabaseTerrace.Application.Filter;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.Service.Dtos;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using SmartLibrary.SceneManage.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Application.AppServices
{
    [Authorize(Policy = "DefaultPolicy")]
    public class SceneManageAppService:IDynamicApiController
    {
        private ISceneManageService _sceneManageService { get; set; }

        public SceneManageAppService(ISceneManageService sceneManageService)
        {
            _sceneManageService = sceneManageService;
        }

        /// <summary>
        /// 获取场景总览列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<SceneOverviewViewModel>> GetSceneOverview([FromQuery] SceneOverviewQuery queryFilter)
        {
            var result = await _sceneManageService.GetSceneOverview(queryFilter);
            return result;
        }

        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<TemplateListViewModel>> GetTemplateList([FromQuery] TemplateListQuery queryFilter)
        {
            var result = await _sceneManageService.GetTemplateList(queryFilter);
            return result;
        }

        /// <summary>
        /// 按终端获取场景列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<SceneListViewModel>> GetSceneListByTerminalId([FromQuery] SceneListQuery queryFilter)
        {
            var result = await _sceneManageService.GetSceneListByTerminalId(queryFilter);
            return result;
        }

        /// <summary>
        /// 获取场景详情
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<SceneDto> GetSceneDetail(string sceneId)
        {

            var result = await _sceneManageService.GetSceneDetail(sceneId);
            return result;
        }


        /// <summary>
        /// 添加场景
        /// </summary>
        /// <param name="sceneDto">应用创建数据</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async Task<string> CreateScene(SceneDto sceneDto)
        {

            var result = await _sceneManageService.CreateScene(sceneDto);
            return result.ToString();
        }

        /// <summary>
        /// 更新场景
        /// </summary>
        /// <param name="sceneDto">应用创建数据</param>
        /// <returns></returns>
        [HttpPut]
        [UnitOfWork]
        public async Task<string> UpdateScene([FromBody] SceneDto sceneDto)
        {

            var result = await _sceneManageService.UpdateScene(sceneDto);
            return result.ToString();
        }

        /// <summary>
        /// 删除场景
        /// </summary>
        /// <param name="sceneId">应用ID</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteScene(string sceneId)
        {
            var result = await _sceneManageService.DeleteScene(sceneId);
            return result;
        }

        /// <summary>
        /// 启用/禁用场景
        /// </summary>
        /// <param name="sceneId">应用ID</param>
        /// <param name="newStatus">新启用状态</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<bool> ChangeSceneStatus(string sceneId, int newStatus)
        {
            var result = await _sceneManageService.ChangeSceneStatus(sceneId, newStatus);
            return result;
        }

        /// <summary>
        /// 按服务类型获取应用列表
        /// </summary>
        /// <param name="appServiceType">服务类型字典值</param>
        /// <param name="terminalType">1-PC端、2-APP端、3-小程序端、4-自适应移动端、5-显示屏</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AppListViewModel>> GetAppListByServiceType(string appServiceType, int terminalType)
        {

            var result = await _sceneManageService.GetAppListByServiceType(appServiceType, terminalType);
            return result;
        }


        /// <summary>
        /// 获取应用组件列表
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AppWidgetListViewModel>> GetAppWidgetListByAppId(string appId)
        {
            var result = await _sceneManageService.GetAppWidgetListByAppId(appId, 0);
            return result;
        }


        /// <summary>
        /// 获取应用栏目列表
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<SysDictModel<string>>> GetAppPlateListByAppId(string appId)
        {
            var result = await _sceneManageService.GetAppPlateListByAppId(appId);
            return result;
        }

        /// <summary>
        /// 获取下拉框字典
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<DictionaryViewModel> GetDictionary()
        {
            var result = await _sceneManageService.GetDictionary();
            return result;
        }

        /// <summary>
        /// 按类型获取下拉框字典
        /// </summary>
        /// <param name="dicType">字典类型 1-学院 2-用户类型 3-用户分组</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<SysDictModel<string>>> GetDictionaryByType(int dicType)
        {
            var result = await _sceneManageService.GetDictionaryByType(dicType);
            return result;
        }

        /// <summary>
        /// 获取场景内所有栏目列表
        /// </summary>
        /// <param name="sceneId">场景Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AppPlateViewModel>> GetAppPlateListBySceneId(string sceneId)
        {
            var result = await _sceneManageService.GetAppPlateListBySceneId(sceneId);
            return result;
        }

        /// <summary>
        /// 获取当前场景访问地址
        /// </summary>
        /// <param name="sceneId">场景Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GetSceneUrlById(string sceneId)
        {
            var result = await _sceneManageService.GetSceneUrlById(sceneId);
            return result;
        }
    }
}
