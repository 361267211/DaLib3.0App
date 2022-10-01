using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.PageParam;
using SmartLibrary.News.Application.ViewModel;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using SmartLibrary.News.Utility;
using SmartLibraryNews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using SmartLibrary.News.Application.Filter;
using SmartLibrary.News.Application.Interceptors;
using SmartLibraryWorkbench;
using DotNetCore.CAP;
using SmartLibrary.Search.Service;
using Furion;
using System.Security.Claims;
using Furion.FriendlyException;
using SmartLibrary.News.Common.Const;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartLibrary.News.Application.Attributes;

namespace SmartLibrary.News.Application.AppServices
{
    /// <summary>
    /// 新闻
    /// </summary>
    public class NewsAppService : IDynamicApiController
    {
        private readonly IDistributedCache _cache;
        private readonly ILableInfoService _lableInfoService;
        private readonly INewsSettingsService _settingsService;
        private readonly INewsTemplateService _templateService;
        private readonly INewsColumnService _columnService;
        private readonly INewsContentService _contentService;
        private readonly IGrpcClientResolver _grpcClient;
        private readonly ICapPublisher _capPublisher;
        private readonly IHttpContextAccessor _httpContext;
        public IConfiguration _configuration { get; }


        public NewsAppService(INewsContentService newsService,
            ILableInfoService lableInfoService,
            INewsSettingsService settingsService,
            INewsTemplateService templateService,
            INewsColumnService columnService,
            IConfiguration configuration,
            IGrpcClientResolver grpcClient,
             ICapPublisher capPublisher,
        IDistributedCache cache)
        {
            _configuration = configuration;
            _lableInfoService = lableInfoService;
            _settingsService = settingsService;
            _templateService = templateService;
            _columnService = columnService;
            _contentService = newsService;
            _cache = cache;
            _grpcClient = grpcClient;
            _capPublisher = capPublisher;
        }

        #region 应用设置&栏目权限设置
        /// <summary>
        /// 保存应用设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsSettingsSave(NewsSettingsDto model)
        {
            return await _settingsService.SaveNewsSettings(model);
        }

        /// <summary>
        /// 获取应用设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<NewsSettingsDto> NewsSettingsGet()
        {
            return await _settingsService.GetNewsSettings();
        }

        /// <summary>
        /// 保存栏目权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> SaveNewsColumnPermissions(NewsColumnPermissionsParam model)
        {
            return await _settingsService.SaveNewsColumnPermissions(model);
        }

        /// <summary>
        /// 获取栏目某一权限管理员列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<ManagerParam>> NewsColumnPermissionsGet([FromBody] SingleColumnPermissionParam param)
        {
            return await _settingsService.GetNewsColumnPermissionsByColumnPerm(param.ColumnID, param.Permission);
        }

        /// <summary>
        /// 获取栏目对应的权限设置
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        [HttpGet]
        //      [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<NewsSingleColumnPermissionsView>> NewsColumnPermissionsByColumnIDGet([FromQuery] string columnID)
        {
            return await _settingsService.GetNewsColumnPermissionsByColumnID(columnID);
        }

        /// <summary>
        /// 获取全部栏目及其管理者
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<NewsColumnPermissionsView>> NewsColumnPermissionsListGet()
        {
            return await _settingsService.GetNewsColumnPermissionsList();
        }
        /// <summary>
        /// 检索获取权限管理员
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<PermissionManagerInfo>> SearchPermissionManager([FromQuery] string searchKey)
        {
            return await _settingsService.SearchPermissionManager(searchKey ?? "");
        }

        #endregion

        #region 标签管理
        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<List<LableInfoDto>> LableInfoGetByType([FromQuery] int type)
        {
            var result = _lableInfoService.GetLableInfo(type);
            return result;
        }

        ///// <summary>
        ///// 获取标签
        ///// </summary>
        ///// <param name="lableIDs">类型</param>
        ///// <returns></returns>
        //[HttpGet]
        //[AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        //public Task<List<LableInfoDto>> LableInfoGet([FromQuery] string[] lableIDs)
        //{
        //    var result = _lableInfoService.GetLableInfo(lableIDs);
        //    return result;
        //}

        ///// <summary>
        ///// 新增标签
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<ApiResultInfoModel> LableInfoAdd([FromBody] LableInfoDto model)
        //{
        //    model.Id = Time2KeyUtils.GetRandOnlyId();
        //    return await _lableInfoService.AddLableInfo(model);
        //}

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> LableInfoUpdate(LableInfoUpdateParam param)
        {
            return await _lableInfoService.UpdateLableInfo(param.Type, param.UpdateParmList);
        }

