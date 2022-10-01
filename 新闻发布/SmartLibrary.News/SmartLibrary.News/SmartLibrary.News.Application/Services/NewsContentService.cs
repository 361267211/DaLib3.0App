using DotNetCore.CAP;
using EFCore.BulkExtensions;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using Grpc.Core;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OneOf;
using SmartLibrary.AppCenter;
using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.Dtos.Cap;
using SmartLibrary.News.Application.Enums;
using SmartLibrary.News.Application.Interceptors;
using SmartLibrary.News.Application.PageParam;
using SmartLibrary.News.Application.ViewModel;
using SmartLibrary.News.Common.Const;
using SmartLibrary.News.Common.Extensions;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using SmartLibrary.News.Utility;
using SmartLibrary.News.Utility.EnumTool;
using SmartLibrary.Search.EsSearchProxy.Core.Interfaces;
using SmartLibraryNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Transactions;

namespace SmartLibrary.News.Application.Services
{
    /// <summary>
    /// 名    称：NewsService
    /// 作    者：张泽军
    /// 创建时间：2021/9/7 19:29:20
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsContentService : NewsGrpcService.NewsGrpcServiceBase, INewsContentService, IScoped
    {
        private ICapPublisher _capPublisher;
        private IRepository<LableInfo> _lableRepository;
        private IRepository<NewsColumn> _newsColumnRepository;
        private IRepository<NewsContent> _newsContentRepository;
        private IRepository<NewsContentExpend> _newsConExpRepository;
        private IRepository<NewsColumnPermissions> _permissionRepository;
        private INewsSettingsService _newsSettingsService;
        private ILableInfoService _lableService;
        private readonly IRepository<NewsTemplate> _templateRepository;
        private IEsProxyService _esProxyService;
        private TenantInfo _tenantInfo;

        public NewsContentService(ICapPublisher capPublisher,
            IRepository<LableInfo> lableRepository,
            IRepository<NewsColumn> newsColumnRepository,
            IRepository<NewsContent> newsContentRepository,
            IRepository<NewsContentExpend> newsConExpRepository,
            IRepository<NewsColumnPermissions> permissionRepository,
            INewsSettingsService newsSettingsService,
            ILableInfoService lableService,
            IRepository<NewsTemplate> templateRepository,
        IEsProxyService esProxyService,
        TenantInfo tenantInfo)
        {
            _capPublisher = capPublisher;
            _lableRepository = lableRepository;
            _newsColumnRepository = newsColumnRepository;
            _newsContentRepository = newsContentRepository;
            _newsConExpRepository = newsConExpRepository;
            _newsSettingsService = newsSettingsService;
            _permissionRepository = permissionRepository;
            _lableService = lableService;
            _templateRepository = templateRepository;
            _esProxyService = esProxyService;
            _tenantInfo = tenantInfo;
        }

        #region NewsContent 新闻内容
        /// <summary>
        /// 添加新闻内容
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> AddNewsContent(NewsContentDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            model.Title = StringUtils.HtmlDecode(model.Title);
            var settings = _newsSettingsService.GetNewsSettings();
            if (settings.Result.SensitiveWords == 1)
            {
                var sensitiveWordsCheck = _newsSettingsService.CheckSensitiveWords(model.Title);
                if (sensitiveWordsCheck)
                {
                    result.Succeeded = false;
                    result.Message = "新闻标题包含敏感词！";
                    return result;
                }
                sensitiveWordsCheck = _newsSettingsService.CheckSensitiveWords(model.Content);
                if (sensitiveWordsCheck)
                {
                    result.Succeeded = false;
                    result.Message = "新闻内容包含敏感词！";
                    return result;
                }
            }
            var maxSortIndex = 0;
            //基于栏目对栏目下的新闻隔离，取出当前最大的序号
            if (_newsContentRepository.Count(e => !e.DeleteFlag && e.ColumnID == model.ColumnID) > 0)
                maxSortIndex = _newsContentRepository.AsQueryable(e => !e.DeleteFlag && e.ColumnID == model.ColumnID).Max(d => d.OrderNum);
            model.OrderNum = maxSortIndex + 1;


            var newsCt = model.ToModel<NewsContent>();
            var column = _newsColumnRepository.FirstOrDefault(d => d.Id == newsCt.ColumnID);
            if (column != null)
            {
                if (column.IsOpenAudit == 1)//开启审核流程
                {
                    if (newsCt.AuditStatus != ((int)AuditStatusEnum.UnSubmit) && newsCt.AuditStatus != ((int)AuditStatusEnum.UnPreliminaryAudit))
                    {
                        result.Succeeded = false;
                        result.Message = "开启审核流程下，新增新闻只能是待提交或待初审！";
                        return result;
                    }
                }
                else
                {
                    if (newsCt.AuditStatus != ((int)AuditStatusEnum.UnPublish) && newsCt.AuditStatus != ((int)AuditStatusEnum.Published))
                    {
                        result.Succeeded = false;
                        result.Message = "关闭审核流程下，新增新闻只能是待发布或已发布！";
                        return result;
                    }
                }
            }

            newsCt.ParentCatalogue = await _lableService.ProcessLablesFromLableStr(2, newsCt.ParentCatalogue);
            newsCt.CreatedTime = DateTime.Now;
            var newsContent = await _newsContentRepository.InsertNowAsync(newsCt);
            //新增对象到ES数据库中
            var newsDirectUrl = "";
            var templateList = from col in _newsColumnRepository.Where(d => !d.DeleteFlag)
                               join temp in _templateRepository.Where(d => !d.DeleteFlag) on col.DefaultTemplate equals temp.Id
                               where col.Id == newsCt.ColumnID
                               select temp;
            if (templateList != null && templateList.Count() == 1)
            {
                newsDirectUrl = templateList.FirstOrDefault().TemplateDetailDirectUrl;
            }

            await SaveNewsToES(newsCt);

            return result;
        }

        private async Task SaveNewsToES(NewsContent news)
        {

            //插入到ES检索库当中
            var fulltext = HTMLFilter.StripHTML(news.Content);
            var request = new Search.EsSearchProxy.Core.Dto.UpsertOwnerNewsRequestParameter
            {
                app_id = "news",
                app_type = Search.EsSearchProxy.Core.Models.OrganNewsType.News,
                click_count = news.HitCount,
                docId = $"news_{App.HttpContext.EnsureOwner()}_{news.Id.ToString().Replace('-', '_')}",
                fulltext = HTMLFilter.StripHTML(news.Content),
                keyword = (news.Keywords ?? "").Split(';', StringSplitOptions.RemoveEmptyEntries),
                owner = App.HttpContext.EnsureOwner(),
                pub_time = news.CreatedTime,
                summary = HTMLFilter.StripHTML(news.Content).Substring(0, Math.Min(HTMLFilter.StripHTML(news.Content).Length, 256)),
                title = news.Title,
                update_time = news.UpdatedTime.HasValue ? news.UpdatedTime.Value : DateTimeOffset.MinValue,
                url = $"/web_newsDetails?id={news.Id}&cid={news.ColumnID}",
            };

            try
            {
                var esItem = await _esProxyService.UpsertOrganNewsAsync(request);
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// 更新新闻内容 （不更新审核状态）
        /// </summary>
        /// <param name="model"></param>
        public async Task<ApiResultInfoModel> UpdateNewsContent(NewsContentDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            model.Title = StringUtils.HtmlDecode(model.Title);
            var newsClOld = _newsContentRepository.Entities.AsNoTracking().Any(d => d.Id == model.Id);
            if (!newsClOld)
            {
                result.Succeeded = false;
                result.Message = "新闻内容不存在！";
                return result;
            }
            var settings = _newsSettingsService.GetNewsSettings();
            if (settings.Result.SensitiveWords == 1)
            {
                var sensitiveWordsCheck = _newsSettingsService.CheckSensitiveWords(model.Title);
                if (sensitiveWordsCheck)
                {
                    result.Succeeded = false;
                    result.Message = "新闻标题包含敏感词！";
                    return result;
                }
                sensitiveWordsCheck = _newsSettingsService.CheckSensitiveWords(model.Content);
                if (sensitiveWordsCheck)
                {
                    result.Succeeded = false;
                    result.Message = "新闻内容包含敏感词！";
                    return result;
                }
            }

            var newsContent = model.ToModel<NewsContent>();
            newsContent.UpdatedTime = DateTime.Now;

            newsContent.ParentCatalogue = _lableService.ProcessLablesFromLableStr(2, newsContent.ParentCatalogue).Result;
            newsContent.CreatedTime = DateTime.Now;


            AuditStatusEnum? auditStatus = Converter.ToType<AuditStatusEnum?>(newsContent.AuditStatus, null);
            if (newsContent == null)
            {
                result.Succeeded = false;
                result.Message = "新闻内容的审核状态不存在！";
                return result;
            }
            // model = _newsContentRepository.Entities.AsNoTracking().FirstOrDefault(d => d.Id == newsContent.Id);
            if (newsContent == null)
            {
                result.Succeeded = false;
                result.Message = "新闻内容不存在！";
                return result;
            }
            newsContent.UpdatedTime = DateTime.Now;
            //model.AuditStatus = newsContent.AuditStatus;
            //TODO 获取当前登录用户名
            FrontNewsAuditProcess process = new FrontNewsAuditProcess
            {
                AuditProcess = newsContent.AuditStatus,
                Name = EnumUtils.GetName(auditStatus.Value),
                AuditManager = "测试管理员（cqviptest）"
            };
            List<FrontNewsAuditProcess> listProcess = new List<FrontNewsAuditProcess>();
            if (!string.IsNullOrEmpty(newsContent.AuditProcessJson))
                listProcess = JsonSerializer.Deserialize<List<FrontNewsAuditProcess>>(newsContent.AuditProcessJson);
            listProcess.Add(process);
            //var options = new JsonSerializerOptions
            //{
            //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.CjkUnifiedIdeographs, UnicodeRanges.CjkUnifiedIdeographsExtensionA),
            //};
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            newsContent.AuditProcessJson = JsonSerializer.Serialize(listProcess, options);




            var newsCon = await _newsContentRepository.UpdateExcludeAsync(newsContent, new[] { nameof(newsContent.CreatedTime), nameof(newsContent.TenantId), nameof(newsContent.OrderNum) });

            //新增对象到ES数据库中
            var newsDirectUrl = "";
            var templateList = from col in _newsColumnRepository.Where(d => !d.DeleteFlag)
                               join temp in _templateRepository.Where(d => !d.DeleteFlag) on col.DefaultTemplate equals temp.Id
                               where col.Id == newsContent.ColumnID
                               select temp;
            if (templateList != null && templateList.Count() == 1)
            {
                newsDirectUrl = templateList.FirstOrDefault().TemplateDetailDirectUrl;
            }
            //存到ES检索库中
            await SaveNewsToES(newsContent);
            return result;
        }

        /// <summary>
        /// 新闻ID转化为es需要的docId
        /// </summary>
        /// <param name="newsIds"></param>
        /// <returns></returns>
        private OneOf<IEnumerable<string>, string> ProcessNewsIDs(OneOf<IEnumerable<string>, string> newsIds)
        {
            if (newsIds.IsT0)
            {
                return OneOf<IEnumerable<string>, string>.FromT0(newsIds.AsT0.Select(d => $"news_{App.HttpContext.EnsureOwner()}_{d.Replace('-', '_')}"));
            }
            else
                return $"news_{App.HttpContext.EnsureOwner()}:{newsIds.AsT1.Replace('-', '_')}";
        }

        /// <summary>
        /// 更新新闻内容审核状态
        /// </summary>
        /// <param name="parm"></param>
        public async Task<ApiResultInfoModel> UpdateNewsContentAuditStatus(NewsContentAuditStatusParm parm)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            AuditStatusEnum? auditStatus = Converter.ToType<AuditStatusEnum?>(parm.AuditStatus, null);
            if (auditStatus == null)
            {
                result.Succeeded = false;
                result.Message = "新闻内容的审核状态不存在！";
                return result;
            }
            var model = _newsContentRepository.Entities.AsNoTracking().FirstOrDefault(d => d.Id == parm.ContentID);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "新闻内容不存在！";
                return result;
            }
            model.UpdatedTime = DateTime.Now;
            model.AuditStatus = parm.AuditStatus;
            //TODO 获取当前登录用户名
            FrontNewsAuditProcess process = new FrontNewsAuditProcess
            {
                AuditProcess = model.AuditStatus,
                Name = EnumUtils.GetName(auditStatus.Value),
                AuditManager = "测试管理员（cqviptest）"
            };
            List<FrontNewsAuditProcess> listProcess = new List<FrontNewsAuditProcess>();
            if (!string.IsNullOrEmpty(model.AuditProcessJson))
                listProcess = JsonSerializer.Deserialize<List<FrontNewsAuditProcess>>(model.AuditProcessJson);
            listProcess.Add(process);
            //var options = new JsonSerializerOptions
            //{
            //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.CjkUnifiedIdeographs, UnicodeRanges.CjkUnifiedIdeographsExtensionA),
            //};
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            model.AuditProcessJson = JsonSerializer.Serialize(listProcess, options);
            var newsCon = await _newsContentRepository.UpdateExcludeAsync(model, new[] { nameof(model.CreatedTime), nameof(model.TenantId), nameof(model.OrderNum) });

            return result;
        }

        /// <summary>
        /// 获取后台栏目新闻内容
        /// </summary>
        /// <param name="pageParm"></param>
        /// <returns></returns>
        public async Task<NewsContentsManageViewModel> GetNewsContent(NewsContentByColumnParm pageParm)
        {
            if (pageParm.AuditStatus == 10)//下架
            {
                pageParm.AuditStatus = null;
                pageParm.status = 2;
            }
            else
            {
                pageParm.status = 1;
            }

            PagedList<NewsContent> pageList = await _newsContentRepository.Entities.Where(d => !d.DeleteFlag && (string.IsNullOrEmpty(pageParm.ColumnID) ? true : d.ColumnID == pageParm.ColumnID)
            && (string.IsNullOrEmpty(pageParm.SearchKey) ? true : (d.Title.Contains(pageParm.SearchKey) || d.Content.Contains(pageParm.SearchKey) || d.PublisherName.Contains(pageParm.SearchKey)))
            && (string.IsNullOrEmpty(pageParm.LableId) ? true : (d.ParentCatalogue).Contains(pageParm.LableId))
            && (pageParm.AuditStatus != null ? d.AuditStatus == pageParm.AuditStatus : true)
            && (pageParm.BeginCreateTime == null ? true : d.CreatedTime > pageParm.BeginCreateTime)
            && (pageParm.EndCreateTime == null ? true : d.CreatedTime < pageParm.EndCreateTime)
            && (pageParm.BeginOperateTime == null ? true : d.UpdatedTime > pageParm.BeginOperateTime)
            && (pageParm.EndOperateTime == null ? true : d.UpdatedTime < pageParm.EndOperateTime)
            && (pageParm.status == 0 ? true : d.Status == pageParm.status)
            ).OrderByDescending(d => d.OrderNum).ToPagedListAsync(pageParm.PageIndex, pageParm.PageSize);

            NewsContentsManageViewModel result = new NewsContentsManageViewModel();
            PagedList<NewsContentsForColumnView> temp = new PagedList<NewsContentsForColumnView>()
            {
                PageIndex = pageList.PageIndex,
                PageSize = pageList.PageSize,
                TotalCount = pageList.TotalCount,
                TotalPages = pageList.TotalPages,
                HasPrevPages = pageList.HasPrevPages,
                HasNextPages = pageList.HasNextPages
            };
            List<NewsContentsForColumnView> listItems = new List<NewsContentsForColumnView>();
            int indexMark = 1;
            var column = _newsColumnRepository.FirstOrDefault(d => !d.DeleteFlag && d.Id == pageParm.ColumnID).ToModel<NewsColumnDto>();
            //bool IsHasCatalogue = false;
            //if (column.Extension.Contains("ParentCatalogue-标签"))
            //    IsHasCatalogue = true;
            foreach (var item in pageList.Items)
            {
                string[] parentCatalogues = item.ParentCatalogue.Split(';');
                var lables = from lab in _lableRepository.Where(d => parentCatalogues.Contains(d.Id))
                             select new KeyValuePair<string, string>(lab.Id, lab.Name);
                listItems.Add(new NewsContentsForColumnView
                {
                    Id = item.Id,
                    IndexNum = (pageList.PageIndex - 1) * pageList.PageSize + indexMark,
                    Title = item.Title,
                    ParentCatalogue = lables.ToList(),
                    Status = item.Status,
                    AduitStatus = item.AuditStatus,
                    NextAuditStatus = GetContentNextAuditStatus(item.ColumnID, item.AuditStatus),
                    Publisher = item.PublisherName,
                    CreatedTime = item.CreatedTime.UtcDateTime,
                    UpdateTime = item.UpdatedTime == null ? null : item.UpdatedTime.Value.UtcDateTime
                });
                indexMark++;
            }
            temp.Items = listItems;
            result.NewsContents = temp;

            var query = from gas in _newsContentRepository.Where(d => !d.DeleteFlag && d.ColumnID == pageParm.ColumnID && d.Status != 2).AsQueryable()
                        group gas by new { gas.AuditStatus } into dateGroup
                        select new AuditStatusCountView { AuditStatus = dateGroup.Key.AuditStatus, Counts = dateGroup.Count() };

            var listAuditStatus = GetColumnManageInfo(pageParm.ColumnID, _tenantInfo.UserKey);
            var tempList = query.ToList();

            //下架的新闻
            listAuditStatus = listAuditStatus.Where(e => e.AuditStatus != 10).ToList();
            listAuditStatus.Add(new AuditStatusCountView
            {
                AuditStatus = 10,
                Counts = _newsContentRepository.Count(e => !e.DeleteFlag && e.Status == 2 && e.ColumnID == pageParm.ColumnID),

            });


            foreach (var item in tempList)
            {
                if (listAuditStatus.FirstOrDefault(d => d.AuditStatus == item.AuditStatus) != null)
                    listAuditStatus.FirstOrDefault(d => d.AuditStatus == item.AuditStatus).Counts = item.Counts;
            }
            result.AuditStatusCountList = listAuditStatus;
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取新闻栏目状态集合以及新闻内容标签集合
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<NewsColumnContentManage> GetNewsColumnContentManage(string columnID)
        {
            NewsColumnContentManage result = new NewsColumnContentManage();
            var column = _newsColumnRepository.FirstOrDefault(d => !d.DeleteFlag && d.Id == columnID);
            if (column.Extension.Contains("ParentCatalogue-标签"))
            {
                var listLableIDs = new List<string>();
                var lables = _newsContentRepository.Where(d => !d.DeleteFlag && d.ColumnID == columnID).AsEnumerable().GroupBy(d => d.ParentCatalogue);
                foreach (var item in lables)
                {
                    if (!string.IsNullOrEmpty(item.Key))
                        listLableIDs.AddRange(item.Key.Split(';'));
                }
                listLableIDs.Distinct();
                var lableList = _lableRepository.Where(d => listLableIDs.Contains(d.Id)).ToList();
                List<KeyValuePair<string, string>> labList = new List<KeyValuePair<string, string>>();
                foreach (var item in lableList)
                {
                    labList.Add(new KeyValuePair<string, string>(item.Id, item.Name));
                }
                result.LableList = labList;
                result.IsHasCatalogue = true;
            }
            result.AuditStatusCountList = GetColumnManageInfo(columnID, _tenantInfo.UserKey);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取栏目对应的管理状态集合
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public List<AuditStatusCountView> GetColumnManageInfo(string columnID, string userKey)
        {
            var listAuditStatusCountView = new List<AuditStatusCountView>();
            var column = _newsColumnRepository.FirstOrDefault(d => d.Id == columnID);
            if (column != null)
            {
                if (column.IsOpenAudit == 1)
                {
                    var auditFlow = column.AuditFlow.Split(';');
                    //userkey是否为空
                    if (!string.IsNullOrEmpty(userKey))
                    {
                        IEnumerable<int> listPower = new List<int>();

                        //1.根据用户的身份信息查询拥有的权限

                        //2.根据用户信息取他的通用角色  1管理员/2操作员/3浏览者  TODO:暂时伪造数据
                        var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
                        UserAppPermissionTypeRequest request1 = new UserAppPermissionTypeRequest { AppId = "news" };
                        UserAppPermissionTypeReply reply1 = new UserAppPermissionTypeReply();
                        try
                        {
                            reply1 = grpcClient1.GetUserAppPermissionType(request1);
                        }
                        catch (Exception)
                        {
                            // throw Oops.Oh("grpc调用异常");
                            reply1 = new UserAppPermissionTypeReply { PermissionType = 1 };
                        }

                        if (reply1.PermissionType == 1)
                        {
                            listPower = _permissionRepository.Where(d => !d.DeleteFlag).ToList().Select(d => d.Permission);
                        }
                        else
                        {
                            listPower = _permissionRepository.Where(d => !d.DeleteFlag && d.ColumnID == columnID && d.ManagerID == userKey).ToList().Select(d => d.Permission);
                        }

                        foreach (var item in auditFlow)
                        {
                            AuditProcessEnum process = Converter.ToType<AuditProcessEnum>(Converter.ObjectToInt(item));
                            //1 管理权限全添加，2 提交流程对应的权限同编辑流程 3 当前流程值包含于权限集合
                            if (listPower.Contains(((int)AuditPowerEunm.Manage)) || (process == AuditProcessEnum.Edit && listPower.Contains(((int)AuditPowerEunm.Edit))) || listPower.Contains(((int)process)))
                                listAuditStatusCountView.Add(new AuditStatusCountView
                                {
                                    AuditStatus = ((int)process)
                                });
                        }
                    }
                    else
                    {
                        //foreach (var item in auditFlow)
                        //{
                        //    listAuditStatusCountView.Add(new AuditStatusCountView
                        //    {
                        //        AuditStatus = Converter.ObjectToInt(item)
                        //    });
                        //}
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(userKey) || _permissionRepository.Any(d => !d.DeleteFlag && d.Permission == ((int)AuditPowerEunm.Manage) && d.ManagerID == userKey))
                    {
                        listAuditStatusCountView.Add(new AuditStatusCountView
                        {
                            AuditStatus = ((int)AuditStatusEnum.Published)
                        });
                        listAuditStatusCountView.Add(new AuditStatusCountView
                        {
                            AuditStatus = ((int)AuditStatusEnum.UnPublish)
                        });
                        listAuditStatusCountView.Add(new AuditStatusCountView
                        {
                            AuditStatus = ((int)AuditStatusEnum.OffShelf)
                        });
                    }
                }
            }

            return listAuditStatusCountView;
        }

        /// <summary>
        /// 获取后台检索新闻
        /// </summary>
        /// <param name="pageParm"></param>
        /// <returns></returns>
        public async Task<PagedList<NewsContentsForSearchView>> GetNewsContentBySearch(NewsContentBySearchKeyParm pageParm)
        {
            var powerColumnIDs = _newsSettingsService.GetColumnIDsByUserKey().Result;
            PagedList<NewsContent> pageList = await _newsContentRepository.Entities.Where(d => !d.DeleteFlag
            && powerColumnIDs.Contains(d.ColumnID)
            && (string.IsNullOrEmpty(pageParm.SearchKey) ? true : (d.Title.Contains(pageParm.SearchKey) || d.Content.Contains(pageParm.SearchKey)))
            ).OrderByDescending(d => d.CreatedTime).ToPagedListAsync(pageParm.PageIndex, pageParm.PageSize);

            PagedList<NewsContentsForSearchView> result = new PagedList<NewsContentsForSearchView>()
            {
                PageIndex = pageList.PageIndex,
                PageSize = pageList.PageSize,
                TotalCount = pageList.TotalCount,
                TotalPages = pageList.TotalPages,
                HasPrevPages = pageList.HasPrevPages,
                HasNextPages = pageList.HasNextPages
            };
            List<NewsContentsForSearchView> listItems = new List<NewsContentsForSearchView>();
            int indexMark = 1;
            foreach (var item in pageList.Items)
            {
                var columnList = from tab in _newsColumnRepository.Where(d => item.ColumnID == d.Id)
                                 select new KeyValuePair<string, string>(tab.Id, tab.Title);
                listItems.Add(new NewsContentsForSearchView
                {
                    Id = item.Id,
                    IndexNum = (pageList.PageIndex - 1) * pageList.PageSize + indexMark,
                    ColumnIDs = columnList.ToList(),
                    Title = item.Title,
                    Publisher = item.PublisherName,
                    PublishDate = item.PublishDate,
                    UpdateTime = item.UpdatedTime == null ? null : item.UpdatedTime.Value.UtcDateTime
                });
                indexMark++;
            }
            result.Items = listItems;
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 批量下架新闻
        /// </summary>
        /// <param name="contentIDs"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> OffShelfNewsContent(string[] contentIDs)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            foreach (var contentID in contentIDs)
            {
                var model = _newsContentRepository.FindOrDefault(contentID);
                if (model == null)
                {
                    result.Succeeded = false;
                    result.Message = "新闻内容不存在！";
                    return result;
                }
                model.Status = (int)NewsContentStatusEnum.OffShelf;
                model.UpdatedTime = DateTime.Now;
                //此处事务必须用带Now的执行方法
                await _newsContentRepository.UpdateNowAsync(model);
            }
            return result;
        }

        /// <summary>
        /// 更新栏目下所有未发布的新闻内容为已发布且已下架
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> PublishOffShelfNewsContent(string columnID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var temp = await _newsContentRepository.Where(d => d.ColumnID == columnID && d.AuditStatus != ((int)AuditStatusEnum.Published)).BatchUpdateAsync(new NewsContent { UpdatedTime = DateTime.Now, AuditStatus = ((int)AuditStatusEnum.Published), Status = (int)NewsContentStatusEnum.OffShelf });
            return result;
        }

        /// <summary>
        /// 批量删除新闻
        /// </summary>
        /// <param name="contentIDs"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNewsContent(string[] contentIDs)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            foreach (var contentID in contentIDs)
            {
                var model = _newsContentRepository.FindOrDefault(contentID);
                if (model == null)
                {
                    result.Succeeded = false;
                    result.Message = "新闻内容不存在！";
                    return result;
                }
                model.DeleteFlag = true;
                model.UpdatedTime = DateTime.Now;
                await _newsContentRepository.UpdateAsync(model);
            }
            //调用ES删除数据
            var esItem = await _esProxyService.DeleteOrganNewsAsync(ProcessNewsIDs(contentIDs));
            return result;
        }

        /// <summary>
        /// 获取后台新闻详情
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public async Task<NewsContentManageView> GetNewsContentManage(string contentID)
        {
            NewsContentManageView result = new NewsContentManageView();
            var newsContent = _newsContentRepository.FirstOrDefault(d => d.Id == contentID);
            var thisColumn = _newsColumnRepository.FindOrDefault(newsContent.ColumnID).ToModel<NewsColumnDto>();
            bool isDetailShowPublishDate = thisColumn.SysMesList.Contains(((int)SysMesListEnum.DetailShowPublishDate).ToString());
            bool isDetailShowHitCount = thisColumn.SysMesList.Contains(((int)SysMesListEnum.DetailShowHitCount).ToString());
            bool isDetailShowAudit = thisColumn.SysMesList.Contains(((int)SysMesListEnum.DetailShowAudit).ToString());

            NewsContentDto newsContentDto = newsContent.ToModel<NewsContentDto>();
            result.Content = newsContentDto;

            var lables = from lab in _lableRepository.Where(d => newsContentDto.ParentCatalogue.Contains(d.Id))
                         select new KeyValuePair<string, string>(lab.Id, lab.Name);
            newsContentDto.ParentCatalogueKV = lables.ToList();
            newsContentDto.ParentCatalogue = string.Join(";", lables.Select(d => d.Value));
            var expendList = _newsConExpRepository.Where(d => !d.DeleteFlag && d.ColumnID == newsContentDto.ColumnID);
            var nextAuditKV = thisColumn.AuditFlowKV.FirstOrDefault(d => d.Key > newsContentDto.AuditStatus);
            result.NextAuditStatus = GetContentNextAuditStatus(newsContent.ColumnID, newsContent.AuditStatus);
            List<KeyValuePair<string, string>> expendResult = new List<KeyValuePair<string, string>>();
            foreach (var item in expendList)
            {
                expendResult.Add(new KeyValuePair<string, string>(item.Filed, item.FiledName));
            }
            result.ExpendFiledList = expendResult;
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取新闻内容的下个审核状态集合
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="currentAudtiStatus"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, string>> GetContentNextAuditStatus(string columnID, int currentAudtiStatus)
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>();
            var column = _newsColumnRepository.FirstOrDefault(d => !d.DeleteFlag && d.Id == columnID);
            int[] auditFlow = column.AuditFlow.Split(';').Select(d => Converter.ObjectToInt(d)).OrderBy(d => d).ToArray();
            //取下一状态的值
            var auditNext = currentAudtiStatus == (int)AuditStatusEnum.Published ? (int)AuditStatusEnum.Published : auditFlow.Where(d => d > currentAudtiStatus).FirstOrDefault();
            var nextAuditStatus = auditNext;
            //nextAuditStatus = nextAuditStatus > 1 ? nextAuditStatus - 1 : nextAuditStatus;//审核状态和审核流程差值1
            var cur = Converter.ToType<AuditStatusEnum>(currentAudtiStatus);
            var next = Converter.ToType<AuditStatusEnum>(nextAuditStatus);
            result.Add(new KeyValuePair<int, string>(nextAuditStatus, EnumUtils.GetDescription(cur) + "通过"));
            List<AuditStatusEnum> unAudit = new List<AuditStatusEnum>() { AuditStatusEnum.UnPreliminaryAudit, AuditStatusEnum.UnSecondAudit, AuditStatusEnum.UnFinallyAudit };
            if (unAudit.Contains(next))//待初审，待二审，待终审 需要返回一个不通过的对应状态
                result.Add(new KeyValuePair<int, string>(((int)AuditStatusEnum.SendBack), EnumUtils.GetDescription(cur) + "不通过"));
            if (next == AuditStatusEnum.Published)
            {
                result.Add(new KeyValuePair<int, string>(((int)AuditStatusEnum.SendBack), EnumUtils.GetDescription(AuditStatusEnum.SendBack)));
            }


            return result;
        }

        /// <summary>
        /// 获取前台列表新闻数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<PagedList<FrontNewsContentListView>> GetProntNewsListData(FrontNewsListParm parm)
        {
            var thisColumn = _newsColumnRepository.FindOrDefault(parm.ColumnID);
            bool isListShowPublishDate = thisColumn.SysMesList.Contains(((int)SysMesListEnum.ListShowPublishDate).ToString());
            bool isListShowHitCount = thisColumn.SysMesList.Contains(((int)SysMesListEnum.ListShowHitCount).ToString());
            bool isListShowContentAttr = thisColumn.SysMesList.Contains(((int)SysMesListEnum.ListShowContentAttr).ToString());
            bool isListShowNewsLable = thisColumn.SysMesList.Contains(((int)SysMesListEnum.ListShowNewsLable).ToString());

            //新闻内容
            PagedList<NewsContent> pageList = await _newsContentRepository.Entities.Where(d => !d.DeleteFlag && d.Status == 1 && d.AuditStatus == ((int)AuditStatusEnum.Published)
            && (d.ColumnIDs.Contains(parm.ColumnID) || d.ColumnID == parm.ColumnID)
            && (string.IsNullOrEmpty(parm.LableID) ? true : d.ParentCatalogue.Contains(parm.LableID))
            && (string.IsNullOrEmpty(parm.SearchKey) ? true : (d.Title.Contains(parm.SearchKey) || d.Content.Contains(parm.SearchKey)))
            ).OrderByDescending(d => d.OrderNum).ToPagedListAsync(parm.PageIndex, parm.PageSize);

            PagedList<FrontNewsContentListView> contentList = new PagedList<FrontNewsContentListView>()
            {
                PageIndex = pageList.PageIndex,
                PageSize = pageList.PageSize,
                TotalCount = pageList.TotalCount,
                TotalPages = pageList.TotalPages,
                HasPrevPages = pageList.HasPrevPages,
                HasNextPages = pageList.HasNextPages
            };
            List<FrontNewsContentListView> listItems = new List<FrontNewsContentListView>();
            int indexMark = 1;
            foreach (var item in pageList.Items)
            {
                var tt = StringUtils.FilterHTML(item.Content);
                var ttt = StringUtils.CutDBCString(StringUtils.FilterHTML(item.Content), 0, parm.ContentCutLength);

                string[] lableIDs = item.ParentCatalogue.Split(';');
                listItems.Add(new FrontNewsContentListView
                {
                    ContentID = item.Id,
                    MainColumnID = item.ColumnID,
                    Title = item.Title,
                    IsShowContent = isListShowContentAttr,
                    Content = StringUtils.CutDBCString(StringUtils.FilterHTML(item.Content), 0, parm.ContentCutLength),
                    IsShowPublishDate = isListShowPublishDate,
                    PublishDate = item.PublishDate,
                    IsShowHitCount = isListShowHitCount,
                    HitCount = item.HitCount,
                    IsShowLablesName = isListShowNewsLable,
                    LablesName = _lableRepository.Where(d => lableIDs.Contains(d.Id)).Select(d => d.Name).ToArray(),
                    ExternalLink = item.ExternalLink ?? "",
                });
                indexMark++;
            }
            contentList.Items = listItems;
            return await Task.FromResult(contentList);
        }


        /// <summary>
        /// 栏目审核流程变更后重置该栏目下所有未发布的新闻的审核状态为待提交
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> ResetAuditStatusNewsContent(string columnID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };

            //var processCount = await _newsContentRepository.Where(a => a.ColumnID == columnID).BatchUpdateAsync(a => new NewsContent { AuditStatus = ((int)AuditStatusEnum.UnSubmit), AuditProcessJson = "" });
            var processCount = await _newsContentRepository.Context.BatchUpdate<NewsContent>()
                .Set(d => d.AuditStatus, d => (int)AuditStatusEnum.UnSubmit)
                .Set(d => d.AuditProcessJson, d => "")
                .Where(d => !d.DeleteFlag && d.ColumnID == columnID && d.AuditStatus != (int)AuditStatusEnum.Published).ExecuteAsync();
            return result;
        }
        /// <summary>
        /// 根据内容id取栏目id
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNewsColumnIdByContentId(string contentId)
        {
            var content = _newsContentRepository.FirstOrDefault(e => !e.DeleteFlag && e.Id == contentId);
            if (content == null)
            {
                throw Oops.Oh("内容id有误").StatusCode(HttpStatusKeys.ExceptionCode);
            }

            return content.ColumnID;
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
            //根据被排序对象id 查出该对象信息
            var sourceModel = _newsContentRepository.FirstOrDefault(d => d.Id == sourceID);
            //筛选同栏目下的所有对象，按排序字段降序排列
            var sameLevel = _newsContentRepository.Where(d => !d.DeleteFlag && d.ColumnID == sourceModel.ColumnID).OrderByDescending(d => d.OrderNum);
            //校验  排序目标是否超出整个栏目的数量
            if (sortIndex > sameLevel.Count())
            {
                result.Succeeded = false;
                result.Message = "排序号超出同级目录个数";
                throw Oops.Oh(result.Message).StatusCode(HttpStatusKeys.ExceptionCode);
                //return result;
            }


            //要排序的目标对象信息
            var switchCata = sameLevel.Take(sortIndex).LastOrDefault();
            //判断是  升序or降序
            bool isUp = sourceModel.OrderNum < switchCata.OrderNum;
            var sourceSortIndex = sourceModel.OrderNum;
            sourceModel.OrderNum = switchCata.OrderNum;
            //优先执行其他的再执行交换的
            if (isUp)
            {
                _newsContentRepository.Context.BatchUpdate<NewsContent>()
                    .Set(d => d.OrderNum, d => d.OrderNum - 1)
                    .Where(d => !d.DeleteFlag && d.ColumnID == sourceModel.ColumnID && d.OrderNum <= switchCata.OrderNum && d.OrderNum >= sourceSortIndex).Execute();
            }
            else
            {
                _newsContentRepository.Context.BatchUpdate<NewsContent>()
                    .Set(d => d.OrderNum, d => d.OrderNum + 1)
                    .Where(d => !d.DeleteFlag && d.ColumnID == sourceModel.ColumnID && d.OrderNum >= switchCata.OrderNum && d.OrderNum <= sourceSortIndex).Execute();
            }
            _newsContentRepository.Update(sourceModel);
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
            //组装结果集
            var result = new ApiResultInfoModel { Succeeded = true };
            //取源ID对象的详情
            var sourceModel = _newsContentRepository.FirstOrDefault(d => d.Id == sourceID);
            //取被目标ID对象的详情
            var tagertModel = _newsContentRepository.FirstOrDefault(d => d.Id == targetCataID);
            //去相同栏目下的所有新闻
            var sameLevel = _newsContentRepository.Where(d => !d.DeleteFlag && d.ColumnID == sourceModel.ColumnID).OrderByDescending(d => d.OrderNum).ToList();
            //从后向前拖-取序号小于目标ID新闻的第一位
            //从前向后拖-取序号大于目标ID新闻的第一位
            var switchCata = isUp ? sameLevel.Where(d => d.OrderNum <= tagertModel.OrderNum)/*.TakeLast(1)*/.FirstOrDefault() : sameLevel.Where(d => d.OrderNum >= tagertModel.OrderNum).TakeLast(1).FirstOrDefault();
            //记录源对象原本的序号
            var sourceSortIndex = sourceModel.OrderNum;
            sourceModel.OrderNum = switchCata.OrderNum;
            //优先执行其他的再执行交换的

            if (isUp)
            {
                _newsContentRepository.Context.BatchUpdate<NewsContent>()
                    .Set(d => d.OrderNum, d => d.OrderNum - 1)
                    .Where(d => !d.DeleteFlag && d.ColumnID == sourceModel.ColumnID && d.OrderNum <= switchCata.OrderNum && d.OrderNum >= sourceSortIndex).Execute();
            }
            else
            {
                _newsContentRepository.Context.BatchUpdate<NewsContent>()
                    .Set(d => d.OrderNum, d => d.OrderNum + 1)
                    .Where(d => !d.DeleteFlag && d.ColumnID == sourceModel.ColumnID && d.OrderNum >= switchCata.OrderNum && d.OrderNum <= sourceSortIndex).Execute();
            }
            _newsContentRepository.Update(sourceModel);
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取前台新闻内容数据
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public async Task<FrontNewsContentView> GetProntNewsContent(string contentID)
        {
            FrontNewsContentView result = new FrontNewsContentView();
            var newsContent = _newsContentRepository.FirstOrDefault(e => !e.DeleteFlag && e.Id == contentID);
            var thisColumn = _newsColumnRepository.FirstOrDefault(e => !e.DeleteFlag && e.Id == newsContent.ColumnID);
            bool isDetailShowPublishDate = thisColumn.SysMesList.Contains(((int)SysMesListEnum.DetailShowPublishDate).ToString());
            bool isDetailShowHitCount = thisColumn.SysMesList.Contains(((int)SysMesListEnum.DetailShowHitCount).ToString());
            bool isDetailShowAudit = thisColumn.SysMesList.Contains(((int)SysMesListEnum.DetailShowAudit).ToString());
            result.IsShowPublishDate = isDetailShowPublishDate;
            result.IsShowHitCount = isDetailShowHitCount;
            result.IsShowAuditProcess = isDetailShowAudit;

            //扩展字段
            result.IsShowAuthor = thisColumn.Extension.Contains("Author-");
            result.IsShowKeywords = thisColumn.Extension.Contains("Keywords-");
            result.IsShowExpirationDate = thisColumn.Extension.Contains("ExpirationDate-");
            result.IsShowJumpLink = thisColumn.Extension.Contains("JumpLink-");
            result.IsShowParentCatalogue = thisColumn.Extension.Contains("ParentCatalogue-");
            result.IsShowExpendFiled1 = thisColumn.Extension.Contains("ExpendFiled1-");
            result.IsShowExpendFiled2 = thisColumn.Extension.Contains("ExpendFiled2-");
            result.IsShowExpendFiled3 = thisColumn.Extension.Contains("ExpendFiled3-");
            result.IsShowExpendFiled4 = thisColumn.Extension.Contains("ExpendFiled4-");
            result.IsShowExpendFiled5 = thisColumn.Extension.Contains("ExpendFiled5-");
            NewsContentDto newsContentDto = newsContent.ToModel<NewsContentDto>();

            if (!string.IsNullOrEmpty(newsContent.AuditProcessJson))
            {
                result.AuditProcessList = JsonSerializer.Deserialize<List<FrontNewsAuditProcess>>(newsContent.AuditProcessJson);
            }

            var lables = from lab in _lableRepository.Where(d => newsContentDto.ParentCatalogue.Contains(d.Id))
                         select new KeyValuePair<string, string>(lab.Id, lab.Name);
            newsContentDto.ParentCatalogueKV = lables.ToList();
            result.Content = newsContentDto;
            //content hitcount++
            newsContent.HitCount++;
            _newsContentRepository.Update(newsContent);

            var expendList = _newsConExpRepository.Where(d => !d.DeleteFlag && d.ColumnID == newsContentDto.ColumnID);

            List<KeyValuePair<string, string>> expendResult = new List<KeyValuePair<string, string>>();
            foreach (var item in expendList)
            {
                expendResult.Add(new KeyValuePair<string, string>(item.Filed, item.FiledName));
            }
            result.ExpendFiledList = expendResult;
            return await Task.FromResult(result);
        }

        //TODO 调用评论应用
        public async Task<List<FrontNewsCommentView>> GetFrontNewsComment(int contentID)
        {
            List<FrontNewsCommentView> commentList = new List<FrontNewsCommentView>();

            return await Task.FromResult(commentList);
        }

        /// <summary>
        /// 获取管理员发布的新闻id-Name键值对
        /// </summary>
        /// <param name="managerID">管理员ID 可空 空则查询全部</param>
        /// <param name="counts">返回个数</param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetPublishNewsContentList(string managerID, int counts)
        {
            var result = from con in _newsContentRepository.Where(d => !d.DeleteFlag)
                         where (string.IsNullOrEmpty(managerID) ? true : con.Publisher == managerID)
                         select new KeyValuePair<string, string>(con.Id, con.Title);

            return await Task.FromResult(result.Take(counts).ToList());
        }

        /// <summary>
        /// 获取新闻总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetAllNewsContentCount()
        {
            var result = await _newsContentRepository.CountAsync(d => !d.DeleteFlag);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取管理员具有相关权限对应审核状态的新闻数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<WorkbenchAuditPageView> GetWorkbenchAudit(WorkbenchAuditNewsListParam param)
        {
            WorkbenchAuditPageView result = new WorkbenchAuditPageView();
            List<WorkbenchAuditView> auduitList = new List<WorkbenchAuditView>();
            var permissionColumnDic = _newsSettingsService.GetPermissionColumnList(param.ManagerID).Result;
            foreach (var item in permissionColumnDic)
            {
                AuditPowerEunm? auditPower = Converter.ToType<AuditPowerEunm?>(item.Key, null);
                if (auditPower == null)
                    continue;
                AuditStatusEnum auditStatus = AuditStatusEnum.UnSubmit;//待提交
                AuditStatusEnum? attachAuditStatus = null;
                switch (auditPower.Value)
                {
                    case AuditPowerEunm.Edit:
                        auditStatus = AuditStatusEnum.UnSubmit;
                        attachAuditStatus = AuditStatusEnum.SendBack;
                        break;
                    case AuditPowerEunm.PreliminaryAudit:
                        auditStatus = AuditStatusEnum.UnPreliminaryAudit;
                        break;
                    case AuditPowerEunm.PreliminaryCheck:
                        auditStatus = AuditStatusEnum.UnPreliminaryCheck;
                        break;
                    case AuditPowerEunm.SecondAudit:
                        auditStatus = AuditStatusEnum.UnSecondAudit;
                        break;
                    case AuditPowerEunm.SecondCheck:
                        auditStatus = AuditStatusEnum.UnSecondCheck;
                        break;
                    case AuditPowerEunm.FinallyAudit:
                        auditStatus = AuditStatusEnum.UnFinallyAudit;
                        break;
                    case AuditPowerEunm.FinallyCheck:
                        auditStatus = AuditStatusEnum.UnFinallyCheck;
                        break;
                    case AuditPowerEunm.Publish:
                        auditStatus = AuditStatusEnum.UnPublish;
                        break;
                    default:
                        break;
                }
                int newsCount = _newsContentRepository.AsEnumerable().Count(d => !d.DeleteFlag && d.AuditStatus == ((int)auditStatus) && item.Value.Contains(d.ColumnID));
                auduitList.Add(new WorkbenchAuditView
                {
                    AuditStatus = ((int)auditStatus),
                    StatusName = EnumUtils.GetName(auditStatus),
                    Count = newsCount,
                    IsCurrent = ((int)auditStatus) == param.AuditStatus
                });
                if (attachAuditStatus != null)//附加状态针对已退回
                {
                    newsCount = _newsContentRepository.AsEnumerable().Count(d => !d.DeleteFlag && d.AuditStatus == ((int)attachAuditStatus) && item.Value.Contains(d.ColumnID));
                    auduitList.Add(new WorkbenchAuditView
                    {
                        AuditStatus = ((int)attachAuditStatus),
                        StatusName = EnumUtils.GetName(attachAuditStatus),
                        Count = newsCount,
                        IsCurrent = ((int)attachAuditStatus) == param.AuditStatus
                    });
                }

                //获取新闻分页
                if (((int)auditStatus) == param.AuditStatus)
                {
                    Expression<Func<NewsContent, bool>> pre = s => !s.DeleteFlag;
                    pre = pre.And(s => s.AuditStatus == ((int)auditStatus));
                    Expression<Func<NewsContent, bool>> preCol = s => 1 == 1;
                    int i = 0;
                    foreach (var lq in item.Value.Split(';'))
                    {
                        if (i == 0)
                            preCol = s => s.ColumnID == lq;
                        else
                            preCol = preCol.Or(s => s.ColumnID == lq);
                        i++;
                    }
                    pre = pre.And(preCol);
                    //var pageList = _newsContentRepository.Entities.Where(d => !d.DeleteFlag && d.AuditStatus == ((int)auditStatus) && d.ColumnIDs.Split(';', StringSplitOptions.None).ToList().Any(c => item.Value.Contains(c))
                    //).OrderByDescending(d => d.PublishDate).ToPagedList(param.PageIndex, param.PageSize);
                    var pageList = _newsContentRepository.Entities.Where(pre).OrderByDescending(d => d.OrderNum).ToPagedList(param.PageIndex, param.PageSize);
                    PagedList<WorkbenchNews> contentList = new PagedList<WorkbenchNews>()
                    {
                        PageIndex = pageList.PageIndex,
                        PageSize = pageList.PageSize,
                        TotalCount = pageList.TotalCount,
                        TotalPages = pageList.TotalPages,
                        HasPrevPages = pageList.HasPrevPages,
                        HasNextPages = pageList.HasNextPages
                    };
                    List<WorkbenchNews> listItems = new List<WorkbenchNews>();
                    foreach (var newsItem in pageList.Items)
                    {
                        string[] lableIDs = newsItem.ParentCatalogue.Split(';');
                        listItems.Add(new WorkbenchNews
                        {
                            ContentID = newsItem.Id,
                            ColumnID = newsItem.ColumnID,
                            Title = newsItem.Title,
                            PublishDate = newsItem.PublishDate,
                        });
                    }
                    contentList.Items = listItems;
                    result.NewsList = contentList;
                }
            }
            result.AuditList = auduitList;
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取单条新闻的操作日志
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public async Task<List<ContentProcessLog>> GetContentProcessLog(string contentID)
        {
            List<ContentProcessLog> result = new List<ContentProcessLog>();
            //TODO 调用日志接口
            result.Add(new ContentProcessLog
            {
                EventName = "攥稿保存",
                Operator = "cqviptest",
                OperatorName = "测试管理员",
                NoteInfo = "",
                OperateTime = DateTime.Now.AddHours(-50)
            });
            result.Add(new ContentProcessLog
            {
                EventName = "提交审查",
                Operator = "cqviptest",
                OperatorName = "测试管理员",
                NoteInfo = "",
                OperateTime = DateTime.Now.AddHours(-48)
            });
            result.Add(new ContentProcessLog
            {
                EventName = "初审新闻",
                Operator = "cqviptest",
                OperatorName = "测试管理员",
                NoteInfo = "",
                OperateTime = DateTime.Now.AddHours(-4050)
            });
            result.Add(new ContentProcessLog
            {
                EventName = "发布新闻",
                Operator = "cqviptest",
                OperatorName = "测试管理员",
                NoteInfo = "",
                OperateTime = DateTime.Now.AddHours(-4050)
            });
            return await Task.FromResult(result);
        }

        public async Task<string> GrpcBaseUriGet(string appId)
        {

            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            AppBaseUriRequest request1 = new AppBaseUriRequest { AppRouteCode = appId };
            AppBaseUriReply reply1 = new AppBaseUriReply();
            try
            {
                reply1 = grpcClient1.GetAppBaseUri(request1);
                return reply1.FrontUrl;
            }
            catch (Exception ex)
            {
                throw Oops.Oh($"应用中心调用异常{ex}");

            }
        }

        /// <summary>
        /// 获取场景新闻信息
        /// </summary>
        /// <param name="columnID">栏目ID</param>
        /// <param name="topNum">新闻条数</param>
        /// <param name="sortField">排序字段 OrderNum,PublishDate,CreatedTime</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public async Task<ProntScenesNewsView> GetProntScenesNews(string columnID, int topNum, string sortField, bool isAsc)
        {


            ProntScenesNewsView result = new ProntScenesNewsView();
            var column = _newsColumnRepository.FirstOrDefault(d => !d.DeleteFlag && d.Id == columnID);
            if (column == null)
                return null;

            var newsHost = await GrpcBaseUriGet("news");

            var jumpListUri = $"{newsHost}/#/web_newsList?cid={columnID}";


            result.ColumnID = column.Id;
            result.ColumnName = column.Title;
            result.DefaultTemplate = column.DefaultTemplate;
            result.JumpLink = jumpListUri;

            var newsList = _newsContentRepository.Entities.Where(d => !d.DeleteFlag && d.Status == 1 && d.AuditStatus == ((int)AuditStatusEnum.Published)
            && (d.ColumnIDs.Contains(columnID) || d.ColumnID == columnID)
            );

            if (!string.IsNullOrEmpty(sortField))
                newsList = newsList.ApplyOrderCustomize(sortField, isAsc);
            else
                newsList = newsList.ApplyOrderCustomize("OrderNum", isAsc);
            newsList = newsList.Take(topNum);

            List<ProntScenesNewsContentView> listNewView = new List<ProntScenesNewsContentView>();
            foreach (var item in newsList.ToList())
            {
                var lables = _lableRepository.FirstOrDefault(d => item.ParentCatalogue.Contains(d.Id));
                var lableName = lables == null ? "" : lables.Name;
                listNewView.Add(new ProntScenesNewsContentView()
                {
                    ContentID = item.Id,
                    Title = item.Title,
                    Lables = lableName,
                    JumpLink = $"{newsHost}/#/web_newsDetails?cid={columnID}&id={item.Id}",
                    CreateTime = item.CreatedTime.UtcDateTime,
                    PublishDate = item.PublishDate,
                    ExternalLink = item.ExternalLink,
                    Cover = item.Cover
                });
            }
            result.ContentList = listNewView;
            return await Task.FromResult(result);
        }

        #endregion

        #region NewsContentExpend 新闻内容扩展字段
        public async void ProcessNewsContentExpend(string columnID, string newExtension, string oldExtension = "")
        {
            string[] expendFileds = new string[] { nameof(NewsContent.ExpendFiled1), nameof(NewsContent.ExpendFiled2), nameof(NewsContent.ExpendFiled3), nameof(NewsContent.ExpendFiled4), nameof(NewsContent.ExpendFiled5) };
            List<KeyValuePair<string, string>> oldExt = new List<KeyValuePair<string, string>>();
            foreach (var item in oldExtension.Split(';'))
            {
                if (item.Split('-').Count() == 2 && expendFileds.Contains(item.Split('-')[0]) && !string.IsNullOrEmpty(item.Split('-')[1]))
                {
                    oldExt.Add(new KeyValuePair<string, string>(item.Split('-')[0], item.Split('-')[1]));
                }
            }

            List<KeyValuePair<string, string>> newExt = new List<KeyValuePair<string, string>>();
            foreach (var item in newExtension.Split(';'))
            {
                if (item.Split('-').Count() == 2 && expendFileds.Contains(item.Split('-')[0]) && !string.IsNullOrEmpty(item.Split('-')[1]))
                {
                    string filed = item.Split('-')[0];
                    string filedName = item.Split('-')[1];
                    //newExt.Add(new KeyValuePair<string, string>(item.Split('-')[0], item.Split('-')[1]));
                    var oldExtFiled = oldExt.FirstOrDefault(d => d.Key == filed);

                    //原自定义字段不存在则新增
                    if (!oldExt.Any(d => d.Key == filed))
                    {
                        NewsContentExpendDto extModel = new NewsContentExpendDto()
                        {
                            ColumnID = columnID,
                            Filed = filed,
                            FiledName = filedName
                        };
                        await AddNewsContentExpend(extModel);
                    }
                    //原自定义字段存在且字段名不一致则更新 然后从原自定义中移除
                    else if (oldExt.Any(d => d.Key == filed) && oldExtFiled.Value != filedName)
                    {
                        await UpdateNewsContentExpend(columnID, filed, filedName);
                        oldExt.Remove(oldExtFiled);
                    }
                    else
                        oldExt.Remove(oldExtFiled);
                }
            }

            foreach (var item in oldExt)
            {
                await DeleteNewsContentExpend(columnID, item.Key);
            }

        }

        //public async void ProcessNewsContentExpend(string columnID, string newExtension, string oldExtension = "")
        //{
        //    string[] expendFileds = new string[] { nameof(NewsContent.ExpendFiled1), nameof(NewsContent.ExpendFiled2), nameof(NewsContent.ExpendFiled3), nameof(NewsContent.ExpendFiled4), nameof(NewsContent.ExpendFiled5) };
        //    List<KeyValuePair<string, string>> oldExt = new List<KeyValuePair<string, string>>();
        //    if (!string.IsNullOrEmpty(oldExtension))
        //        oldExt = System.Text.Json.JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(oldExtension);

        //    List<KeyValuePair<string, string>> newExt = new List<KeyValuePair<string, string>>();
        //    if (!string.IsNullOrEmpty(newExtension))
        //        newExt = System.Text.Json.JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(newExtension);
        //    foreach (var item in newExt)
        //    {
        //        if (expendFileds.Contains(item.Key) && !string.IsNullOrEmpty(item.Value))
        //        {
        //            string filed = item.Key;
        //            string filedName = item.Value;
        //            var oldExtFiled = oldExt.FirstOrDefault(d => d.Key == filed);

        //            //原自定义字段不存在则新增
        //            if (!oldExt.Any(d => d.Key == filed))
        //            {
        //                NewsContentExpendDto extModel = new NewsContentExpendDto()
        //                {
        //                    ColumnID = columnID,
        //                    Filed = filed,
        //                    FiledName = filedName
        //                };
        //                await AddNewsContentExpend(extModel);
        //            }
        //            //原自定义字段存在且字段名不一致则更新 然后从原自定义中移除
        //            else if (oldExt.Any(d => d.Key == filed) && oldExtFiled.Value != filedName)
        //            {
        //                await UpdateNewsContentExpend(columnID, filed, filedName);
        //                oldExt.Remove(oldExtFiled);
        //            }
        //            else
        //                oldExt.Remove(oldExtFiled);
        //        }
        //    }

        //    foreach (var item in oldExt)
        //    {
        //        await DeleteNewsContentExpend(columnID, item.Key);
        //    }

        //}

        /// <summary>
        /// 添加新闻内容扩展字段
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> AddNewsContentExpend(NewsContentExpendDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            model.Id = Time2KeyUtils.GetRandOnlyId();
            var newsConExp = model.ToModel<NewsContentExpend>();
            var modelDB = _newsConExpRepository.Entities.AsNoTracking().FirstOrDefault(d => d.ColumnID == newsConExp.ColumnID && d.Filed == newsConExp.Filed);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "新闻内容扩展字段不存在！";
                return result;
            }
            if (modelDB != null)
            {
                modelDB.DeleteFlag = false;
                modelDB.FiledName = model.FiledName;
                modelDB.UpdatedTime = DateTime.Now;
                var newsColumn = await _newsConExpRepository.UpdateAsync(modelDB);
            }
            else
            {
                newsConExp.CreatedTime = DateTime.Now;
                var newsCE = await _newsConExpRepository.InsertAsync(newsConExp);
            }

            return result;
        }

        /// <summary>
        /// 获取新闻内容扩展字段
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<List<NewsContentExpendDto>> GetNewsContentExpend(string columnID)
        {
            var result = _newsConExpRepository.Entities.Where(d => !d.DeleteFlag && d.ColumnID == columnID
            );
            return await Task.FromResult(result.ToModelList<NewsContentExpendDto>());
        }

        /// <summary>
        /// 获取新闻内容扩展字段
        /// </summary>
        /// <param name="expendIDs"></param>
        /// <returns></returns>
        public async Task<List<NewsContentExpendDto>> AddNewsContentExpend(string[] expendIDs)
        {
            var result = _newsConExpRepository.Entities.Where(d => !d.DeleteFlag && expendIDs.Contains(d.Id)
            );
            return await Task.FromResult(result.ToModelList<NewsContentExpendDto>());
        }

        /// <summary>
        /// 更新新闻内容扩展字段
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="filed"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> UpdateNewsContentExpend(string columnID, string filed, string name)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var model = _newsConExpRepository.Entities.AsNoTracking().FirstOrDefault(d => d.ColumnID == columnID && d.Filed == filed);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "新闻内容扩展字段不存在！";
                return result;
            }
            model.DeleteFlag = false;
            model.FiledName = name;
            model.UpdatedTime = DateTime.Now;
            var newsColumn = await _newsConExpRepository.UpdateAsync(model);
            return result;
        }

        /// <summary>
        /// 删除新闻内容扩展字段
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNewsContentExpend(string columnID, string filed)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var model = _newsConExpRepository.FirstOrDefault(d => !d.DeleteFlag && d.ColumnID == columnID && d.Filed == filed);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "新闻内容扩展字段不存在！";
                return result;
            }
            model.DeleteFlag = true;
            model.UpdatedTime = DateTime.Now;
            await _newsConExpRepository.UpdateAsync(model);
            return result;
        }
        #endregion
    }
}
