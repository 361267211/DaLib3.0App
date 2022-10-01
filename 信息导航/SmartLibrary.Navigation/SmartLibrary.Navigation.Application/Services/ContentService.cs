using DotNetCore.CAP;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using LinqKit;
using Mapster;
using SmartLibrary.Navigation.Application.Enums;
using SmartLibrary.Navigation.Application.PageParam;
using SmartLibrary.Navigation.Application.ViewModel;
using SmartLibrary.Navigation.Common.Extensions;
using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibrary.Navigation.EntityFramework.Core.Entitys;
using SmartLibrary.Navigation.Utility;
using SmartLibrary.Search.EsSearchProxy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    /// <summary>
    /// 名    称：ContentService
    /// 作    者：张泽军
    /// 创建时间：2021/10/11 9:04:50
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ContentService : IContentService, IScoped
    {
        private ICapPublisher _CapPublisher;
        private IRepository<NavigationLableInfo> _LableRepository;
        private IRepository<NavigationColumn> _ColumnRepository;
        private IRepository<Content> _ContentRepository;
        private IRepository<NavigationColumnPermissions> _PermissionRepository;
        private IRepository<NavigationCatalogue> _CatalogueRepository;
        private INavigationSettingsService _SettingsService;
        private INavigationLableInfoService _LableService;
        private readonly IEsProxyService _EsProxyService;
        private TenantInfo _TenantInfo;

        public ContentService(ICapPublisher capPublisher,
                              IRepository<NavigationLableInfo> lableRepository,
                              IRepository<NavigationColumn> columnRepository,
                              IRepository<Content> contentRepository,
                              INavigationSettingsService settingsService,
                              INavigationLableInfoService lableService,
                              IRepository<NavigationColumnPermissions> permissionRepository,
                              IRepository<NavigationCatalogue> catalogueRepository,
                              IEsProxyService esProxyService,
                              TenantInfo tenantInfo)
        {
            _CapPublisher = capPublisher;
            _LableRepository = lableRepository;
            _ColumnRepository = columnRepository;
            _ContentRepository = contentRepository;
            _SettingsService = settingsService;
            _LableService = lableService;
            _PermissionRepository = permissionRepository;
            _CatalogueRepository = catalogueRepository;
            _TenantInfo = tenantInfo;
            _EsProxyService = esProxyService;
        }

        /// <summary>
        /// 添加内容
        /// </summary>
        /// <param name="model"></param>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> AddContent(ContentDto model, string columnID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var maxSortIndex = 0;
            if (_CatalogueRepository.Count() > 0)
                maxSortIndex = _CatalogueRepository.Max(d => d.SortIndex);
            model.SortIndex = maxSortIndex + 1;
            model.Creator = _TenantInfo.UserKey;
            //model.CreatorName = _tenantInfo.UserKey;
            var cata = _CatalogueRepository.FirstOrDefault(d => d.Id == model.CatalogueID);
            if (cata == null)
            {
                result.Succeeded = false;
                result.Message = "所属目录不存在";
                return result;
            }
            if (!string.IsNullOrEmpty(cata.AssociatedCatalog))
            {
                result.Succeeded = false;
                result.Message = "所属目录为关联目录，不能添加内容";
                return result;
            }
            var temp = model.Adapt<Content>();
            temp.CreatedTime = DateTime.Now;
            await _ContentRepository.InsertAsync(temp);

            await this.SaveNavigationToES(temp, columnID);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 修改内容
        /// </summary>
        /// <param name="model"></param>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> UpdateContent(ContentDto model, string columnID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var maxSortIndex = 0;
            if (_CatalogueRepository.Count() > 0)
                maxSortIndex = _CatalogueRepository.Where(d => !d.DeleteFlag).Max(d => d.SortIndex);
            model.SortIndex = maxSortIndex + 1;
            model.Creator = _TenantInfo.UserKey;
            //model.CreatorName = _tenantInfo.UserKey;

            var cata = _CatalogueRepository.FirstOrDefault(d => d.Id == model.CatalogueID);
            if (cata == null)
            {
                result.Succeeded = false;
                result.Message = "所属目录不存在";
                return result;
            }
            if (!string.IsNullOrEmpty(cata.AssociatedCatalog))
            {
                result.Succeeded = false;
                result.Message = "所属目录为关联目录，不能添加内容";
                return result;
            }
            var temp = model.Adapt<Content>();
            temp.UpdatedTime = DateTime.Now;
            await _ContentRepository.UpdateExcludeAsync(temp, new[] { nameof(temp.CreatedTime), nameof(temp.TenantId), nameof(temp.SortIndex) });

            await this.SaveNavigationToES(temp, columnID);

            return await Task.FromResult(result);
        }


        /// <summary>
        /// 保存专题到ES检索库
        /// </summary>
        /// <param name="content"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        private async Task SaveNavigationToES(Content content, string cid)
        {
            //插入到ES检索库当中
            var fulltext = $"{content.ContentsText}";
            var request = new Search.EsSearchProxy.Core.Dto.UpsertOwnerNewsRequestParameter
            {
                fulltext = fulltext,
                owner = App.HttpContext.EnsureOwner(),
                docId = $"navigation_{App.HttpContext.EnsureOwner()}_{content.Id.ToString().Replace('-', '_')}",
                app_id = "navigation",
                app_type = Search.EsSearchProxy.Core.Models.OrganNewsType.Service,
                title = content.Title,
                summary = fulltext.Substring(0, fulltext.Length > 4000 ? 4000 : fulltext.Length),
                url = $"/#/web_detailspage1?id={content.Id}&c_id={cid}",
                pub_time = content.CreatedTime,
                update_time = content.UpdatedTime.HasValue ? content.UpdatedTime.Value : content.CreatedTime,
            };

            try
            {
                var esItem = await _EsProxyService.UpsertOrganNewsAsync(request);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 获取导航内容
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public async Task<ContentDto> GetContent(string contentID)
        {
            var model = await _ContentRepository.FirstOrDefaultAsync(d => d.Id == contentID);
            if (model == null)
                return null;
            var result = model.Adapt<ContentDto>();
            result.RelationCatalogueIDsKV = _CatalogueRepository.Where(d => !d.DeleteFlag && result.RelationCatalogueIDs.Contains(d.Id)).ToDictionary(x => x.Id, y => y.Title).ToList();

            return result;
        }

        /// <summary>
        /// 获取信息导航内容列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="columnID"></param>
        /// <param name="keywords"></param>
        /// <param name="cataID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<PagedList<ContentDto>> GetContentList(int pageIndex, int pageSize, string columnID, string keywords, string cataID, bool? status)
        {
            var listCata = _CatalogueRepository.DetachedEntities.Where(d => !d.DeleteFlag && d.ColumnID == columnID).Select(d => d.Id);

            var pageList = await _ContentRepository.DetachedEntities.Where(d => !d.DeleteFlag
                      && (listCata != null && listCata.Any() ? listCata.Contains(d.CatalogueID) : false)
                      && (string.IsNullOrEmpty(keywords) ? true : (d.Title.Contains(keywords) || d.Contents.Contains(keywords)))
                      && (string.IsNullOrEmpty(cataID) ? true : d.CatalogueID == cataID)
                      && (status == null ? true : d.Status == status)
            ).OrderBy(d => d.CatalogueID)
            .ThenByDescending(d => d.SortIndex)
            .ToPagedListAsync(pageIndex, pageSize);

            var result = new PagedList<ContentDto>
            {
                PageIndex = pageList.PageIndex,
                PageSize = pageList.PageSize,
                TotalCount = pageList.TotalCount,
                TotalPages = pageList.TotalPages,
                HasNextPages = pageList.HasNextPages,
                HasPrevPages = pageList.HasPrevPages
            };
            List<ContentDto> listConDto = new List<ContentDto>();
            int i = 0;
            foreach (var item in pageList.Items)
            {
                i++;
                var dto = item.Adapt<ContentDto>();
                dto.IndexNum = (pageIndex - 1) * pageSize + i;
                dto.CatalogueName = GetCataFullName(dto.CatalogueID);
                dto.RelationCatalogueIDsKV = _CatalogueRepository.DetachedEntities.Where(d => !d.DeleteFlag && dto.RelationCatalogueIDs.Contains(d.Id)).ToDictionary(x => x.Id, y => y.Title).ToList();
                dto.UpdateTime = item.UpdatedTime;
                listConDto.Add(dto);
            }
            result.Items = listConDto;

            return result;
        }

        /// <summary>
        /// 获取目录名称
        /// </summary>
        /// <param name="cataID"></param>
        /// <returns></returns>
        private string GetCataFullName(string cataID)
        {
            var cata = _CatalogueRepository.DetachedEntities.FirstOrDefault(d => !d.DeleteFlag && d.Id == cataID);
            if (cata == null)
                return "";
            var fullPathCode = cata.PathCode.Split('_', StringSplitOptions.RemoveEmptyEntries);
            var cataList = _CatalogueRepository.DetachedEntities.Where(d => !d.DeleteFlag && fullPathCode.Contains(d.Id))
                            .OrderBy(d => d.PathCode.Length).ToList();
            return string.Join("：", cataList.Select(d => d.Title));
        }

        /// <summary>
        /// 批量下架/上架内容
        /// </summary>
        /// <param name="contentIDList"></param>
        /// <param name="isNormal"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> ChangeContentStatus(string[] contentIDList, bool isNormal)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            foreach (var item in contentIDList)
            {
                var content = _ContentRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && d.Id == item);
                if (result == null)
                {
                    result.Succeeded = false;
                    result.Message = $"{item}:内容不存在";
                    return result;
                }
                content.Status = isNormal;
                content.UpdatedTime = DateTime.Now;
                await _ContentRepository.UpdateNowAsync(content);
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 批量删除内容
        /// </summary>
        /// <param name="contentIDList"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteContent(string[] contentIDList)
        {
            var result = new ApiResultInfoModel { Succeeded = true };

            foreach (var contentID in contentIDList)
            {
                var model = _ContentRepository.FindOrDefault(contentID);
                if (model == null)
                {
                    result.Succeeded = false;
                    result.Message = $"{contentID}:内容不存在";
                    return result;
                }
                model.DeleteFlag = true;
                model.UpdatedTime = DateTime.Now;
                await _ContentRepository.UpdateNowAsync(model);
            }

            return result;
        }

        /// <summary>
        /// 获取前台列表新闻数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<PagedList<FrontContentListView>> GetProntContentListData(FrontContentListParm parm)
        {
            var thisColumn = _ColumnRepository.FirstOrDefault(e => !e.DeleteFlag && e.Id == parm.ColumnID);

            bool isListShowPublishDate = thisColumn.SysMesList.Contains(((int)SysMesListEnum.ListShowPublishDate).ToString());
            bool isListShowHitCount = thisColumn.SysMesList.Contains(((int)SysMesListEnum.ListShowHitCount).ToString());
            bool isListShowContentAttr = thisColumn.SysMesList.Contains(((int)SysMesListEnum.ListShowContentAttr).ToString());

            List<NavigationCatalogue> cataList = new List<NavigationCatalogue>();
            if (!string.IsNullOrEmpty(parm.CataID))
            {
                cataList = _CatalogueRepository.DetachedEntities.Where(d => !d.DeleteFlag && d.Status && (d.Id == parm.CataID || d.PathCode.Contains(parm.CataID))).ToList();
            }
            else
            {
                cataList = _CatalogueRepository.DetachedEntities.Where(d => !d.DeleteFlag && d.Status && (d.ColumnID == parm.ColumnID)).ToList();
            }

            List<NavigationCatalogue> colCataList = new();
            colCataList.AddRange(cataList);

            foreach (var cata in cataList)
            {
                if (string.IsNullOrEmpty(cata.AssociatedCatalog))
                    continue;
                colCataList.AddRange(_CatalogueRepository.DetachedEntities.Where(d => !d.DeleteFlag && d.Status && (d.PathCode.Contains(cata.AssociatedCatalog + "_") || d.PathCode == cata.AssociatedCatalog)).ToList());
            }

            var cataIDs = colCataList.Select(d => d.Id).Distinct().ToList();
            //内容
            //动态SQL
            var aa = PredicateBuilder.New<Content>(c => !c.DeleteFlag && c.Status && cataIDs.Contains(c.CatalogueID));
            if (!string.IsNullOrWhiteSpace(parm.SearchKey))
            {
                aa = aa.And(c => c.Title.Contains(parm.SearchKey) || c.Contents.Contains(parm.SearchKey));
            }
            var bb = PredicateBuilder.New<Content>();
            foreach (var item in cataIDs)
            {
                bb = bb.Or(c => c.RelationCatalogueIDs.Contains(item));
            }
            aa = aa.Or(bb);

            PagedList<Content> pageList = await _ContentRepository.DetachedEntities
                        .Where((Expression<Func<Content, bool>>)aa.Expand())
                        .OrderByDescending(d => d.SortIndex)
                        .ToPagedListAsync(parm.PageIndex, parm.PageSize);

            PagedList<FrontContentListView> contentList = new()
            {
                PageIndex = pageList.PageIndex,
                PageSize = pageList.PageSize,
                TotalCount = pageList.TotalCount,
                TotalPages = pageList.TotalPages,
                HasPrevPages = pageList.HasPrevPages,
                HasNextPages = pageList.HasNextPages
            };
            List<FrontContentListView> listItems = new();
            int indexMark = 1;
            foreach (var item in pageList.Items)
            {
                listItems.Add(new FrontContentListView
                {
                    ContentID = item.Id,
                    ColumnID = parm.ColumnID,
                    Title = item.Title,
                    IsShowContent = isListShowContentAttr,
                    Content = StringUtils.CutDBCString(item.ContentsText, 0, parm.ContentCutLength),
                    IsShowPublishDate = isListShowPublishDate,
                    PublishDate = item.PublishDate,
                    IsShowHitCount = isListShowHitCount,
                    HitCount = item.HitCount,
                    LinkUrl = item.LinkUrl,
                });
                indexMark++;
            }
            contentList.Items = listItems;

            return contentList;
        }

        /// <summary>
        /// 获取前台新闻内容数据
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public async Task<FrontContentView> GetProntContent(string contentID)
        {
            FrontContentView result = new FrontContentView();
            var content = await _ContentRepository.FirstOrDefaultAsync(e => !e.DeleteFlag && e.Id == contentID);
            if (content == null)
                return null;
            var cataModel = await _CatalogueRepository.FirstOrDefaultAsync(d => !d.DeleteFlag && d.Id == content.CatalogueID);
            if (cataModel == null)
                return null;
            var thisColumn = await _ColumnRepository.FirstOrDefaultAsync(d => !d.DeleteFlag && d.Id == cataModel.ColumnID);
            if (thisColumn == null)
                return null;
            bool isDetailShowPublishDate = thisColumn.SysMesList.Contains(((int)SysMesListEnum.DetailShowPublishDate).ToString());
            bool isDetailShowHitCount = thisColumn.SysMesList.Contains(((int)SysMesListEnum.DetailShowHitCount).ToString());
            ContentDto contentDto = content.Adapt<ContentDto>();
            result = contentDto.Adapt<FrontContentView>();
            result.IsShowPublishDate = isDetailShowPublishDate;
            result.IsShowHitCount = isDetailShowHitCount;
            //content hitcount++
            content.HitCount++;
            await _ContentRepository.UpdateAsync(content);

            return await Task.FromResult(result);
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
            var sourceModel = _ContentRepository.FirstOrDefault(d => d.Id == sourceID);
            var sameLevel = _ContentRepository.Where(d => !d.DeleteFlag && d.CatalogueID == sourceModel.CatalogueID).OrderByDescending(d => d.SortIndex);
            if (sortIndex > sameLevel.Count())
            {
                result.Succeeded = false;
                result.Message = "排序号超出内容个数";
                return result;
            }
            var switchCata = sameLevel.Take(sortIndex).LastOrDefault();
            var sourceSortIndex = sourceModel.SortIndex;
            sourceModel.SortIndex = switchCata.SortIndex;
            //优先执行其他的再执行交换的
            _ContentRepository.Context.BatchUpdate<Content>()
                .Set(d => d.SortIndex, d => d.SortIndex - 1)
                .Where(d => !d.DeleteFlag && d.CatalogueID == sourceModel.CatalogueID && d.SortIndex <= switchCata.SortIndex && d.SortIndex > sourceSortIndex).Execute();
            _ContentRepository.Update(sourceModel);
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
            var sourceModel = _ContentRepository.FirstOrDefault(d => d.Id == sourceID);
            var tagertModel = _ContentRepository.FirstOrDefault(d => d.Id == targetCataID);

            //定义批量偏移的范围区间
            var maxIndex = isUp ? tagertModel.SortIndex : sourceModel.SortIndex;
            var minIndex = isUp ? sourceModel.SortIndex : tagertModel.SortIndex;

            //交换源和目标的序号
            sourceModel.SortIndex = tagertModel.SortIndex;
            //优先执行其他的再执行交换的

            int change = isUp ? -1 : 1;//上移则区间内都要下降（-1），下移则区间内都要上升（+1）
            _ContentRepository.Context.BatchUpdate<Content>()
                .Set(d => d.SortIndex, d => d.SortIndex + change)
                .Where(d => !d.DeleteFlag && d.SortIndex >= minIndex && d.SortIndex <= maxIndex).Execute();
            _ContentRepository.Update(sourceModel);


            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取单条内容的操作日志
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
            result.Add(new ContentProcessLog
            {
                EventName = "发布内容",
                Operator = "A974C116-2FA4-4445-BE8D-230BA83317F8",
                OperatorName = "测试管理员",
                NoteInfo = "",
                OperateTime = DateTime.Now.AddHours(-45)
            });
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 根据关键词检索导航目录
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public async Task<PagedList<ContentVo>> SearchNavigationContent(string keyWord, int pageIndex, int pageSize)
        {

            var query = from a in _ContentRepository.AsQueryable(e => !e.DeleteFlag && (e.ContentsText.Contains(keyWord ?? "") || e.Title.Contains(keyWord ?? "")))
                        join b in _CatalogueRepository.AsQueryable(e => !e.DeleteFlag) on a.CatalogueID equals b.Id
                        into result
                        from res in result
                        join c in _ColumnRepository.AsQueryable(e => !e.DeleteFlag) on res.ColumnID equals c.Id

                        select new ContentVo
                        {
                            Id = a.Id,
                            Title = a.Title,
                            CatalogueID = a.CatalogueID,
                            CatalogueName = res.Title,
                            ColumnId = res.ColumnID,
                            ColumnName = c.Title,
                            Publisher = a.Publisher,
                            CreatorName = a.CreatorName,
                            Status = a.Status,
                            UpdateTime = a.UpdatedTime,
                            CreatedTime = a.CreatedTime,
                        };
            var pageList = query.OrderByDescending(e => e.CreatedTime).ToPagedList(pageIndex, pageSize);
            return pageList;


        }

    }
}
