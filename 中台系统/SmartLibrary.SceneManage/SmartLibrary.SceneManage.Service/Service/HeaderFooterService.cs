/*********************************************************
 * 名    称：HeaderService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/21 22:06:42
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter;
using SmartLibrary.SceneManage.Common;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.Common.Utility;
using SmartLibrary.SceneManage.EntityFramework.Core.Entitys;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using SmartLibrary.SceneManage.Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Service.Service
{
    public class HeaderFooterService : BaseService, IScoped, IHeaderFooterService
    {


        /// <summary>
        /// 终端实例数据仓储
        /// </summary>
        private readonly IRepository<TerminalInstance> _terminalInstanceRepository;

        /// <summary>
        /// 主题色数据仓储
        /// </summary>
        private readonly IRepository<ThemeColor> _themeColorRepository;

        /// <summary>
        /// 场景分屏数据仓储
        /// </summary>
        private readonly IRepository<SceneScreen> _sceneScreenRepository;
        /// <summary>
        /// 头部模板仓储
        /// </summary>
        private readonly IRepository<HeadTemplateSetting> _headTemplateSettingsRepository;
        /// <summary>
        /// 尾部模板仓储
        /// </summary>
        private readonly IRepository<FootTemplateSetting> _footTemplateSettingsRepository;

        private readonly ISceneManageService _sceneManageService;

        /// <summary>
        /// 场景模板数据仓储
        /// </summary>
        private readonly IRepository<Template> _templateRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="terminalInstanceRepository"></param>
        /// <param name="themeColorRepository"></param>
        /// <param name="sceneScreenRepository"></param>
        public HeaderFooterService(IRepository<TerminalInstance> terminalInstanceRepository
            , IRepository<ThemeColor> themeColorRepository
            , IRepository<SceneScreen> sceneScreenRepository
            , IRepository<HeadTemplateSetting> headTemplateSettingsRepository
            , IRepository<FootTemplateSetting> footTemplateSettingsRepository
            , IGrpcClientResolver grpcClientResolver
            , ISceneManageService sceneManageService
             , IRepository<Template> templateRepository
            ) : base(grpcClientResolver)
        {
            _terminalInstanceRepository = terminalInstanceRepository;
            _themeColorRepository = themeColorRepository;
            _sceneScreenRepository = sceneScreenRepository;
            _headTemplateSettingsRepository = headTemplateSettingsRepository;
            _footTemplateSettingsRepository = footTemplateSettingsRepository;
            _sceneManageService = sceneManageService;
            _templateRepository = templateRepository;
        }

        /// <summary>
        /// 获取头部数据
        /// </summary>
        /// <returns></returns>
        public async Task<HeaderViewModel> GetHeaderData(string templateCode)
        {
            var code = _templateRepository.FirstOrDefault(e => e.Router == templateCode);
            if (code == null)
            {
                throw Oops.Oh("模板不存在");
            };
            HeadTemplateSettingDto templateSetting = await this.GetHeadTemplateSettingsById(code.Id.ToString());



            var majorNav = GetPortalMajorNavagationList("zf^k6Iy4XjBoSds");
            var mainNavigationList = majorNav.Result.NavigationList.Select(p => new HeaderNavigationViewModel
            {
                NavigationIcon = p.Icon,
                NavigationName = p.Name,
                NavigationUrl = p.Url,
                IsOpenNewWindow=p.IsOpenNewWindow,
            }).ToList();
            var result = new HeaderViewModel();
            /*{
                SiteLogo = "/uploads/cqu/scene/logo-text.png",
                SiteName = "维普智慧图书馆",
                SiteUrl = "http://192.168.21.46",
                MainNavigationList = !SiteGlobalConfig.AppBaseConfig.IsMock ? new List<HeaderNavigationViewModel> {
                    new HeaderNavigationViewModel { NavigationName="首页", NavigationIcon="/uploads/cqu/scene/icon1.png", NavigationUrl="http://192.168.21.46/#/index" },
                    new HeaderNavigationViewModel { NavigationName="图书", NavigationIcon="/uploads/cqu/scene/icon2.png", NavigationUrl="#"},
                    new HeaderNavigationViewModel { NavigationName="期刊", NavigationIcon="/uploads/cqu/scene/icon3.png", NavigationUrl="#"},
                    new HeaderNavigationViewModel { NavigationName="数据库", NavigationIcon="/uploads/cqu/scene/icon4.png", NavigationUrl="http://192.168.21.46/databaseguide/#/web_dataBaseHome"},
                    //new HeaderNavigationViewModel { NavigationName="课程文献中心", NavigationIcon="/uploads/cqu/scene/icon5.png", NavigationUrl="#"},
                    //new HeaderNavigationViewModel { NavigationName="馆藏", NavigationIcon="/uploads/cqu/scene/icon6.png", NavigationUrl="#"},
                    //new HeaderNavigationViewModel { NavigationName="学术成果", NavigationIcon="/uploads/cqu/scene/icon1.png", NavigationUrl="#" },
                    //new HeaderNavigationViewModel { NavigationName="信息素养", NavigationIcon="/uploads/cqu/scene/icon2.png", NavigationUrl="#"},
                    //new HeaderNavigationViewModel { NavigationName="捐赠", NavigationIcon="/uploads/cqu/scene/icon3.png", NavigationUrl="#"}
                } : mainNavigationList,
                PersonalLibrary = new HeaderNavigationViewModel { NavigationName = "我的图书馆", NavigationIcon = "/uploads/cqu/scene/icon1.png", NavigationUrl = "http://192.168.21.46/usermanage/#/web_library" },
                LogOn = new HeaderNavigationViewModel { NavigationName = "登录", NavigationIcon = "/uploads/cqu/scene/icon2.png", NavigationUrl = "#" },
                HelpInfo = new HeaderNavigationViewModel { NavigationName = "帮助", NavigationIcon = "/uploads/cqu/scene/icon3.png", NavigationUrl = "#" },



                EnglishSite = new HeaderNavigationViewModel { NavigationName = "English", NavigationIcon = "/uploads/cqu/scene/icon3.png", NavigationUrl = "http://222.198.130.102:8025/home/index" },
                OldSite = new HeaderNavigationViewModel { NavigationName = "旧站", NavigationIcon = "/uploads/cqu/scene/icon3.png", NavigationUrl = "http://lib.cqu.edu.cn/" },

            };*/

            result.SiteLogo = templateSetting.Logo;
            result.NavigationColumnList = new List< List<HeaderNavigationViewModel>>();
            foreach (var columnId in templateSetting.DisplayNavColumn)
            {
                var column = await this.GetPortalMajorNavagationList(columnId);
                var columnVo = column.NavigationList.Adapt<List<HeaderNavigationViewModel>>();
                result.NavigationColumnList.Add( columnVo);
            }


            return result;
        }

        /// <summary>
        /// 获取头部数据
        /// </summary>
        /// <returns></returns>
        public async Task<FootTemplateSettingDto> GetFooterData(string templateCode)
        {

            var code = _templateRepository.FirstOrDefault(e => e.Router == templateCode);
            if (code == null)
            {
                throw Oops.Oh("模板不存在");
            };
            FootTemplateSettingDto templateSetting = await this.GetFootTemplateSettingsById(code.Id.ToString());
            return templateSetting;
        }

        // public async Task<string> Get

        /// <summary>
        /// 获取头部模板高级设置项
        /// </summary>
        /// <param name="headTemplateId"></param>
        /// <returns></returns>
        public async Task<HeadTemplateSettingDto> GetHeadTemplateSettingsById(string headTemplateId)
        {
            var settings = _headTemplateSettingsRepository.FirstOrDefault(e => e.HeadTemplateId == new Guid(headTemplateId))?.Adapt<HeadTemplateSettingDto>();

            return settings == null ? new HeadTemplateSettingDto() : settings;
        }
        /// <summary>
        /// 获取底部模板高级设置项
        /// </summary>
        /// <param name="footTemplateId"></param>
        /// <returns></returns>
        public async Task<FootTemplateSettingDto> GetFootTemplateSettingsById(string footTemplateId)
        {
            var settings = _footTemplateSettingsRepository.FirstOrDefault(e => e.FootTemplateId == new Guid(footTemplateId))?.Adapt<FootTemplateSettingDto>();

            return settings == null ? new FootTemplateSettingDto() : settings;
        }

        /// <summary>
        /// 更新头部模板高级设置项
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public async Task UpdateHeadTemplateSettings(HeadTemplateSettingDto head)
        {
            var headEntity = head.Adapt<HeadTemplateSetting>();
            await _headTemplateSettingsRepository.UpdateNowAsync(headEntity);
        }

        /// <summary>
        /// 新增头部模板高级设置项
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public async Task AddHeadTemplateSettings(HeadTemplateSettingDto head)
        {
            var headEntity = head.Adapt<HeadTemplateSetting>();
            await _headTemplateSettingsRepository.InsertNowAsync(headEntity);
        }

        /// <summary>
        /// 更新底部模板高级设置项
        /// </summary>
        /// <param name="foot"></param>
        /// <returns></returns>
        public async Task UpdateFootTemplateSettings(FootTemplateSettingDto foot)
        {
            var headEntity = foot.Adapt<FootTemplateSetting>();
            await _footTemplateSettingsRepository.UpdateNowAsync(headEntity);
        }

        /// <summary>
        /// 新增底部模板高级设置项
        /// </summary>
        /// <param name="foot"></param>
        /// <returns></returns>
        public async Task AddFootTemplateSettings(FootTemplateSettingDto foot)
        {
            var headEntity = foot.Adapt<FootTemplateSetting>();
            await _footTemplateSettingsRepository.InsertNowAsync(headEntity);
        }

        /// <summary>
        /// 新增底部模板高级设置项
        /// </summary>
        /// <param name="foot"></param>
        /// <returns></returns>
        public async Task<List<SysDictModel<string>>> GetNavColumnList()
        {

            var request = new AppListRequest { AppServiceType = "1", TerminalType = 1, SceneType = 0 };
            var apps = await this.GetAppList(request);
            var appId = apps.AppList.FirstOrDefault(e => e.RouteCode == "navigation")?.AppId;

            var list = await _sceneManageService.GetAppPlateListByAppId(appId);
            return list;

        }

    }
}
