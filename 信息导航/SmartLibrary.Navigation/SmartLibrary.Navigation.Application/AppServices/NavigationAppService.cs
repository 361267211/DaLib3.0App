using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibrary.Navigation.Application.PageParam;
using SmartLibrary.Navigation.Application.Services;
using SmartLibrary.Navigation.Application.ViewModel;
using SmartLibrary.Navigation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.DatabaseTerrace.Application.Filter;
using SmartLibrary.User.RpcService;
using SmartLibrary.Navigation.Application.Filter;
using SmartLibrary.Navigation.Application.Services.RemoteProxy;
using SmartLibrary.Navigation.Application.Attributes;
using SmartLibrary.Navigation.Common.Const;
using Furion;
using SmartLibrary.Navigation.Application.Interceptors;

namespace SmartLibrary.Navigation.Application.AppServices
{
    /// <summary>
    /// 名    称：信息导航
    /// </summary>
    public class NavigationAppService : IDynamicApiController
    {
        private readonly IDistributedCache _cache;
        private readonly INavigationSettingsService _settingsService;
        private readonly IUserCenterService _userCenterService;
        private readonly INavigationLableInfoService _lableInfoService;
        private readonly INavigationTemplateService _templateService;
        private readonly INavigationColumnService _columnService;
        private readonly INavigationCatalogueService _catalogueService;
        private readonly IContentService _contentService;
        public IConfiguration _configuration { get; }
        private readonly IHttp _http;

        public NavigationAppService(IContentService contentService,
                                    INavigationLableInfoService lableInfoService,
                                    INavigationSettingsService settingsService,
                                    INavigationTemplateService templateService,
                                    INavigationColumnService columnService,
                                    INavigationCatalogueService catalogueService,
                                    IUserCenterService userCenterService,
                                    IConfiguration configuration,
                                    IDistributedCache cache,
                                    IHttp http)
        {
            _configuration = configuration;
            _lableInfoService = lableInfoService;
            _settingsService = settingsService;
            _templateService = templateService;
            _columnService = columnService;
            _catalogueService = catalogueService;
            _userCenterService = userCenterService;
            _contentService = contentService;
            _cache = cache;
            _http = http;
        }

        #region 应用设置&栏目权限设置
        /// <summary>
        /// 保存应用设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationSettingsSave(NavigationSettingsDto model)
        {
            return await _settingsService.SaveNavigationSettings(model);
        }

        /// <summary>
        /// 获取应用设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<NavigationSettingsDto> NavigationSettingsGet()
        {
            return await _settingsService.GetNavigationSettings();
        }

        /// <summary>
        /// 保存栏目权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> SaveNavigationColumnPermissions(NavigationColumnPermissionsParam model)
        {
            return await _settingsService.SaveNavigationColumnPermissions(model);
        }

        /// <summary>
        /// 获取全部栏目及其管理者
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<NavigationColumnPermissionsView>> NavigationColumnPermissionsListGet()
        {
            return await _settingsService.GetNavigationColumnPermissionsList();
        }

        /// <summary>
        /// 检索获取权限管理员
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<PermissionManagerInfo>> SearchPermissionManager([FromBody] string searchKey)
        {
            return await _settingsService.SearchPermissionManager(searchKey ?? "");
        }

        #endregion

        #region 标签管理
        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<LableUpdateParm>> LableListGet()
        {
            return await _lableInfoService.GetLableList();
        }

        /// <summary>
        /// 保存标签 
        /// </summary>
        /// <param name="updateParmList">原有标签需要一起提交否则将被删除</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> LableInfoSave(List<LableUpdateParm> updateParmList)
        {
            return await _lableInfoService.SaveLableInfo(updateParmList);
        }
        #endregion

        #region 信息导航模板管理
        /// <summary>
        /// 添加信息导航模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationTemplateAdd(NavigationTemplateDto model)
        {
            return await _templateService.AddNavigationTemplate(model);
        }

