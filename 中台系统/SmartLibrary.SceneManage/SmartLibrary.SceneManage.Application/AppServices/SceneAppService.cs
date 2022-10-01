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
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.DatabaseTerrace.Application.Filter;
using SmartLibrary.SceneManage.Common;
using SmartLibrary.SceneManage.Common.Const;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.Service.Dtos;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using SmartLibrary.SceneManage.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Application.AppServices
{
    //[AuthorizeMultiplePolicy("DefaultPolicy", false)]
    public class SceneAppService : BaseService, IDynamicApiController
    {
        private ISceneManageService _sceneManageService { get; set; }
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        private const string UnAuthKey = "UnAuth";

        public SceneAppService(ISceneManageService sceneManageService
            , IHttpContextAccessor httpContextAccessor
            , IGrpcClientResolver grpcClientResolver) : base(grpcClientResolver)
        {
            _sceneManageService = sceneManageService;
            _httpContextAccessor = httpContextAccessor;
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
        /// 获取场景详情
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<SceneDto> GetSceneDetail(string sceneId)
        {
            var scene = await _sceneManageService.GetSceneDetail(sceneId);
            if (scene.Status == 1)
            {
                throw Oops.Oh("此场景不存在").StatusCode(499);
            }
            if (scene.VisitorLimitType != 0)
            {
                var userKey = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtFields.UserKey)?.Value;
                if (string.IsNullOrEmpty(userKey))
                {
                    _httpContextAccessor.HttpContext.Response.Headers[UnAuthKey] = "1";
                    _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    throw Oops.Oh("此场景需登录后使用").StatusCode((int)HttpStatusCode.Forbidden);
                }
                var user = await GetUserByKey(userKey);
                switch (scene.VisitorLimitType)
                {
                    case 2:
                        if (!scene.SceneUsers.Where(p => p.UserSetType == 1).Select(p => p.UserSetId).Contains(user.College))
                        {
                            throw Oops.Oh("无权限").StatusCode((int)HttpStatusCode.BadRequest);
                        }
                        break;
                    case 3:
                        if (!scene.SceneUsers.Where(p => p.UserSetType == 2).Select(p => p.UserSetId).Contains(user.Type))
                        {
                            throw Oops.Oh("无权限").StatusCode((int)HttpStatusCode.BadRequest);
                        }
                        break;
                    case 4:
                        if (!scene.SceneUsers.Where(p => p.UserSetType == 3).Select(p => p.UserSetId).Any(p=>user.GroupIds.Contains(p)))
                        {
                            throw Oops.Oh("无权限").StatusCode((int)HttpStatusCode.BadRequest);
                        }
                        break;
                    default:
                        break;
                }
            }
            return scene;
        }


        /// <summary>
        /// 获取当前用户的个人中心场景
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<SceneDto> GetPersonalSceneDetail()
        {
            var userKey = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtFields.UserKey)?.Value;
            if (string.IsNullOrEmpty(userKey))
            {
                throw Oops.Oh("缺少UserKey").StatusCode((int)HttpStatusCode.BadRequest);
            }
            var result = await _sceneManageService.GetPersonalSceneDetail(userKey);
            return result;
        }

        /// <summary>
        /// 按服务类型获取应用列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AppListViewModel>> GetPersonalAppList(int terminalType)
        {

            var result = await _sceneManageService.GetPersonalAppList(terminalType);
            return result;
        }


        /// <summary>
        /// 获取应用组件列表
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AppWidgetListViewModel> GetPersonalAppWidgetByAppId(string appId)
        {
            var result = await _sceneManageService.GetPersonalAppWidgetByAppId(appId);
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
        /// 更新个人中心场景
        /// </summary>
        /// <param name="sceneDto">应用创建数据</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async Task<string> SavePersonalScene(SceneDto sceneDto)
        {
            sceneDto.IsSystemScene = false;
            var userKey = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtFields.UserKey)?.Value;
            if (string.IsNullOrEmpty(userKey))
            {
                throw Oops.Oh("缺少UserKey").StatusCode((int)HttpStatusCode.BadRequest);
            }
            sceneDto.UserKey = userKey;
            var result = await _sceneManageService.SavePersonalScene(sceneDto);
            return result.ToString();
        }

        /// <summary>
        /// 设置/取消个人默认首页
        /// </summary>
        /// <param name="sceneId">场景id</param>
        /// <param name="isDefault">是否设为默认首页1-是 0-否</param>
        /// <returns></returns>
        [HttpPut]
        [UnitOfWork]
        public async Task<string> SetPersonalDefaultScene(string sceneId, int isDefault)
        {
            var userKey = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtFields.UserKey)?.Value;
            if (string.IsNullOrEmpty(userKey))
            {
                throw Oops.Oh("缺少UserKey").StatusCode((int)HttpStatusCode.BadRequest);
            }
            var result = await _sceneManageService.SetPersonalDefaultScene(sceneId,isDefault,userKey);
            return result.ToString();
        }

        /// <summary>
        /// 获取个人默认首页场景id
        /// </summary>
        /// <returns></returns>
        public async Task<DefaultSceneViewModel> GetPersonalDefaultScene()
        {
            var userKey = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtFields.UserKey)?.Value;
            
            var result = await _sceneManageService.GetPersonalDefaultScene(userKey);
            return result;
        }
    }
}
