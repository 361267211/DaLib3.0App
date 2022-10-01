using DotNetCore.CAP;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter;
using SmartLibrary.Navigation.Application.Enums;
using SmartLibrary.Navigation.Application.Interceptors;
using SmartLibrary.Navigation.Application.ViewModel;
using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibrary.Navigation.EntityFramework.Core.Entitys;
using SmartLibrary.Navigation.Utility;
using SmartLibrary.Navigation.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    /// <summary>
    /// 名    称：NavigationColumnService
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 14:25:55
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationColumnService : INavigationColumnService, IScoped
    {
        private ICapPublisher _CapPublisher;
        private IRepository<NavigationLableInfo> _LableRepository;
        private IRepository<NavigationColumn> _ColumnRepository;
        private IRepository<NavigationCatalogue> _NavigationRepository;
        private IRepository<Content> _ContentRepository;
        private IRepository<NavigationColumnPermissions> _PermissionRepository;
        private INavigationSettingsService _SettingsService;
        private INavigationLableInfoService _LableService;
        private INavigationCatalogueService _CatalogueService;
        private IContentService _ContentService;
        private readonly IRepository<SysMenuPermission> _SysMenuPermRepository;
        private TenantInfo _TenantInfo;
        private readonly int UserRole = 3;//1 管理者，2 操作者，3 浏览者
        private readonly IHttpClientFactory _HttpClientFactory;

        public NavigationColumnService(ICapPublisher capPublisher,
                                       IRepository<NavigationLableInfo> lableRepository,
                                       IRepository<NavigationColumn> columnRepository,
                                       IRepository<NavigationCatalogue> navigationRepository,
                                       IRepository<Content> contentRepository,
                                       INavigationSettingsService settingsService,
                                       INavigationLableInfoService lableService,
                                       INavigationCatalogueService catalogueService,
                                       IContentService contentService,
                                       IRepository<NavigationColumnPermissions> permissionRepository,
                                       IRepository<SysMenuPermission> sysMenuPermRepository,
                                       IHttpClientFactory httpClientFactory,
                                       TenantInfo tenantInfo)
        {
            _CapPublisher = capPublisher;
            _LableRepository = lableRepository;
            _ColumnRepository = columnRepository;
            _NavigationRepository = navigationRepository;
            _ContentRepository = contentRepository;
            _SettingsService = settingsService;
            _LableService = lableService;
            _CatalogueService = catalogueService;
            _ContentService = contentService;
            _PermissionRepository = permissionRepository;
            _SysMenuPermRepository = sysMenuPermRepository;
            _TenantInfo = tenantInfo;
            _HttpClientFactory = httpClientFactory;
            //UserRoleReply userRole = new UserRoleReply();
            ////调用grpc，获取角色信息，
            //userRole = App.GetService<IGrpcClientResolver>().EnsureClient<UserRoleGrpcService.UserRoleGrpcServiceClient>().GetUserRole(new UserRoleRequest {  UserKey = _tenantInfo.UserKey.ToString() });
            //UserRole = userRole.UserRole;

            //模拟假数据TODO：GRPC调用正常后删除
            UserRole = 2;
        }

        #region NavigationColumn 栏目管理

        /// <summary>
        /// 获取专题栏目信息 根据id
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public async Task<GetColumnLinkInfoReply> GetColumnLinkInfo(string columnId)
        {
            var column = await _ColumnRepository.DetachedEntities.FirstOrDefaultAsync(e => e.Id == columnId && !e.DeleteFlag);
            return column.Adapt<GetColumnLinkInfoReply>();
        }

        /// <summary>
        /// 获取标签分组及信息导航栏目
        /// </summary>
        /// <returns></returns>
        public async Task<List<LableNavigationColumnView>> GetLableNavigationColumnList()
        {
            var managerID = _TenantInfo.UserKey;
            if (UserRole == 1)
            {
                managerID = null;
            }
            List<LableNavigationColumnView> list = new List<LableNavigationColumnView>();
            var result = _LableRepository.Where(d => !d.DeleteFlag).ToList();
            LableNavigationColumnView lableNavigationDefault = new LableNavigationColumnView();
            lableNavigationDefault.LableID = "";
            lableNavigationDefault.LableName = "默认标签";
            var columnListDefault = from col in _ColumnRepository.Where(d => !d.DeleteFlag && string.IsNullOrEmpty(d.Label)).AsQueryable()
                                    join perm in _PermissionRepository.Where(d => !d.DeleteFlag && (string.IsNullOrEmpty(managerID) ? true : d.ManagerID == managerID)) on col.Id equals perm.ColumnID into res
                                    from labNC in res.DefaultIfEmpty()
                                    select new LableNavigationColumn { ColumnID = col.Id, Title = col.Title, Enable = (string.IsNullOrEmpty(managerID) ? true : labNC.ManagerID == managerID) };
            lableNavigationDefault.ColumnList = columnListDefault.Distinct().ToList();
            if (columnListDefault.Count() > 0)
                list.Add(lableNavigationDefault);
            foreach (var item in result)
            {
                LableNavigationColumnView lableNavigation = new LableNavigationColumnView();
                lableNavigation.LableID = item.Id;
                lableNavigation.LableName = item.Title;
                var columnList = from col in _ColumnRepository.Where(d => !d.DeleteFlag && d.Label.Contains(item.Id)).AsQueryable()
                                 join perm in _PermissionRepository.Where(d => !d.DeleteFlag && (string.IsNullOrEmpty(managerID) ? true : d.ManagerID == managerID)) on col.Id equals perm.ColumnID into res
                                 from labNC in res.DefaultIfEmpty()
                                 select new LableNavigationColumn { ColumnID = col.Id, Title = col.Title, Enable = (string.IsNullOrEmpty(managerID) ? true : labNC.ManagerID == managerID) };
                lableNavigation.ColumnList = columnList.Distinct().ToList();
                if (columnList.Count() > 0)
                    list.Add(lableNavigation);
            }
            return await Task.FromResult(list);
        }

        /// <summary>
        /// 获取信息导航栏目
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        public async Task<NavigationColumnDto> GetNavigationColumn(string columnID)
        {
            var result = _ColumnRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && d.Id == columnID);
            if (result == null)
                return null;
            var item = result.Adapt<NavigationColumnDto>();

            var baseUrl = await this.GetBaseUri("navigation");

            item.LinkUrl = $"{baseUrl}/#/web_list?c_id={columnID}";
            var itemLables = item.Label.Split(';');
            var lables = from lab in _LableRepository.Where(d => itemLables.Contains(d.Id))
                         select new KeyValuePair<string, string>(lab.Id, lab.Title);
            item.LabelKV = lables.ToList();



            item.SysMesListKV = EnumUtils.GetValueName(typeof(SysMesListEnum), item.SysMesList);
            return await Task.FromResult(item);
        }

        /// <summary>
        /// 获取应用前台地址
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public async Task<string> GetBaseUri(string appId)
        {
            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            AppBaseUriRequest request1 = new AppBaseUriRequest { AppRouteCode = appId };
            AppBaseUriReply reply1 = new AppBaseUriReply();
            try
            {
                reply1 = await grpcClient1.GetAppBaseUriAsync(request1);
                return reply1.FrontUrl;
            }
            catch (Exception)
            {
                throw Oops.Oh("grpc调用异常");
            }
        }

        /// <summary>
        /// 添加信息导航栏目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> AddNavigationColumn(NavigationColumnDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            if (model.Status != 1 && model.Status != 2)
            {
                result.Succeeded = false;
                result.Message = "栏目状态不存在";
                return result;
            }
            SideListEnum? sideList = Converter.ToType<SideListEnum?>(model.SideList, null);
            if (sideList == null)
            {
                result.Succeeded = false;
                result.Message = "侧边列表选择项不存在";
                return result;
            }

            if (!string.IsNullOrEmpty(model.SysMesList))
            {
                int[] sysMesList = model.SysMesList.Split(';').Select(d => Converter.ObjectToInt(d, -1)).ToArray();
                foreach (var item in sysMesList)
                {
                    SysMesListEnum? temp = Converter.ToType<SysMesListEnum?>(item, null);
                    if (temp == null)
                    {
                        result.Succeeded = false;
                        result.Message = "显示系统信息选择项不存在";
                        return result;
                    }
                }
            }

            var settings = _SettingsService.GetNavigationSettings();
            if (settings.Result.Comments)
            {
                model.IsOpenFeedback = true;
            }

            var navigationCL = model.Adapt<NavigationColumn>();
            navigationCL.CreatedTime = DateTime.Now;

            navigationCL.Label = _LableService.ProcessLablesFromLableStr(navigationCL.Label).Result;
            var nagColumn = await _ColumnRepository.InsertNowAsync(navigationCL);

            await AppColumnOperation(navigationCL.Id, navigationCL.Title, DateTime.Now.ToString(), 1, "/admin_navigationProgram", "navigation");

            #region 新增栏目菜单权限
            var columnID = navigationCL.Id;
            var columnName = navigationCL.Title;

            var maxPath = _SysMenuPermRepository.Where(d => d.Pid == "1").Max(d => d.Path);
            var maxPathID = Converter.ObjectToInt(maxPath) + 1;
            var parentFullPath = $"1-{maxPathID}";
            //栏目管理
            _SysMenuPermRepository.Context.BulkInsert(
               new[]
               {
                    new SysMenuPermission { Id = Guid.NewGuid(), Pid = "1-1-1", Path = maxPathID.ToString(), FullPath = $"1-1-1-{maxPathID}", Name = "更新信息导航栏目API", Type = 5, Router = "路由地址", Component = "组件地址", Permission = $"api:navigation:navigation-column-update:{columnID}", Remark = "更新信息导航栏目API", IsSysMenu = false,Visible=true },
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-2",Path = maxPathID.ToString(), FullPath = $"1-1-2-{maxPathID}", Name="删除信息导航栏目API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-column-delete:{columnID}",Remark="删除信息导航栏目API",IsSysMenu = false,Visible=true },
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-4",Path=maxPathID.ToString(),FullPath=$"1-1-4-{maxPathID}", Name="获取信息导航栏目API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-column-get:{columnID}",Remark="获取信息导航栏目API",IsSysMenu = false,Visible=true },
               }
           );

            //当前栏目
            _SysMenuPermRepository.Context.BulkInsert(
                new[]
                {
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid="1",Path=maxPathID.ToString(),FullPath=parentFullPath, Name=columnName,Type=1,Router="路由地址",Component=$"/admin_programInfo?id={columnID}",Permission=$"column:{columnID}",Remark=columnName,IsSysMenu = false,Visible=true,Sort=maxPathID },
                     //*-1 内容管理
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid=parentFullPath,Path="1",FullPath=$"{parentFullPath}-1", Name="内容管理",Type=1,Router="路由地址",Component="组件地址",Permission=$"content:{columnID}",Remark="内容管理",IsSysMenu = false,Visible=true },
                     //*-1-1 新增
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1",Path="1",FullPath=$"{parentFullPath}-1-1", Name="新增",Type=4,Router="路由地址",Component="组件地址",Permission="add",Remark="新增",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1-1",Path="1",FullPath=$"{parentFullPath}-1-1-1", Name="添加信息导航内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content-add:{columnID}",Remark="添加信息导航内容API",IsSysMenu = false,Visible=true },
                      //*-1-2 上下架
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1",Path="2",FullPath=$"{parentFullPath}-1-2", Name="上下架",Type=4,Router="路由地址",Component="组件地址",Permission="onoffshelf",Remark="上下架",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1-2",Path="1",FullPath=$"{parentFullPath}-1-2-1", Name="批量下架/上架内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:change-content-status:{columnID}",Remark="批量下架/上架内容API",IsSysMenu = false,Visible=true },
                      //*-1-3 删除
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1",Path="3",FullPath=$"{parentFullPath}-1-3", Name="删除",Type=4,Router="路由地址",Component="组件地址",Permission="delete",Remark="删除",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1-3",Path="1",FullPath=$"{parentFullPath}-1-3-1", Name="批量删除内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content-delete:{columnID}",Remark="批量删除内容API",IsSysMenu = false,Visible=true },
                      //*-1-4 编辑
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1",Path="4",FullPath=$"{parentFullPath}-1-4", Name="编辑",Type=4,Router="路由地址",Component="组件地址",Permission="edit",Remark="编辑",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1-4",Path="1",FullPath=$"{parentFullPath}-1-4-1", Name="修改信息导航内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content-update:{columnID}",Remark="修改信息导航内容API",IsSysMenu = false,Visible=true },
                      //*-1-5 排序
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1",Path="5",FullPath=$"{parentFullPath}-1-5", Name="排序",Type=4,Router="路由地址",Component="组件地址",Permission="sort",Remark="排序",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1-5",Path="1",FullPath=$"{parentFullPath}-1-5-1", Name="内容弹窗输入序号排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:sort-content-by-index:{columnID}",Remark="内容弹窗输入序号排序API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1-5",Path="2",FullPath=$"{parentFullPath}-1-5-2", Name="内容拖动排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:sort-content-by-target:{columnID}",Remark="内容拖动排序API",IsSysMenu = false,Visible=true },
                      //*-1-6 查询
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1",Path="6",FullPath=$"{parentFullPath}-1-6", Name="查询",Type=3,Router="路由地址",Component="组件地址",Permission="query",Remark="查询",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1-6",Path="1",FullPath=$"{parentFullPath}-1-6-1", Name="获取信息导航内容列表API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content-list:{columnID}",Remark="获取信息导航内容列表API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1-6",Path="2",FullPath=$"{parentFullPath}-1-6-2", Name="获取信息导航内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content:{columnID}",Remark="获取信息导航内容API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1-6",Path="3",FullPath=$"{parentFullPath}-1-6-3", Name="获取内容操作日志API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content-process-log-get:{columnID}",Remark="获取内容操作日志API",IsSysMenu = false,Visible=true },

                      //*-1 目录管理
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid=parentFullPath,Path="2",FullPath=$"{parentFullPath}-2", Name="目录管理",Type=1,Router="路由地址",Component="组件地址",Permission=$"catalogue:{columnID}",Remark="目录管理",IsSysMenu = false,Visible=true },
                     //*-2-1 新增
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2",Path="1",FullPath=$"{parentFullPath}-2-1", Name="新增",Type=4,Router="路由地址",Component="组件地址",Permission="add",Remark="新增",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2-1",Path="1",FullPath=$"{parentFullPath}-2-1-1", Name="添加信息导航目录API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-catalogue-add:{columnID}",Remark="添加信息导航目录API",IsSysMenu = false,Visible=true },
                      //*-2-2 上下架
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2",Path="2",FullPath=$"{parentFullPath}-2-2", Name="上下架",Type=4,Router="路由地址",Component="组件地址",Permission="onoffshelf",Remark="上下架",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2-2",Path="1",FullPath=$"{parentFullPath}-2-2-1", Name="批量下架/上架目录API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:change-navigation-catalogue-status:{columnID}",Remark="批量下架/上架目录API",IsSysMenu = false,Visible=true },
                      //*-2-3 删除
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2",Path="3",FullPath=$"{parentFullPath}-2-3", Name="删除",Type=4,Router="路由地址",Component="组件地址",Permission="delete",Remark="删除",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2-3",Path="1",FullPath=$"{parentFullPath}-2-3-1", Name="批量删除目录API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-catalogue-delete:{columnID}",Remark="批量删除目录API",IsSysMenu = false,Visible=true },
                      //*-2-4 编辑
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2",Path="4",FullPath=$"{parentFullPath}-2-4", Name="编辑",Type=4,Router="路由地址",Component="组件地址",Permission="edit",Remark="编辑",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2-4",Path="1",FullPath=$"{parentFullPath}-2-4-1", Name="修改信息导航目录API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-catalogue-update:{columnID}",Remark="修改信息导航目录API",IsSysMenu = false,Visible=true },
                      //*-2-5 排序
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2",Path="5",FullPath=$"{parentFullPath}-2-5", Name="排序",Type=4,Router="路由地址",Component="组件地址",Permission="sort",Remark="排序",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2-5",Path="1",FullPath=$"{parentFullPath}-2-5-1", Name="目录弹窗输入序号排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:sort-catalogue-by-index:{columnID}",Remark="目录弹窗输入序号排序API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2-5",Path="2",FullPath=$"{parentFullPath}-2-5-2", Name="目录拖动排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:sort-catalogue-by-target:{columnID}",Remark="目录拖动排序API",IsSysMenu = false,Visible=true },
                      //*-2-6 查询
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2",Path="6",FullPath=$"{parentFullPath}-2-6", Name="查询",Type=3,Router="路由地址",Component="组件地址",Permission="query",Remark="查询",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2-6",Path="1",FullPath=$"{parentFullPath}-2-6-1", Name="获取信息导航目录列表API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-catalogue-list-get:{columnID}",Remark="获取信息导航目录列表API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2-6",Path="2",FullPath=$"{parentFullPath}-2-6-2", Name="获取信息导航目录API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-catalogue-get:{columnID}",Remark="获取信息导航目录API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2-6",Path="3",FullPath=$"{parentFullPath}-2-6-3", Name="获取目录操作日志API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:catalogue-process-log-get:{columnID}",Remark="获取目录操作日志API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2-6",Path="4",FullPath=$"{parentFullPath}-2-6-4", Name="获取全部信息导航目录树API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:all-navigation-catalogue-process-tree-list-get:{columnID}",Remark="获取全部信息导航目录树API",IsSysMenu = false,Visible=true },
                }
            );
            #endregion

            return result;
        }

        /// <summary>
        /// 同步栏目信息到应用中心
        /// </summary>
        /// <param name="columnId"></param>
        /// <param name="columnName"></param>
        /// <param name="createTime"></param>
        /// <param name="operationType"></param>
        /// <param name="visitUrl"></param>
        /// <param name="appRouteCode"></param>
        /// <returns></returns>
        public async Task AppColumnOperation(string columnId, string columnName, string createTime, int operationType, string visitUrl, string appRouteCode)
        {
            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            AppColumnOperationRequest request1 = new AppColumnOperationRequest
            {
                AppRouteCode = appRouteCode,
                ColumnId = columnId,
                ColumnName = columnName,
                CreateTime = createTime,
                OperationType = operationType,
                VisitUrl = visitUrl,
            };
            AppColumnOperationReply reply1 = new AppColumnOperationReply();
            try
            {
                reply1 = await grpcClient1.AppColumnOperationAsync(request1);
                return;
            }
            catch (Exception)
            {
                throw Oops.Oh("grpc调用异常");
            }
        }

        /// <summary>
        /// 更新信息导航栏目
        /// </summary>
        /// <param name="model"></param>
        public async Task<ApiResultInfoModel> UpdateNavigationColumn(NavigationColumnDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            if (model.Status != 1 && model.Status != 2)
            {
                result.Succeeded = false;
                result.Message = "栏目状态不存在";
                return result;
            }
            SideListEnum? sideList = Converter.ToType<SideListEnum?>(model.SideList, null);
            if (sideList == null)
            {
                result.Succeeded = false;
                result.Message = "侧边列表选择项不存在";
                return result;
            }

            if (!string.IsNullOrEmpty(model.SysMesList))
            {
                int[] sysMesList = model.SysMesList.Split(';').Select(d => Converter.ObjectToInt(d, -1)).ToArray();
                foreach (var item in sysMesList)
                {
                    SysMesListEnum? temp = Converter.ToType<SysMesListEnum?>(item, null);
                    if (temp == null)
                    {
                        result.Succeeded = false;
                        result.Message = "显示系统信息选择项不存在";
                        return result;
                    }
                }
            }
            var navigationClOld = _ColumnRepository.Entities.AsNoTracking().FirstOrDefault(d => d.Id == model.Id);
            if (navigationClOld == null)
            {
                result.Succeeded = false;
                result.Message = "栏目不存在！";
                return result;
            }

            var navigationCL = model.Adapt<NavigationColumn>();
            navigationCL.UpdatedTime = DateTime.Now;
            navigationCL.Label = _LableService.ProcessLablesFromLableStr(navigationCL.Label).Result;
            var navigationColumn = await _ColumnRepository.UpdateExcludeAsync(navigationCL, new[] { nameof(navigationCL.CreatedTime), nameof(navigationCL.TenantId) });

            await AppColumnOperation(navigationCL.Id, navigationCL.Title, DateTime.Now.ToString(), 2, "/admin_navigationProgram", "navigation");


            return result;
        }

        /// <summary>
        /// 删除信息导航栏目
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNavigationColumn(string columnID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var model = _ColumnRepository.FindOrDefault(columnID);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "栏目不存在！";
            }
            model.DeleteFlag = true;
            model.UpdatedTime = DateTime.Now;
            await _ColumnRepository.UpdateAsync(model);

            //删除对应权限项目
            await _PermissionRepository.Context.BatchUpdate<SysMenuPermission>()
                .Set(e => e.DeleteFlag, e => true)
                .Where(e => e.Permission.EndsWith($":{columnID}"))
                .ExecuteAsync();

            //同步到应用中心
            await AppColumnOperation(columnID, "", "", 3, "", "navigation");

            return result;
        }

        /// <summary>
        /// 删除信息导航栏目
        /// </summary>
        /// <param name="columnIDs"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNavigationColumn(string[] columnIDs)
        {
            var result = new ApiResultInfoModel { Succeeded = true };

            foreach (var columnID in columnIDs)
            {
                var model = _ColumnRepository.FindOrDefault(columnID);
                if (model == null)
                {
                    result.Succeeded = false;
                    result.Message = "栏目不存在！";
                    return result;
                }
                model.DeleteFlag = true;
                model.UpdatedTime = DateTime.Now;
                await _ColumnRepository.UpdateAsync(model);
            }
            return result;
        }

        /// <summary>
        /// 获取前台信息导航栏目数据
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<List<ProntNavigationColumnListView>> GetProntNavigationColumnList(string columnID)
        {
            var thisColumn = await _ColumnRepository.FindOrDefaultAsync(columnID);
            //左侧显示同标签栏目
            bool isSameLableColumn = thisColumn.SideList == ((int)SideListEnum.SameLableColumn);

            //信息导航栏目
            List<ProntNavigationColumnListView> columnList = new();
            List<NavigationColumn> navigationClList = new();
            //同标签的栏目
            if (isSameLableColumn)
            {
                var columnLables = thisColumn.Label.Split(';');
                foreach (var item in columnLables)
                {
                    var sameLableColumn = await _ColumnRepository.Where(d => d.Label.Contains(item)).ToListAsync();
                    navigationClList.AddRange(sameLableColumn);
                }
            }
            else
            {
                navigationClList.Add(thisColumn);
            }
            foreach (var column in navigationClList.Distinct())
            {
                //左侧显示信息导航内容 
                bool isSameColumnNavigationContent = column.SideList == ((int)SideListEnum.SameColumnNavigationContent);
                var cataList = await _CatalogueService.GetNavigationCatalogueListForPront(column.Id, isSameColumnNavigationContent);
                columnList.Add(new ProntNavigationColumnListView
                {
                    ColumnID = column.Id,
                    Name = column.Title,
                    CatalogueList = cataList
                });
            }

            return columnList;
        }

        /// <summary>
        /// 获取底部链接列表
        /// </summary>
        /// <param name="plateSign"></param>
        /// <param name="count"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public async Task<List<ContentInfoDto>> GetPlatListAsync(string plateSign, int count, int itemType)
        {
            using (var httpClient = this._HttpClientFactory.CreateClient("webapi2.2"))
            {
                var step1 = await httpClient.GetAsync($"/api/v1.0/GetPlatList?plateSign={plateSign}&count={count}&itemType={itemType}");
                step1.EnsureSuccessStatusCode();
                var json = await step1.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json)) return default;
                var temp = JSON.Deserialize<ApiResult<List<ContentInfoDto>>>(json);
                return temp?.result;
            }
        }

        /// <summary>
        /// 获取底部链接列表
        /// </summary>
        /// <param name="plateSign"></param>
        /// <param name="count"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public async Task<string> GetAllianceCertifyUrlAsync()
        {
            using (var httpClient = this._HttpClientFactory.CreateClient("webapi2.2"))
            {
                var step1 = await httpClient.GetAsync("/api/v1.0/GetAllianceCertifyUrl");
                step1.EnsureSuccessStatusCode();
                var json = await step1.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(json)) return default;
                var temp = JSON.Deserialize<ApiResult<string>>(json);
                return temp?.result;
            }
        }

        #endregion
    }
}