        /// <summary>
        /// 获取信息导航模板
        /// </summary>
        /// <param name="tempIDs"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<NavigationTemplateDto>> NavigationTemplateGet([FromBody] string[] tempIDs)
        {
            return await _templateService.GetNavigationTemplate(tempIDs);
        }

        /// <summary>
        /// 获取单个信息导航模板
        /// </summary>
        /// <param name="tempID"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<NavigationTemplateDto> SingleNavigationTemplateGet([FromBody] string tempID)
        {
            return await _templateService.GetSingleNavigationTemplate(tempID);
        }

        /// <summary>
        /// 更新信息导航模板
        /// </summary>
        /// <param name="templateDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationTemplateUpdate(NavigationTemplateDto templateDto)
        {
            return await _templateService.UpdateNavigationTemplate(templateDto);
        }

        /// <summary>
        /// 删除信息导航模板
        /// </summary>
        /// <param name="tempID"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationTemplateDelete([FromBody] string tempID)
        {
            return await _templateService.DeleteNavigationTemplate(tempID);
        }

        /// <summary>
        /// 信息导航头尾模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //  [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<HeadFootTemplateListView> AllHeadFootTemplateGet()
        {
            return await _templateService.AllHeadFootTemplateGet();
        }

        /// <summary>
        /// 根据栏目id获取栏目的头尾模板详情
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        [HttpGet]
        //  [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<HeadFootTemplateDetailView> HeadFootTemplateDetailGetByColumnId([ApiSeat(ApiSeats.ActionEnd)] string columnId)
        {
            var column = await _columnService.GetNavigationColumn(columnId);
            return await _templateService.HeadFootTemplateDetailGet(column.HeadTemplate, column.FootTemplate);
        }


        #region 信息导航头尾模板
        /// <summary>
        /// 添加信息导航头尾模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationBodyTemplateAdd(NavigationBodyTemplateDto model)
        {
            return await _templateService.AddNavigationBodyTemplate(model);
        }

        /// <summary>
        /// 获取信息导航头尾模板
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<NavigationBodyTemplateDto>> NavigationBodyTemplateGetByType([FromBody] int type)
        {
            return await _templateService.GetNavigationBodyTemplate(type);
        }

        /// <summary>
        /// 获取信息导航默认模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<List<NavigationBodyTemplateDto>> NavigationBodyTemplateGet()
        {
            return _templateService.GetNavigationBodyTemplate();
        }

        /// <summary>
        /// 更新信息导航头尾模板
        /// </summary>
        /// <param name="bodyTemplateDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationBodyTemplateUpdate(NavigationBodyTemplateDto bodyTemplateDto)
        {
            return await _templateService.UpdateNavigationBodyTemplate(bodyTemplateDto);
        }

        /// <summary>
        /// 删除信息导航头尾模板
        /// </summary>
        /// <param name="tempBodyID"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationBodyTemplateDelete([FromBody] string tempBodyID)
        {
            return await _templateService.DeleteNavigationBodyTemplate(tempBodyID);
        }
        #endregion
        #endregion

        #region 信息导航栏目


        /// <summary>
        /// 取前台栏目跳转的的信息
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        [QueryParameters]
        public async Task<GetColumnLinkInfoReply> GetColumnLinkInfo(string columnId)
        {
            return await _columnService.GetColumnLinkInfo(columnId);
        }
        /// <summary>
        /// 获取标签分组及信息导航栏目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<LableNavigationColumnView>> NavigationColumnGetByManagerID()
        {
            return await _columnService.GetLableNavigationColumnList();
        }

        /// <summary>
        /// 获取信息导航栏目
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<NavigationColumnDto> NavigationColumnGet([ApiSeat(ApiSeats.ActionEnd)] string columnID)
        {
            return await _columnService.GetNavigationColumn(columnID);
        }

