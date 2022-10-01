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
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Open.Common.Dtos;
using SmartLibrary.Open.Common.Enums;
using SmartLibrary.Open.Common.Utility;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.Services.Dtos.Application;
using SmartLibrary.Open.Services.Dtos.Infomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public class ApplicationService : IScoped, IApplicationService
    {
        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<MicroApplication> _microAppRepository;

        /// <summary>
        /// 应用分支数据仓储
        /// </summary>
        private readonly IRepository<AppBranch> _appBranchRepository;

        /// <summary>
        /// 应用动态数据仓储
        /// </summary>
        private readonly IRepository<AppDynamic> _appDynamicInfoRepository;

        /// <summary>
        /// 活动消息数据仓储
        /// </summary>
        private readonly IRepository<ActivityInfo> _activityInfoRepository;

        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<AppWidget> _appWidgetRepository;

        /// <summary>
        /// 消息数据仓储
        /// </summary>
        private readonly IRepository<Information> _informationRepository;

        /// <summary>
        /// 服务包数据仓储
        /// </summary>
        private readonly IRepository<AppServicePack> _appServicePackRepository;

        /// <summary>
        /// 服务类型数据仓储
        /// </summary>
        private readonly IRepository<AppServiceType> _appServiceTypeRepository;

        /// <summary>
        /// 应用字典数据仓储
        /// </summary>
        private readonly IRepository<AppDictioanry> _appDictioanryRepository;

        /// <summary>
        /// 应用用户授权数据仓储
        /// </summary>
        private readonly IRepository<AppSpecificCustomer> _appSpecificCustomerRepository;

        /// <summary>
        /// 客户应用数据仓储
        /// </summary>
        private readonly IRepository<CustomerAppUsage> _customerAppUsageRepository;

        /// <summary>
        /// 客户应用数据仓储
        /// </summary>
        private readonly IRepository<Customer> _customerRepository;

        /// <summary>
        /// 客户应用数据仓储
        /// </summary>
        private readonly IRepository<Developer> _developerRepository;

        /// <summary>
        /// 应用入口数据仓储
        /// </summary>
        private readonly IRepository<AppBranchEntryPoint> _appBranchEntryPointRepository;

        /// <summary>
        /// 部署环境数据仓储
        /// </summary>
        private readonly IRepository<Deployment> _deploymentRepository;

        /// <summary>
        /// 应用支持排序字段数据仓储
        /// </summary>
        private readonly IRepository<AppAvailibleSortField> _appvailibleSortFieldRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationService(IRepository<MicroApplication> microAppRepository,
                                  IRepository<AppBranch> appBranchRepository,
                                  IRepository<AppServicePack> appServicePackRepository,
                                  IRepository<AppServiceType> appServiceTypeRepository,
                                  IRepository<AppDictioanry> appDictioanryRepository,
                                  IRepository<AppSpecificCustomer> appSpecificCustomerRepository,
                                  IRepository<CustomerAppUsage> customerAppUsageRepository,
                                  IRepository<Developer> developerRepository,
                                  IRepository<AppDynamic> appDynamicInfoRepository,
                                  IRepository<ActivityInfo> activityInfoRepository,
                                  IRepository<Information> informationRepository,
                                  IRepository<Customer> customerRepository,
                                  IRepository<AppBranchEntryPoint> appBranchEntryPointRepository,
                                  IRepository<AppWidget> appWidgetRepository,
                                  IRepository<Deployment> deploymentRepository,
                                  IRepository<AppAvailibleSortField> appvailibleSortFieldRepository)
        {
            _microAppRepository = microAppRepository;
            _appBranchRepository = appBranchRepository;
            _appServicePackRepository = appServicePackRepository;
            _appServiceTypeRepository = appServiceTypeRepository;
            _appDictioanryRepository = appDictioanryRepository;
            _appSpecificCustomerRepository = appSpecificCustomerRepository;
            _customerAppUsageRepository = customerAppUsageRepository;
            _developerRepository = developerRepository;
            _appDynamicInfoRepository = appDynamicInfoRepository;
            _activityInfoRepository = activityInfoRepository;
            _informationRepository = informationRepository;
            _customerRepository = customerRepository;
            _appBranchEntryPointRepository = appBranchEntryPointRepository;
            _appWidgetRepository = appWidgetRepository;
            _deploymentRepository = deploymentRepository;
            _appvailibleSortFieldRepository = appvailibleSortFieldRepository;
        }


        /// <summary>
        /// 获取字典
        /// </summary>
        /// <returns></returns>
        public Task<AppInitModel> GetDictionary()
        {
            var serviceTypeQuery = _appDictioanryRepository.Where(p => p.DictType == "AppServiceType" && !p.DeleteFlag).Select(p => new SysDictModel { Key = p.Name, Value = p.Value });
            var servicePackQuery = _appDictioanryRepository.Where(p => p.DictType == "AppServicePack" && !p.DeleteFlag).Select(p => new SysDictModel { Key = p.Name, Value = p.Value });
            var statusDict = EnumTools.EnumToList<AppStatusEnum>();
            return Task.FromResult(new AppInitModel
            {
                ServiceTypeDict = serviceTypeQuery.ToList(),
                ServicePackDict = servicePackQuery.ToList(),
                StatusDict = statusDict
            });
        }

        /// <summary>
        /// 查询App表格数据
        /// </summary>
        /// <param name="queryFilter">App表格查询条件</param>
        /// <returns></returns>
        public async Task<PagedList<AppTableViewModel>> QueryTableData(AppTableQuery queryFilter)
        {
            var queryApplication = _microAppRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryDev = _developerRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryServiceType = _appServiceTypeRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryCustomer = _customerAppUsageRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryBranch = _appBranchRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();

            var serviceTypes = await _appServiceTypeRepository.Where(p => !p.DeleteFlag).AsNoTracking().ToListAsync();

            var query = from app in queryApplication
                        join dev in queryDev on app.DevId equals dev.Id.ToString()
                        join type in queryServiceType on app.Id.ToString() equals type.AppId
                        where (string.IsNullOrEmpty(queryFilter.ServiceType) || type.DictValue == queryFilter.ServiceType)
            && (!queryFilter.Status.HasValue || app.Status == queryFilter.Status)
            && (!queryFilter.Terminal.HasValue || app.Terminal.Contains(queryFilter.Terminal.ToString()))
            && (string.IsNullOrEmpty(queryFilter.Keyword) || (app.Name.Contains(queryFilter.Keyword) || dev.Name.Contains(queryFilter.Keyword)))
                        group app by new { app.Id, app.Name, app.Terminal, app.Status, app.CreatedTime, app.UpdatedTime, DevName = dev.Name } into g
                        select new AppTableViewModel
                        {
                            ID = g.Key.Id,
                            CreateTime = g.Key.CreatedTime.LocalDateTime,
                            UpdateTime = g.Key.UpdatedTime.HasValue ? g.Key.UpdatedTime.Value.LocalDateTime : null,
                            AppName = g.Key.Name,
                            DevName = g.Key.DevName,
                            Terminal = g.Key.Terminal,
                            Status = g.Key.Status,
                            FormalCount = queryCustomer.Count(p => p.AppId == g.Key.Id.ToString() && !p.DeleteFlag && p.Status == 2 && p.AuthType == 1),
                            TryCount = queryCustomer.Count(p => p.AppId == g.Key.Id.ToString() && !p.DeleteFlag && p.Status == 2 && p.AuthType == 2),
                            Version = queryBranch.FirstOrDefault(p => p.AppId == g.Key.Id.ToString() && p.IsMaster).Version,
                            BranchCount = queryBranch.Count(p => p.AppId == g.Key.Id.ToString()),
                            //ServiceType = queryServiceType.AsQueryable().Where(p=>p.AppID == g.Key.Id.ToString()).Select(p=> p.Id).ToList()
                        };

            if (!string.IsNullOrEmpty(queryFilter.SortField))
            {
                query = query.AsNoTracking().ApplyOrder(queryFilter.SortField, queryFilter.IsAsc);
            }
            var table = await query.AsNoTracking().ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);

            foreach (var item in table.Items)
            {
                item.ServiceType = serviceTypes.Where(p => p.AppId == item.ID.ToString()).Select(p => p.DictValue).ToList();
            }


            return table;
        }

        /// <summary>
        /// 获取应用信息系详情
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public async Task<AppViewModel> GetById(Guid appId)
        {
            var app = await _microAppRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id == appId);
            var queryDev = _developerRepository.Where(p => !p.DeleteFlag && p.Id.ToString() == app.DevId);
            var queryServiceType = _appServiceTypeRepository.Where(p => !p.DeleteFlag && p.AppId == appId.ToString());
            var queryServicePack = _appServicePackRepository.Where(p => !p.DeleteFlag && p.AppId == appId.ToString());
            var queryCustomer = _customerAppUsageRepository.Where(p => !p.DeleteFlag && p.AppId == appId.ToString());
            var queryBranch = _appBranchRepository.Where(p => !p.DeleteFlag && p.AppId == appId.ToString());

            var result = new AppViewModel
            {
                ID = app.Id,
                Terminal = app.Terminal.Split(',').ToList(),
                Status = app.Status,
                AdvisePrice = app.AdvisePrice,
                Desc = app.Desc,
                DevID = app.DevId,
                FreeTry = app.FreeTry,
                Icon = app.Icon,
                Intro = app.Intro,
                Name = app.Name,
                PriceType = app.PriceType,
                Scene = app.UseScene.Split(',').ToList(),
                ServicePacks = queryServicePack.Select(p => p.Id.ToString()).ToList(),
                ServiceTypes = queryServiceType.Select(p => p.Id.ToString()).ToList()
            };

            return result;
        }

        /// <summary>
        /// 查询App版本变更数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<PagedList<AppDynamicInfoViewModel>> QueryVersionRecordData(VersionTableQuery queryFilter)
        {
            var queryApplication = _microAppRepository.Where(p => !p.DeleteFlag);
            var queryAppDynamicInfo = _appDynamicInfoRepository.Where(p => !p.DeleteFlag);
            var queryInfomation = _informationRepository.Where(p => !p.DeleteFlag);
            var queryactivityInfo = _activityInfoRepository.Where(p => !p.DeleteFlag);
            var queryCustomer = _customerAppUsageRepository.Where(p => !p.DeleteFlag);
            var queryBranch = _appBranchRepository.Where(p => !p.DeleteFlag);

            var query = from app in queryApplication
                        join dyn in queryAppDynamicInfo on app.Id.ToString() equals dyn.AppId
                        join inf in queryInfomation on dyn.InfoId equals inf.Id.ToString()
                        where (!queryFilter.AppID.HasValue || app.Id == queryFilter.AppID)
                        select new AppDynamicInfoViewModel
                        {
                            ID = inf.Id,
                            AppName = app.Name,
                            Status = inf.Status,
                            Version = dyn.Version,
                            AppBranchID = dyn.AppBranchId,
                            AppID = app.Id,
                            Content = inf.Content,
                            InfoID = inf.Id,
                            InfoType = dyn.InfoType,
                            PublishDate = dyn.CreatedTime.LocalDateTime,
                            Title = inf.Title

                        };

            if (!string.IsNullOrEmpty(queryFilter.SortField))
            {
                query = query.ApplyOrder(queryFilter.SortField, queryFilter.IsAsc);
            }
            var table = await query.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return table;
        }

        /// <summary>
        /// 查询应用客户数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<PagedList<AppCustomerTableViewModel>> QueryAppCustomerTableData(AppCustomerTableQuery queryFilter)
        {
            var queryApplication = _microAppRepository.Where(p => !p.DeleteFlag);
            var queryCustomerApp = _customerAppUsageRepository.Where(p => !p.DeleteFlag);
            var queryCustomer = _customerRepository.Where(p => !p.DeleteFlag);

            var query = from app in queryApplication
                        join cua in queryCustomerApp on app.Id.ToString() equals cua.AppId
                        join cus in queryCustomer on cua.CustomerId equals cus.Id.ToString()
                        where (!queryFilter.AppID.HasValue || app.Id == queryFilter.AppID)
                        && (!queryFilter.CustomerType.HasValue || cua.AuthType == queryFilter.CustomerType)
                        && (!queryFilter.DueSoon || (cua.ExpireDate - DateTime.Now).TotalDays <= 30)
                        && (string.IsNullOrEmpty(queryFilter.Keyword) || cus.Name.Contains(queryFilter.Keyword))
                        select new AppCustomerTableViewModel
                        {
                            AppID = app.Id,
                            BeginDate = cua.BeginDate,
                            ExpireDate = cua.ExpireDate,
                            CustomerID = cus.Id,
                            CustomerName = cus.Name,
                            CustomerType = cua.AuthType
                        };

            if (!string.IsNullOrEmpty(queryFilter.SortField))
            {
                query = query.ApplyOrder(queryFilter.SortField, queryFilter.IsAsc);
            }
            var table = await query.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return table;
        }

        /// <summary>
        /// 添加应用信息
        /// </summary>
        /// <param name="appDto">应用创建数据</param>
        /// <returns></returns>
        public async Task<Guid> Create(ApplicationDto appDto)
        {
            var app = new MicroApplication
            {
                Id = Guid.NewGuid(),
                Terminal = appDto.Terminal.Join(','),
                Status = appDto.Status,
                AdvisePrice = appDto.AdvisePrice.Value,
                Desc = appDto.Desc,
                DevId = appDto.DevID.ToString(),
                FreeTry = appDto.FreeTry,
                Icon = appDto.Icon,
                Intro = appDto.Intro,
                Name = appDto.Name,
                PriceType = appDto.PriceType,
                CreatedTime = DateTimeOffset.UtcNow,
                UseScene = appDto.Scene.Join(',')
            };
            var result = await _microAppRepository.InsertAsync(app);

            appDto.ServiceTypes.ForEach(p =>
            {
                var appServiceType = new AppServiceType
                {
                    Id = Guid.NewGuid(),
                    AppId = app.Id.ToString(),
                    CreatedTime = DateTimeOffset.UtcNow,
                    DictValue = p

                };
                _appServiceTypeRepository.InsertAsync(appServiceType);
            });

            appDto.ServicePacks.ForEach(p =>
            {
                var appServicePack = new AppServicePack
                {
                    Id = Guid.NewGuid(),
                    AppId = app.Id.ToString(),
                    CreatedTime = DateTimeOffset.UtcNow,
                    DictValue = p

                };
                _appServicePackRepository.InsertAsync(appServicePack);
            });


            return result.Entity.Id;
        }

        /// <summary>
        /// 更新应用信息
        /// </summary>
        /// <param name="appDto">应用修改数据</param>
        /// <returns></returns>
        public async Task<Guid> Update(ApplicationDto appDto)
        {
            var app = await _microAppRepository.FirstOrDefaultAsync(p => p.Id == appDto.ID && !p.DeleteFlag);
            app.Terminal = appDto.Terminal.Join(',');
            app.Status = appDto.Status;
            app.AdvisePrice = appDto.AdvisePrice.Value;
            app.Desc = appDto.Desc;
            app.DevId = appDto.DevID.ToString();
            app.FreeTry = appDto.FreeTry;
            app.Icon = appDto.Icon;
            app.Intro = appDto.Intro;
            app.Name = appDto.Name;
            app.PriceType = appDto.PriceType;
            app.UpdatedTime = DateTimeOffset.UtcNow;
            app.UseScene = appDto.Scene.Join(',');

            var result = await _microAppRepository.UpdateAsync(app);

            var oldAppServiceTypes = _appServiceTypeRepository.Where(p => p.AppId == appDto.ID.ToString()).OrderBy(p => p.DictValue).ToList();
            if (oldAppServiceTypes.Join(',') != appDto.ServiceTypes.OrderBy(p => p).ToList().Join(','))
            {
                oldAppServiceTypes.ForEach(p =>
                {
                    _appServiceTypeRepository.DeleteAsync(p);
                });

                appDto.ServiceTypes.ForEach(p =>
                {
                    var appServiceType = new AppServiceType
                    {
                        Id = Guid.NewGuid(),
                        AppId = app.Id.ToString(),
                        CreatedTime = DateTimeOffset.UtcNow,
                        DictValue = p

                    };
                    _appServiceTypeRepository.InsertAsync(appServiceType);
                });
            }

            var oldAppServicePacks = _appServicePackRepository.Where(p => p.AppId == appDto.ID.ToString()).OrderBy(p => p.DictValue).ToList();
            if (oldAppServicePacks.Join(',') != appDto.ServicePacks.OrderBy(p => p).ToList().Join(','))
            {
                oldAppServicePacks.ForEach(p =>
                {
                    _appServicePackRepository.DeleteAsync(p);
                });
                appDto.ServicePacks.ForEach(p =>
            {
                var appServicePack = new AppServicePack
                {
                    Id = Guid.NewGuid(),
                    AppId = app.Id.ToString(),
                    CreatedTime = DateTimeOffset.UtcNow,
                    DictValue = p

                };
                _appServicePackRepository.InsertAsync(appServicePack);
            });


            }
            return result.Entity.Id;
        }

        /// <summary>
        /// 删除应用
        /// </summary>
        /// <param name="appID">应用ID</param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid appID)
        {
            var app = await _microAppRepository.FirstOrDefaultAsync(p => p.Id == appID && !p.DeleteFlag);

            app.UpdatedTime = DateTimeOffset.UtcNow;
            app.DeleteFlag = true;

            var result = await _microAppRepository.UpdateAsync(app);
            return result.State == Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        /// <summary>
        /// 批量上架应用
        /// </summary>
        /// <param name="appIds">上架应用Id集合</param>
        /// <returns></returns>
        public bool EnableApplication(List<Guid> appIds)
        {
            var result = true;
            var apps = _microAppRepository.Where(p => appIds.Contains(p.Id) && !p.DeleteFlag).ToList();
            apps.ForEach(p =>
            {
                p.UpdatedTime = DateTimeOffset.UtcNow;
                p.Status = 1;

                var updateResult = _microAppRepository.UpdateAsync(p);
                result = result && updateResult.Result.State == Microsoft.EntityFrameworkCore.EntityState.Modified;
            });
            return result;

        }

        /// <summary>
        /// 批量下架应用
        /// </summary>
        /// <param name="appIds">下架应用Id集合</param>
        /// <returns></returns>
        public bool DisableApplication(List<Guid> appIds)
        {

            var result = true;
            var apps = _microAppRepository.Where(p => appIds.Contains(p.Id) && !p.DeleteFlag).ToList();
            apps.ForEach(p =>
            {
                p.UpdatedTime = DateTimeOffset.UtcNow;
                p.Status = 2;

                var updateResult = _microAppRepository.UpdateAsync(p);
                result = result && updateResult.Result.State == Microsoft.EntityFrameworkCore.EntityState.Modified;
            });
            return result;
        }

        /// <summary>
        /// 查询应用数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<PagedList<AppSearchViewModel>> AppSearch(AppSearchQuery queryFilter)
        {
            var queryApplication = _microAppRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryCustomerApp = _customerAppUsageRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryAppBranch = _appBranchRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryDev = _developerRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryServiceType = _appServiceTypeRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryAppEntrance = _appBranchEntryPointRepository.Where(p => !p.DeleteFlag && p.IsSystem && p.IsDefault).AsNoTracking();

            var query = from app in queryApplication
                        join cua in queryCustomerApp on app.Id.ToString() equals cua.AppId
                        join appbr in queryAppBranch on cua.AppBranchId.ToString() equals appbr.AppId
                        join dev in queryDev on app.DevId equals dev.Id.ToString()
                        join type in queryServiceType on app.Id.ToString() equals type.AppId
                        where (app.Status == 1)
                        && (string.IsNullOrEmpty(queryFilter.CustomerId) || cua.CustomerId == queryFilter.CustomerId)
                        group new
                        {
                            app,
                            appbr,
                            cua
                        }
                        by new { app, appbr, DevName = dev.Name }
                        into g
                        select new AppSearchViewModel
                        {
                            ID = g.Key.appbr.Id,
                            CreateTime = g.Key.appbr.CreatedTime.LocalDateTime,
                            UpdateTime = g.Key.appbr.UpdatedTime.HasValue ? g.Key.appbr.UpdatedTime.Value.LocalDateTime : null,
                            Name = g.Key.appbr.Name,
                            AppID = g.Key.app.Id.ToString(),
                            Icon = g.Key.appbr.Icon,
                            StartTime = g.Min(p => p.cua.BeginDate),
                            ExpireTime = g.Max(p => p.cua.ExpireDate),
                            FrontUrl = queryAppEntrance.FirstOrDefault(p => p.UseScene == 1 && p.AppId == g.Key.app.Id.ToString()).VisitUrl,
                            BackendUrl = queryAppEntrance.FirstOrDefault(p => p.UseScene == 2 && p.AppId == g.Key.app.Id.ToString()).VisitUrl,
                            UseScene = g.Key.app.UseScene,
                            Status = g.Key.app.Status,
                            Intro = g.Key.app.Intro,
                            AdvisePrice = g.Key.app.AdvisePrice,
                            DeveloperName = g.Key.DevName,
                            ServiceType = g.Key.app.ServiceType,
                            Terminal = g.Key.app.Terminal
                        };

            if (!string.IsNullOrEmpty(queryFilter.SortField))
            {
                query = query.ApplyOrder(queryFilter.SortField, queryFilter.IsAsc);
            }
            var table = await query.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return table;
        }

        /// <summary>
        /// 查询应用数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<PagedList<AppListViewModel>> GetAppList(AppSearchQuery queryFilter)
        {
            var queryApplication = _microAppRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryCustomerApp = _customerAppUsageRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryCustomer = _customerRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryAppBranch = _appBranchRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryDev = _developerRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var queryServiceType = _appServiceTypeRepository.Where(p => !p.DeleteFlag).AsNoTracking();
            var queryAppEntrance = _appBranchEntryPointRepository.Include(p => p.AppEvents).Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var queryWidget = _appWidgetRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var queryAppvailibleSortField = _appvailibleSortFieldRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var queryDeployment = _deploymentRepository.Where(p => !p.DeleteFlag).AsQueryable().AsNoTracking();
            var table = new PagedList<AppListViewModel>();
            try
            {
                var query = from app in queryApplication
                            join appbr in queryAppBranch on app.Id.ToString() equals appbr.AppId
                            join cua in queryCustomerApp on appbr.Id.ToString() equals cua.AppBranchId
                            join cu in queryCustomer on cua.CustomerId equals cu.Id.ToString()
                            where (app.Status == 1) && (string.IsNullOrEmpty(queryFilter.CustomerId) || cu.Owner == queryFilter.CustomerId)
                            select new { app, appbr, cua };
                var list = await query.ToListAsync();

                var queryResult = list.GroupBy(p => new { p.app, p.appbr }).Select(g => new AppListViewModel
                {
                    ID = g.Key.appbr.Id,
                    CreateTime = g.Key.appbr.CreatedTime.LocalDateTime,
                    UpdateTime = g.Key.appbr.UpdatedTime.HasValue ? g.Key.appbr.UpdatedTime.Value.LocalDateTime : null,
                    Name = g.Key.appbr.Name,
                    AppID = g.Key.app.Id.ToString(),
                    Icon = g.Key.appbr.Icon,
                    StartTime = g.Min(p => p.cua.BeginDate),
                    ExpireTime = g.Max(p => p.cua.ExpireDate),
                    FrontUrl = $"{queryDeployment.FirstOrDefault(p => p.Id.ToString() == g.Key.appbr.DeployeeId).WebGateway}/{g.Key.app.RouteCode}{queryAppEntrance.FirstOrDefault(p => p.UseScene == 1 && p.IsSystem && p.IsDefault && p.AppId == g.Key.app.Id.ToString())?.VisitUrl}",
                    BackendUrl = $"{queryDeployment.FirstOrDefault(p => p.Id.ToString() == g.Key.appbr.DeployeeId).MgrGateway}/{g.Key.app.RouteCode}{queryAppEntrance.FirstOrDefault(p => p.UseScene == 2 && p.IsSystem && p.IsDefault && p.AppId == g.Key.app.Id.ToString())?.VisitUrl}",
                    ApiHost = queryDeployment.FirstOrDefault(p => p.Id.ToString() == g.Key.appbr.DeployeeId).ApiGateway,
                    UseScene = g.Key.app.UseScene,
                    Status = g.Key.app.Status,
                    Intro = g.Key.app.Intro,
                    AdvisePrice = g.Key.app.AdvisePrice,
                    DeveloperName = queryDev.FirstOrDefault(p => p.Id.ToString() == g.Key.app.DevId)?.Name,
                    ServiceType = g.Key.app.ServiceType,
                    Terminal = g.Key.app.Terminal,
                    RouteCode = g.Key.app.RouteCode,
                    PurcaseType = g.OrderByDescending(p => p.cua.ExpireDate).FirstOrDefault()?.cua.AuthType.ToString(),
                    PurcaseTypeName = EnumTools.GetName((AppPurchaseTypeEnum)g.OrderByDescending(p => p.cua.ExpireDate).FirstOrDefault()?.cua.AuthType),
                    AppAvailibleSortFields = queryAppvailibleSortField.Where(p => p.AppId == g.Key.app.Id.ToString())
                                     .Select(p => new AppAvailibleSortFieldViewModel
                                     {
                                         Id = p.Id,
                                         AppId = p.AppId,
                                         SortFieldName = p.SortFieldName,
                                         SortFieldValue = p.SortFieldValue
                                     }).ToList(),
                    AppEntrances = queryAppEntrance.Where(p => p.AppBranchId == g.Key.appbr.Id.ToString())
                                     .Select(p => new AppBranchEntryPointViewModel
                                     {
                                         AppBranchId = p.AppBranchId,
                                         AppEvents = p.AppEvents
                                                  .Select(p => new AppEventViewModel
                                                  {
                                                      EventCode = p.EventCode,
                                                      EventName = p.EventName,
                                                      EventType = p.EventType
                                                  }).ToList(),
                                         AppId = p.AppId,
                                         Code = p.Code,
                                         Id = p.Id,
                                         IsDefault = p.IsDefault,
                                         IsSystem = p.IsSystem,
                                         UseScene = p.UseScene,
                                         Name = p.Name,
                                         VisitUrl = p.VisitUrl,
                                         BusinessType = p.BusinessType
                                     }).ToList(),
                    AppWidgets = queryWidget.Where(p => p.AppId == g.Key.app.Id.ToString()).Select(p => new AppWidgetViewModel
                    {
                        AvailableConfig = p.AvailableConfig,
                        Id = p.Id,
                        AppId = p.AppId,
                        MaxTopCount = p.MaxTopCount,
                        Name = p.Name,
                        Target = p.Target,
                        Cover = p.Cover,
                        TopCountInterval = p.TopCountInterval,
                        Width = p.Width,
                        Height = p.Height,
                        SceneType = p.SceneType,
                        CreateTime = p.CreatedTime,
                        UpdateTime = p.UpdatedTime,
                    }).ToList()
                });

                if (!string.IsNullOrEmpty(queryFilter.SortField))
                {
                    query = query.ApplyOrder(queryFilter.SortField, queryFilter.IsAsc);
                }
                table.Items = queryResult.ToList();
                table.TotalCount = table.Items.Count();
            }
            catch (Exception)
            {
            }
            return table;
        }

        /// <summary>
        /// 获取应用信息系详情
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public async Task<AppSearchViewModel> GetAppDetail(string appId)
        {
            var appBranch = await _appBranchRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == appId);
            var app = await _microAppRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == appBranch.AppId);
            var dev = await _developerRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == app.DevId);
            var queryServiceType = _appServiceTypeRepository.Where(p => !p.DeleteFlag && p.AppId == appBranch.AppId.ToString());
            var queryServicePack = _appServicePackRepository.Where(p => !p.DeleteFlag && p.AppId == appBranch.AppId.ToString());
            var queryCustomer = _customerAppUsageRepository.Where(p => !p.DeleteFlag && p.AppBranchId == appId).OrderByDescending(p => p.ExpireDate);
            var queryAppEntrance = _appBranchEntryPointRepository.Where(p => !p.DeleteFlag && p.IsSystem && p.IsDefault && p.AppBranchId == appBranch.Id.ToString());

            var result = new AppSearchViewModel
            {
                AppID = appBranch.AppId,
                BackendUrl = queryAppEntrance.FirstOrDefault(p => p.UseScene == 2).VisitUrl,
                CreateTime = appBranch.CreatedTime.LocalDateTime,
                DeployeeID = appBranch.DeployeeId,
                ExpireTime = queryCustomer.Max(p => p.ExpireDate),
                FrontUrl = queryAppEntrance.FirstOrDefault(p => p.UseScene == 1).VisitUrl,
                Icon = appBranch.Icon,
                ID = appBranch.Id,
                Name = appBranch.Name,
                Remark = appBranch.Remark,
                StartTime = queryCustomer.Max(p => p.BeginDate),
                UpdateTime = appBranch.UpdatedTime.HasValue ? appBranch.UpdatedTime.Value.LocalDateTime : null,
                Version = appBranch.Version,
                DeveloperName = dev.Name,
                PurcaseTypeName = EnumTools.GetName((AppPurchaseTypeEnum)queryCustomer.FirstOrDefault()?.AuthType),
                SceneTypeName = app.UseScene,
                ServiceTypeName = queryServiceType.Select(p => p.DictValue).ToList().Join(','),
                TerminalName = EnumTools.EnumToList<AppTerminalTypeEnum>().Where(p => app.Terminal.Split(',').Contains(p.Key)).Select(p => p.Value).ToList().Join(','),
                Status = app.Status,
                StatusName = EnumTools.GetName((AppStatusEnum)app.Status),
                Intro = app.Intro,
                AdvisePrice = app.AdvisePrice
            };

            return result;
        }

        /// <summary>
        /// 按类型获取字典
        /// </summary>
        /// <returns></returns>
        public Task<List<SysDictModel>> GetDictionaryByType(string type)
        {
            var result = _appDictioanryRepository.Where(p => p.DictType == type && !p.DeleteFlag).Select(p => new SysDictModel { Key = p.Name, Value = p.Value }).ToList();

            return Task.FromResult(result);
        }

        /// <summary>
        /// App发布信息列表
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<PagedList<AppDynamicInfoViewModel>> GetAppLog(AppLogQuery queryFilter)
        {
            var queryInfomation = _informationRepository.DetachedEntities.Where(p => !p.DeleteFlag);
            var queryactivityInfo = _activityInfoRepository.DetachedEntities.Where(p => !p.DeleteFlag);
            var queryAppDynamic = _appDynamicInfoRepository.DetachedEntities.Where(c => !c.DeleteFlag && c.InfoType == queryFilter.InfoType);

            var query = from info in queryInfomation
                        join appDyna in queryAppDynamic on info.Id.ToString() equals appDyna.InfoId
                        select new AppDynamicInfoViewModel
                        {
                            ID = appDyna.Id,
                            Content = info.Content,
                            InfoID = info.Id,
                            PublishDate = info.PublishDate,
                            Title = info.Title,
                            AppID = new Guid(appDyna.AppId),
                            Version = appDyna.Version
                        };

            if (!string.IsNullOrEmpty(queryFilter.SortField))
            {
                query = query.ApplyOrder(queryFilter.SortField, queryFilter.IsAsc);
            }
            var table = await query.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return table;
        }

        /// <summary>
        /// App发布信息详情
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<AppDynamicInfoViewModel> GetAppLogDetail(string id)
        {
            var info = await _informationRepository.DetachedEntities.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == id);
            var dyn = await _appDynamicInfoRepository.DetachedEntities.FirstOrDefaultAsync(p => !p.DeleteFlag && p.InfoId == id);
            var app = await _microAppRepository.DetachedEntities.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == dyn.AppId);

            var result = new AppDynamicInfoViewModel
            {
                Content = info.Content,
                InfoID = info.Id,
                PublishDate = info.PublishDate,
                Title = info.Title,
                AppName = app.Name,
                AppIcon = app.Icon,
                Version = dyn.Version
            };

            return result;
        }

        /// <summary>
        /// 获取付费应用推荐
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<PayAppListItemDto>> GetPayAppList(PayAppTableQuery queryFilter)
        {
            var existAppQuery = from customerAppUsage in _customerAppUsageRepository.DetachedEntities.Where(c => !c.DeleteFlag && c.Status != 2)
                                join customer in _customerRepository.DetachedEntities.Where(c => !c.DeleteFlag && c.Owner == queryFilter.Owner)
                                on customerAppUsage.CustomerId equals customer.Id.ToString()
                                select customerAppUsage.AppId;

            var microAppQuery = from app in _microAppRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.AdvisePrice > 0 && x.RecommendScore > 0)
                                            .Where(x => string.IsNullOrWhiteSpace(queryFilter.ServiceType) || x.ServiceType == queryFilter.ServiceType)
                                join dev in _developerRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                    on app.DevId equals dev.Id.ToString()
                                where !existAppQuery.Any(c => c == app.Id.ToString())
                                select new PayAppListItemDto
                                {
                                    ID = app.Id,
                                    AppName = app.Name,
                                    AppIcon = app.Icon,
                                    Star = app.RecommendScore,
                                    Content = app.Desc,
                                    Price = app.AdvisePrice,
                                    PriceType = app.PriceType,
                                    IsFreeTry = app.FreeTry,
                                    Developer = dev.Name
                                };
            var pagedList = await microAppQuery.OrderByDescending(x => x.Star)
                                               .ThenByDescending(x => x.ID)
                                               .ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pagedList;
        }


        #region 添加测试数据



        /// <summary>
        /// 添加客户应用
        /// </summary>
        public void AddCustomerAndAppUse()
        {
            try
            {
                var cusId = Guid.NewGuid();
                _appDictioanryRepository.Context.BulkInsert(
                    new[] { new Customer { Id = cusId, Name = "重庆大学", PortalUrl = "http://192.168.21.46", CreatedTime = DateTimeOffset.UtcNow, FileUrl = "http://192.168.21.71:5075", LoginUrl = "", ManageUrl = "http://192.168.21.46:8001", MgrLoginUrl = "", Owner = "cqu", PlatformVersion = "3.0.0", Key = "cqu", SyncKey = "cqu", Secret = "cqu123" } }
                    );

                _appDictioanryRepository.Context.BulkInsert(
        new[] { new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="a7f7618d-2162-43b1-9b0a-ab02c60334d4",AppId="7de4027e-2a02-41e9-9e69-067bbbc2df53",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="f6b78feb-2b7c-47f1-b07f-adf6edd6c352",AppId="27ad8830-1085-4f42-adb7-237c16fb383c",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="beb8dfae-ac8d-4c41-acb9-5c80a8a52433",AppId="f7958d7f-8cca-4ac6-84f8-e85b9f5f6a74",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="a2aebd4c-13da-40ce-b9fd-3b9ce31459f6",AppId="42757eb3-7a18-4619-adb2-cda39aee5602",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="09cbfe6c-8611-40fe-9133-2b20d3c163e3",AppId="0427db88-1c6e-4082-aa00-8edaabb2e4b6",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="50a69dc3-6d1a-4c41-b5bc-2a94ae69ae75",AppId="dba5df91-dc9c-4dd2-9527-3741a425b7b3",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="4a2a6d01-0507-469e-a179-096d3b6c1f70",AppId="f22846b8-53d4-4f5c-a62c-92d28efe447f",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="3dbceb5a-01c5-48fd-9e69-41d13592bd85",AppId="d759fd09-b08e-4e37-99c3-4d4d67baaa6a",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="e2ff0036-ac1d-4760-93a4-fa889d6c9a3f",AppId="b0fb53b3-f7b3-41a5-94a5-c4feb71213e2",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="8419c252-373f-492b-ab9d-930489fd4f48",AppId="a7fedf20-e7be-483d-9471-11d5a9a49203",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="64db9a6b-8816-4f97-b9d8-dfb09aa9c085",AppId="bb67dad7-662f-4858-a0d3-647806365b37",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="e25b0ae5-5327-4530-8081-6d37ce737609",AppId="1308dfee-bc5e-4fa8-b4da-0d5f36fba8d0",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="c3431e6b-7f46-4810-b880-8a5e52fb877a",AppId="9fed516c-9424-4b0d-b4b2-1d72adde9716",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="994c2b4d-c250-4ccf-9d4a-b43bd9a078f7",AppId="5eeb1b4d-9d62-4254-8de0-052e5b1ac48a",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="bc8e4464-e734-4d5f-ba30-84969e84a080",AppId="09bf2453-22af-4676-8cab-7d2ed50e0882",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="36dbe4c7-4cb6-4e04-b1c0-957799387e68",AppId="ea154354-4292-4de4-9718-67ad795401a5",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId="305ed31d-1e9d-4ff0-a261-e8721a9b78da",AppId="786ddebc-e762-416e-a817-0ff56aa97bc1",AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(1),Status=1,CustomerId=cusId.ToString() }
                   }
                   );
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 添加测试数据
        /// </summary>
        /// <returns></returns>
        public void AddTestData()
        {
            _appDictioanryRepository.Context.BulkInsert(
                new[] { new AppDictioanry { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,DictType="AppServiceType",Desc="基础应用",Name="基础应用",Value="1"}
                ,new AppDictioanry { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,DictType="AppServiceType",Desc="资源服务",Name="资源服务",Value="2"}
                ,new AppDictioanry { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,DictType="AppServiceType",Desc="学术与情报",Name="学术与情报",Value="3"}
                ,new AppDictioanry { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,DictType="AppServiceType",Desc="阅读推广",Name="阅读推广",Value="4"}
                ,new AppDictioanry { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,DictType="AppServiceType",Desc="空间服务",Name="空间服务",Value="5"}
                ,new AppDictioanry { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,DictType="AppServiceType",Desc="管理与馆务",Name="管理与馆务",Value="6"}
                ,new AppDictioanry { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,DictType="AppServiceType",Desc="分析决策",Name="分析决策",Value="7"}
                ,new AppDictioanry { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,DictType="AppServicePack",Desc="旗舰版，包含所有应用",Name="旗舰版",Value="1"}
                ,new AppDictioanry { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,DictType="AppServicePack",Desc="智享版，包含30个基础应用和100个扩展应用",Name="智享版",Value="2"}
                ,new AppDictioanry { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,DictType="AppServicePack",Desc="基础版，包含30个基础应用",Name="基础版",Value="3"}
                }
                );

            var devId = Guid.NewGuid();//"3e0e0cda-0b21-4d9d-b95f-b2588cc37fe9";//
            _developerRepository.Context.BulkInsert(
                new[] { new Developer { Id=devId,CreatedTime=DateTimeOffset.UtcNow,Desc="维普智图",Name="维普智图",Account="VipSmart",Address="重庆",Email="vipsmar@cqvip.com",Mobile="13012345678",Password=Guid.NewGuid().ToString()}
                }
                );
            var appId1 = Guid.NewGuid();
            var appId2 = Guid.NewGuid();
            var appId3 = Guid.NewGuid();
            var appId4 = Guid.NewGuid();
            var appId5 = Guid.NewGuid();
            var appId6 = Guid.NewGuid();
            var appId7 = Guid.NewGuid();
            var appId8 = Guid.NewGuid();
            var appId9 = Guid.NewGuid();
            var appId10 = Guid.NewGuid();
            var appId11 = Guid.NewGuid();
            var appId12 = Guid.NewGuid();
            var appId13 = Guid.NewGuid();
            var appId14 = Guid.NewGuid();
            var appId15 = Guid.NewGuid();
            var appId16 = Guid.NewGuid();
            var appId17 = Guid.NewGuid();
            var appId18 = Guid.NewGuid();

            _microAppRepository.Context.BulkInsert(
                new[] { new MicroApplication { Id=appId17,CreatedTime=DateTimeOffset.UtcNow,Desc="服务中台",Name="服务中台",AdvisePrice=10000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(1).png",Intro="服务中台",PriceType=2,RecommendScore=10,RouteCode="scenemanage",ServiceType="1",Status=1,Terminal="1",UseScene="2"}
                ,new MicroApplication { Id=appId1,CreatedTime=DateTimeOffset.UtcNow,Desc="通用检索",Name="通用检索",AdvisePrice=10000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(2).png",Intro="通用检索",PriceType=2,RecommendScore=10,RouteCode="articlesearch",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId2,CreatedTime=DateTimeOffset.UtcNow,Desc="用户中心",Name="用户中心",AdvisePrice=10000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(3).png",Intro="用户中心",PriceType=2,RecommendScore=10,RouteCode="usermanage",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId3,CreatedTime=DateTimeOffset.UtcNow,Desc="应用中心",Name="应用中心",AdvisePrice=10000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(4).png",Intro="应用中心",PriceType=2,RecommendScore=10,RouteCode="appcenter",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId4,CreatedTime=DateTimeOffset.UtcNow,Desc="馆员工作台",Name="馆员工作台",AdvisePrice=10000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(5).png",Intro="馆员工作台",PriceType=2,RecommendScore=10,RouteCode="workbench",ServiceType="1",Status=1,Terminal="1",UseScene="2"}
                ,new MicroApplication { Id=appId5,CreatedTime=DateTimeOffset.UtcNow,Desc="文献专题引擎",Name="文献专题引擎",AdvisePrice=5000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(6).png",Intro="文献专题引擎",PriceType=2,RecommendScore=10,RouteCode="assembly",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId6,CreatedTime=DateTimeOffset.UtcNow,Desc="文献推荐引擎",Name="文献推荐引擎",AdvisePrice=5000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(7).png",Intro="文献推荐引擎",PriceType=2,RecommendScore=10,RouteCode="articlerecommend",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId7,CreatedTime=DateTimeOffset.UtcNow,Desc="数据库导航",Name="数据库导航",AdvisePrice=5000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(8).png",Intro="数据库导航",PriceType=2,RecommendScore=10,RouteCode="databaseguide",ServiceType="2",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId8,CreatedTime=DateTimeOffset.UtcNow,Desc="新闻发布",Name="新闻发布",AdvisePrice=5000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(9).png",Intro="新闻发布",PriceType=2,RecommendScore=10,RouteCode="news",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId9,CreatedTime=DateTimeOffset.UtcNow,Desc="信息导航",Name="信息导航",AdvisePrice=5000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(10).png",Intro="信息导航",PriceType=2,RecommendScore=10,RouteCode="navigation",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId10,CreatedTime=DateTimeOffset.UtcNow,Desc="Opac中心",Name="Opac中心",AdvisePrice=5000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(11).png",Intro="Opac中心",PriceType=2,RecommendScore=10,RouteCode="opac",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId11,CreatedTime=DateTimeOffset.UtcNow,Desc="数据中心",Name="数据中心",AdvisePrice=10000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(12).png",Intro="数据中心",PriceType=2,RecommendScore=10,RouteCode="datacenter",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId12,CreatedTime=DateTimeOffset.UtcNow,Desc="行为分析",Name="行为分析",AdvisePrice=10000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(12).png",Intro="行为分析",PriceType=2,RecommendScore=10,RouteCode="loganalysis",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId13,CreatedTime=DateTimeOffset.UtcNow,Desc="学院分馆",Name="学院分馆",AdvisePrice=5000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(12).png",Intro="学院分馆",PriceType=2,RecommendScore=10,RouteCode="departmentlib",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId14,CreatedTime=DateTimeOffset.UtcNow,Desc="猜你喜欢",Name="猜你喜欢",AdvisePrice=2000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(12).png",Intro="猜你喜欢",PriceType=2,RecommendScore=10,RouteCode="gusseuserlike",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId15,CreatedTime=DateTimeOffset.UtcNow,Desc="通知中心",Name="通知中心",AdvisePrice=2000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(12).png",Intro="通知中心",PriceType=2,RecommendScore=10,RouteCode="noticecenter",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId16,CreatedTime=DateTimeOffset.UtcNow,Desc="活动中心",Name="活动中心",AdvisePrice=2000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(12).png",Intro="活动中心",PriceType=2,RecommendScore=10,RouteCode="activity",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId18,CreatedTime=DateTimeOffset.UtcNow,Desc="积分中心",Name="积分中心",AdvisePrice=2000,DevId=devId.ToString(),FreeTry=false,Icon="/app_icons/icon(12).png",Intro="积分中心",PriceType=2,RecommendScore=10,RouteCode="readerscore",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                }
                );
            #region 基础数据，应用分支，部署环境
            _appServiceTypeRepository.Context.BulkInsert(
                new[] { new AppServiceType { Id=Guid.NewGuid(), AppId=appId17.ToString(),CreatedTime=DateTimeOffset.UtcNow, DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId7.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="2"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId11.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId12.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId13.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId14.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId15.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                ,new AppServiceType {Id=Guid.NewGuid(), AppId=appId16.ToString(),CreatedTime=DateTimeOffset.UtcNow,DictValue="1"}
                }
                );
            var deploymentId1 = Guid.NewGuid();
            var deploymentId2 = Guid.NewGuid();
            var deploymentId3 = Guid.NewGuid();
            _developerRepository.Context.BulkInsert(
                new[] { new Deployment { Id=deploymentId1,CreatedTime=DateTimeOffset.UtcNow,ApiGateway="http://192.168.21.71:8300",Code="",CustomerId="cqu",Desc="重庆大学本地网关",GrpcGateway="http://192.168.21.71:9999",MgrGateway="http://192.168.21.71:9000",WebGateway="http://192.168.21.71:9000",Name="重庆大学本地环境"}
                ,new Deployment { Id=deploymentId2,CreatedTime=DateTimeOffset.UtcNow,ApiGateway="http://192.168.21.71:8300",Code="",CustomerId="smartdemo",Desc="维普智图本地网关",GrpcGateway="http://192.168.21.71:9999",MgrGateway="http://192.168.21.71:9000",WebGateway="http://192.168.21.71:9000",Name="维普智图本地网关"}
                ,new Deployment { Id=deploymentId3,CreatedTime=DateTimeOffset.UtcNow,ApiGateway="http://192.168.21.71:8300",Code="",CustomerId="cloud",Desc="维普智图云端网关",GrpcGateway="http://192.168.21.71:9999",MgrGateway="http://192.168.21.71:9000",WebGateway="http://192.168.21.71:9000",Name="维普智图云端网关"}
                }
                );

            var appBrId1 = Guid.NewGuid();
            var appBrId2 = Guid.NewGuid();
            var appBrId3 = Guid.NewGuid();
            var appBrId4 = Guid.NewGuid();
            var appBrId5 = Guid.NewGuid();
            var appBrId6 = Guid.NewGuid();
            var appBrId7 = Guid.NewGuid();
            var appBrId8 = Guid.NewGuid();
            var appBrId9 = Guid.NewGuid();
            var appBrId10 = Guid.NewGuid();
            var appBrId11 = Guid.NewGuid();
            var appBrId12 = Guid.NewGuid();
            var appBrId13 = Guid.NewGuid();
            var appBrId14 = Guid.NewGuid();
            var appBrId15 = Guid.NewGuid();
            var appBrId16 = Guid.NewGuid();
            var appBrId17 = Guid.NewGuid();

            var appBrId21 = Guid.NewGuid();
            var appBrId22 = Guid.NewGuid();
            var appBrId23 = Guid.NewGuid();
            var appBrId24 = Guid.NewGuid();
            var appBrId25 = Guid.NewGuid();
            var appBrId26 = Guid.NewGuid();
            var appBrId27 = Guid.NewGuid();
            var appBrId28 = Guid.NewGuid();
            var appBrId29 = Guid.NewGuid();
            var appBrId30 = Guid.NewGuid();
            var appBrId31 = Guid.NewGuid();
            var appBrId32 = Guid.NewGuid();
            var appBrId33 = Guid.NewGuid();
            var appBrId34 = Guid.NewGuid();
            var appBrId35 = Guid.NewGuid();
            var appBrId36 = Guid.NewGuid();
            var appBrId37 = Guid.NewGuid();


            var appBrId41 = Guid.NewGuid();
            var appBrId42 = Guid.NewGuid();
            var appBrId43 = Guid.NewGuid();
            var appBrId44 = Guid.NewGuid();
            var appBrId45 = Guid.NewGuid();
            var appBrId46 = Guid.NewGuid();
            var appBrId47 = Guid.NewGuid();
            var appBrId48 = Guid.NewGuid();
            var appBrId49 = Guid.NewGuid();
            var appBrId50 = Guid.NewGuid();
            var appBrId51 = Guid.NewGuid();
            var appBrId52 = Guid.NewGuid();
            var appBrId53 = Guid.NewGuid();
            var appBrId54 = Guid.NewGuid();
            var appBrId55 = Guid.NewGuid();
            var appBrId56 = Guid.NewGuid();
            var appBrId57 = Guid.NewGuid();


            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranch { Id=appBrId17, AppId=appId17.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(1).png",IsMaster=false, Name="服务中台",Remark="服务中台",Version="3.0.0"}
                ,new AppBranch {Id=appBrId1, AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(2).png",IsMaster=false, Name="通用检索", Remark="通用检索",Version="3.0.0"}
                ,new AppBranch {Id=appBrId2, AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(3).png",IsMaster=false, Name="用户中心", Remark="用户中心",Version="3.0.0"}
                ,new AppBranch {Id=appBrId3, AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(4).png",IsMaster=false, Name="应用中心", Remark="应用中心",Version="3.0.0"}
                ,new AppBranch {Id=appBrId4, AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(5).png",IsMaster=false, Name="馆员工作台", Remark="馆员工作台",Version="3.0.0"}
                ,new AppBranch {Id=appBrId5, AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(6).png",IsMaster=false, Name="文献专题引擎", Remark="文献专题引擎",Version="3.0.0"}
                ,new AppBranch {Id=appBrId6, AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(7).png",IsMaster=false, Name="文献推荐引擎", Remark="文献推荐引擎",Version="3.0.0"}
                ,new AppBranch {Id=appBrId7, AppId=appId7.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(8).png",IsMaster=false, Name="数据库导航", Remark="数据库导航",Version="3.0.0"}
                ,new AppBranch {Id=appBrId8, AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(9).png",IsMaster=false, Name="新闻发布", Remark="新闻发布",Version="3.0.0"}
                ,new AppBranch {Id=appBrId9, AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(10).png",IsMaster=false, Name="信息导航", Remark="信息导航",Version="3.0.0"}
                ,new AppBranch {Id=appBrId10, AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(11).png",IsMaster=false, Name="Opac中心", Remark="Opac中心",Version="3.0.0"}
                ,new AppBranch {Id=appBrId11, AppId=appId11.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(12).png",IsMaster=false, Name="数据中心", Remark="数据中心",Version="3.0.0"}
                ,new AppBranch {Id=appBrId12, AppId=appId12.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(13).png",IsMaster=false, Name="行为分析", Remark="行为分析",Version="3.0.0"}
                ,new AppBranch {Id=appBrId13, AppId=appId13.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(14).png",IsMaster=false, Name="学院分馆", Remark="学院分馆",Version="3.0.0"}
                ,new AppBranch {Id=appBrId14, AppId=appId14.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(15).png",IsMaster=false, Name="猜你喜欢", Remark="猜你喜欢",Version="3.0.0"}
                ,new AppBranch {Id=appBrId15, AppId=appId15.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(16).png",IsMaster=false, Name="通知中心", Remark="通知中心",Version="3.0.0"}
                ,new AppBranch {Id=appBrId16, AppId=appId16.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId2.ToString(),Icon="/app_icons/icon(17).png",IsMaster=false, Name="活动中心", Remark="活动中心",Version="3.0.0"}


                , new AppBranch { Id=appBrId37, AppId=appId17.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId1.ToString(),Icon="/app_icons/icon(1).png",IsMaster=false, Name="服务中台",Remark="服务中台",Version="3.0.0" }
                , new AppBranch { Id = appBrId21, AppId = appId1.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(2).png", IsMaster = false, Name = "通用检索", Remark = "通用检索", Version = "3.0.0" }
                , new AppBranch { Id = appBrId22, AppId = appId2.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(3).png", IsMaster = false, Name = "用户中心", Remark = "用户中心", Version = "3.0.0" }
                , new AppBranch { Id = appBrId23, AppId = appId3.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(4).png", IsMaster = false, Name = "应用中心", Remark = "应用中心", Version = "3.0.0" }
                , new AppBranch { Id = appBrId24, AppId = appId4.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(5).png", IsMaster = false, Name = "馆员工作台", Remark = "馆员工作台", Version = "3.0.0" }
                , new AppBranch { Id = appBrId25, AppId = appId5.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(6).png", IsMaster = false, Name = "文献专题引擎", Remark = "文献专题引擎", Version = "3.0.0" }
                , new AppBranch { Id = appBrId26, AppId = appId6.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(7).png", IsMaster = false, Name = "文献推荐引擎", Remark = "文献推荐引擎", Version = "3.0.0" }
                , new AppBranch { Id = appBrId27, AppId = appId7.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(8).png", IsMaster = false, Name = "数据库导航", Remark = "数据库导航", Version = "3.0.0" }
                , new AppBranch { Id = appBrId28, AppId = appId8.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(9).png", IsMaster = false, Name = "新闻发布", Remark = "新闻发布", Version = "3.0.0" }
                , new AppBranch { Id = appBrId29, AppId = appId9.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(10).png", IsMaster = false, Name = "信息导航", Remark = "信息导航", Version = "3.0.0" }
                , new AppBranch { Id = appBrId30, AppId = appId10.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(11).png", IsMaster = false, Name = "Opac中心", Remark = "Opac中心", Version = "3.0.0" }
                , new AppBranch { Id = appBrId31, AppId = appId11.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(12).png", IsMaster = false, Name = "数据中心", Remark = "数据中心", Version = "3.0.0" }
                , new AppBranch { Id = appBrId32, AppId = appId12.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(13).png", IsMaster = false, Name = "行为分析", Remark = "行为分析", Version = "3.0.0" }
                , new AppBranch { Id = appBrId33, AppId = appId13.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(14).png", IsMaster = false, Name = "学院分馆", Remark = "学院分馆", Version = "3.0.0" }
                , new AppBranch { Id = appBrId34, AppId = appId14.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(15).png", IsMaster = false, Name = "猜你喜欢", Remark = "猜你喜欢", Version = "3.0.0" }
                , new AppBranch { Id = appBrId35, AppId = appId15.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(16).png", IsMaster = false, Name = "通知中心", Remark = "通知中心", Version = "3.0.0" }
                , new AppBranch { Id = appBrId36, AppId = appId16.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId1.ToString(), Icon = "/app_icons/icon(17).png", IsMaster = false, Name = "活动中心", Remark = "活动中心", Version = "3.0.0" }


                , new AppBranch { Id=appBrId57, AppId=appId17.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deploymentId3.ToString(),Icon="/app_icons/icon(1).png",IsMaster=true, Name="服务中台",Remark="服务中台",Version="3.0.0" }
                , new AppBranch { Id = appBrId41, AppId = appId1.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(2).png", IsMaster = true, Name = "通用检索", Remark = "通用检索", Version = "3.0.0" }
                , new AppBranch { Id = appBrId42, AppId = appId2.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(3).png", IsMaster = true, Name = "用户中心", Remark = "用户中心", Version = "3.0.0" }
                , new AppBranch { Id = appBrId43, AppId = appId3.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(4).png", IsMaster = true, Name = "应用中心", Remark = "应用中心", Version = "3.0.0" }
                , new AppBranch { Id = appBrId44, AppId = appId4.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(5).png", IsMaster = true, Name = "馆员工作台", Remark = "馆员工作台", Version = "3.0.0" }
                , new AppBranch { Id = appBrId45, AppId = appId5.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(6).png", IsMaster = true, Name = "文献专题引擎", Remark = "文献专题引擎", Version = "3.0.0" }
                , new AppBranch { Id = appBrId46, AppId = appId6.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(7).png", IsMaster = true, Name = "文献推荐引擎", Remark = "文献推荐引擎", Version = "3.0.0" }
                , new AppBranch { Id = appBrId47, AppId = appId7.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(8).png", IsMaster = true, Name = "数据库导航", Remark = "数据库导航", Version = "3.0.0" }
                , new AppBranch { Id = appBrId48, AppId = appId8.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(9).png", IsMaster = true, Name = "新闻发布", Remark = "新闻发布", Version = "3.0.0" }
                , new AppBranch { Id = appBrId49, AppId = appId9.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(10).png", IsMaster = true, Name = "信息导航", Remark = "信息导航", Version = "3.0.0" }
                , new AppBranch { Id = appBrId50, AppId = appId10.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(11).png", IsMaster = true, Name = "Opac中心", Remark = "Opac中心", Version = "3.0.0" }
                , new AppBranch { Id = appBrId51, AppId = appId11.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(12).png", IsMaster = true, Name = "数据中心", Remark = "数据中心", Version = "3.0.0" }
                , new AppBranch { Id = appBrId52, AppId = appId12.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(13).png", IsMaster = true, Name = "行为分析", Remark = "行为分析", Version = "3.0.0" }
                , new AppBranch { Id = appBrId53, AppId = appId13.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(14).png", IsMaster = true, Name = "学院分馆", Remark = "学院分馆", Version = "3.0.0" }
                , new AppBranch { Id = appBrId54, AppId = appId14.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(15).png", IsMaster = true, Name = "猜你喜欢", Remark = "猜你喜欢", Version = "3.0.0" }
                , new AppBranch { Id = appBrId55, AppId = appId15.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(16).png", IsMaster = true, Name = "通知中心", Remark = "通知中心", Version = "3.0.0" }
                , new AppBranch { Id = appBrId56, AppId = appId16.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = deploymentId3.ToString(), Icon = "/app_icons/icon(17).png", IsMaster = true, Name = "活动中心", Remark = "活动中心", Version = "3.0.0" }


                }
                );
            #endregion

            #region 场景管理入口，事件，组件
            var appEvents1 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId17.ToString(),EventName="创建场景",EventDesc="创建场景",EventCode= "SceneManage_CreateScene" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId17.ToString(),EventName="编辑场景",EventDesc="编辑场景",EventCode= "SceneManage_EditScene" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId17.ToString(),EventName="删除场景",EventDesc="删除场景",EventCode= "SceneManage_DeleteScene" }};

            var appEvents2 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId17.ToString(),EventName="创建终端实例",EventDesc="创建终端实例",EventCode= "TerminalManage_CreateTerminalInstance" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId17.ToString(),EventName="编辑终端实例",EventDesc="编辑终端实例",EventCode= "TerminalManage_EditTerminalInstance" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId17.ToString(),EventName="删除终端实例",EventDesc="删除终端实例",EventCode= "TerminalManage_DeleteTerminalInstance" }};

            _deploymentRepository.Context.BulkInsert(
                appEvents1
                );
            _deploymentRepository.Context.BulkInsert(
                appEvents2
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId17.ToString(),AppId=appId17.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="场景管理",UseScene=2,VisitUrl="/scenenmanage",AppEvents=appEvents1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId17.ToString(),AppId=appId17.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="终端管理",UseScene=2,VisitUrl="/terminalmanage",AppEvents=appEvents2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId37.ToString(),AppId=appId17.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="场景管理",UseScene=2,VisitUrl="/scenenmanage",AppEvents=appEvents1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId37.ToString(),AppId=appId17.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="终端管理",UseScene=2,VisitUrl="/terminalmanage",AppEvents=appEvents2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId57.ToString(),AppId=appId17.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="场景管理",UseScene=2,VisitUrl="/scenenmanage",AppEvents=appEvents1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId57.ToString(),AppId=appId17.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="终端管理",UseScene=2,VisitUrl="/terminalmanage",AppEvents=appEvents2 }}
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id= Guid.NewGuid(), AppId=appId17.ToString(),AvailableConfig="",CreatedTime=DateTimeOffset.UtcNow,Cover= "/app_icons/icon(1).png",Desc="头部一",Name="头部一",Target= "/header_sys/temp1"}
                ,new AppWidget { Id= Guid.NewGuid(), AppId=appId17.ToString(),AvailableConfig="",CreatedTime=DateTimeOffset.UtcNow,Cover= "/app_icons/icon(2).png",Desc="头部二",Name="头部二",Target= "/header_sys/temp2"}
                ,new AppWidget { Id= Guid.NewGuid(), AppId=appId17.ToString(),AvailableConfig="",CreatedTime=DateTimeOffset.UtcNow,Cover= "/app_icons/icon(3).png",Desc="头部三",Name="头部三",Target= "/header_sys/temp3"}
                ,new AppWidget { Id= Guid.NewGuid(), AppId=appId17.ToString(),AvailableConfig="",CreatedTime=DateTimeOffset.UtcNow,Cover= "/app_icons/icon(4).png",Desc="底部一",Name="底部一",Target= "/footer_sys/temp1"}
                ,new AppWidget { Id= Guid.NewGuid(), AppId=appId17.ToString(),AvailableConfig="",CreatedTime=DateTimeOffset.UtcNow,Cover= "/app_icons/icon(5).png",Desc="底部二",Name="底部二",Target= "/footer_sys/temp2"}}
                );
            #endregion

            #region 通用检索管理入口，组件
            //var appEvents3 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="创建场景",EventDesc="创建场景",EventCode= "SceneManage_CreateScene" }
            //    ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="编辑场景",EventDesc="编辑场景",EventCode= "SceneManage_EditScene" }
            //    ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="删除场景",EventDesc="删除场景",EventCode= "SceneManage_DeleteScene" }};

            //var appEvents4 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="创建终端实例",EventDesc="创建终端实例",EventCode= "TerminalManage_CreateTerminalInstance" }
            //    ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="编辑终端实例",EventDesc="编辑终端实例",EventCode= "TerminalManage_EditTerminalInstance" }
            //    ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="删除终端实例",EventDesc="删除终端实例",EventCode= "TerminalManage_DeleteTerminalInstance" }};

            //_deploymentRepository.Context.BulkInsert(
            //    appEvents3
            //    );
            //_deploymentRepository.Context.BulkInsert(
            //    appEvents4
            //    );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId1.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="通用检索",UseScene=1,VisitUrl="/articlesearch" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId1.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="站内检索",UseScene=1,VisitUrl="/sitesearch" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId1.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="检索设置",UseScene=2,VisitUrl="/searchmanage"}
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId21.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="通用检索",UseScene=1,VisitUrl="/articlesearch" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId21.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="站内检索",UseScene=1,VisitUrl="/sitesearch" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId21.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="检索设置",UseScene=2,VisitUrl="/searchmanage"}
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId41.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="通用检索",UseScene=1,VisitUrl="/articlesearch" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId41.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="站内检索",UseScene=1,VisitUrl="/sitesearch" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId41.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="检索设置",UseScene=2,VisitUrl="/searchmanage"}}
                );


            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id= Guid.NewGuid(), AppId=appId1.ToString(),AvailableConfig="",CreatedTime=DateTimeOffset.UtcNow,Cover= "/app_icons/icon(1).png",Desc="统一检索一",Name="统一检索一",Target= "/unified_retrieval_sys/temp1"}
                }
                );
            #endregion

            #region OPAC管理入口
            //var appEvents3 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="创建场景",EventDesc="创建场景",EventCode= "SceneManage_CreateScene" }
            //    ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="编辑场景",EventDesc="编辑场景",EventCode= "SceneManage_EditScene" }
            //    ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="删除场景",EventDesc="删除场景",EventCode= "SceneManage_DeleteScene" }};

            //var appEvents4 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="创建终端实例",EventDesc="创建终端实例",EventCode= "TerminalManage_CreateTerminalInstance" }
            //    ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="编辑终端实例",EventDesc="编辑终端实例",EventCode= "TerminalManage_EditTerminalInstance" }
            //    ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppID=appId17.ToString(),EventName="删除终端实例",EventDesc="删除终端实例",EventCode= "TerminalManage_DeleteTerminalInstance" }};

            //_deploymentRepository.Context.BulkInsert(
            //    appEvents3
            //    );
            //_deploymentRepository.Context.BulkInsert(
            //    appEvents4
            //    );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId10.ToString(),AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="图书预约",UseScene=1,VisitUrl="/book" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId10.ToString(),AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="Opac设置",UseScene=2,VisitUrl="/opacmanage"}
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId30.ToString(),AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="图书预约",UseScene=1,VisitUrl="/book" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId30.ToString(),AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="Opac设置",UseScene=2,VisitUrl="/opacmanage"}
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId50.ToString(),AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="图书预约",UseScene=1,VisitUrl="/book" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId50.ToString(),AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="Opac设置",UseScene=2,VisitUrl="/opacmanage"}}
                );
            #endregion

            #region 用户中心入口，事件，组件


            var appEventsUserCenter1 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="新增读者",EventDesc="新增读者",EventCode= "UserCenter_ReaderManage_CreateReader" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="编辑读者",EventDesc="编辑读者",EventCode= "UserCenter_ReaderManage_EditReader" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="删除读者",EventDesc="删除读者",EventCode= "UserCenter_ReaderManage_DeleteReader" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="批量编辑读者",EventDesc="批量编辑读者",EventCode= "UserCenter_ReaderManage_BatchEditReader" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="导入读者",EventDesc="导入读者",EventCode= "UserCenter_ReaderManage_ImportReader" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="导出读者",EventDesc="导出读者",EventCode= "UserCenter_ReaderManage_ExportReader" }};

            var appEventsUserCenter2 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="变动组织",EventDesc="变动组织",EventCode= "UserCenter_StaffManage_ChangeDepart" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="添加临时馆员",EventDesc="添加临时馆员",EventCode= "UserCenter_StaffManage_CreateTempStaff" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="删除馆员",EventDesc="删除馆员",EventCode= "UserCenter_StaffManage_DeleteStaff" }};

            var appEventsUserCenter3 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="新增读者卡",EventDesc="新增读者卡",EventCode= "UserCenter_ReaderCardManage_CreateCard" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="编辑读者卡",EventDesc="编辑读者卡",EventCode= "UserCenter_ReaderCardManage_EditCard" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="删除读者卡",EventDesc="删除读者卡",EventCode= "UserCenter_ReaderCardManage_DeleteCard" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="批量编辑读者卡",EventDesc="批量编辑读者卡",EventCode= "UserCenter_ReaderCardManage_BatchEditCard" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="导出读者卡",EventDesc="导出读者卡",EventCode= "UserCenter_ReaderCardManage_ExportCard" }};

            var appEventsUserCenter4 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="新增属性",EventDesc="新增属性",EventCode= "UserCenter_AttributeManage_CreateProperty" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="编辑属性",EventDesc="编辑属性",EventCode= "UserCenter_AtrributeManage_EditProperty" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="删除属性",EventDesc="删除属性",EventCode= "UserCenter_AtrributeManage_DeleteProperty" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="新增属性组选项",EventDesc="新增属性组选项",EventCode= "UserCenter_AtrributeManage_AddGroupItem" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="编辑属性组选项",EventDesc="编辑属性组选项",EventCode= "UserCenter_AtrributeManage_EditGroupItem" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="删除属性组选项",EventDesc="删除属性组选项",EventCode= "UserCenter_AttributeManage_DeleteGroupItem" }};

            var appEventsUserCenter5 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="新增用户组",EventDesc="新增用户组",EventCode= "UserCenter_UserGroupManage_CreateGroup" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="编辑用户组",EventDesc="编辑用户组",EventCode= "UserCenter_UserGroupManage_UpdateGroup" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="删除用户组",EventDesc="删除用户组",EventCode= "UserCenter_UserGroupManage_DeleteGroup" }};

            var appEventsUserCenter6 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="读者修改审核",EventDesc="读者修改审核",EventCode= "UserCenter_ChangeAudit_ApproveReader" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="属性修改审核",EventDesc="属性修改审核",EventCode= "UserCenter_ChangeAudit_ApproveProperty" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="注册审核",EventDesc="注册审核",EventCode= "UserCenter_ChangeAudit_ApproveRegister" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="领卡审核",EventDesc="领卡审核",EventCode= "UserCenter_ChangeAudit_ApproveCardClaim" }};

            var appEventsUserCenter7 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="基础设置",EventDesc="基础设置",EventCode= "UserCenter_UserSet_BasicConfig" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="新增角色",EventDesc="新增角色",EventCode= "UserCenter_UserSet_CreatRole" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="编辑角色",EventDesc="编辑角色",EventCode= "UserCenter_UserSet_EditRole" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="删除角色",EventDesc="删除角色",EventCode= "UserCenter_UserSet_DeleteRole" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId2.ToString(),EventName="编辑馆员角色",EventDesc="编辑馆员角色",EventCode= "UserCenter_UserSet_EditUserRole" }};

            var appEventsUserCenter8 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId2.ToString(), EventName = "登录设置", EventDesc = "登录设置", EventCode = "UserCenter_LoginSet_LoginConfig" } };

            var appEventsUserCenter9 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId2.ToString(), EventName = "注册设置", EventDesc = "注册设置", EventCode = "UserCenter_RegisterSet_RegisterConfig" } };

            _deploymentRepository.Context.BulkInsert(
                appEventsUserCenter1
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsUserCenter2
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsUserCenter3
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsUserCenter4
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsUserCenter5
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsUserCenter6
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsUserCenter7
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsUserCenter8
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsUserCenter9
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="读者管理",UseScene=2,VisitUrl="/readerlist",AppEvents=appEventsUserCenter1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="馆员管理",UseScene=2,VisitUrl="/librarianmanagement",AppEvents=appEventsUserCenter2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="读者卡管理",UseScene=2,VisitUrl="/readercardlist",AppEvents=appEventsUserCenter3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="属性管理",UseScene=2,VisitUrl="/attributelist",AppEvents=appEventsUserCenter4 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="用户组管理",UseScene=2,VisitUrl="/usergrouplist",AppEvents=appEventsUserCenter5 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="读者修改审批",UseScene=2,VisitUrl="/changeaudit",AppEvents=appEventsUserCenter6 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="管理设置",UseScene=2,VisitUrl="/userset",AppEvents=appEventsUserCenter7 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="登录设置",UseScene=2,VisitUrl="/loginsettings",AppEvents=appEventsUserCenter8 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="注册设置",UseScene=2,VisitUrl="/registersettings",AppEvents=appEventsUserCenter9 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId22.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="读者管理",UseScene=2,VisitUrl="/readerlist",AppEvents=appEventsUserCenter1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId22.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="馆员管理",UseScene=2,VisitUrl="/librarianmanagement",AppEvents=appEventsUserCenter2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId22.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="读者卡管理",UseScene=2,VisitUrl="/readercardlist",AppEvents=appEventsUserCenter3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId22.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="属性管理",UseScene=2,VisitUrl="/attributelist",AppEvents=appEventsUserCenter4 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId22.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="用户组管理",UseScene=2,VisitUrl="/usergrouplist",AppEvents=appEventsUserCenter5 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId22.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="读者修改审批",UseScene=2,VisitUrl="/changeaudit",AppEvents=appEventsUserCenter6 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId22.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="管理设置",UseScene=2,VisitUrl="/userset",AppEvents=appEventsUserCenter7 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId22.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="登录设置",UseScene=2,VisitUrl="/loginsettings",AppEvents=appEventsUserCenter8 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId22.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="注册设置",UseScene=2,VisitUrl="/registersettings",AppEvents=appEventsUserCenter9 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId42.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="读者管理",UseScene=2,VisitUrl="/readerlist",AppEvents=appEventsUserCenter1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId42.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="馆员管理",UseScene=2,VisitUrl="/librarianmanagement",AppEvents=appEventsUserCenter2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId42.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="读者卡管理",UseScene=2,VisitUrl="/readercardlist",AppEvents=appEventsUserCenter3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId42.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="属性管理",UseScene=2,VisitUrl="/attributelist",AppEvents=appEventsUserCenter4 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId42.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="用户组管理",UseScene=2,VisitUrl="/usergrouplist",AppEvents=appEventsUserCenter5 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId42.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="读者修改审批",UseScene=2,VisitUrl="/changeaudit",AppEvents=appEventsUserCenter6 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId42.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="管理设置",UseScene=2,VisitUrl="/userset",AppEvents=appEventsUserCenter7 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId42.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="登录设置",UseScene=2,VisitUrl="/loginsettings",AppEvents=appEventsUserCenter8 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId42.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="注册设置",UseScene=2,VisitUrl="/registersettings",AppEvents=appEventsUserCenter9 }}
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId2.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "我的书斋+猜你喜欢", Name = "我的书斋+猜你喜欢", Target = "/other/my_study_link" } }
                );
            #endregion

            #region 应用中心入口，事件，组件


            var appEventsAppCenter1 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId3.ToString(), EventName = "应用延期", EventDesc = "应用延期", EventCode = "AppCenter_RewnewApp" } };

            var appEventsAppCenter2 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId3.ToString(), EventName = "应用订购", EventDesc = "应用订购", EventCode = "AppCenter_BuyApp" } };

            var appEventsAppCenter3 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId3.ToString(), EventName = "分配应用管理权限", EventDesc = "分配应用管理权限", EventCode = "AppCenter_EditAppPrivilege" } };

            _deploymentRepository.Context.BulkInsert(
                appEventsAppCenter1
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsAppCenter2
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsAppCenter3
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId3.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="已购应用延期",UseScene=2,VisitUrl="/appinfomoreexpire",AppEvents=appEventsAppCenter1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId3.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新应用订购",UseScene=2,VisitUrl="/recommend",AppEvents=appEventsAppCenter2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId3.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="分配应用管理权限",UseScene=2,VisitUrl="/privilegeset",AppEvents=appEventsAppCenter3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId3.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="查看所有应用",UseScene=2,VisitUrl="/appInfo" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId3.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="个人应用中心",UseScene=1,VisitUrl="/myappInfo" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId23.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="已购应用延期",UseScene=2,VisitUrl="/appinfomoreexpire",AppEvents=appEventsAppCenter1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId23.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新应用订购",UseScene=2,VisitUrl="/recommend",AppEvents=appEventsAppCenter2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId23.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="分配应用管理权限",UseScene=2,VisitUrl="/privilegeset",AppEvents=appEventsAppCenter3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId23.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="查看所有应用",UseScene=2,VisitUrl="/appInfo" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId23.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="个人应用中心",UseScene=1,VisitUrl="/myappInfo" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId43.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="已购应用延期",UseScene=2,VisitUrl="/appinfomoreexpire",AppEvents=appEventsAppCenter1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId43.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新应用订购",UseScene=2,VisitUrl="/recommend",AppEvents=appEventsAppCenter2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId43.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="分配应用管理权限",UseScene=2,VisitUrl="/privilegeset",AppEvents=appEventsAppCenter3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId43.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="查看所有应用",UseScene=2,VisitUrl="/appInfo" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId43.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="个人应用中心",UseScene=1,VisitUrl="/myappInfo" }}
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId3.ToString(), AvailableConfig = "1,2,3",MaxTopCount=10,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "应用中心模板一", Name = "应用中心模板一", Target = "/apps_center_sys/temp1" }
                ,new AppWidget { Id = Guid.NewGuid(), AppId = appId3.ToString(), AvailableConfig = "1,2,3",MaxTopCount=10,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "应用中心模板二", Name = "应用中心模板二", Target = "/apps_center_sys/temp2" }}
                );
            #endregion 

            #region 馆员工作台入口


            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId4.ToString(),AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="馆员工作台",UseScene=2,VisitUrl="/myworkbench" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId24.ToString(),AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="馆员工作台",UseScene=2,VisitUrl="/myworkbench" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId44.ToString(),AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="馆员工作台",UseScene=2,VisitUrl="/myworkbench" }}
                );
            #endregion


            #region 文献专题引擎入口，事件，组件


            var appEventsAsm1 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId5.ToString(),EventName="删除已获取的联盟专题",EventDesc="删除已获取的联盟专题",EventCode= "Assembly_UnionShared_DeleteLocalAcquiredAssembly" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId5.ToString(),EventName="编辑已获取的联盟专题",EventDesc="编辑已获取的联盟专题",EventCode= "Assembly_UnionShared_EditLocalAcquiredAssembly" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId5.ToString(),EventName="取消共享本馆专题",EventDesc="取消共享本馆专题",EventCode= "Assembly_UnionShared_CancelSharedAssembly" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId5.ToString(),EventName="重新共享本馆专题",EventDesc="重新共享本馆专题",EventCode= "Assembly_UnionShared_ReSharedAssembly" }};

            var appEventsAsm2 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId5.ToString(),EventName="删除栏目",EventDesc="删除栏目",EventCode= "Assembly_SpecialProgram_DeleteColumn" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId5.ToString(),EventName="编辑栏目",EventDesc="编辑栏目",EventCode= "Assembly_SpecialProgram_EditColumn" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId5.ToString(),EventName="新建栏目",EventDesc="新建栏目",EventCode= "Assembly_SpecialProgram_AddColumn" }};

            var appEventsAsm3 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId5.ToString(), EventName = "新建专题", EventDesc = "新建专题", EventCode = "Assembly_SpecialAdd_AddHotAssembly" } };

            _deploymentRepository.Context.BulkInsert(
                appEventsAsm1
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsAsm2
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsAsm3
                );


            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId5.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="联盟共享专题",UseScene=2,VisitUrl="/unionshared",AppEvents=appEventsAsm1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId5.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/specialprogram",AppEvents=appEventsAsm2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId5.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="热门主题列表",UseScene=2,VisitUrl="/specialadd",AppEvents=appEventsAsm3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId25.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="联盟共享专题",UseScene=2,VisitUrl="/unionshared",AppEvents=appEventsAsm1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId25.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/specialprogram",AppEvents=appEventsAsm2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId25.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="热门主题列表",UseScene=2,VisitUrl="/specialadd",AppEvents=appEventsAsm3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId45.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="联盟共享专题",UseScene=2,VisitUrl="/unionshared",AppEvents=appEventsAsm1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId45.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/specialprogram",AppEvents=appEventsAsm2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId45.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="热门主题列表",UseScene=2,VisitUrl="/specialadd",AppEvents=appEventsAsm3 }}
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId5.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "专题引擎一", Name = "专题引擎一", Target = "/literature_project_sys/temp1" } }
                );
            #endregion

            #region 文献推荐引擎入口，事件，组件


            var appEventsRec1 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId6.ToString(), EventName = "修改文献", EventDesc = "修改文献", EventCode = "ArticleRecommend_UpdateColumn" } };

            var appEventsRec2 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId6.ToString(), EventName = "新增栏目", EventDesc = "新增栏目", EventCode = "ArticleRecommend_CreateColumn" } };


            _deploymentRepository.Context.BulkInsert(
                appEventsRec1
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsRec2
                );


            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId6.ToString(),AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="修改推荐文献",UseScene=2,VisitUrl="/intelligentmanage",AppEvents=appEventsRec1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId6.ToString(),AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新增推荐栏目",UseScene=2,VisitUrl="/intelligentpartadd",AppEvents=appEventsRec2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId26.ToString(),AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="修改推荐文献",UseScene=2,VisitUrl="/intelligentmanage",AppEvents=appEventsRec1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId26.ToString(),AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新增推荐栏目",UseScene=2,VisitUrl="/intelligentpartadd",AppEvents=appEventsRec2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId46.ToString(),AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="修改推荐文献",UseScene=2,VisitUrl="/intelligentmanage",AppEvents=appEventsRec1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId46.ToString(),AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新增推荐栏目",UseScene=2,VisitUrl="/intelligentpartadd",AppEvents=appEventsRec2 }}
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId6.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "文献智能推荐1", Name = "文献智能推荐1", Target = "/literature_recommend_sys/temp1" }
                ,new AppWidget { Id = Guid.NewGuid(), AppId = appId6.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "文献智能推荐2", Name = "文献智能推荐2", Target = "/literature_recommend_sys/temp2" }}
                );
            #endregion

            #region 数据库导航入口，事件，组件


            var appEventsDaG1 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId7.ToString(), EventName = "新增数据库信息", EventDesc = "新增数据库信息", EventCode = "databaseNav_AddDatabase" }
            ,new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId7.ToString(), EventName = "编辑数据库信息", EventDesc = "编辑数据库信息", EventCode = "databaseNav_EditDatabase" }
            ,new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId7.ToString(), EventName = "删除数据库信息", EventDesc = "删除数据库信息", EventCode = "databaseNav_DeleteDatabase" }};



            _deploymentRepository.Context.BulkInsert(
                appEventsDaG1
                );


            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId7.ToString(),AppId=appId7.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="总导航管理",UseScene=2,VisitUrl="/databasenav",AppEvents=appEventsDaG1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId27.ToString(),AppId=appId7.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="总导航管理",UseScene=2,VisitUrl="/databasenav",AppEvents=appEventsDaG1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId47.ToString(),AppId=appId7.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="总导航管理",UseScene=2,VisitUrl="/databasenav",AppEvents=appEventsDaG1 }}
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId7.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "数据库导航1", Name = "数据库导航1", Target = "/database_nav_sys/temp1" } }
                );
            #endregion

            #region 新闻发布入口，事件，组件


            var appEventsNews1 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId8.ToString(),EventName="新增栏目",EventDesc="新增栏目",EventCode= "News_NewsProgram_Create" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId8.ToString(),EventName="编辑栏目",EventDesc="编辑栏目",EventCode= "News_NewsProgram_Edit" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId8.ToString(),EventName="删除栏目",EventDesc="删除栏目",EventCode= "News_NewsProgram_Delete" }};

            var appEventsNews2 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId8.ToString(),EventName="新增新闻",EventDesc="新增新闻",EventCode= "News_NewsInfo_Create" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId8.ToString(),EventName="编辑新闻",EventDesc="编辑新闻",EventCode= "News_NewsInfo_Edit" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId8.ToString(),EventName="删除新闻",EventDesc="删除新闻",EventCode= "News_NewsInfo_Delete" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId8.ToString(),EventName="批量下架",EventDesc="批量下架",EventCode= "News_NewsInfo_OffLine" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId8.ToString(),EventName="新闻审核",EventDesc="新闻审核",EventCode= "News_NewsInfo_Approve" }};

            var appEventsNews3 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId8.ToString(), EventName = "编辑设置", EventDesc = "编辑设置", EventCode = "News_NewsSet_Config" } };

            _deploymentRepository.Context.BulkInsert(
                appEventsNews1
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsNews2
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsNews3
                );


            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId8.ToString(),AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/newsprogram",AppEvents=appEventsNews1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId8.ToString(),AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新闻管理",UseScene=2,VisitUrl="/newsinfo",AppEvents=appEventsNews2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId8.ToString(),AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用设置",UseScene=2,VisitUrl="/newsset",AppEvents=appEventsNews3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId28.ToString(),AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/newsprogram",AppEvents=appEventsNews1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId28.ToString(),AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新闻管理",UseScene=2,VisitUrl="/newsinfo",AppEvents=appEventsNews2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId28.ToString(),AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用设置",UseScene=2,VisitUrl="/newsset",AppEvents=appEventsNews3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId48.ToString(),AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/newsprogram",AppEvents=appEventsNews1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId48.ToString(),AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新闻管理",UseScene=2,VisitUrl="/newsinfo",AppEvents=appEventsNews2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId48.ToString(),AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用设置",UseScene=2,VisitUrl="/newsset",AppEvents=appEventsNews3 }}
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId8.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "新闻发布1", Name = "新闻发布1", Target = "/news_sys/temp1" } }
                );
            #endregion

            #region 信息导航入口，事件，组件


            var appEventsNav1 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId9.ToString(),EventName="新增栏目",EventDesc="新增栏目",EventCode= "Navigation_NavigationProgram_Create" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId9.ToString(),EventName="编辑栏目",EventDesc="编辑栏目",EventCode= "Navigation_NavigationProgram_Edit" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId9.ToString(),EventName="删除栏目",EventDesc="删除栏目",EventCode= "Navigation_NavigationProgram_Delete" }};

            var appEventsNav2 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId9.ToString(),EventName="新增新闻",EventDesc="新增新闻",EventCode= "Navigation_NavigationInfo_Create" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId9.ToString(),EventName="编辑新闻",EventDesc="编辑新闻",EventCode= "Navigation_NavigationInfo_Edit" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId9.ToString(),EventName="删除新闻",EventDesc="删除新闻",EventCode= "Navigation_NavigationInfo_Delete" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId9.ToString(),EventName="批量下架",EventDesc="批量下架",EventCode= "Navigation_NavigationInfo_OffLine" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId9.ToString(),EventName="新闻审核",EventDesc="新闻审核",EventCode= "Navigation_NavigationInfo_Approve" }};

            var appEventsNav3 = new List<AppEvent> { new AppEvent { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppId = appId9.ToString(), EventName = "编辑设置", EventDesc = "编辑设置", EventCode = "Navigation_NavigationSet_Config" } };

            _deploymentRepository.Context.BulkInsert(
                appEventsNav1
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsNav2
                );
            _deploymentRepository.Context.BulkInsert(
                appEventsNav3
                );


            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId9.ToString(),AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/Navigationprogram",AppEvents=appEventsNav1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId9.ToString(),AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新闻管理",UseScene=2,VisitUrl="/Navigationinfo",AppEvents=appEventsNav2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId9.ToString(),AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用设置",UseScene=2,VisitUrl="/Navigationset",AppEvents=appEventsNav3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId29.ToString(),AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/Navigationprogram",AppEvents=appEventsNav1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId29.ToString(),AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新闻管理",UseScene=2,VisitUrl="/Navigationinfo",AppEvents=appEventsNav2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId29.ToString(),AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用设置",UseScene=2,VisitUrl="/Navigationset",AppEvents=appEventsNav3 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId49.ToString(),AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/Navigationprogram",AppEvents=appEventsNav1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId49.ToString(),AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新闻管理",UseScene=2,VisitUrl="/Navigationinfo",AppEvents=appEventsNav2 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId49.ToString(),AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用设置",UseScene=2,VisitUrl="/Navigationset",AppEvents=appEventsNav3 }}
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId9.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "新闻发布1", Name = "新闻发布1", Target = "/service_sys/temp1" } }
                );
            #endregion

            #region Opac中心入口，事件，组件


            var appEventsOpac1 = new List<AppEvent> { new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId10.ToString(),EventName="申请预约",EventDesc="申请预约",EventCode= "Opac_Appointment_Add" }
                ,new AppEvent { Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,AppId=appId10.ToString(),EventName="取消预约",EventDesc="取消预约",EventCode= "Opac_Appointment_Delete" }};


            _deploymentRepository.Context.BulkInsert(
                appEventsOpac1
                );


            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId10.ToString(),AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="预约管理",UseScene=2,VisitUrl="/appointment",AppEvents=appEventsOpac1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId30.ToString(),AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="预约管理",UseScene=2,VisitUrl="/appointment",AppEvents=appEventsOpac1 }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId50.ToString(),AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="预约管理",UseScene=2,VisitUrl="/appointment",AppEvents=appEventsOpac1 }}
                );

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId10.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "新闻发布1", Name = "新闻发布1", Target = "/service_sys/temp1" } }
                );
            #endregion

            #region 数据中心入口，事件，组件
            //暂无
            #endregion

            #region 行为分析入口，事件，组件

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId12.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "本馆数据", Name = "本馆数据", Target = "/other/our_data" } }
                );
            #endregion

            #region 学院分馆入口，事件，组件

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId13.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "学院分馆", Name = "学院分馆", Target = "/department/temp1" } }
                );
            #endregion

            #region 猜你喜欢入口，事件，组件

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId14.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "猜你喜欢", Name = "猜你喜欢", Target = "/other/my_study_link" } }
                );
            #endregion

            #region 通知中心入口，事件，组件

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId14.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "站内信", Name = "站内信", Target = "/notice/site_message" } }
                );
            #endregion

            #region 活动中心入口，事件，组件

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id = Guid.NewGuid(), AppId = appId14.ToString(), AvailableConfig = "", CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "活动日历", Name = "活动日历", Target = "/other/activity_calendar" } }
                );
            #endregion
        }

        /// <summary>
        /// 添加测试数据——积分中心
        /// </summary>
        /// <returns></returns>
        public void AddScoreCenterTestData()
        {
            var appId1 = Guid.NewGuid();
            _microAppRepository.Context.BulkInsert(
                new[] {new MicroApplication { Id=appId1,CreatedTime=DateTimeOffset.UtcNow,Desc="积分中心",Name= "积分中心", AdvisePrice=10000,DevId= "26857b56-e438-4ad5-afb4-9b9bc80aed30", FreeTry=false,Icon="/app_icons/icon(2).png",Intro= "积分中心", PriceType=2,RecommendScore=10,RouteCode="scorecenter",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                });
            #region 基础数据，应用分支，部署环境

            var appBrId1 = Guid.NewGuid();
            _deploymentRepository.Context.BulkInsert(
                new[] { new AppBranch { Id = appBrId1, AppId = appId1.ToString(), CreatedTime = DateTimeOffset.UtcNow, DeployeeId = "0e6558f8-da1a-4144-8877-decf9b07abd8", Icon = "/app_icons/icon(2).png", IsMaster = false, Name = "积分中心", Remark = "积分中心", Version = "3.0.0" } }
                );
            #endregion

            _deploymentRepository.Context.BulkInsert(
         new[] { new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId1.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="通用检索",UseScene=1,VisitUrl="/articlesearch" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId1.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="站内检索",UseScene=1,VisitUrl="/sitesearch" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=false,IsSystem=true,AppBranchId= appBrId1.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="检索设置",UseScene=2,VisitUrl="/searchmanage"}
                });

            _deploymentRepository.Context.BulkInsert(
                new[] { new AppWidget { Id= Guid.NewGuid(), AppId=appId1.ToString(),AvailableConfig="",CreatedTime=DateTimeOffset.UtcNow,Cover= "/app_icons/icon(1).png",Desc="统一检索一",Name="统一检索一",Target= "/unified_retrieval_sys/temp1"}
                }
                );
        }

        /// <summary>
        /// 添加测试数据-应用入口
        /// </summary>
        public void AddAppEntrance()
        {
            //积分中心
            var appId_ScoreCenter = new Guid("24567b8d-4908-5349-a6f6-d47bfda80522");
            var appBrId1_ScoreCenter = new Guid("94561237-ac8d-3c32-acb9-521238a52433");
            var appBrId2_ScoreCenter = new Guid("91234567-ac8d-3c32-acb9-5c80a8a52433");
            var appBrId3_ScoreCenter = new Guid("97654321-ac8d-3c32-acb9-5c80a8a52433");

            //积分中心
            //var appId_ScoreCenter = new Guid("24567b8d-4908-5349-a6f6-d47bfda80522");
            //var appBrId1_ScoreCenter = new Guid("94561237-ac8d-3c32-acb9-521238a52433");
            //var appBrId2_ScoreCenter = new Guid("91234567-ac8d-3c32-acb9-5c80a8a52433");
            //var appBrId3_ScoreCenter = new Guid("97654321-ac8d-3c32-acb9-5c80a8a52433");

            ////通用检索
            //var appId_Search = new Guid("27ad8830-1085-4f42-adb7-237c16fb383c");
            //var appBrId1_Search = new Guid("64186b50-48a4-42a5-b1a4-d698ac966af4");
            //var appBrId2_Search = new Guid("83724c79-8aa3-486e-bacb-cb2a984b2e0c");
            //var appBrId3_Search = new Guid("f6b78feb-2b7c-47f1-b07f-adf6edd6c352");

            ////应用中心
            //var appId_AppCenter = new Guid("42757eb3-7a18-4619-adb2-cda39aee5602");
            //var appBrId1_AppCenter = new Guid("c41da50b-ba5f-4cb8-b9f2-a73049610f4a");
            //var appBrId2_AppCenter = new Guid("9bae47c8-8421-4953-85ab-e8b1d6d3d9ed");
            //var appBrId3_AppCenter = new Guid("a2aebd4c-13da-40ce-b9fd-3b9ce31459f6");

            ////信息导航
            //var appId_Nav = new Guid("a7fedf20-e7be-483d-9471-11d5a9a49203");
            //var appBrId1_Nav = new Guid("1727343a-8af0-4b44-8043-dce64d2ff151");
            //var appBrId2_Nav = new Guid("8419c252-373f-492b-ab9d-930489fd4f48");
            //var appBrId3_Nav = new Guid("8225d227-33df-4d81-8663-d497b42f6fd4");

            ////新闻发布
            //var appId_News = new Guid("b0fb53b3-f7b3-41a5-94a5-c4feb71213e2");
            //var appBrId1_News = new Guid("192220fc-7155-48e9-8026-816b617a4a84");
            //var appBrId2_News = new Guid("1cbb581f-e5d4-44a6-980e-8839d8c6d394");
            //var appBrId3_News = new Guid("e2ff0036-ac1d-4760-93a4-fa889d6c9a3f");


            ////数据库导航
            //var appId_Database = new Guid("d759fd09-b08e-4e37-99c3-4d4d67baaa6a");
            //var appBrId1_Database = new Guid("3dbceb5a-01c5-48fd-9e69-41d13592bd85");
            //var appBrId2_Database = new Guid("1a062325-bbe4-4bf4-9c88-ce25867e2c76");
            //var appBrId3_Database = new Guid("dee17a43-ad7a-444f-a1ba-42232cd15ce8");


            ////文献专题引擎
            //var appId_Asm = new Guid("dba5df91-dc9c-4dd2-9527-3741a425b7b3");
            //var appBrId1_Asm = new Guid("50a69dc3-6d1a-4c41-b5bc-2a94ae69ae75");
            //var appBrId2_Asm = new Guid("edf9dc57-b559-4427-bae9-9f57c71ae851");
            //var appBrId3_Asm = new Guid("c5fd89f3-ce86-4e2f-9d42-58bd4b1ed226");

            ////文献推荐引擎
            //var appId_recomm = new Guid("f22846b8-53d4-4f5c-a62c-92d28efe447f");
            //var appBrId1_recomm = new Guid("4a2a6d01-0507-469e-a179-096d3b6c1f70");
            //var appBrId2_recomm = new Guid("d8b6d105-6b98-435e-873f-5b1cff832145");
            //var appBrId3_recomm = new Guid("94eed6c1-00ed-4523-91c4-49ec4d841612");

            ////用户中心
            //var appId_user = new Guid("f7958d7f-8cca-4ac6-84f8-e85b9f5f6a74");
            //var appBrId1_user = new Guid("0d5e906d-b952-415e-9504-f3b3a54cba35");
            //var appBrId2_user = new Guid("beb8dfae-ac8d-4c41-acb9-5c80a8a52433");
            //var appBrId3_user = new Guid("9bbbf2dd-2d6f-4a66-880f-991ec4a536ce");


            //运行统计
            var appId_log = new Guid("9fed516c-9424-4b0d-b4b2-1d72adde9716");
            var appBrId1_log = new Guid("c3431e6b-7f46-4810-b880-8a5e52fb877a");
            var appBrId2_log = new Guid("394460e0-706f-4c3f-a6eb-1fe4148ca816");
            var appBrId3_log = new Guid("17f14599-a7fa-4b21-a3ed-638cedb97914");


            //文献中心
            var appId_resource = new Guid("5b489493-1585-4a7c-b4e6-c9245d1f6fdc");
            var appBrId1_resource = new Guid("cce8d0dd-acf8-4992-a7a1-249e3b554fc6");


            _deploymentRepository.Context.BulkInsert(
                 new[] {
                // new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_ScoreCenter.ToString(),AppId=appId_ScoreCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="个人积分中心",UseScene=1,VisitUrl="/#/web_integralCenter" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_ScoreCenter.ToString(),AppId=appId_ScoreCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="积分配制",UseScene=2,VisitUrl="/#/admin_integralWork" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_ScoreCenter.ToString(),AppId=appId_ScoreCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="个人积分中心",UseScene=1,VisitUrl="/#/web_integralCenter" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_ScoreCenter.ToString(),AppId=appId_ScoreCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="积分配制",UseScene=2,VisitUrl="/#/admin_integralWork" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_ScoreCenter.ToString(),AppId=appId_ScoreCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="个人积分中心",UseScene=1,VisitUrl="/#/web_integralCenter" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_ScoreCenter.ToString(),AppId=appId_ScoreCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="积分配制",UseScene=2,VisitUrl="/#/admin_integralWork" }
                // new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_Search.ToString(),AppId=appId_Search.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="检索框列表",UseScene=2,VisitUrl="/#/admin_searchboxlist" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_Search.ToString(),AppId=appId_Search.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="站内检索",UseScene=1,VisitUrl="/#/search/webSiteSearch" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_Search.ToString(),AppId=appId_Search.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="检索框列表",UseScene=2,VisitUrl="/#/admin_searchboxlist" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_Search.ToString(),AppId=appId_Search.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="站内检索",UseScene=1,VisitUrl="/#/search/webSiteSearch" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_Search.ToString(),AppId=appId_Search.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="检索框列表",UseScene=2,VisitUrl="/#/admin_searchboxlist" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_Search.ToString(),AppId=appId_Search.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="站内检索",UseScene=1,VisitUrl="/#/search/webSiteSearch" }

                ////,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_AppCenter.ToString(),AppId=appId_AppCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用信息管理",UseScene=2,VisitUrl="/#/admin_appInfo" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_AppCenter.ToString(),AppId=appId_AppCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用中心",UseScene=1,VisitUrl="/#/web_appsCenter" }
                ////,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_AppCenter.ToString(),AppId=appId_AppCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用信息管理",UseScene=2,VisitUrl="/#/admin_appInfo" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_AppCenter.ToString(),AppId=appId_AppCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用中心",UseScene=1,VisitUrl="/#/web_appsCenter" }
                ////,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_AppCenter.ToString(),AppId=appId_AppCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用信息管理",UseScene=2,VisitUrl="/#/admin_appInfo" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_AppCenter.ToString(),AppId=appId_AppCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="应用中心",UseScene=1,VisitUrl="/#/web_appsCenter" }

                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_Nav.ToString(),AppId=appId_Nav.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目设置",UseScene=2,VisitUrl="/#/admin_navigationProgram" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_Nav.ToString(),AppId=appId_Nav.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="导航列表",UseScene=1,VisitUrl="/#/web_list1" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_Nav.ToString(),AppId=appId_Nav.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目设置",UseScene=2,VisitUrl="/#/admin_navigationProgram" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_Nav.ToString(),AppId=appId_Nav.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="导航列表",UseScene=1,VisitUrl="/#/web_list1" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_Nav.ToString(),AppId=appId_Nav.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目设置",UseScene=2,VisitUrl="/#/admin_navigationProgram" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_Nav.ToString(),AppId=appId_Nav.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="导航列表",UseScene=1,VisitUrl="/#/web_list1" }

                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_News.ToString(),AppId=appId_News.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/#/admin_newsProgram" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_News.ToString(),AppId=appId_News.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新闻列表",UseScene=1,VisitUrl="/#/web_list1" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_News.ToString(),AppId=appId_News.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/#/admin_newsProgram" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_News.ToString(),AppId=appId_News.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新闻列表",UseScene=1,VisitUrl="/#/web_list1" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_News.ToString(),AppId=appId_News.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/#/admin_newsProgram" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_News.ToString(),AppId=appId_News.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="新闻列表",UseScene=1,VisitUrl="/#/web_list1" }

                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_Database.ToString(),AppId=appId_Database.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="总导航管理",UseScene=2,VisitUrl="/#/admin_databaseNav" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_Database.ToString(),AppId=appId_Database.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="数据库导航首页",UseScene=1,VisitUrl="/#/web_dataBaseHome" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_Database.ToString(),AppId=appId_Database.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="总导航管理",UseScene=2,VisitUrl="/#/admin_databaseNav" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_Database.ToString(),AppId=appId_Database.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="数据库导航首页",UseScene=1,VisitUrl="/#/web_dataBaseHome" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_Database.ToString(),AppId=appId_Database.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="总导航管理",UseScene=2,VisitUrl="/#/admin_databaseNav" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_Database.ToString(),AppId=appId_Database.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="数据库导航首页",UseScene=1,VisitUrl="/#/web_dataBaseHome" }

                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_Asm.ToString(),AppId=appId_Asm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/#/admin_specialProgram" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_Asm.ToString(),AppId=appId_Asm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="专题列表",UseScene=1,VisitUrl="/#/web_topicList" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_Asm.ToString(),AppId=appId_Asm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/#/admin_specialProgram" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_Asm.ToString(),AppId=appId_Asm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="专题列表",UseScene=1,VisitUrl="/#/web_topicList" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_Asm.ToString(),AppId=appId_Asm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="栏目管理",UseScene=2,VisitUrl="/#/admin_specialProgram" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_Asm.ToString(),AppId=appId_Asm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="专题列表",UseScene=1,VisitUrl="/#/web_topicList" }

                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_recomm.ToString(),AppId=appId_recomm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="文献推荐管理",UseScene=2,VisitUrl="/#/admin_intelligentManage" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_recomm.ToString(),AppId=appId_recomm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="文献推荐列表",UseScene=1,VisitUrl="/#/web_recommendList" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_recomm.ToString(),AppId=appId_recomm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="文献推荐管理",UseScene=2,VisitUrl="/#/admin_intelligentManage" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_recomm.ToString(),AppId=appId_recomm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="文献推荐列表",UseScene=1,VisitUrl="/#/web_recommendList" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_recomm.ToString(),AppId=appId_recomm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="文献推荐管理",UseScene=2,VisitUrl="/#/admin_intelligentManage" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_recomm.ToString(),AppId=appId_recomm.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="文献推荐列表",UseScene=1,VisitUrl="/#/web_recommendList" }

                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_user.ToString(),AppId=appId_user.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="用户管理",UseScene=2,VisitUrl="/#/admin_userManager" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_user.ToString(),AppId=appId_user.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="个人中心",UseScene=1,VisitUrl="/#/web_library" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_user.ToString(),AppId=appId_user.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="用户管理",UseScene=2,VisitUrl="/#/admin_userManager" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2_user.ToString(),AppId=appId_user.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="个人中心",UseScene=1,VisitUrl="/#/web_library" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_user.ToString(),AppId=appId_user.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="用户管理",UseScene=2,VisitUrl="/#/admin_userManager" }
                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_user.ToString(),AppId=appId_user.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="个人中心",UseScene=1,VisitUrl="/#/web_library" }

                //,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3_ScoreCenter.ToString(),AppId=appId_ScoreCenter.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="积分配制",UseScene=2,VisitUrl="/#/admin_integralWork" }
                new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_log.ToString(),AppId=appId_log.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="行为统计",UseScene=2,VisitUrl="/core/statisticsoverview/index" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1_resource.ToString(),AppId=appId_resource.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="文献中心",UseScene=2,VisitUrl="/Asset/Index.aspx" }

                 }


                 );
        }

        /// <summary>
        /// 添加测试数据-应用模板
        /// </summary>
        public void AddAppWidget()
        {

            //积分中心
            var appId_ScoreCenter = new Guid("24567b8d-4908-5349-a6f6-d47bfda80522");
            //通用检索
            var appId_Search = new Guid("27ad8830-1085-4f42-adb7-237c16fb383c");
            //应用中心
            var appId_AppCenter = new Guid("42757eb3-7a18-4619-adb2-cda39aee5602");
            //信息导航
            var appId_Nav = new Guid("a7fedf20-e7be-483d-9471-11d5a9a49203");
            //新闻发布
            var appId_News = new Guid("b0fb53b3-f7b3-41a5-94a5-c4feb71213e2");
            //数据库导航
            var appId_Database = new Guid("d759fd09-b08e-4e37-99c3-4d4d67baaa6a");
            //文献专题引擎
            var appId_Asm = new Guid("dba5df91-dc9c-4dd2-9527-3741a425b7b3");
            //文献推荐引擎
            var appId_recomm = new Guid("f22846b8-53d4-4f5c-a62c-92d28efe447f");
            //用户中心
            var appId_user = new Guid("f7958d7f-8cca-4ac6-84f8-e85b9f5f6a74");
            //馆员工作台
            var appId_wb = new Guid("0427db88-1c6e-4082-aa00-8edaabb2e4b6");
            //猜你喜欢
            var appId_gul = new Guid("09bf2453-22af-4676-8cab-7d2ed50e0882");
            //数据中心
            var appId_dc = new Guid("1308dfee-bc5e-4fa8-b4da-0d5f36fba8d0");
            //活动中心
            var appId_ac = new Guid("786ddebc-e762-416e-a817-0ff56aa97bc1");
            //学院分馆
            var appId_dpm = new Guid("5eeb1b4d-9d62-4254-8de0-052e5b1ac48a");
            //行为分析
            var appId_alynsis = new Guid("9fed516c-9424-4b0d-b4b2-1d72adde9716");
            //通知中心
            var appId_nc = new Guid("ea154354-4292-4de4-9718-67ad795401a5");
            //课程文献中
            var appId_crc = new Guid("64191f16-fd57-48e8-85dc-d44904950dc3");
            //科研专题资料库
            var appId_sac = new Guid("95e5bc21-b9c6-4025-b190-7312e63111b7");
            //图书捐赠
            var appId_donation = new Guid("6c91586a-d485-483f-ab5e-1b9496409899");
            //资源中心
            var appId_rc = new Guid("5b489493-1585-4a7c-b4e6-c9245d1f6fdc");






            _deploymentRepository.Context.BulkInsert(
               new[] {
               //    new AppWidget { Id= Guid.NewGuid(), AppId=appId_Search.ToString(),SceneType=1,Width=6,Height=10,AvailableConfig="2",MaxTopCount=20,TopCountInterval=1,CreatedTime=DateTimeOffset.UtcNow,Cover= "/app_icons/icon(1).png",Desc="统一检索",Name="统一检索",Target= "/unified_retrieval_sys/temp1"}
               //,new AppWidget { Id= Guid.NewGuid(), AppId=appId_Search.ToString(),SceneType=2,Width=6,Height=6,AvailableConfig="2",MaxTopCount=20,TopCountInterval=1,CreatedTime=DateTimeOffset.UtcNow,Cover= "/app_icons/icon(1).png",Desc="个人图书馆-统一检索",Name="个人图书馆-统一检索",Target= "/unified_retrieval_sys/temp2"}
               
               ////,new AppWidget { Id = Guid.NewGuid(), AppId = appId_user.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "2",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "个人图书馆", Name = "个人图书馆", Target = "/other/college_library" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_user.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "2",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "我的书斋", Name = "我的书斋", Target = "/other/my_study_link" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_user.ToString(),SceneType=1,Width=3,Height=10, AvailableConfig = "1,2",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "固定左边模板-多个应用", Name = "固定左边模板-多个应用", Target = "/other/left_menu_list" }

               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_AppCenter.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "应用中心一", Name = "应用中心一", Target = "/apps_center_sys/temp1" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_AppCenter.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "应用中心二", Name = "应用中心二", Target = "/apps_center_sys/temp2" }
               
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_Asm.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "文献专题引擎", Name = "文献专题引擎", Target = "/literature_project_sys/temp1" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_Asm.ToString(),SceneType=2,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "个人图书馆-专题类型", Name = "个人图书馆-专题类型", Target = "/literature_project_sys/temp2" }

               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "文献智能推荐1", Name = "文献推荐引擎", Target = "/literature_recommend_sys/temp1" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "文献智能推荐2", Name = "文献智能推荐2", Target = "/literature_recommend_sys/temp2" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(),SceneType=2,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "个人图书馆-资源推荐类", Name = "个人图书馆-资源推荐类", Target = "/literature_recommend_sys/temp3" }

               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_Database.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "数据库导航", Name = "数据库导航", Target = "/database_nav_sys/temp1" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_Database.ToString(),SceneType=2,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "个人图书馆-常用数据库", Name = "个人图书馆-常用数据库", Target = "/database_nav_sys/temp2" }
               
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_News.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "首页新闻动态", Name = "首页新闻动态", Target = "/news_sys/temp1" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_News.ToString(),SceneType=2,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "个人图书馆-图书馆新闻", Name = "个人图书馆-图书馆新闻", Target = "/news_sys/temp2" }
               
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_Nav.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "信息导航", Name = "信息导航", Target = "/service_sys/temp1" }
               
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_alynsis.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "本馆数据", Name = "本馆数据", Target = "/other/our_data" }
               
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_dpm.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "学院分馆", Name = "学院分馆", Target = "/other/college_library" }
               
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_ScoreCenter.ToString(),SceneType=2,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "个人图书馆-积分任务", Name = "个人图书馆-积分任务", Target = "/integral_center_sys/temp1" }
               
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_nc.ToString(),SceneType=2,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "个人图书馆-通知消息", Name = "个人图书馆-通知消息", Target = "/apps_center_sys/temp3/" }
               
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_ac.ToString(),SceneType=1,Width=6,Height=10, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "活动日历", Name = "活动日历", Target = "/other/activity_calendar" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_ac.ToString(),SceneType=2,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "个人图书馆-图书馆活动", Name = "个人图书馆-图书馆活动", Target = "/other/lib_activity" }
               //new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(),SceneType=1,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "图书频道-新书速递", Name = "图书频道-新书速递", Target = "/literature_recommend_sys/temp4" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(),SceneType=1,Width=6,Height=50, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "图书频道-特色书单", Name = "图书频道-特色书单", Target = "/literature_recommend_sys/temp5" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(),SceneType=1,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "图书频道-标签导航", Name = "图书频道-标签导航", Target = "/literature_recommend_sys/temp6" }
               //, new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(), SceneType = 1, Width = 6, Height = 6, AvailableConfig = "1,2,3", MaxTopCount = 20, TopCountInterval = 1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "图书频道-推荐期刊", Name = "图书频道-推荐期刊", Target = "/literature_recommend_sys/temp7" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(),SceneType=1,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "图书频道-核心刊收录和学科导航", Name = "图书频道-核心刊收录和学科导航", Target = "/literature_recommend_sys/temp8" }
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_alynsis.ToString(),SceneType=1,Width=6,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "图书频道-最新借出图书", Name = "图书频道-最新借出图书", Target = "/loganalysis_sys/temp1" }
                              
                   

               //快应用中心
               //new AppWidget { Id = Guid.NewGuid(), AppId = appId_AppCenter.ToString(),SceneType=1,Width=3,Height=9, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "快应用中心", Name = "快应用中心", Target = "/cqu/apps_center_sys/temp1" }
               ////新书速递-单栏目
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(),SceneType=1,Width=9,Height=30, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "新书速递-单栏目", Name = "新书速递-单栏目", Target = "/cqu/literature_recommend_sys/temp1" }
               ////借阅排行、热门下载-多栏目
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(),SceneType=1,Width=6,Height=30, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "借阅排行、热门下载-多栏目", Name = "借阅排行、热门下载-多栏目", Target = "/cqu/literature_recommend_sys/temp2" }
               ////新闻轮播
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_News.ToString(),SceneType=1,Width=4,Height=29, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "新闻轮播", Name = "新闻轮播", Target = "/cqu/news_sys/temp1" }
               ////新闻列表
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_News.ToString(),SceneType=1,Width=3,Height=42, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "新闻列表", Name = "新闻列表", Target = "/cqu/news_sys/temp2" }
               ////文献情报动态
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_News.ToString(),SceneType=1,Width=3,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "文献情报动态", Name = "文献情报动态", Target = "/cqu/news_sys/temp3" }
               ////期刊频道-统一检索
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_Search.ToString(),SceneType=1,Width=12,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "期刊频道-统一检索", Name = "期刊频道-统一检索", Target = "/unified_retrieval_sys/temp3" }
               ////图书频道-统一检索
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_Search.ToString(),SceneType=1,Width=12,Height=6, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "图书频道-统一检索", Name = "图书频道-统一检索", Target = "/unified_retrieval_sys/temp4" }
               ////统一检索2
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_Search.ToString(),SceneType=1,Width=12,Height=24, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "统一检索2", Name = "统一检索2", Target = "/cqu/unified_retrieval_sys/temp1/" }
               ////本馆数据1
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_alynsis.ToString(),SceneType=1,Width=12,Height=20, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "本馆数据1", Name = "本馆数据1", Target = "/cqu/lib_data/temp1" }
               ////资源中心
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_rc.ToString(),SceneType=1,Width=3,Height=8, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "资源中心", Name = "资源中心", Target = "/cqu/channel_navigation/temp1" }
               ////学院分馆1
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_dpm.ToString(),SceneType=1,Width=3,Height=73, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "学院分馆1", Name = "学院分馆1", Target = "/cqu/department_lib/temp1" }
               ////活动日历1
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_ac.ToString(),SceneType=1,Width=3,Height=42, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "活动日历1", Name = "活动日历1", Target = "/cqu/activity_calendar/temp1" }
               ////我的信息
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_user.ToString(),SceneType=1,Width=3,Height=73, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "我的信息", Name = "我的信息", Target = "/cqu/user_manager/temp1" }
               ////书妹儿推荐
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_recomm.ToString(),SceneType=1,Width=3,Height=30, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "书妹儿推荐", Name = "书妹儿推荐", Target = "/cqu/shumeier_recommended/temp1" }
               ////猜你喜欢
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_gul.ToString(),SceneType=1,Width=3,Height=41, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "猜你喜欢", Name = "猜你喜欢", Target = "/cqu/guess_you_like/temp1" }
               ////课程文献中心
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_crc.ToString(),SceneType=1,Width=9,Height=24, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "课程文献中心", Name = "课程文献中心", Target = "/cqu/course_document_center/temp1" }
               ////科研专题库
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_sac.ToString(),SceneType=1,Width=9,Height=24, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "科研专题库", Name = "科研专题库", Target = "/cqu/scientific_research_library/temp1" }
               ////捐赠致谢
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_donation.ToString(),SceneType=1,Width=3,Height=50, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "捐赠致谢", Name = "捐赠致谢", Target = "/cqu/thanks_for_donation/temp1" }
               ////友情链接
               //,new AppWidget { Id = Guid.NewGuid(), AppId = appId_Nav.ToString(),SceneType=1,Width=12,Height=20, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "友情链接", Name = "友情链接", Target = "/cqu/friendly_link/temp1" }
               ////友情链接
               new AppWidget { Id = Guid.NewGuid(), AppId = appId_Nav.ToString(),SceneType=1,Width=2,Height=29, AvailableConfig = "1,2,3",MaxTopCount=20,TopCountInterval=1, CreatedTime = DateTimeOffset.UtcNow, Cover = "/app_icons/icon(1).png", Desc = "图片导航", Name = "图片导航", Target = "/cqu/navigation_sys/temp1" }




                    }


               );
        }

        /// <summary>
        /// 添加测试数据——积分中心
        /// </summary>
        /// <returns></returns>
        public void AddAppTestData()
        {
            try
            {
                var appId1 = Guid.NewGuid();
                var appId2 = Guid.NewGuid();
                var appId3 = Guid.NewGuid();
                var appId4 = Guid.NewGuid();

                var devId = "26857b56-e438-4ad5-afb4-9b9bc80aed30";
                var deployeeId = "0e6558f8-da1a-4144-8877-decf9b07abd8";
                var cusId = "1fc3b47e-76a8-4ab0-bc60-c43fd9a9a2a9";

                _microAppRepository.Context.BulkInsert(
          new[] {new MicroApplication { Id=appId1,CreatedTime=DateTimeOffset.UtcNow,Desc="课程文献中心",Name= "课程文献中心", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/courseresourcecenter.svg",Intro= "课程文献中心", PriceType=2,RecommendScore=10,RouteCode="courseresourcecenter",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId2,CreatedTime=DateTimeOffset.UtcNow,Desc="科研专题资料库",Name= "科研专题资料库", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/scientificresearchresource.svg",Intro= "科研专题资料库", PriceType=2,RecommendScore=10,RouteCode="scientificresearchresource",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId3,CreatedTime=DateTimeOffset.UtcNow,Desc="文献中心",Name= "文献中心", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/resourcecenter.svg",Intro= "文献中心", PriceType=2,RecommendScore=10,RouteCode="resourcecenter",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId4,CreatedTime=DateTimeOffset.UtcNow,Desc="图书捐赠",Name= "图书捐赠", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/donation.svg",Intro= "图书捐赠", PriceType=2,RecommendScore=10,RouteCode="donation",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                    });
                #region 基础数据，应用分支，部署环境
                var appBrId1 = Guid.NewGuid();
                var appBrId2 = Guid.NewGuid();
                var appBrId3 = Guid.NewGuid();
                var appBrId4 = Guid.NewGuid();
                _microAppRepository.Context.BulkInsert(
         new[] { new AppBranch {Id=appBrId1, AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/courseresourcecenter.svg",IsMaster=false, Name="课程文献中心", Remark="课程文献中心",Version="3.0.0"}
                ,new AppBranch {Id=appBrId2, AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/scientificresearchresource.svg",IsMaster=false, Name="科研专题资料库", Remark="科研专题资料库",Version="3.0.0"}
                ,new AppBranch {Id=appBrId3, AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/resourcecenter.svg",IsMaster=false, Name="文献中心", Remark="文献中心",Version="3.0.0"}
                ,new AppBranch {Id=appBrId4, AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/donation.svg",IsMaster=false, Name="图书捐赠", Remark="图书捐赠",Version="3.0.0"}
                    });

                _microAppRepository.Context.BulkInsert(
        new[] { new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId1.ToString(),AppId=appId1.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId2.ToString(),AppId=appId2.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId3.ToString(),AppId=appId3.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId4.ToString(),AppId=appId4.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
                   });
            }
            catch (Exception ex)
            {
            }
            #endregion
        }


        /// <summary>
        /// 添加重大需要的应用,全部跳转到2.2版本
        /// </summary>
        public void AddTestAppsForCqu()
        {
            //开发者标识
            var devId = "26857b56-e438-4ad5-afb4-9b9bc80aed30";
            //部署环境标识
            var deployeeId = "0e6558f8-da1a-4144-8877-decf9b07abd8";
            //客户标识
            var cusId = "1fc3b47e-76a8-4ab0-bc60-c43fd9a9a2a9";

            var appId1 = Guid.NewGuid();
            var appId2 = Guid.NewGuid();
            var appId3 = Guid.NewGuid();
            var appId4 = Guid.NewGuid();
            var appId5 = Guid.NewGuid();
            var appId6 = Guid.NewGuid();
            var appId7 = Guid.NewGuid();
            var appId8 = Guid.NewGuid();
            var appId9 = Guid.NewGuid();
            var appId10 = Guid.NewGuid();
            var appId11 = Guid.NewGuid();
            var appId12 = Guid.NewGuid();
            var appId13 = Guid.NewGuid();
            var appId14 = Guid.NewGuid();
            var appId15 = Guid.NewGuid();

            // 插入MicroApplication表
            _microAppRepository.Context.BulkInsert(
          new[] {new MicroApplication { Id=appId1,CreatedTime=DateTimeOffset.UtcNow,Desc="参考咨询",Name= "参考咨询", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "参考咨询", PriceType=2,RecommendScore=10,RouteCode="reference",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId2,CreatedTime=DateTimeOffset.UtcNow,Desc="阅读记录",Name= "阅读记录", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "阅读记录", PriceType=2,RecommendScore=10,RouteCode="readrecord",ServiceType="1",Status=1,Terminal="1",UseScene="1"}
                ,new MicroApplication { Id=appId3,CreatedTime=DateTimeOffset.UtcNow,Desc="图书借阅",Name= "图书借阅", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "图书借阅", PriceType=2,RecommendScore=10,RouteCode="bookborrow",ServiceType="1",Status=1,Terminal="1",UseScene="1"}
                ,new MicroApplication { Id=appId4,CreatedTime=DateTimeOffset.UtcNow,Desc="离校申请",Name= "离校申请", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "离校申请", PriceType=2,RecommendScore=10,RouteCode="leaveschool",ServiceType="1",Status=1,Terminal="1",UseScene="1"}
                ,new MicroApplication { Id=appId5,CreatedTime=DateTimeOffset.UtcNow,Desc="友位空间预约",Name= "友位空间预约", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "友位空间预约", PriceType=2,RecommendScore=10,RouteCode="spaceorder",ServiceType="6",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId6,CreatedTime=DateTimeOffset.UtcNow,Desc="活动报名",Name= "活动报名", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "活动报名", PriceType=2,RecommendScore=10,RouteCode="activityorder",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId7,CreatedTime=DateTimeOffset.UtcNow,Desc="问卷调查",Name= "问卷调查", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "问卷调查", PriceType=2,RecommendScore=10,RouteCode="questionorder",ServiceType="1",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId8,CreatedTime=DateTimeOffset.UtcNow,Desc="审批管理",Name= "审批管理", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "审批管理", PriceType=2,RecommendScore=10,RouteCode="approvalmgr",ServiceType="6",Status=1,Terminal="1",UseScene="2"}
                ,new MicroApplication { Id=appId9,CreatedTime=DateTimeOffset.UtcNow,Desc="报修管理",Name= "报修管理", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "报修管理", PriceType=2,RecommendScore=10,RouteCode="repairmgr",ServiceType="6",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId10,CreatedTime=DateTimeOffset.UtcNow,Desc="ERMS",Name= "ERMS", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "ERMS", PriceType=2,RecommendScore=10,RouteCode="erms",ServiceType="6",Status=1,Terminal="1",UseScene="2"}
                ,new MicroApplication { Id=appId11,CreatedTime=DateTimeOffset.UtcNow,Desc="文献数据查重",Name= "文献数据查重", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "文献数据查重", PriceType=2,RecommendScore=10,RouteCode="checkrepeat",ServiceType="6",Status=1,Terminal="1",UseScene="2"}
                ,new MicroApplication { Id=appId12,CreatedTime=DateTimeOffset.UtcNow,Desc="大数据分析",Name= "大数据分析", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "大数据分析", PriceType=2,RecommendScore=10,RouteCode="bigdata",ServiceType="7",Status=1,Terminal="1",UseScene="3"}
                ,new MicroApplication { Id=appId13,CreatedTime=DateTimeOffset.UtcNow,Desc="数据接口管理",Name= "数据接口管理", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "数据接口管理", PriceType=2,RecommendScore=10,RouteCode="apimgr",ServiceType="6",Status=1,Terminal="1",UseScene="2"}
                ,new MicroApplication { Id=appId14,CreatedTime=DateTimeOffset.UtcNow,Desc="数据授权管理",Name= "数据授权管理", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "数据授权管理", PriceType=2,RecommendScore=10,RouteCode="dataauthmgr",ServiceType="6",Status=1,Terminal="1",UseScene="2"}
                ,new MicroApplication { Id=appId15,CreatedTime=DateTimeOffset.UtcNow,Desc="数据监控日志",Name= "数据监控日志", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",Intro= "数据监控日志", PriceType=2,RecommendScore=10,RouteCode="datamonitorlog",ServiceType="6",Status=1,Terminal="1",UseScene="2"}
                    });
            //分支数据
            var appBrId1 = Guid.NewGuid();
            var appBrId2 = Guid.NewGuid();
            var appBrId3 = Guid.NewGuid();
            var appBrId4 = Guid.NewGuid();
            var appBrId5 = Guid.NewGuid();
            var appBrId6 = Guid.NewGuid();
            var appBrId7 = Guid.NewGuid();
            var appBrId8 = Guid.NewGuid();
            var appBrId9 = Guid.NewGuid();
            var appBrId10 = Guid.NewGuid();
            var appBrId11 = Guid.NewGuid();
            var appBrId12 = Guid.NewGuid();
            var appBrId13 = Guid.NewGuid();
            var appBrId14 = Guid.NewGuid();
            var appBrId15 = Guid.NewGuid();
            _microAppRepository.Context.BulkInsert(
         new[] { new AppBranch {Id=appBrId1, AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="参考咨询", Remark="参考咨询",Version="3.0.0"}
                ,new AppBranch {Id=appBrId2, AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="阅读记录", Remark="阅读记录",Version="3.0.0"}
                ,new AppBranch {Id=appBrId3, AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="图书借阅", Remark="图书借阅",Version="3.0.0"}
                ,new AppBranch {Id=appBrId4, AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="离校申请", Remark="离校申请",Version="3.0.0"}
                ,new AppBranch {Id=appBrId5, AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="友位空间预约", Remark="友位空间预约",Version="3.0.0"}
                ,new AppBranch {Id=appBrId6, AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="活动报名", Remark="活动报名",Version="3.0.0"}
                ,new AppBranch {Id=appBrId7, AppId=appId7.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="问卷调查", Remark="问卷调查",Version="3.0.0"}
                ,new AppBranch {Id=appBrId8, AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="审批管理", Remark="审批管理",Version="3.0.0"}
                ,new AppBranch {Id=appBrId9, AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="报修管理", Remark="报修管理",Version="3.0.0"}
                ,new AppBranch {Id=appBrId10, AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="ERMS", Remark="ERMS",Version="3.0.0"}
                ,new AppBranch {Id=appBrId11, AppId=appId11.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="文献数据查重", Remark="文献数据查重",Version="3.0.0"}
                ,new AppBranch {Id=appBrId12, AppId=appId12.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="大数据分析", Remark="大数据分析",Version="3.0.0"}
                ,new AppBranch {Id=appBrId13, AppId=appId13.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="数据接口管理", Remark="数据接口管理",Version="3.0.0"}
                ,new AppBranch {Id=appBrId14, AppId=appId14.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="数据授权管理", Remark="数据授权管理",Version="3.0.0"}
                ,new AppBranch {Id=appBrId15, AppId=appId15.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="数据监控日志", Remark="数据监控日志",Version="3.0.0"}
                });
            //客户使用
            _microAppRepository.Context.BulkInsert(
        new[] { new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId1.ToString(),AppId=appId1.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId2.ToString(),AppId=appId2.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId3.ToString(),AppId=appId3.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId4.ToString(),AppId=appId4.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId5.ToString(),AppId=appId5.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId6.ToString(),AppId=appId6.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId7.ToString(),AppId=appId7.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId8.ToString(),AppId=appId8.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId9.ToString(),AppId=appId9.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId10.ToString(),AppId=appId10.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId11.ToString(),AppId=appId11.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId12.ToString(),AppId=appId12.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId13.ToString(),AppId=appId13.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId14.ToString(),AppId=appId14.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId15.ToString(),AppId=appId15.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=cusId }
                   });
            //入口
            _deploymentRepository.Context.BulkInsert(
         new[] { //参考咨询 1
                 new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="参考咨询",UseScene=1,VisitUrl="/consult/new" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="参考咨询",UseScene=2,VisitUrl="/Portal/ContentManage/ConsultManage.aspx" }
                //阅读记录 2
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="阅读记录",UseScene=1,VisitUrl="/user/viewrecord"}
                //图书借阅 3
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="图书借阅",UseScene=1,VisitUrl="/user/loan"}
                //离校申请 4
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId4.ToString(),AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="离校申请",UseScene=1,VisitUrl="/user/leaveschool"}
                //空间预约 5
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId5.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="空间预约",UseScene=1,VisitUrl="/wechatredirect?aid=1&code=99173AEFB0E9665556FF114A000D1245&readerid=&link=3" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId5.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="2",Name="空间预约",UseScene=2,VisitUrl="/core/microapps/link?redirecturl=html/space/spaceListTO.html&apptype=4" }
                //活动报名 6
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId6.ToString(),AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="活动报名",UseScene=1,VisitUrl="/microapps/index?redirecturl=%2Factivity-lecture" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId6.ToString(),AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="2",Name="活动报名",UseScene=2,VisitUrl="/core/microapps/link?redirecturl=/activity-manage/activityInfoList&apptype=5" }
                //问卷调查 7
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId7.ToString(),AppId=appId7.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="问卷调查",UseScene=1,VisitUrl="/microapps/index?redirecturl=%2Fquestionnaire" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId7.ToString(),AppId=appId7.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="2",Name="问卷调查",UseScene=2,VisitUrl="/core/microapps/link?redirecturl=/survey-manage/surveyManage&apptype=5" }
                //审批管理 8
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId8.ToString(),AppId=appId8.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="审批管理",UseScene=2,VisitUrl="/core/microapps/link?redirecturl=/approval-manage/approvalApplyManage&apptype=5" }
                //报修管理 9
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId9.ToString(),AppId=appId9.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="报修管理",UseScene=2,VisitUrl="/core/microapps/link?redirecturl=/repairs-manage/repairsManage&apptype=5" }
                //ERMS 10
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId10.ToString(),AppId=appId10.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="ERMS",UseScene=2,VisitUrl="/Asset/Purchase/Overview.aspx"}
                //数据查重 11
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId11.ToString(),AppId=appId11.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="数据查重",UseScene=2,VisitUrl="/Asset/AssetTool/ExamineRepeatManage.aspx"}
                //大数据分析 12
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId12.ToString(),AppId=appId12.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="大数据分析",UseScene=2,VisitUrl="/core/RealtimeData/Report?sceneId=1"}
                //数据接口管理 13
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId13.ToString(),AppId=appId13.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="数据接口管理",UseScene=2,VisitUrl="/core/Api/Index"}
                //数据授权管理 14
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId14.ToString(),AppId=appId14.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="数据授权管理",UseScene=2,VisitUrl="/core/AuthorityRule/List"}
                //数据监控日志 15
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId15.ToString(),AppId=appId15.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="数据监控日志",UseScene=2,VisitUrl="/core/DataApplication/ArticleUpdateLog"}
                });
        }

        /// <summary>
        /// 添加快应用中心跳转到2.2应用
        /// </summary>
        public void AddQuickAppForCqu()
        {
            //开发者标识
            var devId = "26857b56-e438-4ad5-afb4-9b9bc80aed30";
            //部署环境标识
            var deployeeId = "0e6558f8-da1a-4144-8877-decf9b07abd8";
            //客户标识
            var customerId = "1fc3b47e-76a8-4ab0-bc60-c43fd9a9a2a9";

            //文献传递，课程文献中心，读者阅读报告，调查问卷，科研专题库，查收查引
            var appId1 = Guid.NewGuid();
            var appId2 = Guid.NewGuid();
            var appId3 = Guid.NewGuid();
            var appId4 = Guid.NewGuid();
            var appId5 = Guid.NewGuid();
            var appId6 = Guid.NewGuid();
            // 插入MicroApplication表
            _microAppRepository.Context.BulkInsert(
          new[] {new MicroApplication { Id=appId1,Desc="文献传递",Name= "文献传递",Intro="文献传递", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg", PriceType=2,RecommendScore=10,
                    RouteCode="documentdelivery",ServiceType="2",Status=1,Terminal="1",UseScene="3",CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now}
                ,new MicroApplication { Id=appId2,Desc="课程文献中心",Name= "课程文献中心",Intro="课程文献中心", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",PriceType=2,RecommendScore=10,
                    RouteCode="coursecenter",ServiceType="2",Status=1,Terminal="1",UseScene="3",CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now}
                ,new MicroApplication { Id=appId3,Desc="读者阅读报告",Name= "读者阅读报告",Intro="读者阅读报告", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",PriceType=2,RecommendScore=10,
                    RouteCode="readreport",ServiceType="4",Status=1,Terminal="1",UseScene="1",CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now}
                ,new MicroApplication { Id=appId4,Desc="调查问卷",Name= "调查问卷",Intro="调查问卷", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",PriceType=2,RecommendScore=10,
                    RouteCode="questionnaire",ServiceType="4",Status=1,Terminal="1",UseScene="3",CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now}
                ,new MicroApplication { Id=appId5,Desc="科研专题库",Name= "科研专题库",Intro="科研专题库", AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",PriceType=2,RecommendScore=10,
                    RouteCode="scientific",ServiceType="2",Status=1,Terminal="1",UseScene="3",CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now}
                ,new MicroApplication { Id=appId6,Desc="查收查引",Name= "查收查引",Intro="查收查引",AdvisePrice=10000,DevId= devId, FreeTry=false,Icon="/app_icons/icon-donation.svg",PriceType=2,RecommendScore=10,
                    RouteCode="checkcitation",ServiceType="4",Status=1,Terminal="1",UseScene="3",CreatedTime=DateTimeOffset.Now,UpdatedTime=DateTimeOffset.Now, DeleteFlag=false}
                    });
            //分支数据
            var appBrId1 = Guid.NewGuid();
            var appBrId2 = Guid.NewGuid();
            var appBrId3 = Guid.NewGuid();
            var appBrId4 = Guid.NewGuid();
            var appBrId5 = Guid.NewGuid();
            var appBrId6 = Guid.NewGuid();
            _microAppRepository.Context.BulkInsert(
         new[] { new AppBranch {Id=appBrId1, AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="文献传递", Remark="文献传递",Version="3.0.0"}
                ,new AppBranch {Id=appBrId2, AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="课程文献中心", Remark="课程文献中心",Version="3.0.0"}
                ,new AppBranch {Id=appBrId3, AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="读者阅读报告", Remark="读者阅读报告",Version="3.0.0"}
                ,new AppBranch {Id=appBrId4, AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="调查问卷", Remark="调查问卷",Version="3.0.0"}
                ,new AppBranch {Id=appBrId5, AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="科研专题库", Remark="科研专题库",Version="3.0.0"}
                ,new AppBranch {Id=appBrId6, AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,DeployeeId=deployeeId,Icon="/app_icons/icon-donation.svg",IsMaster=false, Name="查收查引", Remark="查收查引",Version="3.0.0"}
                });
            //客户使用
            _microAppRepository.Context.BulkInsert(
        new[] { new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId1.ToString(),AppId=appId1.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=customerId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId2.ToString(),AppId=appId2.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=customerId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId3.ToString(),AppId=appId3.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=customerId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId4.ToString(),AppId=appId4.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=customerId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId5.ToString(),AppId=appId5.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=customerId }
               ,new CustomerAppUsage { Id = Guid.NewGuid(), CreatedTime = DateTimeOffset.UtcNow, AppBranchId=appBrId6.ToString(),AppId=appId6.ToString(),AuthType=1,BeginDate=DateTime.Today,ExpireDate=DateTime.Today.AddMonths(12),Status=1,CustomerId=customerId }
                   });
            //入口
            _deploymentRepository.Context.BulkInsert(
         new[] { //文献传递 1
                 new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="文献传递",UseScene=1,VisitUrl="/UserDelegate/Index?tid=1" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId1.ToString(),AppId=appId1.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="2",Name="文献传递",UseScene=2,VisitUrl="/Portal/ContentManage/LiteratureTransferList.aspx?state=-1" }
                //课程文献中心 2
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="课程文献中心",UseScene=1,VisitUrl="/assembly/assemblylist?tid=1"}
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId2.ToString(),AppId=appId2.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="2",Name="课程文献中心",UseScene=2,VisitUrl="/Portal/ContentManage/AsmObjectList.aspx?tid=1"}
                //读者阅读报告 3
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId3.ToString(),AppId=appId3.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="读者阅读报告",UseScene=1,VisitUrl="/readerreport/index.html?readerGroup=1&sceneType=4&sceneId=37&id=11&set=pc"}
                //调查问卷 4
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId4.ToString(),AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="调查问卷",UseScene=1,VisitUrl="/microapps/index?redirecturl=%2Fquestionnaire"}
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId4.ToString(),AppId=appId4.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="2",Name="调查问卷",UseScene=2,VisitUrl="/core/microapps/link?redirecturl=/survey-manage/surveyManage&apptype=5"}
                //科研专题库 5
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId5.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="科研专题库",UseScene=1,VisitUrl="/assembly/assemblylist?tid=2" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId5.ToString(),AppId=appId5.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="2",Name="科研专题库",UseScene=2,VisitUrl="/Portal/ContentManage/AsmObjectList.aspx?tid=2" }
                //查收查引 6
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId6.ToString(),AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="1",Name="查收查引",UseScene=1,VisitUrl="/UserDelegate/Index?tid=3" }
                ,new AppBranchEntryPoint { Id=Guid.NewGuid(),IsDefault=true,IsSystem=true,AppBranchId= appBrId6.ToString(),AppId=appId6.ToString(),CreatedTime=DateTimeOffset.UtcNow,Code="2",Name="查收查引",UseScene=2,VisitUrl="/Portal/ContentManage/DelegationObjectList.aspx?tid=3" }
                });
        }

        /// <summary>
        /// 添加应用更新日志
        /// </summary>
        public void AddTestAppLogs()
        {
            var appInfoList = new List<Information>();
            var appDynamicList = new List<AppDynamic>();
            var appBranch = _appBranchRepository.DetachedEntities.Where(c => !c.DeleteFlag).ToList();
            foreach (var item in appBranch)
            {
                var info = new Information
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTimeOffset.Now,
                    UpdatedTime = DateTimeOffset.Now,
                    DeleteFlag = false,
                    Status = (int)EnumInformationStatus.正常,
                    Title = $"{item.Name}已上线服务，欢迎试用！",
                    Content = $"{item.Name}已上线服务，欢迎试用！",
                    PublishDate = DateTime.Now
                };
                appInfoList.Add(info);
                var dynamic = new AppDynamic
                {
                    Id = Guid.NewGuid(),
                    DeleteFlag = false,
                    CreatedTime = DateTimeOffset.Now,
                    UpdatedTime = DateTimeOffset.Now,
                    InfoType = (int)EnumInfoType.应用动态,
                    InfoId = info.Id.ToString(),
                    AppId = item.AppId,
                    AppBranchId = item.Id.ToString(),
                    Version = item.Version
                };
                appDynamicList.Add(dynamic);
                var info1 = new Information
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTimeOffset.Now,
                    UpdatedTime = DateTimeOffset.Now,
                    DeleteFlag = false,
                    Status = (int)EnumInformationStatus.正常,
                    Title = $"{item.Name}使用教程",
                    Content = $"{item.Name}使用教程",
                    PublishDate = DateTime.Now
                };
                appInfoList.Add(info1);
                var dynamic1 = new AppDynamic
                {
                    Id = Guid.NewGuid(),
                    DeleteFlag = false,
                    CreatedTime = DateTimeOffset.Now,
                    UpdatedTime = DateTimeOffset.Now,
                    InfoType = (int)EnumInfoType.使用教程,
                    InfoId = info1.Id.ToString(),
                    AppId = item.AppId,
                    AppBranchId = item.Id.ToString(),
                    Version = item.Version
                };
                appDynamicList.Add(dynamic1);
            }
            _appDynamicInfoRepository.Context.BulkInsert(appInfoList);
            _appDynamicInfoRepository.Context.BulkInsert(appDynamicList);
        }


        #endregion
    }
}
