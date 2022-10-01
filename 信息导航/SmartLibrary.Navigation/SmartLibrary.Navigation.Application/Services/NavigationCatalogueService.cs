using DotNetCore.CAP;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Google.Protobuf.WellKnownTypes;
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
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Transactions;

namespace SmartLibrary.Navigation.Application.Services
{
    /// <summary>
    /// 导航目录
    /// </summary>
    public class NavigationCatalogueService : INavigationCatalogueService, IScoped
    {
        private ICapPublisher _CapPublisher;
        private IRepository<NavigationLableInfo> _LableRepository;
        private IRepository<NavigationColumn> _ColumnRepository;
        private IRepository<Content> _ContentRepository;
        private IRepository<NavigationColumnPermissions> _PermissionRepository;
        private IRepository<NavigationCatalogue> _CatalogueRepository;
        private INavigationSettingsService _SettingsService;
        private INavigationLableInfoService _LableService;
        private TenantInfo _TenantInfo;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="capPublisher"></param>
        /// <param name="lableRepository"></param>
        /// <param name="columnRepository"></param>
        /// <param name="contentRepository"></param>
        /// <param name="settingsService"></param>
        /// <param name="lableService"></param>
        /// <param name="permissionRepository"></param>
        /// <param name="catalogueRepository"></param>
        /// <param name="tenantInfo"></param>
        public NavigationCatalogueService(ICapPublisher capPublisher,
                                          IRepository<NavigationLableInfo> lableRepository,
                                          IRepository<NavigationColumn> columnRepository,
                                          IRepository<Content> contentRepository,
                                          INavigationSettingsService settingsService,
                                          INavigationLableInfoService lableService,
                                          IRepository<NavigationColumnPermissions> permissionRepository,
                                          IRepository<NavigationCatalogue> catalogueRepository,
                                          TenantInfo tenantInfo)
        {
            _CapPublisher = capPublisher;
            _LableRepository = lableRepository;
            _ColumnRepository = columnRepository;
            _ContentRepository = contentRepository;
            _SettingsService = settingsService;
            _LableService = lableService;
            //_contentService = contentService;
            _PermissionRepository = permissionRepository;
            _CatalogueRepository = catalogueRepository;
            _TenantInfo = tenantInfo;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<UserData> GetUserByKey(string userKey)
        {
            if (string.IsNullOrEmpty(userKey))
            {
                throw Oops.Oh("请登陆").StatusCode((int)HttpStatusCode.Forbidden);
            }
            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            StringValue request1 = new StringValue { Value = userKey ?? "" };
            UserData reply1 = new UserData();
            try
            {
                reply1 = await grpcClient1.GetUserByKeyAsync(request1);
                return reply1;
            }
            catch (Exception)
            {
                throw Oops.Oh("grpc调用异常");
            }
        }

        /// <summary>
        /// 添加信息导航目录 若是添加一级目录则ParentID传"0"
        /// </summary>
        /// <returns></returns>        
        public async Task<ApiResultInfoModel> AddNavigationCatalogue(NavigationCatalogueDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var maxSortIndex = 0;
            if (_CatalogueRepository.Count() > 0)
                maxSortIndex = _CatalogueRepository.Max(d => d.SortIndex);
            model.SortIndex = maxSortIndex + 1;
            model.Creator = _TenantInfo.UserKey;
            var user = await GetUserByKey(_TenantInfo.UserKey);
            model.CreatorName = user.Name;


            if (model.ParentID == "0")
            {
                model.PathCode = model.Id;
            }
            else
            {
                var parent = _CatalogueRepository.FirstOrDefault(d => d.Id == model.ParentID);
                if (parent == null)
                {
                    result.Succeeded = false;
                    result.Message = "父级不存在";
                    return result;
                }
                if (!string.IsNullOrEmpty(parent.AssociatedCatalog))
                {
                    result.Succeeded = false;
                    result.Message = "父级已关联其他目录，不允许添加子级";
                    return result;
                }
                model.PathCode = parent.PathCode + "_" + model.Id;
            }

            if (model.NavigationType != ((int)NavigationTypeEunm.Associated))
            {
                model.AssociatedCatalog = "";
            }
            if (!string.IsNullOrEmpty(model.AssociatedCatalog))
            {
                var associatedCata = _CatalogueRepository.FirstOrDefault(d => d.Id == model.AssociatedCatalog);
                if (associatedCata == null)
                {
                    result.Succeeded = false;
                    result.Message = "关联目录不存在";
                    return result;
                }
                if (!string.IsNullOrEmpty(associatedCata.AssociatedCatalog))
                {
                    result.Succeeded = false;
                    result.Message = "关联目录已关联其他目录，不允许再关联";
                    return result;
                }
            }
            var navigationCL = model.Adapt<NavigationCatalogue>();
            navigationCL.CreatedTime = DateTime.Now;
            var nagColumn = await _CatalogueRepository.InsertNowAsync(navigationCL);

            result.Result = nagColumn.Entity.Id;

            return result;
        }

        /// <summary>
        /// 修改信息导航目录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> UpdateNavigationCatalogue(NavigationCatalogueDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var navigationClOld = await _CatalogueRepository.Entities.AsNoTracking().FirstOrDefaultAsync(d => d.Id == model.Id);
            if (navigationClOld == null)
            {
                result.Succeeded = false;
                result.Message = "导航目录不存在！";
                return result;
            }
            if (model.ParentID == "0")
            {
                model.PathCode = model.Id;
            }
            else
            {
                var parent = _CatalogueRepository.FirstOrDefault(d => d.Id == model.ParentID);
                if (parent == null)
                {
                    result.Succeeded = false;
                    result.Message = "父级不存在";
                    return result;
                }
                if (!string.IsNullOrEmpty(parent.AssociatedCatalog))
                {
                    result.Succeeded = false;
                    result.Message = "父级已关联其他目录，不允许添加子级";
                    return result;
                }
                model.PathCode = parent.PathCode + "_" + model.Id;
            }
            if (model.NavigationType != ((int)NavigationTypeEunm.Associated))
            {
                model.AssociatedCatalog = "";
            }
            if (!string.IsNullOrEmpty(model.AssociatedCatalog))
            {
                var associatedCata = _CatalogueRepository.FirstOrDefault(d => d.Id == model.AssociatedCatalog);
                if (associatedCata == null)
                {
                    result.Succeeded = false;
                    result.Message = "关联目录不存在";
                    return result;
                }
                if (!string.IsNullOrEmpty(associatedCata.AssociatedCatalog))
                {
                    result.Succeeded = false;
                    result.Message = "关联目录已关联其他目录，不允许再关联";
                    return result;
                }
            }
            var navigationCL = model.Adapt<NavigationCatalogue>();
            navigationCL.UpdatedTime = DateTime.Now;
            var navigationColumn = await _CatalogueRepository.UpdateExcludeAsync(navigationCL, new[] { nameof(navigationCL.CreatedTime), nameof(navigationCL.TenantId), nameof(navigationCL.SortIndex) });
            //更新子级PathCode
            //furion此方法有误
            //_catalogueRepository.Where(d => d.PathCode.Contains(navigationClOld.PathCode + "_")).BatchUpdate(new NavigationCatalogue { UpdatedTime=DateTime.Now});
            await _CatalogueRepository.Context.BatchUpdate<NavigationCatalogue>()
                 .Set(b => b.PathCode, b => b.PathCode.Replace(navigationCL.PathCode, model.PathCode))
                 .Where(d => d.PathCode.Contains(navigationClOld.PathCode + "_")).ExecuteAsync();

            return result;
        }