        /// <summary>
        /// 新增信息导航栏目
        /// </summary>
        /// <param name="navigationColumn"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [ColumnChange]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationColumnAdd([FromBody] NavigationColumnParam navigationColumn)
        {
            NavigationColumnDto navigationColumnDto = new NavigationColumnDto
            {
                Title = navigationColumn.Title,
                Label = navigationColumn.Label,
                Status = navigationColumn.Status,
                LinkUrl = navigationColumn.LinkUrl,
                DefaultTemplate = navigationColumn.DefaultTemplate,
                ColumnIcon = navigationColumn.ColumnIcon,
                SideList = navigationColumn.SideList,
                SysMesList = navigationColumn.SysMesList,
                CoverHeight = navigationColumn.CoverHeight,
                CoverWidth = navigationColumn.CoverWidth,
                IsLoginAcess = navigationColumn.IsLoginAcess,
                IsOpenFeedback = navigationColumn.IsOpenFeedback,
                HeadTemplate = navigationColumn.HeadTemplate,
                FootTemplate = navigationColumn.FootTemplate,
                UserGroups = navigationColumn.UserGroups,
                UserTypes = navigationColumn.UserTypes,
            };
            navigationColumnDto.Id = Time2KeyUtils.GetRandOnlyId();

            return await _columnService.AddNavigationColumn(navigationColumnDto);
        }

        /// <summary>
        /// 更新信息导航栏目
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <param name="navigationColumn"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationColumnUpdate([ApiSeat(ApiSeats.ActionEnd)] string columnID, NavigationColumnParam navigationColumn)
        {
            NavigationColumnDto navigationColumnDto = new NavigationColumnDto
            {
                //Id = navigationColumn.Id,
                Id = columnID,
                Title = navigationColumn.Title,
                Label = navigationColumn.Label,
                Status = navigationColumn.Status,
                LinkUrl = navigationColumn.LinkUrl,
                DefaultTemplate = navigationColumn.DefaultTemplate,
                ColumnIcon = navigationColumn.ColumnIcon,
                SideList = navigationColumn.SideList,
                SysMesList = navigationColumn.SysMesList,
                CoverHeight = navigationColumn.CoverHeight,
                CoverWidth = navigationColumn.CoverWidth,
                IsLoginAcess = navigationColumn.IsLoginAcess,
                IsOpenFeedback = navigationColumn.IsOpenFeedback,
                HeadTemplate = navigationColumn.HeadTemplate,
                FootTemplate = navigationColumn.FootTemplate,
                UserGroups = navigationColumn.UserGroups,
                UserTypes = navigationColumn.UserTypes,
            };
            return await _columnService.UpdateNavigationColumn(navigationColumnDto);
        }

        /// <summary>
        /// 删除信息导航栏目
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationColumnDelete([ApiSeat(ApiSeats.ActionEnd)] string columnID)
        {
            return await _columnService.DeleteNavigationColumn(columnID);
        }

        /// <summary>
        /// 获取前台信息导航栏目数据
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<List<ProntNavigationColumnListView>> ProntNavigationColumnListGet([FromQuery] string columnID)
        {
            var result = _columnService.GetProntNavigationColumnList(columnID);
            return result;
        }
        #endregion

        #region 目录管理

        /// <summary>
        /// 根据关键词查找内容
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        [QueryParameters]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<PagedList<ContentVo>> NavigationContentSearch(string keyWord, int pageIndex, int pageSize)
        {
            var navContentList = await _contentService.SearchNavigationContent(keyWord, pageIndex, pageSize);
            return navContentList;
        }


        /// <summary>
        /// 添加信息导航目录
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationCatalogueAdd([ApiSeat(ApiSeats.ActionEnd)] string columnID, NavigationCatalogueParam param)
        {
            string titleStyle = string.Join(";", param.TitleStyleKV.Select(d => d.Key + "-" + d.Value));
            NavigationCatalogueDto model = new NavigationCatalogueDto
            {
                Id = Time2KeyUtils.GetRandOnlyId(),
                ColumnID = param.ColumnID,
                Title = param.Title,
                TitleStyle = titleStyle,
                Alias = param.Alias,
                ParentID = param.ParentID,
                NavigationType = param.NavigationType,
                AssociatedCatalog = param.AssociatedCatalog,
                ExternalLinks = param.ExternalLinks,
                IsOpenNewWindow = param.IsOpenNewWindow,
                Cover = param.Cover,
                Status = param.Status
            };
            return await _catalogueService.AddNavigationCatalogue(model);
        }

