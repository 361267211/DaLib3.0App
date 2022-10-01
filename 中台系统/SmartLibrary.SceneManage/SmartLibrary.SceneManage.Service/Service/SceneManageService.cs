/*********************************************************
 * 名    称：SceneManageService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/21 9:46:10
 * 描    述：场景管理服务
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter;
using SmartLibrary.SceneManage.Common;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.Common.Utility;
using SmartLibrary.SceneManage.EntityFramework.Core.Entitys;
using SmartLibrary.SceneManage.Service.Dtos;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using SmartLibrary.SceneManage.Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static SmartLibrary.AppCenter.AppCenterGrpcService;

namespace SmartLibrary.SceneManage.Service.Service
{
    /// <summary>
    /// 场景管理服务
    /// </summary>
    public class SceneManageService : BaseService, IScoped, ISceneManageService
    {
        /// <summary>
        /// 场景数据仓储
        /// </summary>
        private readonly IRepository<Scene> _sceneRepository;

        /// <summary>
        /// 场景布局数据仓储
        /// </summary>
        private readonly IRepository<Layout> _layoutRepository;

        /// <summary>
        /// 场景内应用数据仓储
        /// </summary>
        private readonly IRepository<SceneApp> _sceneAppRepository;

        /// <summary>
        /// 场景用户数据仓储
        /// </summary>
        private readonly IRepository<SceneUser> _sceneUserRepository;

        /// <summary>
        /// 场景模板数据仓储
        /// </summary>
        private readonly IRepository<Template> _templateRepository;

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
        /// 场景内应用栏目数据仓储
        /// </summary>
        private readonly IRepository<SceneAppPlate> _sceneAppPlateRepository;

        /// <summary>
        /// 头部模板高级设置
        /// </summary>
        private readonly IRepository<HeadTemplateSetting> _headTemplateSettingsRepository;
        /// <summary>
        /// 底部模板高级设置
        /// </summary>
        private readonly IRepository<FootTemplateSetting> _footTemplateSettingsRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sceneRepository"></param>
        /// <param name="layoutRepository"></param>
        /// <param name="sceneAppRepository"></param>
        /// <param name="sceneUserRepository"></param>
        /// <param name="templateRepository"></param>
        /// <param name="terminalInstanceRepository"></param>
        /// <param name="themeColorRepository"></param>
        /// <param name="sceneScreenRepository"></param>
        /// <param name="sceneAppPlateRepository"></param>
        /// <param name="grpcClientResolver"></param>
        public SceneManageService(IRepository<Scene> sceneRepository
            , IRepository<Layout> layoutRepository
            , IRepository<SceneApp> sceneAppRepository
            , IRepository<SceneUser> sceneUserRepository
            , IRepository<Template> templateRepository
            , IRepository<TerminalInstance> terminalInstanceRepository
            , IRepository<ThemeColor> themeColorRepository
            , IRepository<SceneScreen> sceneScreenRepository
            , IRepository<SceneAppPlate> sceneAppPlateRepository
            , IRepository<HeadTemplateSetting> headTemplateSettingsRepository
            , IRepository<FootTemplateSetting> footTemplateSettingsRepository
            , IGrpcClientResolver grpcClientResolver) : base(grpcClientResolver)
        {
            _sceneRepository = sceneRepository;
            _layoutRepository = layoutRepository;
            _sceneAppRepository = sceneAppRepository;
            _sceneUserRepository = sceneUserRepository;
            _templateRepository = templateRepository;
            _terminalInstanceRepository = terminalInstanceRepository;
            _themeColorRepository = themeColorRepository;
            _sceneScreenRepository = sceneScreenRepository;
            _sceneAppPlateRepository = sceneAppPlateRepository;
            _footTemplateSettingsRepository = footTemplateSettingsRepository;
            _headTemplateSettingsRepository = headTemplateSettingsRepository;
        }

        /// <summary>
        /// 获取场景总览列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<List<SceneOverviewViewModel>> GetSceneOverview(SceneOverviewQuery queryFilter)
        {
            var queryScene = _sceneRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var queryTermunalInstance = _terminalInstanceRepository.Where(p => !p.DeleteFlag).AsQueryable().OrderByDescending(p=>p.IsSystemInstance).AsNoTracking();

            var query = from tm in queryTermunalInstance
                        select new SceneOverviewViewModel
                        {
                            TerminalId = tm.Id.ToString(),
                            TerminalName = tm.Name,
                            TerminalType = tm.TerminalType,
                            SceneList = (from sc in queryScene
                                         where sc.TerminalInstanceId == tm.Id.ToString()
                                         && (sc.UserKey == null || sc.UserKey.ToLower() == "default")
                                        && (!queryFilter.Status.HasValue || sc.Status == queryFilter.Status)
                             && (!queryFilter.IsSystemScene.HasValue || sc.IsSystemScene == (queryFilter.IsSystemScene.Value == 1))
                                         select new SceneListViewModel
                                         {
                                             Cover = sc.Cover,
                                             Id = sc.Id.ToString(),
                                             IsSystemScene = sc.IsSystemScene,
                                             Name = sc.Name,
                                             Status = sc.Status,
                                             VisitUrl = sc.VisitUrl
                                         }).Take(queryFilter.TopCount).ToList()
                        };

            var table = await query.ToListAsync();
            return table;
        }

        /// <summary>
        /// 按终端获取场景列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SceneListViewModel>> GetSceneListByTerminalId(SceneListQuery queryFilter)
        {
            var queryScene = _sceneRepository.Where(p => !p.DeleteFlag).AsQueryable();
            var queryTermunalInstance = _terminalInstanceRepository.Where(p => !p.DeleteFlag).AsQueryable();

            var query = from sc in queryScene
                        where (string.IsNullOrEmpty(queryFilter.TerminalId) || sc.TerminalInstanceId == queryFilter.TerminalId)
                                && (sc.UserKey == null || sc.UserKey.ToLower() == "default")
                                && (!queryFilter.Status.HasValue || sc.Status == queryFilter.Status)
                             && (!queryFilter.IsSystemScene.HasValue || sc.IsSystemScene == (queryFilter.IsSystemScene.Value == 1))
                        select new SceneListViewModel
                        {
                            Cover = sc.Cover,
                            Id = sc.Id.ToString(),
                            IsSystemScene = sc.IsSystemScene,
                            Name = sc.Name,
                            Status = sc.Status,
                            VisitUrl = sc.VisitUrl
                        };
            if (!string.IsNullOrEmpty(queryFilter.SortField))
            {
                query = query.ApplyOrder(queryFilter.SortField, queryFilter.IsAsc);
            }
            var table = await query.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return table;
        }


        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <returns></returns>
        public async Task<PagedList<TemplateListViewModel>> GetTemplateList(TemplateListQuery queryFilter)
        {
            var queryTemplate = _templateRepository.Where(p => !p.DeleteFlag).AsQueryable();
            var appBaseUrl = await GetAppBaseUrl();

            var query = from tp in queryTemplate
                        where (!queryFilter.LayoutId.HasValue || tp.LayoutId == queryFilter.LayoutId.ToString())
                        && (!queryFilter.Type.HasValue || tp.Type == queryFilter.Type)
                        let ft = queryTemplate.FirstOrDefault(p => p.Id.ToString() == tp.DefaultFooterTemplateId)
                        let ht = queryTemplate.FirstOrDefault(p => p.Id.ToString() == tp.DefaultHeaderTemplateId)
                        select new TemplateListViewModel
                        {
                            Id = tp.Id.ToString(),
                            Type = tp.Type,
                            LayoutId = int.Parse(tp.LayoutId ?? "0"),
                            TemplateCode = tp.Router.TrimStart('/').Replace("/", "_"),
                            Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}/{tp.Router}",
                            //Router = $"SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl{tp.Router}",
                            Cover = tp.Cover,
                            Name = tp.Name,
                            DefaultHeaderTemplate = new TemplateListViewModel { Id = ht.Id.ToString(), TemplateCode = ht.Router.TrimStart('/').Replace("/", "_"), Cover = ht.Cover, Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}/{ht.Router}", Name = ht.Name },
                            DefaultFooterTemplate = new TemplateListViewModel { Id = ft.Id.ToString(), TemplateCode = ft.Router.TrimStart('/').Replace("/", "_"), Cover = ft.Cover, Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}/{ft.Router}", Name = ft.Name },
                            AspectRatio = tp.AspectRatio,
                            ColumnCount = tp.ColumnCount,
                            IsLock = tp.IsLock,
                            ScreenCount = tp.ScreenCount,
                            Width = tp.Width,
                            BackgroundColor = tp.BackgroundColor
                        };
            if (!string.IsNullOrEmpty(queryFilter.SortField))
            {
                query = query.ApplyOrder(queryFilter.SortField, queryFilter.IsAsc);
            }

            var table = await query.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return table;
        }


        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <returns></returns>
        public async Task<DefaultHeaderFooterViewModel> GetDefaultTemplateList()
        {
            var scene = await _sceneRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.SceneType == 1 && p.Status == 0);
            var userUcene = await _sceneRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.UserKey.ToLower() == "default" && p.Status == 0);
            var queryTemplate = _templateRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();

            var ft = await queryTemplate.FirstOrDefaultAsync(p => p.Id.ToString() == scene.FooterTemplateId);
            var ht = await queryTemplate.FirstOrDefaultAsync(p => p.Id.ToString() == scene.HeaderTemplateId);

            var appBaseUrl = await GetAppBaseUrl();

            var result = new DefaultHeaderFooterViewModel
            {
                ApiRouter = SiteGlobalConfig.AppBaseConfig.AppRouteCode,
                ThemeColor = scene.ThemeColor,
                UserCenterName = userUcene.Name,
                HeaderTemplate = ht == null ? new TemplateListViewModel() : new TemplateListViewModel { Id = ht.Id.ToString(), TemplateCode = ht.Router.TrimStart('/').Replace("/", "_"), Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}{ht.Router}", Name = ht.Name },
                FooterTemplate = ft == null ? new TemplateListViewModel() : new TemplateListViewModel { Id = ft.Id.ToString(), TemplateCode = ft.Router.TrimStart('/').Replace("/", "_"), Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}{ft.Router}", Name = ft.Name },
            };

            return result;
        }

        /// <summary>
        /// 获取模板详情
        /// </summary>
        /// <returns></returns>
        public async Task<TemplateListViewModel> GetTemplateDetail(string templateId)
        {
            var result = new TemplateListViewModel();

            var appBaseUrl = await GetAppBaseUrl();
            var tp = await _templateRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == templateId);
            if (tp == null)
            {
                return null;
            }
            //头尾模板
            if (tp.Type == 2 || tp.Type == 3)
            {
                var footsetting = await _footTemplateSettingsRepository.FirstOrDefaultAsync(e => e.FootTemplateId == tp.Id);
                var headsetting = await _headTemplateSettingsRepository.FirstOrDefaultAsync(e => e.HeadTemplateId == tp.Id);
                result = new TemplateListViewModel
                {
                    Id = tp.Id.ToString(),
                    Type = tp.Type,
                    LayoutId = int.Parse(tp.LayoutId),
                    TemplateCode = tp.Router.TrimStart('/').Replace("/", "_"),
                    Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}{tp.Router}",
                    Cover = $"{tp.Cover}",
                    Name = tp.Name,
                    AspectRatio = tp.AspectRatio,
                    ColumnCount = tp.ColumnCount,
                    IsLock = tp.IsLock,
                    ScreenCount = tp.ScreenCount,
                    Width = tp.Width,
                    BackgroundColor = tp.BackgroundColor,
                    Content = footsetting?.Content,
                    JsPath = footsetting?.JsPath,
                    DisplayNavColumn = headsetting?.DisplayNavColumn,
                    Logo = headsetting?.Logo,
                };

            }
            else if (tp.Type == 1)
            {
                var ft = await _templateRepository.FirstOrDefaultAsync(p => p.Id.ToString() == tp.DefaultFooterTemplateId);
                var ht = await _templateRepository.FirstOrDefaultAsync(p => p.Id.ToString() == tp.DefaultHeaderTemplateId);


                result = new TemplateListViewModel
                {
                    Id = tp.Id.ToString(),
                    Type = tp.Type,
                    LayoutId = int.Parse(tp.LayoutId),
                    TemplateCode = tp.Router.TrimStart('/').Replace("/", "_"),
                    Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}{tp.Router}",
                    Cover = $"{tp.Cover}",
                    Name = tp.Name,
                    DefaultHeaderTemplate = ht == null ? null : new TemplateListViewModel { Id = ht.Id.ToString(), TemplateCode = ht.Router.TrimStart('/').Replace("/", "_"), Cover = $"{ht.Cover}", Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}{ht.Router}", Name = ht.Name },
                    DefaultFooterTemplate = ft == null ? null : new TemplateListViewModel { Id = ft.Id.ToString(), TemplateCode = ft.Router.TrimStart('/').Replace("/", "_"), Cover = $"{ft.Cover}", Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}{ft.Router}", Name = ft.Name },
                    AspectRatio = tp.AspectRatio,
                    ColumnCount = tp.ColumnCount,
                    IsLock = tp.IsLock,
                    ScreenCount = tp.ScreenCount,
                    Width = tp.Width,
                    BackgroundColor = tp.BackgroundColor
                };

            }
            return result;
        }


        /// <summary>
        /// 获取场景详情
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public async Task<SceneDto> GetSceneDetail(string sceneId)
        {
            var queryScene = _sceneRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var querySceneScreen = _sceneScreenRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var querySceneUser = _sceneUserRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var querySceneApp = _sceneAppRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var queryTerminalInstance = _terminalInstanceRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var queryTemplate = _templateRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var querySceneAppPlate = _sceneAppPlateRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();

            var appBaseUrl = await GetAppBaseUrl();

            var query = from sc in queryScene
                        join tm in queryTerminalInstance on sc.TerminalInstanceId equals tm.Id.ToString()
                        where (sc.Id.ToString() == sceneId)
                        let ft = queryTemplate.FirstOrDefault(p => p.Id.ToString() == sc.FooterTemplateId)
                        let ht = queryTemplate.FirstOrDefault(p => p.Id.ToString() == sc.HeaderTemplateId)
                        let tp = queryTemplate.FirstOrDefault(p => p.Id.ToString() == sc.TemplateId)
                        select new SceneDto
                        {
                            Id = sc.Id.ToString(),
                            Cover = sc.Cover,
                            HeaderTemplate = new TemplateListViewModel { Id = ht.Id.ToString(), TemplateCode = ht.Router.TrimStart('/').Replace("/", "_"), Cover = $"{ht.Cover}", Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}{ht.Router}", Name = ht.Name },
                            FooterTemplate = new TemplateListViewModel { Id = ft.Id.ToString(), TemplateCode = ft.Router.TrimStart('/').Replace("/", "_"), Cover = $"{ft.Cover}", Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}{ft.Router}", Name = ft.Name },
                            IsSystemScene = sc.IsSystemScene,
                            LayoutId = int.Parse(sc.LayoutId),
                            Name = sc.Name,
                            Status = sc.Status,
                            IsPersonalIndex = sc.IsPersonalIndex,
                            Template = new TemplateListViewModel
                            {
                                Id = tp.Id.ToString(),
                                TemplateCode = tp.Router.TrimStart('/').Replace("/", "_"),
                                Cover = $"{tp.Cover}",
                                Router = $"{(SiteGlobalConfig.AppBaseConfig.IsMock ? SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl : appBaseUrl.FrontUrl)}{tp.Router}",
                                Name = tp.Name,
                                ColumnCount = tp.ColumnCount,
                                AspectRatio = tp.AspectRatio,
                                IsLock = tp.IsLock,
                                ScreenCount = tp.ScreenCount,
                                Width = tp.Width,
                                BackgroundColor = tp.BackgroundColor
                            },
                            TerminalInstanceId = sc.TerminalInstanceId,
                            TerminalInstanceName = tm.Name,
                            ThemeColor = sc.ThemeColor,
                            VisitorLimitType = sc.VisitorLimitType,
                            VisitUrl = sc.VisitUrl,
                            UserKey = sc.UserKey,
                            SceneScreens = (from ss in querySceneScreen
                                            where ss.SceneId == sc.Id.ToString()
                                            select new SceneScreenDto
                                            {
                                                Id = ss.Id.ToString(),
                                                OrderIndex = ss.OrderIndex,
                                                ScreenName = ss.ScreenName,
                                                Height = ss.Height,
                                                SceneApps = (from sa in querySceneApp
                                                             where sa.ScreenId == ss.Id.ToString()
                                                             select new SceneAppDto
                                                             {
                                                                 Id = sa.Id.ToString(),
                                                                 AppId = sa.AppId,
                                                                 ParentSceneAppId = sa.ParentSceneAppId,
                                                                 AppPlateItems = (
                                                                  from ap in querySceneAppPlate
                                                                  where (string.IsNullOrEmpty(sa.ParentSceneAppId) ? ap.SceneAppId == sa.Id.ToString() : ap.SceneAppId == sa.ParentSceneAppId)
                                                                  select new AppPlateItem
                                                                  {
                                                                      Id = ap.AppPlateId,
                                                                      SortType = ap.SortType,
                                                                      TopCount = ap.TopCount,
                                                                      OrderIndex = ap.OrderIndex
                                                                  }
                                                                 ).OrderBy(p => p.OrderIndex).ToList(),
                                                                 Height = sa.Height,
                                                                 AppWidget = new AppWidgetListViewModel { Id = sa.AppWidgetId },
                                                                 SceneId = sa.SceneId,
                                                                 SceneScreenId = ss.Id.ToString(),
                                                                 Width = sa.Width,
                                                                 XIndex = sa.XIndex,
                                                                 YIndex = sa.YIndex
                                                             }).OrderBy(s => s.YIndex).ThenBy(s => s.XIndex).ToList()
                                            }).OrderBy(p => p.OrderIndex).ToList(),
                            SceneUsers = (from su in querySceneUser
                                          where su.SceneId == sc.Id.ToString()
                                          select new SceneUserDto
                                          {
                                              Id = su.Id.ToString(),
                                              SceneId = su.SceneId,
                                              UserSetId = su.UserSetId,
                                              UserSetType = su.UserSetType
                                          }).ToList()
                        };

            var result = await query.FirstOrDefaultAsync();

            if (result != null)
            {
                foreach (var screen in result.SceneScreens)
                {
                    foreach (var item in screen.SceneApps)
                    {
                        var widgetList = await GetAppWidgetListByAppId(item.AppId, 0);
                        item.AppWidget = widgetList.FirstOrDefault(p => p.Id == item.AppWidget.Id.ToString());
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取当前用户个人中心场景
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<SceneDto> GetPersonalSceneDetail(string userKey)
        {
            var scene = await _sceneRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.UserKey == userKey);
            if (scene == null)
            {
                scene = await _sceneRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.UserKey == "Default");
            }

            var result = await GetSceneDetail(scene.Id.ToString());
            return result;
        }


        /// <summary>
        /// 保存用户的个人中心场景
        /// </summary>
        /// <param name="sceneDto">应用创建数据</param>
        /// <returns></returns>
        public async Task<Guid> SavePersonalScene(SceneDto sceneDto)
        {
            var myScene = await _sceneRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.UserKey == sceneDto.UserKey);
            var result = Guid.Empty;
            if (myScene != null)
            {
                sceneDto.Id = myScene.Id.ToString();
                result = await UpdateScene(sceneDto);
            }
            else
            {
                sceneDto.Id = "";
                result = await CreateScene(sceneDto);
            }
            return result;
        }


        /// <summary>
        /// 设置/取消个人默认首页
        /// </summary>
        /// <param name="sceneId">场景id</param>
        /// <param name="isDefault">是否设为默认首页1-是 0-否</param>
        /// <param name="userKey">用户标识</param>
        /// <returns></returns>
        public async Task<bool> SetPersonalDefaultScene(string sceneId, int isDefault, string userKey)
        {
            var myScene = await _sceneRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == sceneId && p.UserKey == userKey);
            if (myScene == null)
            {
                throw Oops.Oh("只能设置自己的个人首页").StatusCode((int)HttpStatusCode.BadRequest);
            }
            myScene.IsPersonalIndex = isDefault == 1;
            var result = await _sceneRepository.UpdateAsync(myScene);
            return result.State == EntityState.Modified;
        }

        /// <summary>
        /// 获取个人默认首页场景id
        /// </summary>
        /// <param name="userKey">用户标识</param>
        /// <returns></returns>
        public async Task<DefaultSceneViewModel> GetPersonalDefaultScene(string userKey)
        {
            var result = new DefaultSceneViewModel { IsPersonalIndex = false };
            if (!string.IsNullOrEmpty(userKey))
            {
                result.IsPersonalIndex = await _sceneRepository.AnyAsync(p => !p.DeleteFlag && p.UserKey == userKey && p.IsPersonalIndex);
            }
            var myScene = await _sceneRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.SceneType == 1 && p.Status == 0);
            result.SceneId = myScene?.Id.ToString();
            return result;
        }

        /// <summary>
        /// 添加场景
        /// </summary>
        /// <param name="sceneDto">应用创建数据</param>
        /// <returns></returns>
        public async Task<Guid> CreateScene(SceneDto sceneDto)
        {
            var scene = new Scene
            {
                Id = Guid.NewGuid(),
                Cover = await GetCoverByTerminalInstanceId(sceneDto.TerminalInstanceId),
                FooterTemplateId = sceneDto.FooterTemplate.Id,
                HeaderTemplateId = sceneDto.HeaderTemplate.Id,
                IsSystemScene = sceneDto.IsSystemScene,
                LayoutId = sceneDto.LayoutId.ToString(),
                Name = sceneDto.Name,
                UserKey = sceneDto.UserKey,
                Status = sceneDto.Status,
                TemplateId = sceneDto.Template.Id,
                TerminalInstanceId = sceneDto.TerminalInstanceId,
                ThemeColor = sceneDto.ThemeColor,
                VisitorLimitType = sceneDto.VisitorLimitType,
                VisitUrl = sceneDto.VisitUrl ?? "/#/scenePreview",
                CreatedTime = DateTimeOffset.UtcNow
            };
            var result = await _sceneRepository.InsertAsync(scene);

            var sceneUsers = new List<SceneUser>();
            sceneDto.SceneUsers.ToList().ForEach(p =>
            {
                sceneUsers.Add(new SceneUser
                {
                    Id = Guid.NewGuid(),
                    SceneId = result.Entity.Id.ToString(),
                    CreatedTime = DateTimeOffset.UtcNow,
                    UserSetId = p.UserSetId,
                    UserSetType = p.UserSetType
                });
            });
            var sceneUserResult = _sceneUserRepository.InsertAsync(sceneUsers);

            var sceneScreens = new List<SceneScreen>();
            var sceneApps = new List<SceneApp>();
            var sceneAppPlates = new List<SceneAppPlate>();
            sceneDto.SceneScreens.ToList().ForEach(p =>
            {
                var screenId = Guid.NewGuid();
                sceneScreens.Add(new SceneScreen
                {
                    Id = screenId,
                    CreatedTime = DateTimeOffset.UtcNow,
                    OrderIndex = p.OrderIndex,
                    Height = p.Height,
                    SceneId = result.Entity.Id.ToString(),
                    ScreenName = p.ScreenName ?? $"第{p.OrderIndex}屏"
                });
                p.SceneApps.ToList().ForEach(s =>
                {
                    var sceneAppId = Guid.NewGuid();
                    sceneApps.Add(new SceneApp
                    {
                        Id = sceneAppId,
                        CreatedTime = DateTimeOffset.UtcNow,
                        SceneId = result.Entity.Id.ToString(),
                        ParentSceneAppId = string.IsNullOrEmpty(sceneDto.UserKey) ? string.Empty : s.Id,
                        AppId = s.AppId,
                        AppWidgetId = s.AppWidget.Id,
                        Height = s.Height,
                        ScreenId = screenId.ToString(),
                        Width = s.Width,
                        XIndex = s.XIndex,
                        YIndex = s.YIndex
                    });
                    s.AppPlateItems.ForEach(ap =>
                    {
                        sceneAppPlates.Add(new SceneAppPlate
                        {
                            Id = Guid.NewGuid(),
                            AppPlateId = ap.Id,
                            CreatedTime = DateTimeOffset.UtcNow,
                            SceneAppId = sceneAppId.ToString(),
                            SortType = ap.SortType,
                            TopCount = ap.TopCount ?? 0,
                            SceneId = scene.Id.ToString(),
                            OrderIndex = ap.OrderIndex
                        });
                    });
                });
            });
            try
            {
                var sceneScreenResult = _sceneScreenRepository.InsertAsync(sceneScreens);
                var sceneAppResult = _sceneAppRepository.InsertAsync(sceneApps);
                var sceneAppPlateResult = _sceneAppPlateRepository.InsertAsync(sceneAppPlates);
            }
            catch (Exception ex)
            {

            }


            return result.Entity.Id;
        }

        /// <summary>
        /// 更新场景
        /// </summary>
        /// <param name="sceneDto">应用创建数据</param>
        /// <returns></returns>
        public async Task<Guid> UpdateScene(SceneDto sceneDto)
        {
            var scene = await _sceneRepository.FirstOrDefaultAsync(p => p.Id.ToString() == sceneDto.Id && !p.DeleteFlag);

            scene.Cover = sceneDto.Cover;
            scene.FooterTemplateId = sceneDto.FooterTemplate.Id;
            scene.HeaderTemplateId = sceneDto.HeaderTemplate.Id;
            scene.IsSystemScene = sceneDto.IsSystemScene;
            scene.LayoutId = sceneDto.LayoutId.ToString();
            scene.Name = sceneDto.Name;
            //scene.UserKey = sceneDto.UserKey;
            scene.Status = sceneDto.Status;
            scene.TemplateId = sceneDto.Template.Id;
            scene.TerminalInstanceId = sceneDto.TerminalInstanceId;
            scene.ThemeColor = sceneDto.ThemeColor;
            scene.VisitorLimitType = sceneDto.VisitorLimitType;
            scene.VisitUrl = sceneDto.VisitUrl;
            scene.UpdatedTime = DateTimeOffset.UtcNow;

            var result = await _sceneRepository.UpdateAsync(scene);

            var sceneUsers = new List<SceneUser>();
            sceneDto.SceneUsers.ToList().ForEach(p =>
            {
                sceneUsers.Add(new SceneUser
                {
                    Id = string.IsNullOrEmpty(p.Id) ? Guid.NewGuid() : new Guid(p.Id),
                    SceneId = result.Entity.Id.ToString(),
                    CreatedTime = DateTimeOffset.UtcNow,
                    UserSetId = p.UserSetId,
                    UserSetType = p.UserSetType
                });
            });
            var sceneUserDeleteResult = _sceneUserRepository.Context.DeleteRange<SceneUser>(p => p.SceneId == sceneDto.Id);
            var sceneUserResult = _sceneUserRepository.InsertAsync(sceneUsers);

            var sceneScreens = new List<SceneScreen>();
            var sceneApps = new List<SceneApp>();
            var sceneAppPlates = new List<SceneAppPlate>();
            sceneDto.SceneScreens.ToList().ForEach(p =>
            {
                var screenId = string.IsNullOrEmpty(p.Id) ? Guid.NewGuid() : new Guid(p.Id);
                sceneScreens.Add(new SceneScreen
                {
                    Id = screenId,
                    CreatedTime = DateTimeOffset.UtcNow,
                    OrderIndex = p.OrderIndex,
                    Height = p.Height,
                    SceneId = result.Entity.Id.ToString(),
                    ScreenName = p.ScreenName ?? $"第{p.OrderIndex}屏"
                });
                p.SceneApps.ToList().ForEach(s =>
                {
                    var sceneAppId = string.IsNullOrEmpty(s.Id) ? Guid.NewGuid() : new Guid(s.Id);
                    sceneApps.Add(new SceneApp
                    {
                        Id = sceneAppId,
                        CreatedTime = DateTimeOffset.UtcNow,
                        SceneId = result.Entity.Id.ToString(),
                        AppId = s.AppId,
                        AppWidgetId = s.AppWidget.Id,
                        Height = s.Height,
                        ScreenId = screenId.ToString(),
                        ParentSceneAppId = s.ParentSceneAppId,
                        Width = s.Width,
                        XIndex = s.XIndex,
                        YIndex = s.YIndex
                    });
                    s.AppPlateItems.ForEach(ap =>
                    {
                        sceneAppPlates.Add(new SceneAppPlate
                        {
                            Id = Guid.NewGuid(),
                            AppPlateId = ap.Id,
                            CreatedTime = DateTimeOffset.UtcNow,
                            SceneAppId = sceneAppId.ToString(),
                            SortType = ap.SortType,
                            TopCount = ap.TopCount ?? 0,
                            SceneId = scene.Id.ToString(),
                            OrderIndex = ap.OrderIndex
                        });
                    });
                });
            });

            var sceneScreenDeleteResult = _sceneUserRepository.Context.DeleteRange<SceneScreen>(p => p.SceneId == sceneDto.Id);
            var sceneAppDeleteResult = _sceneAppRepository.Context.DeleteRange<SceneApp>(p => p.SceneId == sceneDto.Id);
            var sceneAppPlateDeleteResult = _sceneAppPlateRepository.Context.DeleteRange<SceneAppPlate>(p => p.SceneId == sceneDto.Id);
            var sceneScreenResult = _sceneScreenRepository.InsertAsync(sceneScreens);
            var sceneAppResult = _sceneAppRepository.InsertAsync(sceneApps);
            var sceneAppPlateResult = _sceneAppPlateRepository.InsertAsync(sceneAppPlates);


            return result.Entity.Id;
        }


        /// <summary>
        /// 启用/禁用场景
        /// </summary>
        /// <param name="sceneDto">场景Id</param>
        /// <returns></returns>
        public async Task<bool> ChangeSceneStatus(string sceneId, int newStatus)
        {
            var scene = await _sceneRepository.FirstOrDefaultAsync(p => p.Id.ToString() == sceneId && !p.DeleteFlag);

            scene.UpdatedTime = DateTimeOffset.UtcNow;
            scene.Status = newStatus;

            var result = await _sceneRepository.UpdateAsync(scene);
            return result.State == EntityState.Modified;
        }

        /// <summary>
        /// 删除场景
        /// </summary>
        /// <param name="sceneId">应用ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteScene(string sceneId)
        {
            var scene = await _sceneRepository.FirstOrDefaultAsync(p => p.Id.ToString() == sceneId && !p.DeleteFlag);

            scene.UpdatedTime = DateTimeOffset.UtcNow;
            scene.DeleteFlag = true;

            var result = await _sceneRepository.UpdateAsync(scene);
            return result.State == EntityState.Modified;
        }

        /// <summary>
        /// 按服务类型获取应用列表
        /// </summary>
        /// <param name="appServiceType">服务类型字典值</param>
        /// <param name="terminalType">1-PC端、2-APP端、3-小程序端、4-自适应移动端、5-显示屏</param>
        /// <returns></returns>
        public async Task<List<AppListViewModel>> GetAppListByServiceType(string appServiceType, int terminalType)
        {
            //var queryScene = _sceneRepository.Where(p => !p.DeleteFlag);
            if (!SiteGlobalConfig.AppBaseConfig.IsMock)
            {
                var result =
                        new List<AppListViewModel> {
                    new AppListViewModel{Name="新闻资讯",AppId="b0fb53b3-f7b3-41a5-94a5-c4feb71213e2",Id="appbr125-1717-4562-b3fc-2c963f66afa6",Icon ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="信息导航",AppId="a7fedf20-e7be-483d-9471-11d5a9a49203",Id="appbr225-1717-4562-b3fc-2c963f66afa6",Icon ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg" },
                    new AppListViewModel{Name="文献推荐引擎",AppId="dba5df91-dc9c-4dd2-9527-3741a425b7b3",Id="appbr325-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="文献智能推荐",AppId="f22846b8-53d4-4f5c-a62c-92d28efe447f",Id="appbr425-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="数据统计分析",AppId="9fed516c-9424-4b0d-b4b2-1d72adde9716",Id="appbr525-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="统一检索",AppId="27ad8830-1085-4f42-adb7-237c16fb383c",Id="appbr625-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="应用中心",AppId="42757eb3-7a18-4619-adb2-cda39aee5602",Id="appbr725-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="数据库导航",AppId="d759fd09-b08e-4e37-99c3-4d4d67baaa6a",Id="appbr825-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="通知中心",AppId="ea154354-4292-4de4-9718-67ad795401a5",Id="appbr925-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="活动中心",AppId="786ddebc-e762-416e-a817-0ff56aa97bc1",Id="appbra25-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="积分中心",AppId="24567b8d-4908-5349-a6f6-d47bfda80522",Id="appbrb25-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"}
                        };

                return result;
            }
            else
            {
                var request = new AppListRequest { AppServiceType = appServiceType == "0" ? "" : appServiceType, TerminalType = terminalType, SceneType = 0 };
                var result = await GetAppList(request);
                return result.AppList.Select(p => new AppListViewModel
                {
                    AppId = p.AppId,
                    Icon = $"{p.Icon}",
                    Id = p.AppId,
                    Name = p.Name,
                    ServiceType = p.ServiceType
                }).ToList();
            }
        }

        /// <summary>
        /// 获取当前用户个人中心应用列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppListViewModel>> GetPersonalAppList(int terminalType)
        {
            var scene = await _sceneRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.UserKey.ToLower() == "default");
            var sceneAppIds = _sceneAppRepository.Where(p => p.SceneId == scene.Id.ToString()).Select(p => p.AppId).ToList();

            if (!SiteGlobalConfig.AppBaseConfig.IsMock)
            {
                var result =
                        new List<AppListViewModel> {
                    new AppListViewModel{Name="新闻资讯",AppId="b0fb53b3-f7b3-41a5-94a5-c4feb71213e2",Id="appbr125-1717-4562-b3fc-2c963f66afa6",Icon ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="文献推荐引擎",AppId="dba5df91-dc9c-4dd2-9527-3741a425b7b3",Id="appbr325-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="文献智能推荐",AppId="f22846b8-53d4-4f5c-a62c-92d28efe447f",Id="appbr425-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="统一检索",AppId="27ad8830-1085-4f42-adb7-237c16fb383c",Id="appbr625-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="数据库导航",AppId="d759fd09-b08e-4e37-99c3-4d4d67baaa6a",Id="appbr825-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="通知中心",AppId="ea154354-4292-4de4-9718-67ad795401a5",Id="appbr925-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="活动中心",AppId="786ddebc-e762-416e-a817-0ff56aa97bc1",Id="appbra25-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"},
                    new AppListViewModel{Name="积分中心",AppId="24567b8d-4908-5349-a6f6-d47bfda80522",Id="appbrb25-1717-4562-b3fc-2c963f66afa6", Icon = "/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"}
                        };

                return result.Where(p => sceneAppIds.Contains(p.AppId)).ToList();
            }
            else
            {
                var request = new AppListRequest { TerminalType = terminalType, SceneType = 2 };
                var result = await GetAppList(request);
                return result.AppList.Where(p => sceneAppIds.Contains(p.AppId)).Select(p => new AppListViewModel
                {
                    AppId = p.AppId,
                    Icon = $"{p.Icon}",
                    Id = p.AppId,
                    Name = p.Name
                }).ToList();
            }
        }

        /// <summary>
        /// 获取个人中心应用组件列表
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <returns></returns>
        public async Task<AppWidgetListViewModel> GetPersonalAppWidgetByAppId(string appId)
        {
            var scene = await _sceneRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.UserKey.ToLower() == "default");
            var sceneApp = await _sceneAppRepository.FirstOrDefaultAsync(p => p.SceneId == scene.Id.ToString() && p.AppId == appId);
            var sceneAppPlate = await _sceneAppPlateRepository.FirstOrDefaultAsync(p => p.SceneId == scene.Id.ToString() && p.SceneAppId == sceneApp.Id.ToString());
            if (!SiteGlobalConfig.AppBaseConfig.IsMock)
            {
                var list = await GetAppWidgetListByAppId(appId, 2);
                var result = list.FirstOrDefault(p => p.SceneType == 2 && p.Id == sceneApp.AppWidgetId);
                result.AppColumn = new AppPlateItem
                {
                    Id = sceneAppPlate.AppPlateId,
                    SortType = sceneAppPlate.SortType,
                    TopCount = sceneAppPlate.TopCount,
                    ParentSceneAppId = sceneApp.Id.ToString()
                };
                return result;
            }
            else
            {
                var request = new AppWidgetListRequest { AppId = appId, SceneType = 2 };
                var result = await GetAppWidgetList(request);
                var widget = result.AppWidgetList.FirstOrDefault();
                if (widget == null)
                {
                    return new AppWidgetListViewModel();
                }
                var finalResult = new AppWidgetListViewModel
                {
                    AppId = widget.AppId,
                    AvailableConfig = widget.AvailableConfig,
                    Cover = widget.Cover,
                    Height = widget.Height,
                    Id = widget.Id,
                    Name = widget.Name,
                    SceneType = widget.SceneType,
                    SortList = widget.SortList.Select(p => new SysDictModel<string>
                    {
                        Key = p.Key,
                        Value = p.Value
                    }).ToList(),
                    Target = widget.Target,
                    TopCountList = widget.TopCountList.Select(p => new SysDictModel<int>
                    {
                        Key = p.Key,
                        Value = p.Value
                    }).ToList(),
                    WidgetCode = widget.WidgetCode,
                    Width = widget.Width,
                    CreateTime = string.IsNullOrEmpty(widget.CreateTime) ? DateTime.MinValue : DateTime.Parse(widget.CreateTime),
                    UpdateTime = string.IsNullOrEmpty(widget.UpdateTime) ? DateTime.MinValue : DateTime.Parse(widget.UpdateTime),
                    AppColumn = new AppPlateItem
                    {
                        Id = sceneAppPlate.AppPlateId,
                        SortType = sceneAppPlate.SortType,
                        TopCount = sceneAppPlate.TopCount,
                        ParentSceneAppId = sceneApp.Id.ToString()
                    }
                };
                finalResult.SortList.Add(new SysDictModel<string> { Key = "默认", Value = "Default" });
                return finalResult;
            }
        }


        /// <summary>
        /// 获取应用组件列表
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="sceneType">适用场景 0-全部，1-通用，2-个人中心</param>
        /// <returns></returns>
        public async Task<List<AppWidgetListViewModel>> GetAppWidgetListByAppId(string appId, int sceneType)
        {
            //var queryScene = _sceneRepository.Where(p => !p.DeleteFlag);
            if (!SiteGlobalConfig.AppBaseConfig.IsMock)
            {
                switch (appId)
                {
                    case "b0fb53b3-f7b3-41a5-94a5-c4feb71213e2":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="首页新闻动态", SceneType=1, Width=6,Height=10,AppId="b0fb53b3-f7b3-41a5-94a5-c4feb71213e2",Id="1002a080-b03f-4aa6-84bf-88319b77fa20",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/news_sys/temp1", WidgetCode="news_sys_temp1", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string>{Key="添加时间倒序",Value= "CreatedTime-DESC" } } }
                    ,new AppWidgetListViewModel{Name="个人图书馆-图书馆新闻", SceneType=2, Width=6,Height=6,AppId="b0fb53b3-f7b3-41a5-94a5-c4feb71213e2",Id="add8698e-cef4-425f-b947-e7be12325ed9",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/news_sys/temp2", WidgetCode="news_sys_temp2", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string>{Key="添加时间倒序",Value= "CreatedTime-DESC" } } }};
                    case "a7fedf20-e7be-483d-9471-11d5a9a49203":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="信息导航", SceneType=1, Width=6,Height=10,AppId="a7fedf20-e7be-483d-9471-11d5a9a49203",Id="0523c418-1cd7-4b5d-9116-003913fd5073",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/service_sys/temp1", WidgetCode="service_sys_temp1", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }}};
                    case "dba5df91-dc9c-4dd2-9527-3741a425b7b3":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="文献推荐引擎", SceneType=1, Width=6,Height=10,AppId="dba5df91-dc9c-4dd2-9527-3741a425b7b3",Id="bf1c4adf-0ff3-42b0-984c-4d02549a5bb1",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/literature_project_sys/temp1", WidgetCode="literature_project_sys_temp1", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="默认排序",Value= "OrderIndex-Desc" },new SysDictModel<string> {Key="本月点击量倒序",Value= "MonthClickNum-Desc" },new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }}
                    ,new AppWidgetListViewModel{Name="个人图书馆-专题类型", SceneType=2, Width=6,Height=6,AppId="dba5df91-dc9c-4dd2-9527-3741a425b7b3",Id="55a52d9f-5d98-4ee5-883e-98ce8a745192",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/literature_project_sys/temp2", WidgetCode="literature_project_sys_temp2", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="默认排序",Value= "OrderIndex-Desc" },new SysDictModel<string> {Key="本月点击量倒序",Value= "MonthClickNum-Desc" },new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }}};
                    case "f22846b8-53d4-4f5c-a62c-92d28efe447f":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="文献智能推荐1", SceneType=1, Width=6,Height=10,AppId="f22846b8-53d4-4f5c-a62c-92d28efe447f",Id="9d809769-6c9f-4bf2-b41b-417f952f1973",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/literature_recommend_sys/temp1", WidgetCode="literature_recommend_sys_temp1", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }},
                    new AppWidgetListViewModel{Name="文献智能推荐2", SceneType=1, Width=6,Height=10,AppId="f22846b8-53d4-4f5c-a62c-92d28efe447f",Id="54d6af9b-5ec1-4df0-b638-57ba1dc245bd",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/literature_recommend_sys/temp2", WidgetCode="literature_recommend_sys_temp2", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }},
                    new AppWidgetListViewModel{Name="个人图书馆-资源推荐类", SceneType=2, Width=6,Height=6,AppId="f22846b8-53d4-4f5c-a62c-92d28efe447f",Id="9d1be2b6-d498-4768-b614-7ce00e989b92",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/literature_recommend_sys/temp3", WidgetCode="literature_recommend_sys_temp3", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }}};

                    case "5eeb1b4d-9d62-4254-8de0-052e5b1ac48a":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="个人图书馆", SceneType=1, Width=6,Height=10,AppId="5eeb1b4d-9d62-4254-8de0-052e5b1ac48a",Id="6a4a01c7-f3a3-49cb-be57-9effddd1b182",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/other/college_library", WidgetCode="other_college_library", AvailableConfig="2",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }} };
                    case "9fed516c-9424-4b0d-b4b2-1d72adde9716":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="本馆数据", SceneType=1, Width=6,Height=10,AppId="9fed516c-9424-4b0d-b4b2-1d72adde9716",Id="b0212294-7a89-4dc8-94c1-1a8c2f2e780e",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/other/our_data", WidgetCode="other_our_data", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }},
                    new AppWidgetListViewModel{Name="个人图书馆", SceneType=1, Width=6,Height=10,AppId="5eeb1b4d-9d62-4254-8de0-052e5b1ac48a",Id="6a4a01c7-f3a3-49cb-be57-9effddd1b182",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/other/college_library", WidgetCode="other_college_library", AvailableConfig="2",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }},
                    };
                    case "f7958d7f-8cca-4ac6-84f8-e85b9f5f6a74":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="我的书斋", SceneType=1, Width=6,Height=10,AppId="f7958d7f-8cca-4ac6-84f8-e85b9f5f6a74",Id="f3c74136-e6a5-40be-9341-a5aed6102161",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/other/my_study_link", WidgetCode="other_my_study_link", AvailableConfig="2",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }},
                    new AppWidgetListViewModel{Name="固定左边模板-多个应用", SceneType=1, Width=3,Height=10,AppId="f7958d7f-8cca-4ac6-84f8-e85b9f5f6a74",Id="c46d4627-4155-4927-b51c-1ff33a40e84d",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/other/left_menu_list", WidgetCode="other_left_menu_list", AvailableConfig="1,2",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }}};
                    case "27ad8830-1085-4f42-adb7-237c16fb383c":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="统一检索", SceneType=1, Width=6,Height=10,AppId="27ad8830-1085-4f42-adb7-237c16fb383c",Id="2ab6756f-2442-4f92-ac3a-16bd1ca91915",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/unified_retrieval_sys/temp1", WidgetCode="unified_retrieval_sys_temp1", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }},
                    new AppWidgetListViewModel{Name="个人图书馆-统一检索", SceneType=2, Width=6,Height=6,AppId="27ad8830-1085-4f42-adb7-237c16fb383c",Id="9f9d1d62-f6be-403c-85e9-716e6d9058ff",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/unified_retrieval_sys/temp2", WidgetCode="unified_retrieval_sys_temp2", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }}};
                    case "42757eb3-7a18-4619-adb2-cda39aee5602":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="应用中心1", SceneType=1, Width=6,Height=10,AppId="42757eb3-7a18-4619-adb2-cda39aee5602",Id="4fbe2e08-9f09-4230-b72e-185a6de74947",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/apps_center_sys/temp1", WidgetCode="apps_center_sys_temp1", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }},
                    new AppWidgetListViewModel{Name="应用中心2", SceneType=1, Width=6,Height=10,AppId="42757eb3-7a18-4619-adb2-cda39aee5602",Id="e704b9cd-cbee-4a19-ad60-30fbcdf212ed",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/apps_center_sys/temp2", WidgetCode="apps_center_sys_temp2", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }}};
                    case "d759fd09-b08e-4e37-99c3-4d4d67baaa6a":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="数据库导航", SceneType=1, Width=6,Height=10,AppId="d759fd09-b08e-4e37-99c3-4d4d67baaa6a",Id="c3c438fa-1980-4e90-a431-bf73f8f23153",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/database_nav_sys/temp1", WidgetCode="database_nav_sys_temp1", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="访问量倒序",Value="VisitCount-DESC"},new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }}
                    ,new AppWidgetListViewModel{Name="个人图书馆-常用数据库", SceneType=2, Width=6,Height=10,AppId="d759fd09-b08e-4e37-99c3-4d4d67baaa6a",Id="54fb8968-2bcb-4dd1-bf39-a581c01e937f",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/database_nav_sys/temp2", WidgetCode="database_nav_sys_temp2", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="访问量倒序",Value="VisitCount-DESC"},new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }}};
                    case "ea154354-4292-4de4-9718-67ad795401a5":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="个人图书馆-通知消息", SceneType=2, Width=6,Height=6,AppId="ea154354-4292-4de4-9718-67ad795401a5",Id="82beac5c-f62e-469e-ae01-2fc5cb6a9a24",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/apps_center_sys/temp3", WidgetCode="apps_center_sys_temp3", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="默认排序",Value= "OrderIndex-Desc" },new SysDictModel<string> {Key="发送时间倒序",Value= "CreatedTime-DESC" } }}};
                    case "786ddebc-e762-416e-a817-0ff56aa97bc1":
                        return new List<AppWidgetListViewModel> {
                            new AppWidgetListViewModel{Name="活动日历", SceneType=1, Width=6,Height=10,AppId="786ddebc-e762-416e-a817-0ff56aa97bc1",Id="dd366ff2-2d0e-4e3e-9d97-b3cc42b4e196",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/other/activity_calendar", WidgetCode="other_activity_calendar", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="添加时间倒序",Value= "CreatedTime-DESC" } }},
                    new AppWidgetListViewModel{Name="个人图书馆-图书馆活动", SceneType=2, Width=6,Height=6,AppId="786ddebc-e762-416e-a817-0ff56aa97bc1",Id="93c7b6af-d8f1-472b-ba79-fe70fac45c5f",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/other/lib_activity", WidgetCode="other_lib_activity", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="默认排序",Value= "OrderIndex-Desc" },new SysDictModel<string> {Key="活动时间倒序",Value= "CreatedTime-DESC" } }}};
                    case "24567b8d-4908-5349-a6f6-d47bfda80522":
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="个人图书馆-积分任务", SceneType=2, Width=6,Height=6,AppId="24567b8d-4908-5349-a6f6-d47bfda80522",Id="7bb399f4-15c7-4c1f-8efa-f55642b12777",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/integral_center_sys/temp1", WidgetCode="integral_center_sys_temp1", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string> {Key="默认排序",Value= "OrderIndex-Desc" },new SysDictModel<string> {Key="活动时间倒序",Value= "CreatedTime-DESC" } }}};

                    default:
                        return new List<AppWidgetListViewModel> {
                    new AppWidgetListViewModel{Name="首页新闻动态", SceneType=1, Width=6,Height=10,AppId="b0fb53b3-f7b3-41a5-94a5-c4feb71213e2",Id="1002a080-b03f-4aa6-84bf-88319b77fa20",Cover ="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg",CreateTime=DateTime.Now,Target=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/news_sys/temp1", WidgetCode="news_sys_temp1", AvailableConfig="1,2,3",UpdateTime=DateTime.Now,TopCountList=new List<SysDictModel<int>>{ new SysDictModel<int>{Key="5",Value=5},new SysDictModel<int>{Key="6",Value=6},new SysDictModel<int>{Key="7",Value=7},new SysDictModel<int>{Key="8",Value=8},new SysDictModel<int>{Key="9",Value=9},new SysDictModel<int>{Key="10",Value=10},new SysDictModel<int>{Key="12",Value=12},new SysDictModel<int>{Key="14",Value=14},new SysDictModel<int>{Key="16",Value=16},new SysDictModel<int>{Key="18",Value=18},new SysDictModel<int>{Key="20",Value=20} },SortList=new List<SysDictModel<string>>{ new SysDictModel<string>{Key="添加时间倒序",Value= "CreatedTime-DESC" } } }};

                };
            }
            else
            {
                var request = new AppWidgetListRequest { AppId = appId, SceneType = sceneType };
                var result = await GetAppWidgetList(request);
                var finalResult = result.AppWidgetList.Select(widget => new AppWidgetListViewModel
                {
                    AppId = widget.AppId,
                    AvailableConfig = widget.AvailableConfig,
                    Cover = widget.Cover,
                    Height = widget.Height,
                    Id = widget.Id,
                    Name = widget.Name,
                    SceneType = widget.SceneType,
                    SortList = widget.SortList.Select(p => new SysDictModel<string>
                    {
                        Key = p.Key,
                        Value = p.Value
                    }).ToList(),
                    Target = $"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}{widget.Target}",
                    TopCountList = widget.TopCountList.Select(p => new SysDictModel<int>
                    {
                        Key = p.Key,
                        Value = p.Value
                    }).ToList(),
                    WidgetCode = widget.WidgetCode,
                    Width = widget.Width,
                    CreateTime = string.IsNullOrEmpty(widget.CreateTime) ? DateTime.MinValue : DateTime.Parse(widget.CreateTime),
                    UpdateTime = string.IsNullOrEmpty(widget.UpdateTime) ? DateTime.MinValue : DateTime.Parse(widget.UpdateTime)
                }).ToList();
                finalResult.ForEach(p => { p.SortList.Add(new SysDictModel<string> { Key = "默认", Value = "Default" }); });
                return finalResult;
            }
        }


        /// <summary>
        /// 获取当前场景访问地址
        /// </summary>
        /// <param name="sceneId">场景Id</param>
        /// <returns></returns>
        public async Task<string> GetSceneUrlById(string sceneId)
        {
            var scene = await _sceneRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.SceneType == 1 && p.Status == 0);
            //var queryScene = _sceneRepository.Where(p => !p.DeleteFlag);
            return sceneId == scene.Id.ToString() ? "/" : $"/#/page?id={sceneId}";
        }

        /// <summary>
        /// 获取栏目使用情况
        /// </summary>
        /// <param name="columnIdList"></param>
        /// <returns></returns>
        public async Task<List<SysDictModel<int>>> GetSceneUseageByColumnId(List<string> columnIdList)
        {
            var scene = _sceneRepository.Where(p => !p.DeleteFlag && (p.UserKey == null || p.UserKey.ToLower() == "default")).AsNoTracking();
            var sceneApp = _sceneAppPlateRepository.Where(p => !p.DeleteFlag && columnIdList.Contains(p.AppPlateId)).AsNoTracking();
            try
            {
                var query = from s in scene
                            join sa in sceneApp
                            on s.Id.ToString() equals sa.SceneId
                            group sa by sa.AppPlateId into g
                            select new SysDictModel<int>
                            {
                                Key = g.Key,
                                Value = g.Count()
                            };
                return await query.ToListAsync();
            }
            catch (Exception ex) {
                return new List<SysDictModel<int>>();
            }
        }


        /// <summary>
        /// 获取应用栏目列表
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <returns></returns>
        public async Task<List<SysDictModel<string>>> GetAppPlateListByAppId(string appId)
        {
            //var queryScene = _sceneRepository.Where(p => !p.DeleteFlag);
            if (!SiteGlobalConfig.AppBaseConfig.IsMock)
            {
                switch (appId)
                {
                    case "b0fb53b3-f7b3-41a5-94a5-c4feb71213e2":
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="大新闻",Value="Bd63e6Imm7sBR$DM" },
                    new SysDictModel<string>{Key="测试栏目",Value="BsdE$6ImtraDkybx" },
                    new SysDictModel<string>{Key="科研栏目",Value="CkFtq6ImscLCY6oH"},
                    new SysDictModel<string>{Key="保留栏目",Value="Ebh6r6Imm3UE3MQ5"}
                    };
                    case "a7fedf20-e7be-483d-9471-11d5a9a49203":
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="导航目录6",Value="C$o266Ir84HBl0S3" },
                    new SysDictModel<string>{Key="测试0125",Value="Ep5ex6IrkpMFXe2M" },
                    new SysDictModel<string>{Key="测试126",Value="FgPMt6Ir8pZ95Gj"},
                    new SysDictModel<string>{Key="导航目录1",Value="EAaRW6Ir8vxFdLGp"}
                    };
                    case "dba5df91-dc9c-4dd2-9527-3741a425b7b3":
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="馆长推荐",Value="26221bdd-ee72-4a59-8d96-21fdf7e83f93" },
                    new SysDictModel<string>{Key="天文栏目",Value="b9425909-9fc6-4315-90aa-ce8ba7846f23" },
                    new SysDictModel<string>{Key="超管栏目",Value="e1f754ed-327b-4eeb-b3d4-00dab1f8c912"}
                    };
                    case "f22846b8-53d4-4f5c-a62c-92d28efe447f":
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="文化",Value="0d041b6f-1408-4ed0-9fed-12c3ea965ec1" },
                    new SysDictModel<string>{Key="新书速递",Value="1e9b03bb-7d64-4043-9ee9-f68e53f4646b" },
                    new SysDictModel<string>{Key="医学",Value="2b173548-45f9-49f5-8d20-30c20d76ce77"},
                    new SysDictModel<string>{Key="生活",Value="56fbc460-b4c5-4d1f-9310-02f672c5803b"},
                    new SysDictModel<string>{Key="自然科学",Value="5935d389-acad-4e88-8877-730435afb9db"},
                    new SysDictModel<string>{Key="流行",Value="71014c1e-83aa-4e8d-a8cb-dcf2760bb733"},
                    new SysDictModel<string>{Key="文学",Value="e43dac57-941c-4a27-8704-76340819d7b8"}
                    };
                    case "9fed516c-9424-4b0d-b4b2-1d72adde9716":
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="热门数据库",Value="1ceb8abe-f5cc-4d2b-a524-3a55acce293b" },
                    new SysDictModel<string>{Key="123",Value="15164479-81e6-40d1-9cc5-17fefa3905ae" },
                    new SysDictModel<string>{Key="首页检索框",Value="c018b5d8-81a4-4343-86c3-7fab1c617ae6"},
                    new SysDictModel<string>{Key="全部应用",Value="1067c97d-2d96-4935-b1c3-e10f9940e6a8" },
                    new SysDictModel<string>{Key="门户应用",Value="2648cab1-8c94-4caf-8953-e678ab67d730" },
                    new SysDictModel<string>{Key="新闻应用",Value="e96463c7-fdb6-4a9f-8f3e-aeecff0e5e6a" }
                    };
                    case "27ad8830-1085-4f42-adb7-237c16fb383c":
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="标准检索框",Value="15164479-81e6-40d1-9cc5-17fefa3905ae" },
                    new SysDictModel<string>{Key="首页检索框",Value="c018b5d8-81a4-4343-86c3-7fab1c617ae6" }
                    };
                    case "42757eb3-7a18-4619-adb2-cda39aee5602":
                        return new List<SysDictModel<string>> {

                    new SysDictModel<string>{Key="全部",Value="1067c97d-2d96-4935-b1c3-e10f9940e6a8" },
                    new SysDictModel<string>{Key="门户",Value="2648cab1-8c94-4caf-8953-e678ab67d730" },
                    new SysDictModel<string>{Key="新闻",Value="e96463c7-fdb6-4a9f-8f3e-aeecff0e5e6a" }
                    };
                    case "d759fd09-b08e-4e37-99c3-4d4d67baaa6a":
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="全选栏目",Value="25dd3598-7652-438a-8996-5da8bd722168" },
                    new SysDictModel<string>{Key="中文栏目",Value="44638490-b66f-419b-bcea-fdfd0bd91a73" },
                    new SysDictModel<string>{Key="期刊，军事",Value="5cdc3e90-2af6-42bf-b332-4bf9e13d6308"},
                    new SysDictModel<string>{Key="外文栏目",Value="cb9750b9-3673-4ca3-9c44-703ae04dbab6"}
                    };
                    default:
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="热门数据库",Value="1ceb8abe-f5cc-4d2b-a524-3a55acce293b" },
                    new SysDictModel<string>{Key="试用数据库",Value="appplate-2717-4562-b3fc-2c963f66afa7" },
                    new SysDictModel<string>{Key="推荐数据库",Value="appplate-3717-4562-b3fc-2c963f66afa8"}
                    };
                };
            }
            else
            {
                var request = new AppColumnListRequest { AppId = appId };
                var result = await GetAppColumnList(request);
                return result.AppColumnList.Select(p => new SysDictModel<string>
                {
                    Key = p.Name,
                    Value = p.ColumnId
                }).ToList();
            }
        }

        /// <summary>
        /// 获取下拉框字典
        /// </summary>
        /// <returns></returns>
        public async Task<DictionaryViewModel> GetDictionary()
        {
            if (!SiteGlobalConfig.AppBaseConfig.IsMock)
            {
                var result = new DictionaryViewModel
                {
                    AppPlateSortType = EnumTools.EnumToList<AppPlateSortTypeEnum>(),
                    SceneStatus = EnumTools.EnumToList<SceneStatusEnum>(),
                    VisitorLimitType = EnumTools.EnumToList<VisitorLimitTypeEnum>(),
                    SceneLayout = EnumTools.EnumToList<SceneLayoutTypeEnum>(),
                    SceneTemplate = new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="现代简约",Value="13a85f64-5717-4562-b3fc-2c963f66afa6" },
                    new SysDictModel<string>{Key="Metro风格",Value="24a85f64-5717-4562-b3fc-2c963f66afa6" }
                    },
                    SceneThemeColor = new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="中国红",Value="template1" },
                    //new SysDictModel<string>{Key="天空蓝",Value="template2" },
                    new SysDictModel<string>{Key="黑白",Value="template1 template0" }
                    },
                    AppServiceType = new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="常用服务",Value="sta15f64-5717-4562-b3fc-2c963f66afa6" },
                    new SysDictModel<string>{Key="资源服务",Value="sta25f64-5717-4562-b3fc-2c963f66afa6" },
                    new SysDictModel<string>{Key="学术服务",Value="sta35f64-5717-4562-b3fc-2c963f66afa6" },
                    new SysDictModel<string>{Key="个人图书馆",Value="sta45f64-5717-4562-b3fc-2c963f66afa6" }
                    }
                };
                return result;
            }
            else
            {
                var serviceTypes = await GetServiceType();
                var result = new DictionaryViewModel
                {
                    AppPlateSortType = EnumTools.EnumToList<AppPlateSortTypeEnum>(),
                    SceneStatus = EnumTools.EnumToList<SceneStatusEnum>(),
                    VisitorLimitType = EnumTools.EnumToList<VisitorLimitTypeEnum>(),
                    SceneLayout = EnumTools.EnumToList<SceneLayoutTypeEnum>(),
                    SceneTemplate = new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="现代简约",Value="13a85f64-5717-4562-b3fc-2c963f66afa6" },
                    new SysDictModel<string>{Key="Metro风格",Value="24a85f64-5717-4562-b3fc-2c963f66afa6" }
                    },
                    SceneThemeColor = new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="中国红",Value="template1" },
                    //new SysDictModel<string>{Key="天空蓝",Value="template2" },
                    new SysDictModel<string>{Key="黑白",Value="template1 template0" }
                    },
                    AppServiceType = serviceTypes.ServiceTypeList.Select(p => new SysDictModel<string>
                    {
                        Key = p.Value,
                        Value = p.Key
                    }).ToList()
                };
                return result;
            }
        }


        /// <summary>
        /// 按类型获取下拉框字典
        /// </summary>
        /// <param name="dicType">字典类型 1-学院 2-用户类型 3-用户分组</param>
        /// <returns></returns>
        public async Task<List<SysDictModel<string>>> GetDictionaryByType(int dicType)
        {
            if (!SiteGlobalConfig.AppBaseConfig.IsMock)
            {
                switch (dicType)
                {
                    case 1:
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="计算机学院",Value="3fa85f64-5717-4562-b3fc-2c963f66afa6" },
                    new SysDictModel<string>{Key="软件与大数据学院",Value="3fa85f64-5717-4562-b3fc-2c963f66afa7" },
                    new SysDictModel<string>{Key="体育学院",Value="3fa85f64-5717-4562-b3fc-2c963f66afa8"},
                    new SysDictModel<string>{Key="建筑城规学院",Value="3fa85f64-5717-4562-b3fc-2c963f66afa9"},
                    new SysDictModel<string>{Key="人文学院",Value="3fa85f64-5717-4562-b3fc-2c963f66af10"},
                    new SysDictModel<string>{Key="新闻学院",Value="3fa85f64-5717-4562-b3fc-2c963f66af11"},
                    new SysDictModel<string>{Key="电气工程学院",Value="3fa85f64-5717-4562-b3fc-2c963f66af12"}
                    };
                    case 2:
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="校内读者A",Value="1" },
                    new SysDictModel<string>{Key="校内读者B",Value="2" },
                    new SysDictModel<string>{Key="毕业生",Value="3"},
                    new SysDictModel<string>{Key="校友读者",Value="4"},
                    new SysDictModel<string>{Key="社会读者",Value="5"}
                    };
                    case 3:
                        return new List<SysDictModel<string>> {
                    new SysDictModel<string>{Key="本科生",Value="1" },
                    new SysDictModel<string>{Key="应届毕业生",Value="2" },
                    new SysDictModel<string>{Key="硕研生",Value="3"},
                    new SysDictModel<string>{Key="博研生",Value="4"},
                    new SysDictModel<string>{Key="教职工",Value="5"}
                    };
                    default:
                        return new List<SysDictModel<string>>();
                }
            }
            else
            {
                return (await GetUserDictionary(dicType)).Items.Select(p => new SysDictModel<string>
                {
                    Key = p.Key,
                    Value = p.Value
                }).ToList();
            }
        }


        /// <summary>
        /// 获取场景内所有栏目列表
        /// </summary>
        /// <param name="sceneId">场景Id</param>
        /// <returns></returns>
        public async Task<List<AppPlateViewModel>> GetAppPlateListBySceneId(string sceneId)
        {
            //var queryScene = _sceneRepository.Where(p => !p.DeleteFlag);
            if (!SiteGlobalConfig.AppBaseConfig.IsMock)
            {
                var result = new List<AppPlateViewModel> {
            new AppPlateViewModel{ AppId="app85f64-5717-4562-b3fc-2c963f66af12"
            ,AppName="新闻发布"
            ,Icon="/uploads/cqu/20211025/b3b567fba0894b2586408c6ce836f384.jpg"
            ,PlateList=new List<AppPlateItem>{
                new AppPlateItem{ CreateTime=DateTime.Now,Id="appplate-5717-4562-b3fc-2c963f66af12",Name="馆内动态",VisitUrl="#"},
                new AppPlateItem{ CreateTime=DateTime.Now,Id="appplate-6717-4562-b3fc-2c963f66af12",Name="新闻公告",VisitUrl="#"}
            } },
            new AppPlateViewModel{ AppId="app95f64-5717-4562-b3fc-2c963f66af12"
            ,AppName="数据库导航"
            ,Icon="/uploads/cqu/20211025/f8048d98464d41ed8bc4e8a6e0954f03.jpg"
            ,PlateList=new List<AppPlateItem>{
                new AppPlateItem{ CreateTime=DateTime.Now,Id="1ceb8abe-f5cc-4d2b-a524-3a55acce293b",Name="热门数据库",VisitUrl=$"{SiteGlobalConfig.AppBaseConfig.TemplateSiteUrl}/database_nav_sys/temp1"},
                new AppPlateItem{ CreateTime=DateTime.Now,Id="appplate-8717-4562-b3fc-2c963f66af12",Name="试用数据库",VisitUrl="#"}
            } }
            };

                return result;
            }
            else
            {
                var scene = await GetSceneDetail(sceneId);
                var appIds = "";
                var appPlateDic = new Dictionary<string, List<string>>();
                foreach (var screen in scene.SceneScreens)
                {
                    foreach (var sceneApp in screen.SceneApps)
                    {
                        appIds = $"{appIds}{(string.IsNullOrEmpty(appIds) ? "" : ",")}{sceneApp.AppId}";
                        if (!appPlateDic.ContainsKey(sceneApp.AppId))
                        {
                            appPlateDic.Add(sceneApp.AppId, sceneApp.AppPlateItems.Select(p => p.Id).ToList());
                        }
                        else
                        {
                            appPlateDic[sceneApp.AppId].AddRange(sceneApp.AppPlateItems.Select(p => p.Id));
                        }
                    }
                }
                var request = new AppListRequest { AppRouteCodes = appIds };
                var result = await GetAppList(request);
                var finalResult = result.AppList.Select(p => new AppPlateViewModel
                {
                    AppId = p.AppId,
                    Icon = p.Icon,
                    AppName = p.Name,
                    PlateList = GetAppColumnList(new AppColumnListRequest { AppId = p.AppId }).Result.AppColumnList.Where(i => appPlateDic[p.AppId].Contains(i.ColumnId)).Select(pl => new AppPlateItem
                    {
                        CreateTime = string.IsNullOrEmpty(pl.CreateTime) ? DateTime.MinValue : DateTime.Parse(pl.CreateTime),
                        Id = pl.ColumnId,
                        Name = pl.Name,
                        VisitUrl = $"/{p.RouteCode}/#{pl.VisitUrl}"
                    }).ToList()
                }).ToList();
                return finalResult.Where(p => p.PlateList != null && p.PlateList.Count > 0).ToList();
            }
        }

        #region 私有方法


        /// <summary>
        /// 根据场景实例获取封面
        /// </summary>
        /// <param name="terminalInstanceId">场景实例Id</param>
        /// <returns></returns>
        private async Task<string> GetCoverByTerminalInstanceId(string terminalInstanceId)
        {
            var terminalInstance = await _terminalInstanceRepository.FirstOrDefaultAsync(p => p.Id.ToString() == terminalInstanceId && !p.DeleteFlag);

            switch (terminalInstance.TerminalType)
            {
                case 1:
                    return "/scene/tp_PC.png";
                case 2:
                case 3:
                case 4:
                    return "/scene/tp_APP.png";
                case 5:
                    return "/scene/tp_SCREEN.png";
                default:
                    return "/scene/tp_PC.png";
            }
        }
        #endregion
    }
}