        ///// <summary>
        ///// 删除标签
        ///// </summary>
        ///// <param name="labelID"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[UnitOfWork]
        //public async Task<ApiResultInfoModel> LableInfoDelete([FromBody] string labelID)
        //{
        //    return await _lableInfoService.DeleteLableInfo(labelID);
        //}
        #endregion

        #region 新闻模板管理
        /// <summary>
        /// 添加新闻模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsTemplateAdd(NewsTemplateDto model)
        {
            return await _templateService.AddNewsTemplate(model);
        }

        /// <summary>
        /// 获取新闻模板
        /// </summary>
        /// <param name="tempIDs"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<NewsTemplateDto>> NewsTemplateGet([FromQuery] string[] tempIDs)
        {
            return await _templateService.GetNewsTemplate(tempIDs);
        }

        /// <summary>
        /// 获取单个新闻模板
        /// </summary>
        /// <param name="tempID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<NewsTemplateDto> SingleNewsTemplateGet([FromQuery] string tempID)
        {
            return await _templateService.GetSingleNewsTemplate(tempID);
        }

        /// <summary>
        /// 更新新闻模板
        /// </summary>
        /// <param name="templateDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsTemplateUpdate(NewsTemplateDto templateDto)
        {
            return await _templateService.UpdateNewsTemplate(templateDto);
        }

        /// <summary>
        /// 删除新闻模板
        /// </summary>
        /// <param name="tempID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsTemplateDelete([FromQuery] string tempID)
        {
            return await _templateService.DeleteNewsTemplate(tempID);
        }

        #region 新闻头尾模板
        /// <summary>
        /// 添加新闻头尾模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsBodyTemplateAdd(NewsBodyTemplateDto model)
        {
            return await _templateService.AddNewsBodyTemplate(model);
        }

        /// <summary>
        /// 获取新闻头尾模板
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<NewsBodyTemplateDto>> NewsBodyTemplateGetByType([FromQuery] int type)
        {
            return await _templateService.GetNewsBodyTemplate(type);
        }

        /// <summary>
        /// 获取新闻头尾模板
        /// </summary>
        /// <param name="tempBodyIDs"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<NewsBodyTemplateDto>> NewsBodyTemplateGet([FromQuery] string[] tempBodyIDs)
        {
            return await _templateService.GetNewsBodyTemplate(tempBodyIDs);
        }

        /// <summary>
        /// 更新新闻头尾模板
        /// </summary>
        /// <param name="bodyTemplateDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsBodyTemplateUpdate(NewsBodyTemplateDto bodyTemplateDto)
        {
            return await _templateService.UpdateNewsBodyTemplate(bodyTemplateDto);
        }

        /// <summary>
        /// 删除新闻头尾模板
        /// </summary>
        /// <param name="tempBodyID"></param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsBodyTemplateDelete([FromQuery] string tempBodyID)
        {
            return await _templateService.DeleteNewsBodyTemplate(tempBodyID);
        }
        #endregion
        #endregion

        #region 新闻栏目
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
        /// 获取标签分组及新闻栏目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<LableNewsColumnView>> NewsColumnGetByManagerID()
        {
            return await _columnService.GetLableNewsColumnList();
        }