        /// <summary>
        /// 更新信息导航目录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationCatalogueUpdate([ApiSeat(ApiSeats.ActionEnd)] string columnID, NavigationCatalogueParam param)
        {
            string titleStyle = string.Join(";", param.TitleStyleKV.Select(d => d.Key + "-" + d.Value));
            NavigationCatalogueDto model = new NavigationCatalogueDto
            {
                Id = param.Id,
                ColumnID = param.ColumnID,
                Title = param.Title,
                TitleStyle = titleStyle,
                Alias = param.Alias,
                ParentID = param.ParentID,
                NavigationType = param.NavigationType,
                AssociatedCatalog = param.AssociatedCatalog,
                ExternalLinks = param.ExternalLinks,
                IsOpenNewWindow = param.IsOpenNewWindow,
                Cover = param.Cover,
                Status = param.Status,
                Creator = param.Creator,
                CreatorName = param.CreatorName
            };
            return await _catalogueService.UpdateNavigationCatalogue(model);
        }

        /// <summary>
        /// 获取信息导航目录
        /// </summary>
        /// <param name="catalogueID">目录ID</param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<NavigationCatalogueDto> NavigationCatalogueGet([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromQuery] string catalogueID)
        {
            return await _catalogueService.GetNavigationCatalogue(catalogueID);
        }

        /// <summary>
        /// 获取信息导航目录列表
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<NavigationCatalogueView>> NavigationCatalogueListGet([ApiSeat(ApiSeats.ActionEnd)] string columnID)
        {
            return await _catalogueService.GetNavigationCatalogueList(columnID);
        }

        /// <summary>
        /// 获取全部信息导航目录树
        /// </summary>
        /// <param name="catalogueTreeParam"></param>
        /// <returns></returns>
        [HttpPost]
        //     [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<NavigationCatalogueTreeView>> AllNavigationCatalogueTreeListGet([ApiSeat(ApiSeats.ActionEnd)] string columnID, CatalogueTreeParam catalogueTreeParam)
        {
            return await _catalogueService.GetAllNavigationCatalogueTreeList(catalogueTreeParam.ParentCataID, catalogueTreeParam.ColumnID);
        }

        /// <summary>
        /// 批量下架/上架目录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> ChangeNavigationCatalogueStatus([ApiSeat(ApiSeats.ActionEnd)] string columnID, ChangeNavigationCatalogueStatusParam param)
        {
            return await _catalogueService.ChangeNavigationCatalogueStatus(param.CataIDList, param.IsNormal);
        }

        /// <summary>
        /// 批量删除目录
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="cataIDList">ID集合</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NavigationCatalogueDelete([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromBody] string[] cataIDList)
        {
            return await _catalogueService.DeleteNavigationCatalogue(cataIDList);
        }

        /// <summary>
        /// 目录弹窗输入序号排序
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="parma">SourceID:源ID,SortIndex:排序号</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<ApiResultInfoModel> SortCatalogueByIndex([ApiSeat(ApiSeats.ActionEnd)] string columnID, SortContentByIndexParam parma)
        {
            var result = _catalogueService.SortModel(parma.SourceID, parma.SortIndex);
            return result;
        }

        /// <summary>
        /// 目录拖动排序
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="parma">SourceID:源ID,TargetCataID:目标ID, isUp:位置</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<ApiResultInfoModel> SortCatalogueByTarget([ApiSeat(ApiSeats.ActionEnd)] string columnID, SortContentByTargetParam parma)
        {
            var result = _catalogueService.SortModel(parma.SourceID, parma.TargetCataID, parma.IsUp);
            return result;
        }

