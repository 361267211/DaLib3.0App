using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter.Application.Dtos.SceneUse;
using SmartLibrary.AppCenter.Application.Services.Application;
using SmartLibrary.AppCenter.Application.Services.Common;
using SmartLibrary.AppCenter.Common.BaseService;
using SmartLibrary.AppCenter.Common.Enums;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.SceneUse
{
    /// <summary>
    /// 应用中心-前台接口
    /// </summary>
    public class SceneUseService : ServiceBase, ISceneUseService, IScoped
    {
        private readonly IRepository<Navigation> _RepositoryNavigation;
        private readonly IRepository<AppNavigation> _RepositoryAppNavigation;
        private readonly ApplicationService _ApplicationService;
        private readonly GeneralService _GeneralService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repositoryNavgation"></param>
        /// <param name="repositoryAppNavigation"></param>
        /// <param name="applicationService"></param>
        /// <param name="generalService"></param>
        public SceneUseService(IRepository<Navigation> repositoryNavgation,
                               IRepository<AppNavigation> repositoryAppNavigation,
                               ApplicationService applicationService,
                               GeneralService generalService)
        {
            _RepositoryNavigation = repositoryNavgation;
            _RepositoryAppNavigation = repositoryAppNavigation;
            _ApplicationService = applicationService;
            _GeneralService = generalService;
        }

        /// <summary>
        /// 根据栏目ID获取应用列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<SceneUseItemDto> GetSceneUseItemById(SceneUseRequsetParameter parameter)
        {
            var result = new SceneUseItemDto();

            var navigation = await _RepositoryNavigation.FirstOrDefaultAsync(c => c.Id.ToString() == parameter.ColumnId);
            if (navigation != null)
            {
                result.Id = navigation.Id.ToString();
                result.Name = navigation.Name;

                var appNavigations = await _RepositoryAppNavigation.Where(c => c.NavigationId == navigation.Id.ToString() && !c.DeleteFlag).ToListAsync();
                if (appNavigations != null && appNavigations.Any())
                {
                    var orderType = 0;
                    var orderFiled = "";
                    var isAsc = true;
                    if (!string.IsNullOrWhiteSpace(parameter.OrderRule))
                    {
                        var orderRuleArray = parameter.OrderRule.Split('-');
                        if (orderRuleArray.Length == 2)
                        {
                            orderFiled = orderRuleArray[0];
                            isAsc = orderRuleArray[1] == "ASC";

                            if (orderFiled == "OrderIndex" && isAsc) orderType = 0;
                            else if (orderFiled == "OrderIndex" && !isAsc) orderType = 1;
                            else if (orderFiled == "VisitCount" && isAsc) orderType = 2;
                            else if (orderFiled == "VisitCount" && !isAsc) orderType = 3;
                            else if (orderFiled == "CreatedTime" && isAsc) orderType = 4;
                            else if (orderFiled == "CreatedTime" && !isAsc) orderType = 5;
                        }
                        else
                        {
                            orderType = 1;
                        }
                    }

                    appNavigations = orderType switch
                    {
                        //默认
                        0 => appNavigations.OrderBy(c => c.OrderIndex).ToList(),
                        //默认升序
                        1 => appNavigations.OrderByDescending(c => c.OrderIndex).ToList(),
                        //访问量
                        2 => appNavigations.OrderBy(c => c.OrderIndex).ToList(),
                        //访问量 升序
                        3 => appNavigations.OrderByDescending(c => c.OrderIndex).ToList(),
                        //创建时间
                        4 => appNavigations.OrderBy(c => c.CreatedTime).ToList(),
                        //创建时间 升序
                        5 => appNavigations.OrderByDescending(c => c.CreatedTime).ToList(),
                        //默认
                        _ => appNavigations.OrderByDescending(c => c.OrderIndex).ToList(),
                    };
                    appNavigations = appNavigations.Take(parameter.Count).ToList();

                    var allApps = await _ApplicationService.GetAllApp();
                    var appCenter = allApps.FirstOrDefault(c => c.RouteCode == "appcenter");
                    //应用中心地址前面半截
                    var appCenterPreUrl = appCenter?.FrontUrl.Split('#')[0];

                    result.Apps = new List<SceneUseAppInfoDto>();
                    foreach (var item in appNavigations)
                    {
                        var info = allApps.FirstOrDefault(c => c.AppId == item.AppId);
                        if (info != null)
                        {
                            result.Apps.Add(new SceneUseAppInfoDto
                            {
                                AppId = info.AppId,
                                AppName = info.AppName,
                                AppIcon = info.AppIcon,
                                FrontUrl = info.Terminal == ((int)TerminalEnum.H5).ToString() ? $"{appCenterPreUrl}#/web_appsDetails?id={info.AppId}" : info.FrontUrl,
                            });
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 批量获取栏目内容
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<SceneUseDto> GetSceneUseByIdBatch(List<SceneUseRequsetParameter> parameters)
        {
            var result = new SceneUseDto() { Items = new List<SceneUseItemDto>() };

            var allApps = await _ApplicationService.GetAllApp();

            var currentApp = allApps.FirstOrDefault(c => c.RouteCode == "appcenter");
            if (currentApp != null)
            {
                result.MoreUrl = currentApp.FrontUrl.Split('#')[0] + "#/web_appsCenter";
            }

            foreach (var parameter in parameters)
            {
                result.Items.Add(await GetSceneUseItemById(parameter));
            }

            return result;
        }

        /// <summary>
        /// 获取首页应用
        /// </summary>
        /// <returns></returns>
        public async Task<SceneUseDto> GetAppForIndex()
        {
            var result = new SceneUseDto() { Items = new List<SceneUseItemDto>() };

            var allApps = await _ApplicationService.GetAllApp();
            var allTypes = await _GeneralService.GetAppType();

            //获取应用中心地址
            var currentApp = allApps.FirstOrDefault(c => c.RouteCode == "appcenter");
            var appCenterPreUrl = currentApp?.FrontUrl.Split('#')[0];
            result.MoreUrl = appCenterPreUrl + "#web_appsCenter";

            var types = new List<string> { "2", "3", "4" };
            foreach (var type in types)
            {
                var item = new SceneUseItemDto
                {
                    Id = type,
                    Name = allTypes.FirstOrDefault(p => p.Value == type)?.Name
                };
                item.Apps = allApps.Where(p => p.AppType == type).Select(p => new SceneUseAppInfoDto
                {
                    AppIcon = p.AppIcon,
                    AppId = p.AppId,
                    AppName = p.AppName,
                    FrontUrl = p.Terminal == ((int)TerminalEnum.H5).ToString() ? $"{appCenterPreUrl}#/web_appsDetails?id={p.AppId}" : p.FrontUrl
                }).ToList();

                result.Items.Add(item);
            }

            return result;
        }
    }
}
