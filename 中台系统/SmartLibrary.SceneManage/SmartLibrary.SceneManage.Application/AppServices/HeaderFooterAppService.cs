/*********************************************************
 * 名    称：HeaderFooterAppService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/22 22:21:10
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.AppCenter;
using SmartLibrary.DatabaseTerrace.Application.Filter;
using SmartLibrary.SceneManage.Common;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.EntityFramework.Core.Entitys;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using SmartLibrary.SceneManage.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartLibrary.AppCenter.AppCenterGrpcService;

namespace SmartLibrary.SceneManage.Application.AppServices
{
    //[AuthorizeMultiplePolicy("DefaultPolicy", false)]
    public class HeaderFooterAppService : IDynamicApiController
    {
        private IHeaderFooterService _headerFooterService { get; set; }
        private ISceneManageService _sceneManageService { get; set; }
        /// <summary>
        /// 场景模板数据仓储
        /// </summary>
        private readonly IRepository<Template> _templateRepository;

        public HeaderFooterAppService(IHeaderFooterService headerFooterService,
            ISceneManageService sceneManageService,
            IRepository<Template> templateRepository
            )
        {
            _templateRepository = templateRepository;
            _sceneManageService = sceneManageService;
            _headerFooterService = headerFooterService;
        }

        /// <summary>
        /// 获取头部信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [QueryParameters]
        public async Task<HeaderViewModel> GetHeaderData(string templateCode)
        {
            var result = await _headerFooterService.GetHeaderData(  templateCode);
            return result;
        }

        /// <summary>
        /// 获取底部信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [QueryParameters]
        public async Task<FootTemplateSettingDto> GetFooterData(string templateCode)
        {
            var result = await _headerFooterService.GetFooterData(  templateCode);
            return result;
        }

        /// <summary>
        /// 获取头部模板高级设置项
        /// </summary>
        /// <param name="headTemplateId"></param>
        /// <returns></returns>
        [HttpGet]
        [QueryParameters]
        public async Task<HeadTemplateSettingDto> GetHeadTemplateSettingsById(string headTemplateId)
        {
            var settings = await _headerFooterService.GetHeadTemplateSettingsById(headTemplateId);

            return settings == null ? new HeadTemplateSettingDto() : settings;
        }

        /// <summary>
        /// 获取底部模板高级设置项
        /// </summary>
        /// <param name="footTemplateId"></param>
        /// <returns></returns>
        [HttpGet]
        [QueryParameters]
        public async Task<FootTemplateSettingDto> GetFootTemplateSettingsById(string footTemplateId)
        {
            var settings = await _headerFooterService.GetFootTemplateSettingsById(footTemplateId);


            return settings == null ? new FootTemplateSettingDto() : settings;
        }

        /// <summary>
        /// 更新头部模板高级设置项
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task HeadTemplateSettingsUpdate([FromBody] HeadTemplateSettingDto head)
        {

            await _headerFooterService.UpdateHeadTemplateSettings(head);
        }

        /// <summary>
        /// 新增头部模板高级设置项
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task HeadTemplateSettingsAdd([FromBody] HeadTemplateSettingDto head)
        {
            await _headerFooterService.AddHeadTemplateSettings(head);

        }

        /// <summary>
        /// 更新底部模板高级设置项
        /// </summary>
        /// <param name="foot"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task FootTemplateSettingsUpdate([FromBody] FootTemplateSettingDto foot)
        {
            await _headerFooterService.UpdateFootTemplateSettings(foot);

        }

        /// <summary>
        /// 新增底部模板高级设置项
        /// </summary>
        /// <param name="foot"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task FootTemplateSettingsAdd([FromBody] FootTemplateSettingDto foot)
        {
            await _headerFooterService.AddFootTemplateSettings(foot);

        }


        /// <summary>
        /// 获取信息导航栏目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [QueryParameters]
        public async Task<List<SysDictModel<string>>> GetNavColumnList()
        {
 
            return await _headerFooterService.GetNavColumnList();
 
        }
    }
}