        /// <summary>
        /// 获取目录操作日志
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="cataID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<ContentProcessLog>> CatalogueProcessLogGet([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromQuery] string cataID)
        {
            var result = await _catalogueService.GetCatalogueProcessLog(cataID);
            return result;
        }

        /// <summary>
        /// 获取场景导航目录信息
        /// </summary>
        /// <param name="prontScenes"></param>
        /// <returns></returns>
        [HttpPost]
        //[AuthorizeMultiplePolicy(PolicyKey.UnAuthKey, false)]

        public async Task<List<ProntScenesNavaigationView>> GetProntScenesCatalogue([FromBody] List<ProntSceneCatalogueParam> prontScenes)
        {

            //判断是否登陆
            var userKey = App.User.FindFirst("UserKey")?.Value;

            UserData userData = new UserData();
            if (!string.IsNullOrEmpty(userKey))
            {
                userData = await App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>().GetUserByKeyAsync(new Google.Protobuf.WellKnownTypes.StringValue { Value = userKey });

            }



            //将授权通过的栏目组装展示
            var reply = new List<ProntScenesNavaigationView>();
            foreach (var item in prontScenes)
            {


                var sortFieldArray = item.SortField.Split('-');
                var realSortField = "";
                bool IsAsc = false;
                if (sortFieldArray.Length == 2)
                {
                    realSortField = sortFieldArray[0];
                    IsAsc = sortFieldArray[1].ToLower() == "asc";
                }




                var result = await _catalogueService.GetProntScenesCatalogue(item.ColumnId, item.Count, realSortField, IsAsc);

                //校验当前用户是否有该栏目的权限
                var column = await _columnService.GetNavigationColumn(item.ColumnId);
                var okUserGroup = column.UserGroups.Any(e => userData.GroupIds.Contains(e));
                var okUserType = column.UserTypes.Any(e => userData.Type == e);
                if ((!okUserGroup && !okUserType) && (column.UserTypes.Count != 0 || column.UserGroups.Count != 0))
                {
                    result.CatalogueList = new List<ProntScenesNavaigationCatalogueView>();
                }

                reply.Add(result);
            }
            return reply;
        }

        /// <summary>
        /// 取用户分组的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [QueryParameters]
        //  [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<DictItem>> GetUserGroupsList(int pageIndex, int pageSize, string keyWord)
        {
            List<DictItem> result = await _userCenterService.GetUserGroupsList(pageIndex, pageSize, keyWord);
            return result;
        }

        /// <summary>
        /// 取用户分组的列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [QueryParameters]
        //  [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<DictItem>> GetUserTypesList(int pageIndex, int pageSize, string keyWord)
        {
            List<DictItem> result = await _userCenterService.GetUserTypesList(pageIndex, pageSize, keyWord);
            return result;
        }
        #endregion

        #region 内容管理
        /// <summary>
        /// 添加信息导航内容
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> ContentAdd([ApiSeat(ApiSeats.ActionEnd)] string columnID, ContentParam param)
        {
            string titleStyle = string.Join(";", param.TitleStyleKV.Select(d => d.Key + "-" + d.Value));
            ContentDto model = new ContentDto
            {
                Id = Time2KeyUtils.GetRandOnlyId(),
                Title = param.Title,
                TitleStyle = titleStyle,
                SubTitle = param.SubTitle,
                CatalogueID = param.CatalogueID,
                RelationCatalogueIDs = param.RelationCatalogueIDs,
                Contents = param.Contents,
                LinkUrl = param.LinkUrl,
                Publisher = param.Publisher,
                PublishDate = param.PublishDate,
                Status = param.Status
            };
            return await _contentService.AddContent(model, columnID);
        }