        /// <summary>
        /// 获取信息导航目录
        /// </summary>
        /// <param name="catalogueID">目录ID</param>
        /// <returns></returns>
        public async Task<NavigationCatalogueDto> GetNavigationCatalogue(string catalogueID)
        {
            var result = _CatalogueRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && d.Id == catalogueID);
            if (result == null)
                return null;
            var item = result.Adapt<NavigationCatalogueDto>();
            return await Task.FromResult(item);
        }

        /// <summary>
        /// 获取信息导航目录列表
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        public async Task<List<NavigationCatalogueView>> GetNavigationCatalogueList(string columnID)
        {
            List<NavigationCatalogueView> result = new List<NavigationCatalogueView>();
            var allList = _CatalogueRepository.Entities.Where(d => !d.DeleteFlag && d.ColumnID == columnID).ToList();
            var parentList = allList.Where(d => d.ParentID == "0").OrderByDescending(d => d.SortIndex).ToList();
            int index = 1;
            foreach (var item in parentList)
            {
                var childList = new List<NavigationCatalogueView>();
                ////关联目录 则获取关联目录对应的子集
                //if (!string.IsNullOrEmpty(item.AssociatedCatalog))
                //{
                //    var associatedCatalog = _catalogueRepository.FirstOrDefault(d => d.Id == item.AssociatedCatalog);
                //    if (associatedCatalog != null)
                //    {
                //        var associatedAllChildList = _catalogueRepository.Where(d => !d.DeleteFlag && d.PathCode.Contains(associatedCatalog.PathCode)).ToList();
                //        childList = GetChildNavigationCatalogues(associatedCatalog.Id, associatedAllChildList);
                //    }
                //}
                //else
                childList = GetChildNavigationCatalogues(item.Id, allList);
                NavigationCatalogueView temp = new NavigationCatalogueView()
                {
                    IndexNum = index,
                    Id = item.Id,
                    Title = item.Title,
                    ParentID = item.ParentID,
                    Status = item.Status,
                    Creator = item.Creator,
                    CreatorName = item.CreatorName,
                    SortIndex = item.SortIndex,
                    CreateTime = item.CreatedTime.DateTime,
                    IsAssociated = string.IsNullOrEmpty(item.AssociatedCatalog),
                    ChildList = childList
                };
                result.Add(temp);
                index++;
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取子目录
        /// </summary>
        /// <param name="parentCataID"></param>
        /// <param name="allList"></param>
        /// <returns></returns>
        private List<NavigationCatalogueView> GetChildNavigationCatalogues(string parentCataID, List<NavigationCatalogue> allList)
        {
            List<NavigationCatalogueView> result = new List<NavigationCatalogueView>();
            var thisList = allList.Where(d => d.ParentID == parentCataID).OrderByDescending(d => d.SortIndex).ToList();
            int index = 1;
            foreach (var item in thisList)
            {
                var childList = new List<NavigationCatalogueView>();
                ////关联目录 则获取关联目录对应的子集
                //if (!string.IsNullOrEmpty(item.AssociatedCatalog))
                //{
                //    var associatedCatalog = _catalogueRepository.FirstOrDefault(d => d.Id == item.AssociatedCatalog);
                //    if (associatedCatalog != null)
                //    {
                //        var associatedAllChildList = _catalogueRepository.Where(d => !d.DeleteFlag && d.PathCode.Contains(associatedCatalog.PathCode)).ToList();
                //        childList = GetChildNavigationCatalogues(associatedCatalog.Id, associatedAllChildList);
                //    }
                //}
                //else
                childList = GetChildNavigationCatalogues(item.Id, allList);
                NavigationCatalogueView temp = new NavigationCatalogueView()
                {
                    IndexNum = index,
                    Id = item.Id,
                    Title = item.Title,
                    ParentID = item.ParentID,
                    Status = item.Status,
                    Creator = item.Creator,
                    CreatorName = item.CreatorName,
                    SortIndex = item.SortIndex,
                    CreateTime = item.CreatedTime.DateTime,
                    IsAssociated = string.IsNullOrEmpty(item.AssociatedCatalog),
                    ChildList = childList
                };
                result.Add(temp);
                index++;
            }
            return result;
        }

        /// <summary>
        /// 获取前台信息导航目录树
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <param name="isContent">是否显示内容</param>
        /// <returns></returns>
        public async Task<List<ProntNavigationCatalogue>> GetNavigationCatalogueListForPront(string columnID, bool isContent)
        {
            List<ProntNavigationCatalogue> result = new List<ProntNavigationCatalogue>();

            var allList = await _CatalogueRepository.DetachedEntities.Where(d => !d.DeleteFlag && d.ColumnID == columnID && d.Status)
                                .ToListAsync();
            var parentList = allList.Where(d => d.ParentID == "0").OrderByDescending(d => d.SortIndex).ToList();
            int index = 1;
            foreach (var item in parentList)
            {
                var childList = new List<ProntNavigationCatalogue>();

                //关联目录 则获取关联目录对应的子集
                if (!string.IsNullOrEmpty(item.AssociatedCatalog))
                {
                    var associatedCatalog = await _CatalogueRepository.DetachedEntities.FirstOrDefaultAsync(d => d.Id == item.AssociatedCatalog);
                    if (associatedCatalog != null)
                    {
                        var associatedAllChildList = await _CatalogueRepository.DetachedEntities.Where(d => !d.DeleteFlag && d.Status && d.PathCode.Contains(associatedCatalog.PathCode))
                                                            .ToListAsync();
                        childList = await GetChildNavigationCataloguesForFrontAsync(associatedCatalog.Id, associatedAllChildList, isContent);
                    }
                }
                else
                {
                    childList = await GetChildNavigationCataloguesForFrontAsync(item.Id, allList, isContent);
                }
                ProntNavigationCatalogue temp = new()
                {
                    ColumnID = item.ColumnID,
                    CatalogueID = item.Id,
                    Name = item.Title,
                    IsOpenNewWindow = item.IsOpenNewWindow,
                    ExternalLinks = item.ExternalLinks,
                    NavigationType = item.NavigationType,
                    CatalogueList = childList,
                    ContentList = isContent ? _ContentRepository.DetachedEntities.Where(d => !d.DeleteFlag && d.Status && d.CatalogueID == item.Id).ToDictionary(x => x.Id, y => y.Title).ToList() : null
                };
                result.Add(temp);
                index++;
            }

            return result;
        }

        /// <summary>
        /// 获取子目录
        /// </summary>
        /// <param name="parentCataID"></param>
        /// <param name="allList"></param>
        /// <param name="isContent"></param>
        /// <returns></returns>
        private async Task<List<ProntNavigationCatalogue>> GetChildNavigationCataloguesForFrontAsync(string parentCataID, List<NavigationCatalogue> allList, bool isContent)
        {
            List<ProntNavigationCatalogue> result = new List<ProntNavigationCatalogue>();

            var thisList = allList.Where(d => d.ParentID == parentCataID).OrderByDescending(d => d.SortIndex).ToList();
            int index = 1;
            foreach (var item in thisList)
            {
                var childList = new List<ProntNavigationCatalogue>();
                //关联目录 则获取关联目录对应的子集
                if (!string.IsNullOrEmpty(item.AssociatedCatalog))
                {
                    var associatedCatalog = await _CatalogueRepository.DetachedEntities.FirstOrDefaultAsync(d => d.Id == item.AssociatedCatalog);
                    if (associatedCatalog != null)
                    {
                        var associatedAllChildList = await _CatalogueRepository.DetachedEntities.Where(d => !d.DeleteFlag && d.Status && d.PathCode.Contains(associatedCatalog.PathCode)).ToListAsync();
                        childList = await GetChildNavigationCataloguesForFrontAsync(associatedCatalog.Id, associatedAllChildList, isContent);
                    }
                }
                else
                {
                    childList = await GetChildNavigationCataloguesForFrontAsync(item.Id, allList, isContent);
                }
                ProntNavigationCatalogue temp = new()
                {
                    ColumnID = item.ColumnID,
                    CatalogueID = item.Id,
                    Name = item.Title,
                    CatalogueList = childList,
                    ContentList = isContent ? _ContentRepository.DetachedEntities.Where(d => !d.DeleteFlag && d.Status && d.CatalogueID == item.Id).ToDictionary(x => x.Id, y => y.Title).ToList() : null
                };
                result.Add(temp);
                index++;
            }
            return result;
        }

        /// <summary>
        /// 获取全部信息导航目录树
        /// </summary>
        /// <param name="parentCataID">添加一级栏目则传"0"</param>
        /// <param name="columnID">栏目ID 传空则获取全部</param>
        /// <returns></returns>
        public async Task<List<NavigationCatalogueTreeView>> GetAllNavigationCatalogueTreeList(string parentCataID, string columnID = "")
        {
            List<NavigationCatalogueTreeView> result = new List<NavigationCatalogueTreeView>();

            //取全部非关联目录
            var allList = _CatalogueRepository.Entities.Where(d => !d.DeleteFlag && string.IsNullOrEmpty(d.AssociatedCatalog) && (string.IsNullOrEmpty(columnID) ? true : d.ColumnID == columnID)).ToList();
            if (parentCataID != "0" && !string.IsNullOrEmpty(parentCataID))
            {
                var excludeCataID = "";
                var parentCata = _CatalogueRepository.FirstOrDefault(d => !d.DeleteFlag && d.Id == parentCataID);
                if (parentCata != null)
                {
                    //当前目录层级
                    excludeCataID = parentCata.PathCode.Split('_')[0];
                    allList.RemoveAll(d => d.PathCode.Contains(excludeCataID));
                }
            }
            var parentList = allList.Where(d => d.ParentID == "0").OrderBy(d => d.ColumnID).ThenBy(d => d.PathCode.Length).ThenByDescending(d => d.SortIndex).ToList();
            foreach (var item in parentList)
            {
                var childList = new List<NavigationCatalogueTreeView>();
                ////关联目录 则获取关联目录对应的子集
                //if (!string.IsNullOrEmpty(item.AssociatedCatalog))
                //{
                //    var associatedCatalog = allList.FirstOrDefault(d => d.Id == item.AssociatedCatalog);
                //    if (associatedCatalog != null)
                //    {
                //        var associatedAllChildList = allList.Where(d => !d.DeleteFlag && d.PathCode.Contains(associatedCatalog.PathCode)).ToList();
                //        childList = GetAllChildNavigationCataloguesTree(associatedCatalog.Id, associatedAllChildList);
                //    }
                //}
                //else
                childList = GetAllChildNavigationCataloguesTree(item.Id, allList);
                NavigationCatalogueTreeView temp = new NavigationCatalogueTreeView()
                {
                    Id = item.Id,
                    Title = item.Title,
                    ParentID = item.ParentID,
                    ChildList = childList
                };
                result.Add(temp);
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取所有导航目录树
        /// </summary>
        /// <param name="parentCataID"></param>
        /// <param name="allList"></param>
        /// <returns></returns>
        private List<NavigationCatalogueTreeView> GetAllChildNavigationCataloguesTree(string parentCataID, List<NavigationCatalogue> allList)
        {
            List<NavigationCatalogueTreeView> result = new List<NavigationCatalogueTreeView>();
            var thisList = allList.Where(d => d.ParentID == parentCataID).OrderByDescending(d => d.SortIndex).ToList();
            foreach (var item in thisList)
            {
                var childList = new List<NavigationCatalogueTreeView>();
                ////关联目录 则获取关联目录对应的子集
                //if (!string.IsNullOrEmpty(item.AssociatedCatalog))
                //{
                //    var associatedCatalog = allList.FirstOrDefault(d => d.Id == item.AssociatedCatalog);
                //    if (associatedCatalog != null)
                //    {
                //        var associatedAllChildList = allList.Where(d => !d.DeleteFlag && d.PathCode.Contains(associatedCatalog.PathCode)).ToList();
                //        childList = GetAllChildNavigationCataloguesTree(associatedCatalog.Id, associatedAllChildList);
                //    }
                //}
                //else
                childList = GetAllChildNavigationCataloguesTree(item.Id, allList);
                NavigationCatalogueTreeView temp = new NavigationCatalogueTreeView()
                {
                    Id = item.Id,
                    Title = item.Title,
                    ParentID = item.ParentID,
                    ChildList = childList
                };
                result.Add(temp);
            }
            return result;
        }

        /// <summary>
        /// 批量下架/上架目录
        /// </summary>
        /// <param name="cataIDList"></param>
        /// <param name="isNormal"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> ChangeNavigationCatalogueStatus(string[] cataIDList, bool isNormal)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var item in cataIDList)
                    {
                        var cata = _CatalogueRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && d.Id == item);
                        if (result == null)
                        {
                            result.Succeeded = false;
                            result.Message = $"{item}:目录不存在";
                            return result;
                        }
                        cata.Status = isNormal;
                        cata.UpdatedTime = DateTime.Now;
                        await _CatalogueRepository.UpdateNowAsync(cata);
                        //子类
                        _CatalogueRepository.Context.BatchUpdate<NavigationCatalogue>()
                        .Set(b => b.Status, b => isNormal)
                        .Where(d => !d.DeleteFlag && d.PathCode.Contains(cata.PathCode)).Execute();
                        //关联目录
                        var associatedCata = _CatalogueRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && d.Id == cata.AssociatedCatalog);
                        if (associatedCata != null)
                        {
                            associatedCata.Status = isNormal;
                            associatedCata.UpdatedTime = DateTime.Now;
                            await _CatalogueRepository.UpdateNowAsync(associatedCata);
                            //关联目录子类
                            _CatalogueRepository.Context.BatchUpdate<NavigationCatalogue>()
                            .Set(b => b.Status, b => isNormal)
                            .Where(d => !d.DeleteFlag && d.PathCode.Contains(associatedCata.PathCode)).Execute();
                        }

                        //被关联目录
                        var beAssociatedCata = _CatalogueRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && d.AssociatedCatalog == cata.Id);
                        if (beAssociatedCata != null)
                        {
                            beAssociatedCata.Status = isNormal;
                            beAssociatedCata.UpdatedTime = DateTime.Now;
                            await _CatalogueRepository.UpdateNowAsync(beAssociatedCata);
                            //被关联目录子类
                            _CatalogueRepository.Context.BatchUpdate<NavigationCatalogue>()
                            .Set(b => b.Status, b => isNormal)
                            .Where(d => !d.DeleteFlag && d.PathCode.Contains(beAssociatedCata.PathCode)).Execute();
                        }
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    result.Succeeded = false;
                    result.Message = ex.Message;
                }
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 批量删除目录
        /// </summary>
        /// <param name="cataIDList"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNavigationCatalogue(string[] cataIDList)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            using (var tran = _CatalogueRepository.Database.BeginTransaction())
            {
                try
                {
                    foreach (var cataID in cataIDList)
                    {
                        var model = _CatalogueRepository.FindOrDefault(cataID);
                        if (model == null)
                        {
                            result.Succeeded = false;
                            result.Message = $"{cataID}:目录不存在";
                            return result;
                        }
                        model.DeleteFlag = true;
                        model.UpdatedTime = DateTime.Now;
                        await _CatalogueRepository.UpdateNowAsync(model);
                        //子类
                        //_catalogueRepository.Where(d => !d.DeleteFlag && d.PathCode.Contains(model.PathCode)).BatchUpdate(new NavigationCatalogue { DeleteFlag = true, UpdatedTime = DateTime.Now });
                        _CatalogueRepository.Context.BatchUpdate<NavigationCatalogue>()
                        .Set(b => b.DeleteFlag, b => true)
                        .Where(d => !d.DeleteFlag && d.PathCode.Contains(model.PathCode)).Execute();
                        //关联目录
                        var associatedCata = _CatalogueRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && d.Id == model.AssociatedCatalog);
                        if (associatedCata != null)
                        {
                            associatedCata.DeleteFlag = true;
                            associatedCata.UpdatedTime = DateTime.Now;
                            await _CatalogueRepository.UpdateNowAsync(associatedCata);
                            //关联目录子类
                            _CatalogueRepository.Context.BatchUpdate<NavigationCatalogue>()
                            .Set(b => b.DeleteFlag, b => true)
                            .Where(d => !d.DeleteFlag && d.PathCode.Contains(associatedCata.PathCode)).Execute();
                        }
                        //被关联目录
                        var beAssociatedCata = _CatalogueRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && d.AssociatedCatalog == model.Id);
                        if (beAssociatedCata != null)
                        {
                            beAssociatedCata.DeleteFlag = true;
                            beAssociatedCata.UpdatedTime = DateTime.Now;
                            await _CatalogueRepository.UpdateNowAsync(beAssociatedCata);
                            //被关联目录子类
                            _CatalogueRepository.Context.BatchUpdate<NavigationCatalogue>()
                            .Set(b => b.DeleteFlag, b => true)
                            .Where(d => !d.DeleteFlag && d.PathCode.Contains(beAssociatedCata.PathCode)).Execute();
                        }
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result.Succeeded = false;
                    result.Message = ex.Message;
                }
            }
            return result;
        }

        /// <summary>
        /// 弹窗输入序号排序
        /// </summary>
        /// <param name="sourceID">源ID</param>
        /// <param name="sortIndex">排序号</param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> SortModel(string sourceID, int sortIndex)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var sourceModel = _CatalogueRepository.FirstOrDefault(d => d.Id == sourceID);
            var sameLevel = _CatalogueRepository.Where(d => !d.DeleteFlag && d.ParentID == sourceModel.ParentID && d.ColumnID == sourceModel.ColumnID).OrderByDescending(d => d.SortIndex).ToList();
            if (sortIndex > sameLevel.Count())
            {
                result.Succeeded = false;
                result.Message = "排序号超出同级目录个数";
                return result;
            }
            var switchCata = sameLevel.Take(sortIndex).LastOrDefault();
            var sourceSortIndex = sourceModel.SortIndex;
            sourceModel.SortIndex = switchCata.SortIndex;
            //优先执行其他的再执行交换的
            _CatalogueRepository.Context.BatchUpdate<NavigationCatalogue>()
                .Set(d => d.SortIndex, d => d.SortIndex - 1)
                .Where(d => !d.DeleteFlag && d.SortIndex <= switchCata.SortIndex && d.SortIndex > sourceSortIndex).Execute();
            _CatalogueRepository.Update(sourceModel);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 拖动排序
        /// </summary>
        /// <param name="sourceID">源ID</param>
        /// <param name="targetCataID">目标ID</param>
        /// <param name="isUp">位置</param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> SortModel(string sourceID, string targetCataID, bool isUp)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var sourceModel = _CatalogueRepository.FirstOrDefault(d => d.Id == sourceID);
            var tagertModel = _CatalogueRepository.FirstOrDefault(d => d.Id == targetCataID);
            var sameLevel = _CatalogueRepository.Where(d => !d.DeleteFlag && d.ParentID == sourceModel.ParentID && d.ColumnID == sourceModel.ColumnID);



            //定义批量偏移的范围区间
            var maxIndex = isUp ? tagertModel.SortIndex : sourceModel.SortIndex;
            var minIndex = isUp ? sourceModel.SortIndex : tagertModel.SortIndex;

            //交换源和目标的序号
            sourceModel.SortIndex = tagertModel.SortIndex;
            //优先执行其他的再执行交换的

            int change = isUp ? -1 : 1;//上移则区间内都要下降（-1），下移则区间内都要上升（+1）
            _CatalogueRepository.Context.BatchUpdate<NavigationCatalogue>()
                .Set(d => d.SortIndex, d => d.SortIndex + change)
                .Where(d => !d.DeleteFlag && d.ParentID == d.ParentID && d.ColumnID == d.ColumnID && d.SortIndex >= minIndex && d.SortIndex <= maxIndex).Execute();
            _CatalogueRepository.Update(sourceModel);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取单条内容的操作日志
        /// </summary>
        /// <param name="cataID"></param>
        /// <returns></returns>
        public async Task<List<ContentProcessLog>> GetCatalogueProcessLog(string cataID)
        {
            List<ContentProcessLog> result = new List<ContentProcessLog>();
            //TODO 调用日志接口
            result.Add(new ContentProcessLog
            {
                EventName = "新增保存",
                Operator = "A974C116-2FA4-4445-BE8D-230BA83317F8",
                OperatorName = "测试管理员",
                NoteInfo = "",
                OperateTime = DateTime.Now.AddHours(-50)
            });
            result.Add(new ContentProcessLog
            {
                EventName = "修改提交",
                Operator = "A974C116-2FA4-4445-BE8D-230BA83317F8",
                OperatorName = "测试管理员",
                NoteInfo = "",
                OperateTime = DateTime.Now.AddHours(-48)
            });
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取场景导航目录信息
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <param name="topNum">导航目录条数</param>
        /// <param name="sortField">排序字段 SortIndex,CreatedTime</param>
        /// <param name="isAsc">是否升序</param>
        /// <param name="isOnlyParentCata">是否只取第一级</param>
        /// <returns></returns>
        public async Task<ProntScenesNavaigationView> GetProntScenesCatalogue(string columnID, int topNum, string sortField, bool isAsc, bool isOnlyParentCata = true)
        {



            var baseUri = await this.GetBaseUri("navigation");


            ProntScenesNavaigationView result = new ProntScenesNavaigationView();
            var column = _ColumnRepository.FirstOrDefault(d => !d.DeleteFlag && d.Id == columnID);
            if (column == null)
                return null;
            result.ColumnID = column.Id;
            result.ColumnName = column.Title;
            result.JumnpLink = $"{baseUri}/#/web_list?c_id={column.Id}";
            List<NavigationCatalogue> listCata = new List<NavigationCatalogue>();

            var allList = _CatalogueRepository.Entities.Where(d => !d.DeleteFlag && d.ColumnID == columnID).ToList();
            var parentList = allList.Where(d => d.ParentID == "0" && d.Status == true).ToList();
            foreach (var item in parentList)
            {
                //关联目录 则获取关联目录对应的子集
                if (!string.IsNullOrEmpty(item.AssociatedCatalog))
                {
                    var associatedCatalog = _CatalogueRepository.FirstOrDefault(d => d.Id == item.AssociatedCatalog);
                    if (associatedCatalog != null)
                    {
                        listCata.Add(associatedCatalog);
                        if (!isOnlyParentCata)
                        {
                            var associatedAllChildList = _CatalogueRepository.Where(d => !d.DeleteFlag && d.PathCode.Contains(associatedCatalog.PathCode)).ToList();
                            listCata.AddRange(GetChildNavigationCataloguesForScenes(associatedCatalog.Id, associatedAllChildList, isOnlyParentCata));
                        }
                    }
                }
                else
                {
                    listCata.Add(item);
                    if (!isOnlyParentCata)
                        listCata.AddRange(GetChildNavigationCataloguesForScenes(item.Id, allList, isOnlyParentCata));
                }
            }

            var listCataQuery = listCata.AsQueryable();
            if (!string.IsNullOrEmpty(sortField))
                listCata = listCataQuery.ApplyOrderCustomize(sortField, isAsc).ToList();
            else
                listCata = listCataQuery.ApplyOrderCustomize("SortIndex", isAsc).ToList();
            listCata = listCata.Take(topNum).ToList();
            List<ProntScenesNavaigationCatalogueView> listCataView = new List<ProntScenesNavaigationCatalogueView>();
            foreach (var item in listCata)
            {
                listCataView.Add(new ProntScenesNavaigationCatalogueView()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Cover = item.Cover,
                    NavigationType = item.NavigationType,
                    ExternalLinks = item.ExternalLinks,
                    IsOpenNewWindow = item.IsOpenNewWindow,
                    JumpLink = $"{baseUri}/#/web_list?c_id={column.Id}&cataID={item.Id}"
                }); ;
            }
            result.CatalogueList = listCataView;
            return (result);
        }

        /// <summary>
        /// 获取场景子目录
        /// </summary>
        /// <param name="parentCataID"></param>
        /// <param name="allList"></param>
        /// <param name="isOnlyParentCata"></param>
        /// <returns></returns>
        private List<NavigationCatalogue> GetChildNavigationCataloguesForScenes(string parentCataID, List<NavigationCatalogue> allList, bool isOnlyParentCata)
        {
            List<NavigationCatalogue> result = new List<NavigationCatalogue>();
            var thisList = allList.Where(d => d.ParentID == parentCataID).OrderByDescending(d => d.SortIndex).ToList();
            foreach (var item in thisList)
            {
                //关联目录 则获取关联目录对应的子集
                if (!string.IsNullOrEmpty(item.AssociatedCatalog))
                {
                    var associatedCatalog = _CatalogueRepository.FirstOrDefault(d => d.Id == item.AssociatedCatalog);
                    if (associatedCatalog != null)
                    {
                        result.Add(associatedCatalog);
                        if (isOnlyParentCata)
                        {
                            var associatedAllChildList = _CatalogueRepository.Where(d => !d.DeleteFlag && d.PathCode.Contains(associatedCatalog.PathCode)).ToList();
                            result.AddRange(GetChildNavigationCataloguesForScenes(associatedCatalog.Id, associatedAllChildList, isOnlyParentCata));
                        }
                    }
                }
                else
                {
                    result.Add(item);
                    if (isOnlyParentCata)
                        result.AddRange(GetChildNavigationCataloguesForScenes(item.Id, allList, isOnlyParentCata));
                }
            }
            return result;
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
        /// 根据目录id获取导航
        /// </summary>
        /// <param name="colId"></param>
        /// <returns></returns>
        public List<NavigationCatalogueDto> GetCataListByColId(string colId)
        {
            var list = _CatalogueRepository.Where(e => !e.DeleteFlag && (e.ColumnID == colId)).OrderByDescending(e => e.SortIndex).ProjectToType<NavigationCatalogueDto>().ToList();
            return list;
        }
    }
}