        /// <summary>
        /// 获取新闻栏目
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.UnAuthKey, false)]
        public async Task<NewsColumnDto> NewsColumnGet([ApiSeat(ApiSeats.ActionEnd)] string columnID)
        {
            var column = await _columnService.GetNewsColumn(columnID);
            var uri = await _contentService.GrpcBaseUriGet("news");
            column.LinkUrl = $"{uri}/#/web_newsList?cid={column.Id}";
            return column;
        }

        /// <summary>
        /// 根据栏目id获取新闻栏目的模板信息
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        [HttpGet]
        // [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<NewsColumnTemplateView> NewsColumnTemplateGetByColumnId([ApiSeat(ApiSeats.ActionEnd)] string columnID)
        {
            var newsColumn = await _columnService.GetNewsColumn(columnID);

            NewsColumnTemplateView templateView = new NewsColumnTemplateView
            {
                ColumnName = newsColumn.Title,
                ColumnTemplate = newsColumn.DefaultTemplate,
            };
            return templateView;
        }

        /// <summary>
        /// 获取当前栏目之外的其他栏目键值对
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<KeyValuePair<string, string>>> DeliveryColumnListGet([FromQuery] string columnID)
        {
            return await _columnService.GetDeliveryColumnList(columnID);
        }

        /// <summary>
        /// 新增新闻栏目
        /// </summary>
        /// <param name="newsColumn"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsColumnAdd([FromBody] NewsColumnParam newsColumn)
        {
            string extension = string.Join(";", newsColumn.ExtensionKV.Select(d => d.Key + "-" + d.Value));
            var visitingList = "";
            if (newsColumn.VisitingListModel != null)
            {
                visitingList = $"{newsColumn.VisitingListModel.Type}|{string.Join(";", newsColumn.VisitingListModel.VisitList.Select(d => d.Key + "-" + d.Value).ToList())}";
            }
            NewsColumnDto newsColumnDto = new NewsColumnDto
            {
                Title = newsColumn.Title,
                Alias = newsColumn.Alias,
                Label = newsColumn.Label,
                Terminals = newsColumn.Terminals,
                Status = newsColumn.Status,
                Extension = extension,
                LinkUrl = newsColumn.LinkUrl,
                DefaultTemplate = newsColumn.DefaultTemplate,
                SideList = newsColumn.SideList,
                SysMesList = newsColumn.SysMesList,
                IsOpenCover = newsColumn.IsOpenCover,
                // CoverSize = newsColumn.CoverSize, 废弃
                IsLoginAcess = newsColumn.IsLoginAcess,
                VisitingList = visitingList,
                IsOpenComment = newsColumn.IsOpenComment,
                //IsOpenLeaveMes = newsColumn.IsOpenLeaveMes,
                IsOpenAudit = newsColumn.IsOpenAudit,
                AuditFlow = newsColumn.AuditFlow,
                HeadTemplate = newsColumn.HeadTemplate,
                FootTemplate = newsColumn.FootTemplate,
                CoverWidth = newsColumn.CoverWidth,
                CoverHeight = newsColumn.CoverHeight,
                AcessAll = newsColumn.AcessAll
            };
            newsColumnDto.Id = Time2KeyUtils.GetRandOnlyId();

            var result = await _columnService.AddNewsColumn(newsColumnDto);
            if (!string.IsNullOrWhiteSpace(newsColumnDto.AuditFlow))
                await _grpcClient.EnsureClient<AppToDoEventGrpcService.AppToDoEventGrpcServiceClient>().AddAppToDoEventAsync(new AppToDoEventRequest
                {
                    AppID = "news",
                    EventId = "News_NewsInfo_Create",
                    NextEventId = "News_NewsInfo_Approve",
                    Name = "新闻审核",
                    ParentObjID = newsColumnDto.Id,
                    ParentObjName = newsColumnDto.Title
                });



            return result;
        }

        /// <summary>
        /// 更新新闻栏目
        /// </summary>
        /// <param name="newsColumn"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsColumnUpdate([ApiSeat(ApiSeats.ActionEnd)] string columnID, NewsColumnParam newsColumn)
        {
            string extension = string.Join(";", newsColumn.ExtensionKV.Select(d => d.Key + "-" + d.Value));
            var visitingList = "";

            if (!newsColumn.AcessAll)
            {
                if (newsColumn.VisitingListModel == null)
                {
                    throw Oops.Oh("未指定授权访问的对象或者类型").StatusCode(HttpStatusKeys.ExceptionCode);
                }
                visitingList = $"{newsColumn.VisitingListModel.Type}|{string.Join(";", newsColumn.VisitingListModel.VisitList.Select(d => d.Key + "-" + d.Value).ToList())}";
            }
            NewsColumnDto newsColumnDto = new NewsColumnDto
            {
                //Id=newsColumn.Id,
                Id = columnID,
                Title = newsColumn.Title,
                Alias = newsColumn.Alias,
                Label = newsColumn.Label,
                Terminals = newsColumn.Terminals,
                Status = newsColumn.Status,
                Extension = extension,
                LinkUrl = newsColumn.LinkUrl,
                DefaultTemplate = newsColumn.DefaultTemplate,
                SideList = newsColumn.SideList,
                SysMesList = newsColumn.SysMesList,
                IsOpenCover = newsColumn.IsOpenCover,
                //CoverSize = newsColumn.CoverSize,废弃
                IsLoginAcess = newsColumn.IsLoginAcess,
                VisitingList = visitingList,
                IsOpenComment = newsColumn.IsOpenComment,
                //IsOpenLeaveMes = newsColumn.IsOpenLeaveMes,
                IsOpenAudit = newsColumn.IsOpenAudit,
                AuditFlow = newsColumn.AuditFlow,
                HeadTemplate = newsColumn.HeadTemplate,
                FootTemplate = newsColumn.FootTemplate,
                CoverWidth = newsColumn.CoverWidth,
                CoverHeight = newsColumn.CoverHeight,
                AcessAll = newsColumn.AcessAll
            };
            return await _columnService.UpdateNewsColumn(newsColumnDto);
        }

        /// <summary>
        /// 删除新闻栏目
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsColumnDelete([ApiSeat(ApiSeats.ActionEnd)] string columnID)
        {
            return await _columnService.DeleteNewsColumn(columnID);
        }

        /// <summary>
        /// 获取前台新闻栏目数据
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<List<ProntNewsColumnListView>> ProntNewsColumnListGet([FromQuery] string columnID)
        {
            var result = _columnService.GetProntNewsColumnList(columnID);
            return result;
        }

        /// <summary>
        /// 获取栏目指定不同源类型的用户
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<KeyValuePair<string, string>>> UserPermissionListGet([FromQuery] int type)
        {
            return await _columnService.GetUserPermissionList(type);
        }
        #endregion

        #region 新闻内容
        /// <summary>
        /// 新增新闻内容
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsContentAdd([ApiSeat(ApiSeats.ActionEnd)] string columnID, NewsContentParam param)
        {
            string titleStyle = string.Join(";", param.TitleStyleKV.Select(d => d.Key + "-" + d.Value));
            NewsContentDto model = new NewsContentDto
            {
                //ColumnID = param.ColumnID,
                ColumnID = columnID,
                ColumnIDs = param.ColumnIDs,
                Title = param.Title,
                TitleStyle = titleStyle,
                SubTitle = param.SubTitle,
                ParentCatalogue = param.ParentCatalogue,
                Content = param.Content,
                Cover = param.Cover,
                Author = param.Author,
                Publisher = param.Publisher,
                PublishDate = param.PublishDate,
                Status = param.Status,
                Terminals = string.Join(";", param.Terminals),
                //Terminals = param.Terminals,
                AuditStatus = param.AuditStatus,
                Keywords = param.Keywords,
                ExpirationDate = param.ExpirationDate,
                JumpLink = param.JumpLink,
                HitCount = param.HitCount,
                AuditProcessJson = param.AuditProcessJson,
                ExpendFiled1 = param.ExpendFiled1,
                ExpendFiled2 = param.ExpendFiled2,
                ExpendFiled3 = param.ExpendFiled3,
                ExpendFiled4 = param.ExpendFiled4,
                ExpendFiled5 = param.ExpendFiled5,
                ExternalLink = param.ExternalLink,
                ContentEditor = param.ContentEditor,

            };
            model.Id = Time2KeyUtils.GetRandOnlyId();



            var result = await _contentService.AddNewsContent(model);
            var owner = App.HttpContext.User.FindFirstValue("OrgCode");
            //发送事件
            await _capPublisher.PublishAppEventAsync(owner: owner, eventCode: "News_NewsInfo_Create", new AppEventSimpleMsgModel
            {
                ObjectId = model.Id,
                ParentObjectId = model.ColumnID,
                ObjectName = model.Title,
                ParentObjectName = "待查",

            });
            return result;
        }

        /// <summary>
        /// 更新新闻内容
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsContentUpdate([ApiSeat(ApiSeats.ActionEnd)] string columnID, NewsContentParam param)
        {
            string titleStyle = string.Join(";", param.TitleStyleKV.Select(d => d.Key + "-" + d.Value));
            NewsContentDto model = new NewsContentDto
            {
                Id = param.Id,
                ColumnID = param.ColumnID,
                ColumnIDs = param.ColumnIDs,
                Title = param.Title,
                TitleStyle = titleStyle,
                SubTitle = param.SubTitle,
                ParentCatalogue = param.ParentCatalogue,
                Content = param.Content,
                Cover = param.Cover,
                Author = param.Author,
                Publisher = param.Publisher,
                PublishDate = param.PublishDate,
                Status = param.Status,
                Terminals = string.Join(";", param.Terminals),
                //Terminals = param.Terminals,
                AuditStatus = param.AuditStatus,
                Keywords = param.Keywords,
                ExpirationDate = param.ExpirationDate,
                JumpLink = param.JumpLink,
                HitCount = param.HitCount,
                AuditProcessJson = param.AuditProcessJson,
                ContentEditor = param.ContentEditor,
                ExpendFiled1 = param.ExpendFiled1,
                ExpendFiled2 = param.ExpendFiled2,
                ExpendFiled3 = param.ExpendFiled3,
                ExpendFiled4 = param.ExpendFiled4,
                ExpendFiled5 = param.ExpendFiled5,
                ExternalLink = param.ExternalLink,
            };
            return await _contentService.UpdateNewsContent(model);
        }

        /// <summary>
        /// 获取后台新闻详情
        /// </summary>
        /// <param name="contentID">新闻ID</param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<NewsContentManageView> NewsContentManageGet([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromQuery] string contentID)
        {
            return await _contentService.GetNewsContentManage(contentID);
        }

        /// <summary>
        /// 更新新闻内容审核状态
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<ApiResultInfoModel> NewsContentUpdateAuditStatus([ApiSeat(ApiSeats.ActionEnd)] string columnID, NewsContentAuditStatusParm parm)
        {
            return await _contentService.UpdateNewsContentAuditStatus(parm);
        }

        /// <summary>
        /// 获取后台新闻栏目状态集合以及新闻内容标签集合
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<NewsColumnContentManage> NewsColumnContentManageGet([ApiSeat(ApiSeats.ActionEnd)] string columnID)
        {
            var result = _contentService.GetNewsColumnContentManage(columnID);
            return result;
        }

        /// <summary>
        /// 获取后台栏目新闻内容
        /// </summary>
        /// <param name="pageParm"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<NewsContentsManageViewModel> NewsContentGetByColumn([ApiSeat(ApiSeats.ActionEnd)] string columnID, NewsContentByColumnParm pageParm)
        {
            var result = _contentService.GetNewsContent(pageParm);
            return result;
        }

        /// <summary>
        /// 获取后台检索新闻
        /// </summary>
        /// <param name="pageParm"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<PagedList<NewsContentsForSearchView>> NewsContentGetBySearch(NewsContentBySearchKeyParm pageParm)
        {
            var result = _contentService.GetNewsContentBySearch(pageParm);
            return result;
        }

        /// <summary>
        /// 批量下架新闻
        /// </summary>
        /// <param name="contentIDs"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<ApiResultInfoModel> NewsContentOffShelf([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromBody] string[] contentIDs)
        {
            var result = _contentService.OffShelfNewsContent(contentIDs);
            return result;
        }

        /// <summary>
        /// 批量删除新闻
        /// </summary>
        /// <param name="contentIDs"></param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<ApiResultInfoModel> NewsContentDelete([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromBody] string[] contentIDs)
        {
            var result = _contentService.DeleteNewsContent(contentIDs);


            return result;
        }

        /// <summary>
        /// 获取前台列表新闻列表数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionObjAttribute("Column")]
        [AuthorizeMultiplePolicy(PolicyKey.PortalColumn, false)]
        public Task<PagedList<FrontNewsContentListView>> ProntNewsListDataGet([FromQuery] string columnID, [FromBody] FrontNewsListParm parm)
        {
            var result = _contentService.GetProntNewsListData(parm);
            return result;
        }

        /// <summary>
        /// 获取前台新闻数据
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionObjAttribute("Content")]
        [AuthorizeMultiplePolicy(PolicyKey.PortalColumn, false)]
        public Task<FrontNewsContentView> ProntNewsContentGet([FromQuery] string contentID)
        {
            var result = _contentService.GetProntNewsContent(contentID);


            return result;
        }

        /// <summary>
        /// 弹窗输入序号排序
        /// </summary>
        /// <param name="parma">SourceID:源ID,SortIndex:排序号</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<ApiResultInfoModel> SortContentByIndex([ApiSeat(ApiSeats.ActionEnd)] string columnID, SortContentByIndexParam parma)
        {
            var result = _contentService.SortModel(parma.SourceID, parma.SortIndex);
            return result;
        }

        /// <summary>
        /// 拖动排序
        /// </summary>
        /// <param name="parma">SourceID:源ID,TargetCataID:目标ID, isUp:位置</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public Task<ApiResultInfoModel> SortContentByTarget([ApiSeat(ApiSeats.ActionEnd)] string columnID, SortContentByTargetParam parma)
        {
            var result = _contentService.SortModel(parma.SourceID, parma.TargetCataID, parma.IsUp);
            return result;
        }

        /// <summary>
        /// 获取新闻内容操作日志
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<ContentProcessLog>> ContentProcessLogGet([ApiSeat(ApiSeats.ActionEnd)] string columnID, [FromQuery] string contentID)
        {
            var result = await _contentService.GetContentProcessLog(contentID);
            return result;
        }

        /// <summary>
        /// 获取新闻内容操作日志
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<HeadFootTemplateView> GetTemplateDetailByColumnId([ApiSeat(ApiSeats.ActionEnd)] string columnID)
        {

            return await _templateService.GetTemplateDetailByColumnId(columnID);
        }

        /// <summary>
        /// 获取场景新闻信息
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <param name="topNum">新闻条数</param>
        /// <param name="sortField">排序字段 OrderNum-Desc,PublishDate-Asc,CreatedTime-Desc</param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<ProntScenesNewsView>> GetProntScenesNews([FromBody] List<FrontNewsParam> frontNewsParam)
        {
            var reply = new List<ProntScenesNewsView>();
            foreach (var item in frontNewsParam)
            {
                if (item.SortField == "Default")
                    item.SortField = "OrderNum-Desc";

                var sortFieldArray = item.SortField.Split('-');
                var realSortField = "";
                bool IsAsc = false;
                if (sortFieldArray.Length == 2)
                {
                    realSortField = sortFieldArray[0];
                    IsAsc = sortFieldArray[1].ToLower() == "asc";
                }
                var result = await _contentService.GetProntScenesNews(item.ColumnId, item.Count, realSortField, IsAsc);
                reply.Add(result);
            }
            return reply;
        }

        /// <summary>
        /// 获取场景新闻信息
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <param name="topNum">新闻条数</param>
        /// <param name="sortField">排序字段 OrderNum-Desc,PublishDate-Asc,CreatedTime-Desc</param>
        /// <returns></returns>
        [HttpPost]
        // [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task<List<ProntScenesNewsView>> GetProntScenesNewsCqu([FromBody] List<FrontNewsParam> frontNewsParam)
        {
            var reply = new List<ProntScenesNewsView>();
            foreach (var item in frontNewsParam)
            {
                if (item.SortField == "Default")
                    item.SortField = "OrderNum-Desc";

                var sortFieldArray = item.SortField.Split('-');
                var realSortField = "";
                bool IsAsc = false;
                if (sortFieldArray.Length == 2)
                {
                    realSortField = sortFieldArray[0];
                    IsAsc = sortFieldArray[1].ToLower() == "asc";
                }
                ProntScenesNewsView result = await _contentService.GetProntScenesNews(item.ColumnId, item.Count, realSortField, IsAsc);

                List<ProntScenesChildCatorageView> allChildCatorage = new List<ProntScenesChildCatorageView>();
                foreach (var content in result.ContentList)
                {
                    var childCats = content.Lables.Split(';', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var label in childCats)
                    {
                        var child = allChildCatorage.FirstOrDefault(e => e.ChildCatorage == label);
                        if (child != null)
                        {
                            child.ContentList.Add(content);
                        }
                        else
                        {
                            var contentList = new List<ProntScenesNewsContentView>();
                            contentList.Add(content);
                            allChildCatorage.Add(new ProntScenesChildCatorageView
                            {
                                ChildCatorage = label,
                                ContentList = contentList,
                            });
                        }
                    }


                }
                result.Childs = allChildCatorage;

                reply.Add(result);
            }
            return reply;
        }

        #endregion

        #region 馆员工作台
        ///// <summary>
        ///// 馆员工作台-发布新闻
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<WorkbenchPublishView> WorkbenchPublish(WorkbenchPublishParam param)
        //{
        //    WorkbenchPublishView result = new WorkbenchPublishView();
        //    result.ColumnCounts = _columnService.GetAllNewsColumnCount().Result;
        //    result.ContentCounts = _contentService.GetAllNewsContentCount().Result;
        //    result.ColumnList = _columnService.GetPublishColumnList(param.ManagerID, param.Count).Result;
        //    result.ContentList = _contentService.GetPublishNewsContentList(param.ManagerID, param.Count).Result;
        //    return await Task.FromResult(result);
        //}

        ///// <summary>
        ///// 馆员工作台-获取具有相关权限对应审核状态的新闻
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<WorkbenchAuditPageView> WorkbenchAuditListGet(WorkbenchAuditNewsListParam param)
        //{
        //    return await _contentService.GetWorkbenchAudit(param);
        //}
        #endregion



    }
}