        /// <summary>
        /// 修改信息导航内容
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> ContentUpdate([ApiSeat(ApiSeats.ActionEnd)] string columnID, ContentParam param)
        {
            string titleStyle = string.Join(";", param.TitleStyleKV.Select(d => d.Key + "-" + d.Value));
            ContentDto model = new ContentDto
            {
                Id = param.Id,
                Title = param.Title,
                TitleStyle = titleStyle,
                SubTitle = param.SubTitle,
                CatalogueID = param.CatalogueID,
                RelationCatalogueIDs = param.RelationCatalogueIDs,
                Contents = param.Contents,
                LinkUrl = param.LinkUrl,
                Publisher = param.Publisher,
                PublishDate = param.PublishDate,
                Status = param.Status
            };
            return await _contentService.UpdateContent(model, columnID);
        }

        /// <summary>
        /// 获取信息导航内容
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="contentID">内容ID</param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ContentDto> GetContent([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromQuery] string contentID)
        {
            return await _contentService.GetContent(contentID);
        }

        /// <summary>
        /// 获取信息导航内容列表
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<PagedList<ContentDto>> GetContentList([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromBody] ManageContentPageParam pageParam)
        {
            return await _contentService.GetContentList(pageParam.PageIndex, pageParam.PageSize, pageParam.ColumnID, pageParam.Keywords, pageParam.CataID, pageParam.Status);
        }

        /// <summary>
        /// 批量下架/上架内容
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> ChangeContentStatus([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromBody] ChangeContentStatusParam param)
        {
            return await _contentService.ChangeContentStatus(param.ContentIDList, param.IsNormal);
        }

        /// <summary>
        /// 批量删除内容
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="contentIDList">内容ID集合</param>
        /// <returns></returns>
        [HttpDelete]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> ContentDelete([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromBody] string[] contentIDList)
        {
            return await _contentService.DeleteContent(contentIDList);
        }


        /// <summary>
        /// 内容弹窗输入序号排序
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="parma">SourceID:源ID,SortIndex:排序号</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> SortContentByIndex([ApiSeat(ApiSeats.ActionEnd)] string columnID, SortContentByIndexParam parma)
        {
            var result = await _contentService.SortModel(parma.SourceID, parma.SortIndex);
            return result;
        }

        /// <summary>
        /// 内容拖动排序
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="parma">SourceID:源ID,TargetCataID:目标ID, isUp:位置</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> SortContentByTarget([ApiSeat(ApiSeats.ActionEnd)] string columnID, SortContentByTargetParam parma)
        {
            var result = await _contentService.SortModel(parma.SourceID, parma.TargetCataID, parma.IsUp);
            return result;
        }

        /// <summary>
        /// 获取前台列表内容数据
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [PermissionObjAttribute("ColumnId")]
        [AuthorizeMultiplePolicy(PolicyKey.PortalColumn, false)]
        public async Task<PagedList<FrontContentListView>> GetProntContentListData([ApiSeat(ApiSeats.ActionEnd)] string columnID, FrontContentListParm parm)
        {
            var result = await _contentService.GetProntContentListData(parm);
            return result;
        }

        /// <summary>
        /// 获取前台内容数据
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="contentID">内容ID</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [PermissionObjAttribute("ColumnId")]

        //  [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<FrontContentView> GetProntContent([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromBody] string contentID)
        {
            var result = await _contentService.GetProntContent(contentID);
            return result;
        }

        /// <summary>
        /// 获取内容操作日志
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="contentID"></param>
        /// <returns></returns>
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<ContentProcessLog>> ContentProcessLogGet([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromBody] string contentID)
        {
            var result = await _contentService.GetContentProcessLog(contentID);
            return result;
        }
        #endregion

        #region 重大首页拓展
        public async Task<FootLinkView> GetLinkList()
        {
            FootLinkView res = new FootLinkView();

            //专题网站
            var ZTWZ = await _columnService.GetPlatListAsync(plateSign: "ZTWZ", count: 50, itemType: 0);
            //快速链接
            var KSLJ = await _columnService.GetPlatListAsync(plateSign: "KSLJ", count: 50, itemType: 0);

            var url = await _columnService.GetAllianceCertifyUrlAsync();

            res.AssemblyLink = ZTWZ;
            res.FastLink = KSLJ;
            res.AllianceCertifyUrl = url;
            return res;
        }
        #endregion
    }
}
