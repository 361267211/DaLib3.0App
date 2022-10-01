

using Furion;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DataEncryption;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.LinqBuilder;
using Grpc.Core;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using SmartLibrary.AppCenter;
using SmartLibrary.DatabaseTerrace.Application.Dto;
using SmartLibrary.DatabaseTerrace.Application.Interceptors;
using SmartLibrary.DatabaseTerrace.Application.Services.RemoteProxy;
using SmartLibrary.DatabaseTerrace.Common.Const;
using SmartLibrary.DatabaseTerrace.Common.Dtos;
using SmartLibrary.DatabaseTerrace.Common.Extensions;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Enum;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.SysConst;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.ViewModels;
using SmartLibrary.DataCenter;
using SmartLibrary.SceneManage;
using SmartLibrary.Search.EsSearchProxy.Core.Interfaces;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Application
{
    public class DatabaseTerraceService : IScoped, IDatabaseTerraceService
    {
        private readonly IDistributedCache _cache;

        private readonly IMapper _mapper;
        private readonly CenterHelper _http;

        private readonly IRepository<EntityFramework.Core.Entitys.DatabaseTerrace> _databaseRepository;
        private readonly IRepository<DatabaseAcessUrl> _directUrlRepository;
        private readonly IRepository<CustomLabel> _labelRepository;
        private readonly IRepository<DomainEscsAttr> _domainEscsAttrRepository;
        private readonly IRepository<DatabaseColumn> _databaseColumnRepository;
        private readonly IRepository<DatabaseColumnRule> _databaseColumnRuleRepository;
        private readonly IRepository<DatabaseTerraceSettings> _settingsRepository;
        private readonly IRepository<DatabaseUrlName> _databaseUrlNameRepository;
        private readonly IRepository<DatabaseSubscriber> _subscriberRepository;
        private readonly IRepository<ProviderResource> _providerResourceRepository;
        private readonly IRepository<DatabaseDefaultTemplate> _defaultTemplateRepository;

        private readonly IEsProxyService _esProxyService;


        public DatabaseTerraceService(
            CenterHelper http,
            IDistributedCache cache,
            IMapper mapper,
            IEsProxyService esProxyService,

            IRepository<EntityFramework.Core.Entitys.DatabaseTerrace> databaseRepository,
            IRepository<DatabaseAcessUrl> directUrlRepository,
            IRepository<CustomLabel> labelRepository,
            IRepository<DomainEscsAttr> domainEscsAttrRepository,
            IRepository<DatabaseColumn> databaseColumnRepository,
            IRepository<DatabaseColumnRule> databaseColumnRuleRepository,
            IRepository<DatabaseTerraceSettings> settingsRepository,
            IRepository<DatabaseUrlName> databaseUrlNameRepository,
            IRepository<DatabaseSubscriber> subscriberRepository,
            IRepository<ProviderResource> providerResourceRepository,
            IRepository<DatabaseDefaultTemplate> defaultTemplateRepository

            )
        {
            _http = http;
            _cache = cache;
            _mapper = mapper;
            _esProxyService = esProxyService;

            _databaseRepository = databaseRepository;
            _directUrlRepository = directUrlRepository;
            _labelRepository = labelRepository;
            _domainEscsAttrRepository = domainEscsAttrRepository;
            _databaseColumnRepository = databaseColumnRepository;
            _databaseColumnRuleRepository = databaseColumnRuleRepository;
            _settingsRepository = settingsRepository;
            _databaseUrlNameRepository = databaseUrlNameRepository;
            _subscriberRepository = subscriberRepository;
            _providerResourceRepository = providerResourceRepository;
            _defaultTemplateRepository = defaultTemplateRepository;
        }

        /// <summary>
        /// 获取数据库平台数据集合
        /// </summary>
        /// <param name="serachKey">检索关键词</param>
        /// <param name="languageId">语言类型</param>
        /// <param name="articleType">文献类型</param>
        /// <param name="domainEscs">学科分类</param>
        /// <param name="purchaseType">采购状态</param>
        /// <param name="status">状态</param>
        /// <param name="timeliness">时效性</param>
        /// <param name="initials">首字母</param>
        /// <param name="sortType">排序类型</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">单页数量</param>
        /// <returns></returns>
        public async Task<PagedList<DatabaseTerraceDto>> GetDatabaseTerraceList(string serachKey, int languageId, string articleType, string domainEscs, string purchaseType, int status, int timeliness, string initials, int sortType, string label, int pageIndex, int pageSize, bool? IsShow)
        {

            #region 组配表达式
            //组配表达式
            Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> lambda = x => !x.DeleteFlag;

            //TODO:获取供应商的 ID-名称  键值对,取 string[ID]
            // lambda = lambda.And(e => e.IsShow);

            //数据库是否前台可见
            if (IsShow.HasValue)
            {
                if (IsShow.Value)
                {
                    lambda = lambda.And(e => e.IsShow);
                }
            }

            //搜索关键字条件
            if (!string.IsNullOrEmpty(serachKey))
            {
                Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> keyWordLamda = e => e.Title.Contains(serachKey);
                keyWordLamda = keyWordLamda.Or(e => e.InformationText.Contains(serachKey));
                keyWordLamda = keyWordLamda.Or(e => e.UseGuideText.Contains(serachKey));
                keyWordLamda = keyWordLamda.Or(e => e.ResourceStatisticsText.Contains(serachKey));

                lambda = lambda.And(keyWordLamda);
            }
            //文献类型条件
            if (!string.IsNullOrEmpty(articleType))
            {
                lambda = lambda.And(e => e.ArticleTypes.Contains($";{articleType};"));
            }
            //语言类型
            if (languageId != 0)
            {
                lambda = lambda.And(e => e.Language == languageId.ToString());
            }
            //学科类型条件
            if (!string.IsNullOrEmpty(domainEscs))
            {
                lambda = lambda.And(e => e.DomainEscs.Contains($";{domainEscs};"));
            }
            //采购状态条件
            if (!string.IsNullOrEmpty(purchaseType))
            {
                lambda = lambda.And(e => e.PurchaseType == purchaseType);
            }
            //状态条件 如果为9 则仅排除隐藏数据库
            if (status == 9)
            {
                lambda = lambda.And(e => e.Status != 3);
            }
            else if (status != 0)
            {
                lambda = lambda.And(e => e.Status == status);
            }
            //首字母
            if (!string.IsNullOrEmpty(initials))
            {
                lambda = lambda.And(e => e.Initials == initials);
            }
            if (!string.IsNullOrEmpty(label))
            {
                lambda = lambda.And(e => e.Label.Contains(label));

            }

            //  筛选页签
            if (timeliness != 0)
            {
                switch (timeliness)
                {
                    case 1:
                        lambda = lambda.And(e => e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now && e.Status != 3);
                        break;
                    case 2:
                        lambda = lambda.And(e => e.ExpiryBeginTime > DateTime.Now || e.ExpiryEndTime < DateTime.Now);
                        break;
                    case 3:
                        lambda = lambda.And(e => e.Status == 3);
                        break;
                    default://前台默认找效期内的
                        lambda = lambda.And(e => e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now);
                        break;
                }
            }


            #endregion
            var databasePage = new PagedList<DatabaseTerraceDto>();

            switch (sortType)
            {
                case 1://默认排序
                    databasePage = _databaseRepository.Entities.Where(lambda).OrderByDescending(e => e.OrderIndex).ToPagedList(pageIndex, pageSize).Adapt<PagedList<DatabaseTerraceDto>>();
                    break;
                case 2://首字母排序
                    databasePage = _databaseRepository.Entities.Where(lambda).OrderBy(e => e.Initials).ToPagedList(pageIndex, pageSize).Adapt<PagedList<DatabaseTerraceDto>>();
                    break;
                case 3://访问量倒叙
                    databasePage = _databaseRepository.Entities.Where(lambda).OrderByDescending(e => e.TotalClickNum).ToPagedList(pageIndex, pageSize).Adapt<PagedList<DatabaseTerraceDto>>();
                    break;
                default:
                    databasePage = _databaseRepository.Entities.Where(lambda).OrderByDescending(e => e.OrderIndex).ToPagedList(pageIndex, pageSize).Adapt<PagedList<DatabaseTerraceDto>>();
                    break;
            }
            return databasePage;
        }

        public async Task<List<DatabaseTerraceDto>> GetAllDatabaseTerrace()
        {
            return _databaseRepository.Where(e => !e.DeleteFlag).ProjectToType<DatabaseTerraceDto>().ToList();

        }

        /// <summary>
        /// 获取3个页签的统计数据
        /// </summary>
        /// <param name="serachKey"></param>
        /// <param name="languageId"></param>
        /// <param name="articleType"></param>
        /// <param name="domainEscs"></param>
        /// <param name="purchaseType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<StatisticsCountDto> GetStatisticsCount(string serachKey, int languageId, string articleType, string domainEscs, string purchaseType, int status, bool isShow)
        {
            //组配表达式
            Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> lambda = x => !x.DeleteFlag;

            //TODO:获取供应商的 ID-名称  键值对,取 string[ID]

            if (isShow)
            {
                lambda = lambda.And(e => e.IsShow);
            }

            //搜索关键字条件
            if (!string.IsNullOrEmpty(serachKey))
            {
                Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> keyWordLamda = e => e.Title.Contains(serachKey);
                keyWordLamda = keyWordLamda.Or(e => e.InformationText.Contains(serachKey));
                keyWordLamda = keyWordLamda.Or(e => e.UseGuideText.Contains(serachKey));
                keyWordLamda = keyWordLamda.Or(e => e.ResourceStatisticsText.Contains(serachKey));

                lambda = lambda.And(keyWordLamda);
            }
            //文献类型条件
            if (!string.IsNullOrEmpty(articleType))
            {
                lambda = lambda.And(e => e.ArticleTypes.Contains($";{articleType};"));
            }
            //学科类型条件
            if (!string.IsNullOrEmpty(domainEscs))
            {
                lambda = lambda.And(e => e.DomainEscs.Contains($";{domainEscs};"));
            }
            //采购类型条件
            if (!string.IsNullOrEmpty(purchaseType))
            {
                lambda = lambda.And(e => e.PurchaseType == purchaseType);
            }
            //状态条件
            if (status != 0)
            {
                lambda = lambda.And(e => e.Status == status);
            }

            if (languageId != 0)
            {
                lambda = lambda.And(e => e.Language == languageId.ToString());
            }

            var database = _databaseRepository.Entities.Where(lambda).ToList();

            return new StatisticsCountDto
            {
                EffectiveCount = database.Count(e => e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now && e.Status != 3),
                OverdueCount = database.Count(e => e.ExpiryBeginTime > DateTime.Now || e.ExpiryEndTime < DateTime.Now),
                HiddenCount = database.Count(e => e.Status == 3)
            };
        }

        /// <summary>
        /// 获取数据库编辑表单
        /// </summary>
        /// <param name="databaseID"></param>
        /// <returns></returns>
        public async Task<DatabaseTerraceDto> GetDatabaseTerrace(Guid databaseID)
        {

            var directUrlDto = _directUrlRepository.Entities.Where(e => e.DatabaseID == databaseID).ToList().Adapt<List<DatabaseAcessUrlDto>>();

            DatabaseTerraceDto databaseDto = _mapper.Map<DatabaseTerraceDto>(_databaseRepository.Entities.Find(databaseID));

            databaseDto.DirectUrls = directUrlDto;
            return databaseDto;
        }

        /// <summary>
        /// 插入数据库平台的信息
        /// </summary>
        /// <param name="databaseTerraceDto"></param>
        public async Task InsertDatabaseTerrace(DatabaseTerraceDto databaseTerraceDto)
        {
            //更新标签库,更新label字段
            databaseTerraceDto.Label = await InsertCustomLabels(databaseTerraceDto.DisplayLabel.ToList<string>());

            var database = databaseTerraceDto.Adapt<EntityFramework.Core.Entitys.DatabaseTerrace>();

            //新增数据的处理
            database.Id = Guid.NewGuid();

            //处理排序字段
            var maxIndex = 0;
            try
            {
                maxIndex = _databaseRepository.Max(e => e.OrderIndex);
            }
            catch (Exception)
            {
                maxIndex = 0;
            }
            database.OrderIndex = maxIndex + 1;

            //点击量初始化
            database.MonthClickNum = 0;
            database.TotalClickNum = 0;

            //创建时间赋初值
            database.CreatedTime = DateTime.Now;


            //更新链接名称库
            InsertAcessUrlName(databaseTerraceDto.DirectUrls);
            //更新数据库与链接的关系表
            await _directUrlRepository.Context.DeleteRangeAsync<DatabaseAcessUrl>(e => e.DatabaseID == databaseTerraceDto.Id);

            if (databaseTerraceDto.DirectUrls != null)
            {
                var url = databaseTerraceDto.DirectUrls.Adapt<List<DatabaseAcessUrl>>();
                url.ForEach(e =>
                {
                    e.Id = Guid.NewGuid();
                    e.DatabaseID = database.Id;
                });
                _directUrlRepository.Insert(url);
            }

            try
            {
                //更新数据库数据
                var databaseNew = await _databaseRepository.InsertAsync(database);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                await this.SaveDatabaseGuideToES(database);
            }

        }


        private async Task SaveDatabaseGuideToES(EntityFramework.Core.Entitys.DatabaseTerrace database)
        {
            //插入到ES检索库当中
            var fulltext = $"{database.InformationText}\r\n{database.ResourceStatisticsText}\r\n{database.UseGuideText}";
            var request = new Search.EsSearchProxy.Core.Dto.UpsertOwnerNewsRequestParameter
            {
                fulltext = fulltext,
                owner = App.HttpContext.EnsureOwner(),
                docId = $"databaseguide_{App.HttpContext.EnsureOwner()}_{database.Id.ToString().Replace('-', '_')}",
                app_id = "databaseguide",
                app_type = Search.EsSearchProxy.Core.Models.OrganNewsType.Page,
                title = database.Title,
                summary = fulltext.Substring(0, fulltext.Length >= 4000 ? 4000 : fulltext.Length),
                url = $"/#/web_dataBaseDetail?databaseid={database.Id}",
                pub_time = database.CreatedTime,
                update_time = database.UpdatedTime.HasValue ? database.UpdatedTime.Value : database.CreatedTime,
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
        /// 获取现存数据库总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetDatabaseTotalCount()
        {
            var count = await _databaseRepository.CountAsync(e => !e.DeleteFlag);
            return count;
        }

        /// <summary>
        /// 更新数据库平台的信息
        /// </summary>
        /// <param name="databaseTerraceDto"></param>
        public async Task UpdateDatabaseTerrace(DatabaseTerraceDto databaseTerraceDto)
        {
            //刷新更新时间
            databaseTerraceDto.UpdatedTime = DateTime.Now;
            //更新标签库,更新label字段
            databaseTerraceDto.Label = await InsertCustomLabels(databaseTerraceDto.DisplayLabel.ToList<string>());

            //更新链接名称库
            InsertAcessUrlName(databaseTerraceDto.DirectUrls);

            //更新数据库与链接的关系表
            await _directUrlRepository.Context.DeleteRangeAsync<DatabaseAcessUrl>(e => e.DatabaseID == databaseTerraceDto.Id);

            if (databaseTerraceDto.DirectUrls != null)
            {
                var url = databaseTerraceDto.DirectUrls.Adapt<List<DatabaseAcessUrl>>();
                url.ForEach(e =>
                {
                    e.Id = Guid.NewGuid();
                    e.DatabaseID = databaseTerraceDto.Id;
                });
                _directUrlRepository.Insert(url);
            }



            var database = databaseTerraceDto.Adapt<EntityFramework.Core.Entitys.DatabaseTerrace>();

            //更新数据
            var databaseNew = _databaseRepository.UpdateNow(database);

            //更新到ES库
            await SaveDatabaseGuideToES(database);


        }





        /// <summary>
        /// 保存自定义标签
        /// </summary>
        /// <param name="databaseLabels"></param>
        /// <returns></returns>
        public async Task<List<string>> InsertCustomLabels(List<string> databaseLabels)
        {
            List<string> newLabelIdList = new List<string>();
            //检查是否标签都在，新增的标签将存入现有标签管理
            if (databaseLabels.Count == 0)
                return newLabelIdList;

            List<CustomLabel> customLabels = new List<CustomLabel>();
            var labelsQuery = from e in _labelRepository.AsQueryable()
                              select new OptionDto
                              {
                                  Key = e.LabelName,
                                  Value = e.Id.ToString()
                              };

            List<OptionDto> labels = labelsQuery.ToList();
            foreach (var label in databaseLabels)
            {
                var newId = Guid.NewGuid();
                if (!labels.Any(e => e.Key == label))
                {
                    customLabels.Add(new CustomLabel
                    {
                        Id = newId,
                        LabelName = label,
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now,
                        DeleteFlag = false,
                    });
                    newLabelIdList.Add(newId.ToString());
                }
                else
                {
                    newLabelIdList.Add(labels.FirstOrDefault(e => e.Key == label).Value);

                }
            }
            _labelRepository.InsertNow(customLabels);
            return newLabelIdList;

        }

        /// <summary>
        /// 批量修改数据库导航平台的状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status">1-正常 2-推荐 3-隐藏</param>
        public async Task BatchRecommendDatabaseTerrace(List<Guid> ids)
        {
            _databaseRepository.Context.BatchUpdate<EntityFramework.Core.Entitys.DatabaseTerrace>()
                    .Set(b => b.Status, b => 2)
                    .Set(b => b.UpdatedTime, b => DateTime.Now)
                    .Where(e => ids.Contains(e.Id))
                    .Execute();
        }

        /// <summary>
        /// 新增自定义的链接名称
        /// </summary>
        /// <param name="databaseAcessUrls"></param>
        /// <returns></returns>
        public async Task InsertAcessUrlName(List<DatabaseAcessUrlDto> databaseAcessUrls)
        {
            if (databaseAcessUrls == null)
            {
                return;
            }
            //检查是否标签都在，新增的标签将存入现有标签管理
            if (databaseAcessUrls.Count > 0)
            {
                List<DatabaseUrlName> AcessUrls = new List<DatabaseUrlName>();
                var urlQuery = from e in _databaseUrlNameRepository.AsQueryable()
                               select new String(e.Name);

                List<string> urls = urlQuery.ToList();
                foreach (var databaseAcessUrl in databaseAcessUrls)
                {
                    if (!urls.Contains(databaseAcessUrl.Name))
                    {
                        AcessUrls.Add(new DatabaseUrlName
                        {
                            Id = Guid.NewGuid(),
                            Name = databaseAcessUrl.Name,
                            CreatedTime = DateTime.Now,
                            UpdatedTime = DateTime.Now,
                        });
                    }
                }
                _databaseUrlNameRepository.InsertNow(AcessUrls);
            }
        }



        /// <summary>
        /// 批量删除数据库导航平台的数据  逻辑删
        /// </summary>
        /// <param name="ids"></param>
        public async Task BatchDeleteTerrace(List<Guid> ids)
        {

            var deleteItems = _databaseRepository.Context.BatchUpdate<EntityFramework.Core.Entitys.DatabaseTerrace>()
                        .Set(b => b.DeleteFlag, b => true)
                        .Set(b => b.UpdatedTime, b => DateTime.Now)
                      .Where(e => ids.Contains(e.Id))
                        .Execute();

            //删除ES检索库中的专题信息
            var strList = ids.Select(e => $"databaseguide_{App.HttpContext.EnsureOwner()}_{e.ToString().Replace('-', '_')}");
            var strIds = string.Join(';', strList);

            await _esProxyService.DeleteOrganNewsAsync(strIds);

        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetLabelDtos()
        {

            var query = from a in _labelRepository.AsQueryable(e => !e.DeleteFlag)
                        select new OptionDto
                        {
                            Key = a.LabelName,
                            Value = a.Id.ToString(),
                        };
            var test = query.ToList();

            return query.ToList();
        }

        /// <summary>
        /// 获取在用的学科分类的信息 List<string>
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetDomainEsc()
        {
            var query = from a in _domainEscsAttrRepository.AsQueryable(e => !e.DeleteFlag)
                        select new String(a.Id);

            return query.ToList();
        }

        /// <summary>
        /// 获取在用的学科分类的信息  List<OptionDto>
        /// </summary>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetDomainEscDtos()
        {
            //获取完整学科分类树
            var trees = await GetDomainTree();
            //获取在用的学科分类树
            var escs = await GetDomainEsc();

            List<OptionDto> options = new List<OptionDto>();
            foreach (var tree in trees)
            {
                if (escs.Contains(tree.Value))
                {
                    options.Add(tree);
                }
                if (tree.Child != null)
                {
                    foreach (var item in tree.Child)
                    {
                        if (escs.Contains(item.Value))
                        {
                            options.Add(item);
                        }
                    }
                }
            }
            options.ForEach(e => e.Child = null);
            return options;
        }

        /// <summary>
        /// 从数据中心获取学科分类树
        /// </summary>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetDomainTree()
        {

            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<DomainInfoService.DomainInfoServiceClient>();
            AllDomainInfoTreeRequest request1 = new AllDomainInfoTreeRequest { Type = 1, Level = 2 };
            AllDomainInfoTreeReply reply1 = new AllDomainInfoTreeReply();
            try
            {
                reply1 = await grpcClient1.GetAllDomainInfoTreesAsync(request1);
            }
            catch (Exception)
            {
            }
            var subjectTree = reply1.DomainTrees.Adapt<List<OptionDto>>();

            return subjectTree;

        }


        /// <summary>
        /// 获取导航栏目列表
        /// </summary>
        /// <returns></returns>
        public async Task<PagedList<DatabaseColumnDto>> GetDatabaseColumns(int pageIndex, int pageSize)
        {

            //获取在用的数据库导航栏目和规则集合
            var rule = _databaseColumnRuleRepository.Entities.Where(e => !e.DeleteFlag).ToList();
            var databaseColumnDto = _databaseColumnRepository.Entities.Where(e => !e.DeleteFlag).OrderByDescending(e => e.CreatedTime).ToPagedList(pageIndex, pageSize).Adapt<PagedList<DatabaseColumnDto>>();



            foreach (var column in databaseColumnDto.Items)
            {

                column.MatchCount = (await this.GetDatabaseColumnPreview(column.Id)).Count;


                var artTypeRuleStr = "文献类型=";
                var domainTypeRuleStr = "学科分类=";
                var languageTypeRuleStr = "主要语言=";
                var purchaseTypeRuleStr = "采购状态=";
                var statusTypeRuleStr = "状态=";
                var labelStr = "自定义标签=";
                var orderRuleStr = $"排序规则={ Enum.GetName(typeof(OrderRuleEnum), int.Parse(column.OrderRule))}";

                var hasArtTypeRule = false;
                var hasDomainTypeRule = false;
                var hasLanguageTypeRule = false;
                var hasPurchaseTypeRule = false;
                var hasStatusTypeRule = false;
                var hasLabelRule = false;




                //该栏目下的规则集合
                var rules = rule.Where(e => e.ColumnID == column.Id).OrderBy(e => e.RuleKey);

                foreach (var ru in rules)
                {
                    if (ru.RuleKey == "文献类型")
                    {
                        artTypeRuleStr += $"{ ru.RuleValueName};";
                        hasArtTypeRule = true;
                    }
                    else if (ru.RuleKey == "学科分类")
                    {
                        domainTypeRuleStr += $"{ ru.RuleValueName};";
                        hasDomainTypeRule = true;
                    }
                    else if (ru.RuleKey == "主要语言")
                    {
                        languageTypeRuleStr += $"{ ru.RuleValueName};";
                        hasLanguageTypeRule = true;
                    }
                    else if (ru.RuleKey == "采购状态")
                    {
                        purchaseTypeRuleStr += $"{ ru.RuleValueName};";
                        hasPurchaseTypeRule = true;
                    }
                    else if (ru.RuleKey == "状态")
                    {
                        statusTypeRuleStr += $"{ ru.RuleValueName};";
                        hasStatusTypeRule = true;
                    }
                    else if (ru.RuleKey == "自定义标签")
                    {
                        labelStr += $"{ ru.RuleValueName};";
                        hasLabelRule = true;
                    }

                }


                //在此处将规则字符串拼好
                column.Rule = $"{(hasArtTypeRule ? artTypeRuleStr.Trim(';') : string.Empty)},{(hasDomainTypeRule ? domainTypeRuleStr.Trim(';') : string.Empty)},{(hasLanguageTypeRule ? languageTypeRuleStr.Trim(';') : string.Empty)},{(hasPurchaseTypeRule ? purchaseTypeRuleStr.Trim(';') : string.Empty)},{(hasStatusTypeRule ? statusTypeRuleStr.Trim(';') : string.Empty)},{(hasLabelRule ? labelStr.Trim(';') : string.Empty)};{orderRuleStr}";


                //重置判别标签
                hasArtTypeRule = false;
                hasDomainTypeRule = false;
                hasLanguageTypeRule = false;
                hasPurchaseTypeRule = false;
                hasStatusTypeRule = false;
                hasLabelRule = false;


            }
            return databaseColumnDto;
        }

        /// <summary>
        /// 获取导航栏目表单
        /// </summary>
        /// <returns></returns>
        public async Task<DatabaseColumnDto> GetDatabaseColumn(Guid columnID)
        {
            var databaseColumn = _databaseColumnRepository.FirstOrDefault(e => e.Id == columnID && !e.DeleteFlag).Adapt<DatabaseColumnDto>();

            var rules = _databaseColumnRuleRepository.Where(e => !e.DeleteFlag && e.ColumnID == columnID).ToList();

            databaseColumn.ArticleTypeDtos = new List<string>();
            databaseColumn.DomainEscDtos = new List<string>();
            databaseColumn.languageDtos = new List<string>();
            databaseColumn.PurchaseTypeDtos = new List<string>();
            databaseColumn.StatusDtos = new List<string>();
            databaseColumn.Labels = new List<string>();

            foreach (var rule in rules)
            {
                /*                     if (rule.RuleKey == "文献类型") databaseColumn.ArticleTypeDtos.Add(new OptionDto { Key = rule.RuleValueName, Value = rule.RuleValue });
                                else if (rule.RuleKey == "学科分类") databaseColumn.DomainEscDtos.Add(new OptionDto { Key = rule.RuleValueName, Value = rule.RuleValue });
                                else if (rule.RuleKey == "主要语言") databaseColumn.languageDtos.Add(new OptionDto { Key = rule.RuleValueName, Value = rule.RuleValue });
                                else if (rule.RuleKey == "采购状态") databaseColumn.PurchaseTypeDtos.Add(new OptionDto { Key = rule.RuleValueName, Value = rule.RuleValue });
                                else if (rule.RuleKey == "状态") databaseColumn.StatusDtos.Add(new OptionDto { Key = rule.RuleValueName, Value = rule.RuleValue });
                                else if (rule.RuleKey == "自定义标签") databaseColumn.Labels.Add(rule.RuleValueName);
                                else if (rule.RuleKey == "排序规则") databaseColumn.OrderIndexRule = int.Parse(rule.RuleValue);*/

                if (rule.RuleKey == Enum.GetName(FilterTypeEnum.文献类型)) databaseColumn.ArticleTypeDtos.Add(rule.RuleValue);
                else if (rule.RuleKey == Enum.GetName(FilterTypeEnum.学科分类)) databaseColumn.DomainEscDtos.Add(rule.RuleValue);
                else if (rule.RuleKey == Enum.GetName(FilterTypeEnum.主要语言)) databaseColumn.languageDtos.Add(rule.RuleValue);
                else if (rule.RuleKey == Enum.GetName(FilterTypeEnum.采购状态)) databaseColumn.PurchaseTypeDtos.Add(rule.RuleValue);
                else if (rule.RuleKey == Enum.GetName(FilterTypeEnum.状态)) databaseColumn.StatusDtos.Add(rule.RuleValue);
                else if (rule.RuleKey == Enum.GetName(FilterTypeEnum.自定义标签)) databaseColumn.Labels.Add(rule.RuleValue);
                //     else if (rule.RuleKey == "排序规则") databaseColumn.OrderIndexRule = int.Parse(rule.RuleValue);

            }

            return databaseColumn;
        }



        /// <summary>
        /// 获取数据库栏目的数据库预览列表
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<List<DatabaseTerraceDto>> GetDatabaseColumnPreview(Guid columnID)
        {
            var column = _databaseColumnRepository.Find(columnID);
            //获取该栏目下所有在用的规则
            var rules = _databaseColumnRuleRepository.Where(e => !e.DeleteFlag && e.ColumnID == columnID).OrderBy(e => e.RuleKey).ToList();

            //组配表达式 
            Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> lambda = x => !x.DeleteFlag;

            //组配文献类型规则
            var artRules = rules.Where(e => e.RuleKey == Enum.GetName(FilterTypeEnum.文献类型)).ToList();
            Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> artLambda = x => 1 == 1;
            if (artRules.Count > 0)
            {
                var firstRule = artRules.First();
                artLambda = e => e.ArticleTypes.Contains(firstRule.RuleValue);
                foreach (var ru in artRules)
                {
                    if (ru.Id == firstRule.Id)
                    {
                    }
                    artLambda = artLambda.Or(e => e.ArticleTypes.Contains(ru.RuleValue));
                }
            }

            //组配学科分类规则
            var domainRules = rules.Where(e => e.RuleKey == Enum.GetName(FilterTypeEnum.学科分类)).ToList();
            Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> domainLambda = x => 1 == 1;
            if (domainRules.Count > 0)
            {
                var firstRule = domainRules.First();
                domainLambda = e => e.DomainEscs.Contains(firstRule.RuleValue);
                foreach (var ru in domainRules)
                {
                    if (ru.Id == firstRule.Id)
                    {

                    }
                    domainLambda = domainLambda.Or(e => e.DomainEscs.Contains(ru.RuleValue));
                }
            }

            //组配采购状态规则
            var purchaseRules = rules.Where(e => e.RuleKey == Enum.GetName(FilterTypeEnum.采购状态)).ToList();
            Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> purchaseLambda = x => 1 == 1;
            if (purchaseRules.Count > 0)
            {
                var firstRule = purchaseRules.First();
                purchaseLambda = e => e.PurchaseType.Contains(firstRule.RuleValue);
                foreach (var ru in purchaseRules)
                {
                    if (ru.Id == firstRule.Id)
                    {

                    }

                    purchaseLambda = purchaseLambda.Or(e => e.PurchaseType == ru.RuleValue);
                }
            }

            //组配语言类型规则
            var languageRules = rules.Where(e => e.RuleKey == Enum.GetName(FilterTypeEnum.主要语言)).ToList();
            Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> languageLambda = x => 1 == 1;
            if (languageRules.Count > 0)
            {
                var firstRule = languageRules.First();
                languageLambda = e => e.Language.Contains(firstRule.RuleValue);
                foreach (var ru in languageRules)
                {
                    if (ru.Id == firstRule.Id)
                    {

                    }

                    languageLambda = languageLambda.Or(e => e.PurchaseType == ru.RuleValue);
                }
            }

            //组配状态规则
            var statusRules = rules.Where(e => e.RuleKey == Enum.GetName(FilterTypeEnum.状态)).ToList();
            Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> statusLambda = x => 1 == 1;
            if (statusRules.Count > 0)
            {
                var firstRule = statusRules.First();
                statusLambda = e => e.Status == int.Parse(firstRule.RuleValue);
                foreach (var ru in statusRules)
                {
                    if (ru.Id == firstRule.Id)
                    {
                    }
                    statusLambda = statusLambda.Or(e => e.Status == int.Parse(ru.RuleValue));
                }
            }
            //组配标签规则
            var labelRules = rules.Where(e => e.RuleKey == Enum.GetName(FilterTypeEnum.自定义标签)).ToList();
            Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> labelLambda = x => 1 == 1;
            if (labelRules.Count > 0)
            {
                var firstRule = labelRules.First();
                labelLambda = e => e.Label.Contains(firstRule.RuleValue);
                foreach (var ru in labelRules)
                {
                    if (ru.Id != firstRule.Id)
                    {
                        labelLambda = labelLambda.Or(e => e.Label.Contains(ru.RuleValue));
                    }

                }
            }

            //组装所有表达式
            var finalLamda = lambda.And(artLambda).And(domainLambda).And(purchaseLambda).And(languageLambda).And(statusLambda).And(labelLambda);

            //查询满足条件的数据库信息，以所选规则排序，
            var orderRule = rules.FirstOrDefault(e => e.RuleKey == Enum.GetName(FilterTypeEnum.排序规则));
            var databaseTerrace = new List<DatabaseTerraceDto>();
            switch (column.OrderRule)
            {
                case (int)OrderRuleEnum.默认排序:
                    databaseTerrace = _databaseRepository.Where(finalLamda).OrderBy(e => e.OrderIndex).ProjectToType<DatabaseTerraceDto>().ToList();
                    break;
                case (int)OrderRuleEnum.月点击量:
                    databaseTerrace = _databaseRepository.Where(finalLamda).OrderBy(e => e.MonthClickNum).ProjectToType<DatabaseTerraceDto>().ToList();
                    break;
                case (int)OrderRuleEnum.总点击量:
                    databaseTerrace = _databaseRepository.Where(finalLamda).OrderBy(e => e.TotalClickNum).ProjectToType<DatabaseTerraceDto>().ToList();
                    break;
                case (int)OrderRuleEnum.创建时间:
                    databaseTerrace = _databaseRepository.Where(finalLamda).OrderBy(e => e.CreatedTime).ProjectToType<DatabaseTerraceDto>().ToList();
                    break;
                default:
                    throw Oops.Oh($"该栏目无排序规则，数据异常。").StatusCode(HttpStatusKeys.ExceptionCode);
            }
            return databaseTerrace;
        }
        /// <summary>
        /// 获取数据库应用的配置
        /// </summary>
        /// <returns></returns>
        public async Task<DatabaseTerraceSettingsDto> GetDatabaseSettings()
        {


            //处理头尾模板
            var settings = _settingsRepository.FirstOrDefault().Adapt<DatabaseTerraceSettingsDto>();
            var headFoot = await GetTemplateDetailGrpc(settings.HeadTemplate, settings.FootTemplate);

            settings.HeadTemplateModel = new TemplateModel();
            settings.HeadTemplateModel.ApiRouter = headFoot.ApiRouter;
            settings.HeadTemplateModel.Router = headFoot.HeaderRouter;
            settings.HeadTemplateModel.TemplateCode = headFoot.HeaderTemplateCode;

            settings.FootTemplateModel = new TemplateModel();
            settings.FootTemplateModel.ApiRouter = headFoot.ApiRouter;
            settings.FootTemplateModel.Router = headFoot.FooterRouter;
            settings.FootTemplateModel.TemplateCode = headFoot.FooterTemplateCode;
            return settings;
        }
        /// <summary>
        /// 保存数据库的应用设置
        /// </summary>
        /// <param name="databaseTerraceSettings"></param>
        public async Task SaveDatabaseSettings(DatabaseTerraceSettingsDto databaseTerraceSettings)
        {
            databaseTerraceSettings.UpdatedTime = DateTime.Now;
            _settingsRepository.UpdateNow(databaseTerraceSettings.Adapt<DatabaseTerraceSettings>());
        }

        /// <summary>
        /// 收藏数据库
        /// </summary>
        /// <param name="databaseId"></param>
        public async Task CollectionDatabase(Guid databaseId)
        {
            var userKey = App.User?.FindFirstValue(Const.userKey);
            _subscriberRepository.Insert(new DatabaseSubscriber { Id = Guid.NewGuid(), DeleteFlag = false, DatabaseID = databaseId, UserKey = userKey, CreatedTime = DateTime.Now, UpdatedTime = DateTime.Now });
        }
        /// <summary>
        /// 获取收藏的数据库
        /// </summary>
        /// <returns></returns>
        public async Task<List<DatabaseSubscriberDto>> GetCollectionDatabase()
        {
            var userKey = App.User?.FindFirstValue(Const.userKey);

            var collections = _subscriberRepository.Where(e => e.UserKey == userKey && !e.DeleteFlag).ToList();


            var subscriberQuery = from p in _databaseRepository.AsQueryable(e => e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now && !e.DeleteFlag)
                                  join d in _subscriberRepository.AsQueryable(e => e.UserKey == userKey && !e.DeleteFlag) on p.Id equals d.DatabaseID
                                  select new DatabaseSubscriberDto
                                  {
                                      Id = d.Id,
                                      DatabaseID = p.Id,
                                      TypeName = Const.我的收藏,
                                      Title = p.Title,
                                      UserKey = userKey,
                                      Cover = p.Cover,
                                      DeleteFlag = p.DeleteFlag,
                                      DatabaseCreatedTime=p.CreatedTime,
                                      CreatedTime=d.CreatedTime

                                  };
            var subList = subscriberQuery.OrderByDescending(e=>e.CreatedTime).ToList();


            return subList;
        }

        /// <summary>
        /// 获取已经收藏的数据库列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DatabaseTerraceDto>> GetSubscribeDatabase()
        {
            var userKey = App.User?.FindFirstValue(Const.userKey);

            var collections = _subscriberRepository.Where(e => e.UserKey == userKey && !e.DeleteFlag).ToList();


            var subscriberQuery = from p in _databaseRepository.AsQueryable()
                                  join d in _subscriberRepository.AsQueryable(e => e.UserKey == userKey && !e.DeleteFlag) on p.Id equals d.DatabaseID
                                  select new DatabaseTerraceDto
                                  {
                                      Id = p.Id,
                                      Title = p.Title,
                                      Cover = p.Cover,
                                      PortalUrl = p.IndirectUrl,
                                      IndirectUrl = p.IndirectUrl,

                                  };
            var subList = subscriberQuery.ToList();
            return subList;
        }

        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="databaseId"></param>
        public async Task DeleteCollectionDatabase(Guid databaseId)
        {
            var userKey = App.User?.FindFirstValue(Const.userKey);

            var subscriber = _subscriberRepository.FirstOrDefault(e => e.UserKey == userKey && e.DatabaseID == databaseId);
            subscriber.DeleteFlag = true;
            subscriber.UpdateNow();

        }

        /// <summary>
        /// 取总点击量排行最高的数据库信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<DatabaseTerraceDto>> GetDatabasesOrderByTotalClick()
        {
            //按总点击量排序获取的数据
            return _databaseRepository.Where(e => !e.DeleteFlag && e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now)
                .OrderByDescending(e => e.TotalClickNum)
                .ProjectToType<DatabaseTerraceDto>().ToList();
        }

        /// <summary>
        /// 按照月度点击量排序查询数据库
        /// </summary>
        /// <returns></returns>
        public async Task<List<DatabaseTerraceDto>> GetDatabasesOrderByMonthClick()
        {
            //按月点击量排序，取前面的几个
            return _databaseRepository.Where(e => !e.DeleteFlag && e.UpdatedTime > DateTime.Now.AddDays(1 - DateTime.Now.Day) && e.ExpiryBeginTime > DateTime.Now && e.ExpiryEndTime > DateTime.Now)
                .OrderByDescending(e => e.MonthClickNum)
                .ProjectToType<DatabaseTerraceDto>().ToList();
        }

        public async Task BatchRecoverDatabaseTerrace(List<Guid> ids)
        {
            _databaseRepository.Context.BatchUpdate<EntityFramework.Core.Entitys.DatabaseTerrace>()
                  .Set(b => b.Status, b => 1)
                  .Where(e => ids.Contains(e.Id))
                  .Execute();
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
            AppColumnOperationRequest request1 = new AppColumnOperationRequest { AppRouteCode = appRouteCode, ColumnId = columnId, ColumnName = columnName, CreateTime = createTime, OperationType = operationType, VisitUrl = visitUrl, };
            AppColumnOperationReply reply1 = new AppColumnOperationReply();
            try
            {
                reply1 = await grpcClient1.AppColumnOperationAsync(request1);
                return;
            }
            catch (Exception)
            {
                throw Oops.Oh("grpc调用异常").StatusCode(HttpStatusKeys.ExceptionCode);
            }
        }

        /// <summary>
        /// 新增数据库栏目 
        /// </summary>
        /// <param name="databaseColumnDto"></param>
        public async Task InsertDatabaseColumn(DatabaseColumnDto databaseColumnDto)
        {
            //刷新时间
            databaseColumnDto.CreatedTime = DateTime.Now;
            databaseColumnDto.UpdatedTime = DateTime.Now;

            //新增数据数据库栏目
            EntityEntry<DatabaseColumn> databaseColumn = _databaseColumnRepository.InsertNow(databaseColumnDto.Adapt<DatabaseColumn>());
            //保存规则内容
            await UpdateDatabaseColumnRules(databaseColumnDto, databaseColumn);


            #region 同步到应用中心
            await AppColumnOperation(databaseColumnDto.Id.ToString(), databaseColumnDto.ColumnName, databaseColumnDto.CreatedTime.ToString(), 1, "/admin_databaseNavigation", "databaseguide");

            #endregion

        }
        /// <summary>
        /// 更新数据栏目的数据
        /// </summary>
        /// <param name="databaseColumnDto"></param>
        /// <returns></returns>
        public async Task UpdateDatabaseColumn(DatabaseColumnDto databaseColumnDto)
        {
            //刷新时间
            databaseColumnDto.UpdatedTime = DateTime.Now;

            //更新数据库栏目表
            EntityEntry<DatabaseColumn> databaseColumn = _databaseColumnRepository.UpdateNow(databaseColumnDto.Adapt<DatabaseColumn>());
            //保存规则内容
            await UpdateDatabaseColumnRules(databaseColumnDto, databaseColumn);

            #region 同步到应用中心
            await AppColumnOperation(databaseColumnDto.Id.ToString(), databaseColumnDto.ColumnName, databaseColumnDto.CreatedTime.ToString(), 2, "/admin_databaseNavigation", "databaseguide");

            #endregion

        }

        /// <summary>
        /// 更新栏目下的规则数据，1物理删除，2新增
        /// </summary>
        /// <param name="databaseColumnDto"></param>
        /// <param name="databaseColumn"></param>
        /// <returns></returns>
        public async Task UpdateDatabaseColumnRules(DatabaseColumnDto databaseColumnDto, EntityEntry<DatabaseColumn> databaseColumn)
        {
            var sysSourcesTypes = await this.GetSysSourceTypeDto();
            var cusSourcesTypes = await this.GetCusSourceTypeDto();
            var sourcesTypes = new List<OptionDto>();

            sourcesTypes.AddRange(sysSourcesTypes);
            sourcesTypes.AddRange(cusSourcesTypes);
            List<DatabaseColumnRule> databaseColumnRules = new List<DatabaseColumnRule>();
            #region 提取栏目下的规则数据
            //文献类型规则
            databaseColumnDto.ArticleTypeDtos.ForEach(e => databaseColumnRules.Add(new DatabaseColumnRule
            {
                Id = Guid.NewGuid(),
                ColumnID = databaseColumn.Entity.Id,
                RuleKey = "文献类型",
                RuleValue = e,
                RuleValueName = sourcesTypes.First(p => p.Value == e).Key,
                DeleteFlag = false,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            }));

            var domain = await GetDomainEscDtos();

            //学科分类规则
            databaseColumnDto.DomainEscDtos.ForEach(e => databaseColumnRules.Add(new DatabaseColumnRule
            {
                Id = Guid.NewGuid(),
                ColumnID = databaseColumn.Entity.Id,
                RuleKey = "学科分类",
                RuleValue = e,
                RuleValueName = domain.First(p => p.Value == e).Key,
                DeleteFlag = false,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            }));

            //采购状态规则
            databaseColumnDto.PurchaseTypeDtos.ForEach(e => databaseColumnRules.Add(new DatabaseColumnRule
            {
                Id = Guid.NewGuid(),
                ColumnID = databaseColumn.Entity.Id,
                RuleKey = "采购状态",
                RuleValue = e,
                RuleValueName = Enum.GetName(typeof(PurchaseTypeEnum), int.Parse(e)).ToString(),
                DeleteFlag = false,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            }));

            //状态规则
            databaseColumnDto.StatusDtos.ForEach(e => databaseColumnRules.Add(new DatabaseColumnRule
            {
                Id = Guid.NewGuid(),
                ColumnID = databaseColumn.Entity.Id,
                RuleKey = "状态",
                RuleValue = e,
                RuleValueName = Enum.GetName(typeof(StatusEnum), int.Parse(e)).ToString(),
                DeleteFlag = false,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            }));

            //主要语言规则
            databaseColumnDto.languageDtos.ForEach(e => databaseColumnRules.Add(new DatabaseColumnRule
            {
                Id = Guid.NewGuid(),
                ColumnID = databaseColumn.Entity.Id,
                RuleKey = "主要语言",
                RuleValue = e,
                RuleValueName = Enum.GetName(typeof(LanguageTypeEnum), int.Parse(e)).ToString(),
                DeleteFlag = false,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            }));


            var label = await GetLabelDtos();


            //自定义标签规则
            databaseColumnDto.Labels.ForEach(e => databaseColumnRules.Add(new DatabaseColumnRule
            {
                Id = Guid.NewGuid(),
                ColumnID = databaseColumn.Entity.Id,
                RuleKey = "自定义标签",
                RuleValue = e,
                RuleValueName = label.First(p => p.Value == e).Key,
                DeleteFlag = false,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now
            }));

            //物理删除该栏目下所有的规则数据
            var delCount = _databaseColumnRuleRepository.Context.DeleteRange<DatabaseColumnRule>(e => e.ColumnID == databaseColumnDto.Id);
            #endregion

            //批量新增本次添加的规则数据
            _databaseColumnRuleRepository.InsertNow(databaseColumnRules);
        }
        /// <summary>
        /// 批量删除导航栏目
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task BatchDeleteDatabaseColumn(List<Guid> ids)
        {
            //删除栏目适配的规则  逻辑删

            await _providerResourceRepository.Context.BatchUpdate<DatabaseColumn>()
                  .Set(b => b.DeleteFlag, b => true)
                  .Where(e => !e.DeleteFlag && ids.Contains(e.Id))
                  .ExecuteAsync();

            await _providerResourceRepository.Context.BatchUpdate<DatabaseColumnRule>()
                  .Set(b => b.DeleteFlag, b => true)
                  .Where(e => !e.DeleteFlag && ids.Contains(e.ColumnID))
                  .ExecuteAsync();

            var strIds = string.Join(';', ids.Select(e => e.ToString()));
            #region 同步到应用中心
            await AppColumnOperation(strIds, "", "", 3, "/admin_databaseNavigation", "databaseguide");
            await _esProxyService.DeleteOrganNewsAsync(strIds);
            #endregion
        }

        /// <summary>
        /// 批量删除导航栏目
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task BatchDeleteLabel(List<Guid> ids)
        {
            //删除栏目适配的规则  逻辑删

            await _labelRepository.Context.BatchUpdate<CustomLabel>()
                  .Set(b => b.DeleteFlag, b => true)
                  .Where(e => !e.DeleteFlag && ids.Contains(e.Id))
                  .ExecuteAsync();


        }

        /// <summary>
        /// 保存在用学科分类 物理删
        /// </summary>
        /// <param name="domainEscs"></param>
        /// <returns></returns>
        public async Task SaveDomainEscDtos(List<string> domainEscs)
        {
            List<DomainEscsAttr> domainEscsAttrs = new List<DomainEscsAttr>();
            domainEscs.ForEach(e => domainEscsAttrs.Add(new DomainEscsAttr
            {
                Id = e,
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                DeleteFlag = false
            }));

            _domainEscsAttrRepository.Context.DeleteRange<DomainEscsAttr>();
            _domainEscsAttrRepository.InsertNow(domainEscsAttrs);
        }

        public async Task<PagedList<OptionDto>> GetCoustomLabels(int pageIndex, int pageSize)
        {
            return await _labelRepository.Where(e => !e.DeleteFlag).ProjectToType<OptionDto>().ToPagedListAsync(pageIndex, pageSize);

        }
        /// <summary>
        /// 获取链接名称
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<OptionDto>> GetAcessUrlName(int pageIndex, int pageSize)
        {
            return await _databaseUrlNameRepository.Where(e => !e.DeleteFlag).ProjectToType<OptionDto>().ToPagedListAsync(pageIndex, pageSize);

        }

        public async Task BatchDeleteAcessUrlName(List<Guid> ids)
        {
            await _databaseUrlNameRepository.Context.DeleteRangeAsync<DatabaseUrlName>(e => ids.Contains(e.Id));
        }

        public async Task SaveCustomLabels(List<OptionDto> labels)
        {
            IEnumerable<Guid> labelIds = labels.Select(e => new Guid(e.Value));
            //找出已有的标签
            var oldLabels = _labelRepository.Where(e => !e.DeleteFlag && labelIds.Contains(e.Id)).ToList();
            List<Guid> oldLabelIds = oldLabels.Select(e => e.Id).ToList();
            //找出新增的标签
            var newLabel = labels.Where(e => !oldLabelIds.Contains(new Guid(e.Value))).Adapt<List<CustomLabel>>();
            foreach (var item in oldLabels)
            {
                item.LabelName = labels.First(e => oldLabelIds.Contains(new Guid(e.Value))).Key;
                item.UpdatedTime = DateTime.Now;
            }



            //更新标签
            _labelRepository.Context.UpdateRange(oldLabels);
            //插入新标签
            await _labelRepository.Context.BulkInsertAsync<CustomLabel>(newLabel);
        }

        /// <summary>
        /// 通过拖拽进行排序
        /// </summary>
        /// <param name="sourceArtIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="isUp"></param>
        /// <returns></returns>
        public async Task SortDatabaseByDrag(int sourceArtIndex, int destinationIndex, bool isUp)
        {
            int maxIndex = isUp ? destinationIndex : sourceArtIndex;
            int minIndex = isUp ? sourceArtIndex : destinationIndex;

            var source = _databaseRepository.Entities.Where(e => !e.DeleteFlag && e.OrderIndex >= minIndex && e.OrderIndex <= maxIndex).ToList();

            //排序
            source = this.SortData(source, sourceArtIndex, destinationIndex, isUp);

            _databaseRepository.Update(source);
            //将剩余数据存回表中

        }


        /// <summary>
        /// 通用的数据排序
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sourceArtIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="isUp"></param>
        /// <returns></returns>
        public List<EntityFramework.Core.Entitys.DatabaseTerrace> SortData(List<EntityFramework.Core.Entitys.DatabaseTerrace> source, int sourceArtIndex, int destinationIndex, bool isUp)
        {
            int change = isUp ? -1 : 1;

            source.ForEach(e =>
            {
                if (e.OrderIndex == sourceArtIndex)
                {
                    e.OrderIndex = destinationIndex;
                }
                else
                {
                    e.OrderIndex += change;

                }
            });

            return source;
        }

        /// <summary>
        /// 绝对序号进行排序
        /// </summary>
        /// <param name="sourceArtIndex"></param>
        /// <param name="absoluteIndex"></param>
        /// <returns></returns>
        public async Task SortSourceFromImportByDestination(int sourceArtIndex, int absoluteIndex)
        {

            if (_databaseRepository.Entities.Count() < absoluteIndex || absoluteIndex <= 0)
                Oops.Oh("输入的目标序号超出数据总量，请确认后输入正确序号").StatusCode(HttpStatusKeys.ExceptionCode);


            bool isUp;



            //查找绝对序号对应的文献的序号
            var tagartDatabase = _databaseRepository.Where(e => !e.DeleteFlag && e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now && e.Status != 3).OrderByDescending(e => e.OrderIndex).Skip(absoluteIndex - 1).Take(1).FirstOrDefault();


            var destIndex = tagartDatabase.OrderIndex;

            if (destIndex == sourceArtIndex)
            {
                return;
            }

            if (sourceArtIndex > destIndex)
            {
                isUp = false;
            }
            else
            {
                isUp = true;
            }

            //确定orderIndex的上下限
            int maxIndex = isUp ? destIndex : sourceArtIndex;
            int minIndex = isUp ? sourceArtIndex : destIndex;

            var source = _databaseRepository.Entities.Where(e => !e.DeleteFlag && e.OrderIndex >= minIndex && e.OrderIndex <= maxIndex && e.Status != 3).OrderByDescending(e => e.OrderIndex).ToList();

            source = SortData(source, sourceArtIndex, destIndex, isUp);
            //将排序后数据存回DB
            _databaseRepository.Update(source);
        }




        /// <summary>
        /// 获取中问的数据库
        /// </summary>
        /// <returns></returns>
        public Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> GetChineseDatabasesQuery(string artType, string domain, int languagId, string searchKey, string purchaseType, int status, string initials, string label)
        {

            var lamda = LinqExpression.Create<EntityFramework.Core.Entitys.DatabaseTerrace>(e => !e.DeleteFlag && e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now && e.Language == ((int)LanguageTypeEnum.中文).ToString() && (e.PurchaseType == ((int)PurchaseTypeEnum.订购).ToString() || e.PurchaseType == ((int)PurchaseTypeEnum.OA免费).ToString()));


            lamda = lamda.AndIf(!string.IsNullOrEmpty(searchKey), e => e.Title.Contains(searchKey) || e.ResourceStatisticsText.Contains(searchKey) || e.UseGuideText.Contains(searchKey) || e.InformationText.Contains(searchKey));

            lamda = lamda.AndIf(!string.IsNullOrEmpty(artType), e => e.ArticleTypes.Contains(artType));
            lamda = lamda.AndIf(!string.IsNullOrEmpty(domain), e => e.DomainEscs.Contains(domain));
            lamda = lamda.AndIf(languagId != 0, e => e.Language == languagId.ToString());


            lamda = lamda.AndIf(!string.IsNullOrEmpty(purchaseType), e => e.PurchaseType == purchaseType);
            lamda = lamda.AndIf(status != 0, e => e.Status == status);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(initials), e => e.Initials == initials);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(label), e => e.Label.Contains(label));


            return lamda;

        }

        /// <summary>
        /// 获取外文的数据库
        /// </summary>
        /// <returns></returns>
        public Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> GetForeignDatabasesQuery(string artType, string domain, int languagId, string searchKey, string purchaseType, int status, string initials, string label)
        {
            var lamda = LinqExpression.Create<EntityFramework.Core.Entitys.DatabaseTerrace>(e => !e.DeleteFlag && e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now && e.Language != ((int)LanguageTypeEnum.中文).ToString() && (e.PurchaseType == ((int)PurchaseTypeEnum.订购).ToString() || e.PurchaseType == ((int)PurchaseTypeEnum.OA免费).ToString()));
            lamda = lamda.AndIf(!string.IsNullOrEmpty(artType), e => e.ArticleTypes.Contains(artType));
            lamda = lamda.AndIf(!string.IsNullOrEmpty(domain), e => e.DomainEscs.Contains(domain));
            lamda = lamda.AndIf(languagId != 0, e => e.Language == languagId.ToString());

            lamda = lamda.AndIf(!string.IsNullOrEmpty(searchKey), e => e.Title.Contains(searchKey) || e.ResourceStatisticsText.Contains(searchKey) || e.UseGuideText.Contains(searchKey) || e.InformationText.Contains(searchKey));

            lamda = lamda.AndIf(!string.IsNullOrEmpty(purchaseType), e => e.PurchaseType == purchaseType);
            lamda = lamda.AndIf(status != 0, e => e.Status == status);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(initials), e => e.Initials == initials);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(label), e => e.Label.Contains(label));

            return lamda;
        }

        /// <summary>
        /// 获取试用的数据库
        /// </summary>
        /// <returns></returns>
        public Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> GetProbationDatabaseQuery(string artType, string domain, int languagId, string searchKey, string purchaseType, int status, string initials, string label)
        {
            var lamda = LinqExpression.Create<EntityFramework.Core.Entitys.DatabaseTerrace>(e => !e.DeleteFlag && e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now && (e.PurchaseType == ((int)PurchaseTypeEnum.试用).ToString()));
            lamda = lamda.AndIf(!string.IsNullOrEmpty(artType), e => e.ArticleTypes.Contains(artType));
            lamda = lamda.AndIf(!string.IsNullOrEmpty(domain), e => e.DomainEscs.Contains(domain));
            lamda = lamda.AndIf(languagId != 0, e => e.Language == languagId.ToString());

            lamda = lamda.AndIf(!string.IsNullOrEmpty(searchKey), e => e.Title.Contains(searchKey) || e.ResourceStatisticsText.Contains(searchKey) || e.UseGuideText.Contains(searchKey) || e.InformationText.Contains(searchKey));

            lamda = lamda.AndIf(!string.IsNullOrEmpty(purchaseType), e => e.PurchaseType == purchaseType);
            lamda = lamda.AndIf(status != 0, e => e.Status == status);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(initials), e => e.Initials == initials);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(label), e => e.Label.Contains(label));

            return lamda;
        }

        /// <summary>
        /// 获取自建的数据库
        /// </summary>
        /// <returns></returns>
        public Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> GetSelfBuiltDatabasesQuery(string artType, string domain, int languagId, string searchKey, string purchaseType, int status, string initials, string label)
        {
            var lamda = LinqExpression.Create<EntityFramework.Core.Entitys.DatabaseTerrace>(e => !e.DeleteFlag && e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now && (e.PurchaseType == ((int)PurchaseTypeEnum.自建).ToString()));
            lamda = lamda.AndIf(!string.IsNullOrEmpty(artType), e => e.ArticleTypes.Contains(artType));
            lamda = lamda.AndIf(!string.IsNullOrEmpty(domain), e => e.DomainEscs.Contains(domain));
            lamda = lamda.AndIf(languagId != 0, e => e.Language == languagId.ToString());

            lamda = lamda.AndIf(!string.IsNullOrEmpty(searchKey), e => e.Title.Contains(searchKey) || e.ResourceStatisticsText.Contains(searchKey) || e.UseGuideText.Contains(searchKey) || e.InformationText.Contains(searchKey));

            lamda = lamda.AndIf(!string.IsNullOrEmpty(purchaseType), e => e.PurchaseType == purchaseType);
            lamda = lamda.AndIf(status != 0, e => e.Status == status);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(initials), e => e.Initials == initials);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(label), e => e.Label.Contains(label));

            return lamda;
        }

        /// <summary>
        /// 获取其他的数据库
        /// </summary>
        /// <returns></returns>
        public Expression<Func<EntityFramework.Core.Entitys.DatabaseTerrace, bool>> GetOtherDatabasesQuery(string artType, string domain, int languagId, string searchKey, string purchaseType, int status, string initials, string label)
        {
            var lamda = LinqExpression.Create<EntityFramework.Core.Entitys.DatabaseTerrace>(e => !e.DeleteFlag && e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now && e.PurchaseType == ((int)PurchaseTypeEnum.其他).ToString());
            lamda = lamda.AndIf(!string.IsNullOrEmpty(artType), e => e.ArticleTypes.Contains(artType));
            lamda = lamda.AndIf(!string.IsNullOrEmpty(domain), e => e.DomainEscs.Contains(domain));
            lamda = lamda.AndIf(languagId != 0, e => e.Language == languagId.ToString());
            lamda = lamda.AndIf(!string.IsNullOrEmpty(searchKey), e => e.Title.Contains(searchKey) || e.ResourceStatisticsText.Contains(searchKey) || e.UseGuideText.Contains(searchKey) || e.InformationText.Contains(searchKey));

            lamda = lamda.AndIf(!string.IsNullOrEmpty(purchaseType), e => e.PurchaseType == purchaseType);
            lamda = lamda.AndIf(status != 0, e => e.Status == status);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(initials), e => e.Initials == initials);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(label), e => e.Label.Contains(label));

            return lamda;
        }

        /// <summary>
        /// 获取单页模式下的数据库列表
        /// </summary>
        /// <returns></returns>
        public async Task<DatabaseSinglePageVO> GetDatabaseSinglePageList(string artType, string domain, int languagId, int sortType, string searchKey, string purchaseType, int status, string initials, string label)
        {
            var ChineseDatabasesQuery = this.GetChineseDatabasesQuery(artType, domain, languagId, searchKey, purchaseType, status, initials, label);
            var ForeignDatabasesQuery = this.GetForeignDatabasesQuery(artType, domain, languagId, searchKey, purchaseType, status, initials, label);
            var ProbationDatabaseQuery = this.GetProbationDatabaseQuery(artType, domain, languagId, searchKey, purchaseType, status, initials, label);
            var SelfBuiltDatabasesQuery = this.GetSelfBuiltDatabasesQuery(artType, domain, languagId, searchKey, purchaseType, status, initials, label);
            var OtherDatabasesQuery = this.GetOtherDatabasesQuery(artType, domain, languagId, searchKey, purchaseType, status, initials, label);



            List<DatabaseTerraceDto> ChineseDatabases = new List<DatabaseTerraceDto>();
            List<DatabaseTerraceDto> ForeignDatabases = new List<DatabaseTerraceDto>();
            List<DatabaseTerraceDto> ProbationDatabase = new List<DatabaseTerraceDto>();
            List<DatabaseTerraceDto> SelfBuiltDatabases = new List<DatabaseTerraceDto>();
            List<DatabaseTerraceDto> OtherDatabases = new List<DatabaseTerraceDto>();

            //获取数据列表
            switch (sortType)
            {
                case 1:
                    ChineseDatabases = _databaseRepository.Where(ChineseDatabasesQuery).OrderByDescending(e => e.OrderIndex).ProjectToType<DatabaseTerraceDto>().ToList();
                    ForeignDatabases = _databaseRepository.Where(ForeignDatabasesQuery).OrderByDescending(e => e.OrderIndex).ProjectToType<DatabaseTerraceDto>().ToList();
                    ProbationDatabase = _databaseRepository.Where(ProbationDatabaseQuery).OrderByDescending(e => e.OrderIndex).ProjectToType<DatabaseTerraceDto>().ToList();
                    SelfBuiltDatabases = _databaseRepository.Where(SelfBuiltDatabasesQuery).OrderByDescending(e => e.OrderIndex).ProjectToType<DatabaseTerraceDto>().ToList();
                    OtherDatabases = _databaseRepository.Where(OtherDatabasesQuery).OrderByDescending(e => e.OrderIndex).ProjectToType<DatabaseTerraceDto>().ToList();
                    break;
                case 2:
                    ChineseDatabases = _databaseRepository.Where(ChineseDatabasesQuery).OrderByDescending(e => e.TotalClickNum).ProjectToType<DatabaseTerraceDto>().ToList();
                    ForeignDatabases = _databaseRepository.Where(ForeignDatabasesQuery).OrderByDescending(e => e.TotalClickNum).ProjectToType<DatabaseTerraceDto>().ToList();
                    ProbationDatabase = _databaseRepository.Where(ProbationDatabaseQuery).OrderByDescending(e => e.TotalClickNum).ProjectToType<DatabaseTerraceDto>().ToList();
                    SelfBuiltDatabases = _databaseRepository.Where(SelfBuiltDatabasesQuery).OrderByDescending(e => e.TotalClickNum).ProjectToType<DatabaseTerraceDto>().ToList();
                    OtherDatabases = _databaseRepository.Where(OtherDatabasesQuery).OrderByDescending(e => e.TotalClickNum).ProjectToType<DatabaseTerraceDto>().ToList();
                    break;
                case 3:
                    ChineseDatabases = _databaseRepository.Where(ChineseDatabasesQuery).OrderBy(e => e.Initials).ProjectToType<DatabaseTerraceDto>().ToList();
                    ForeignDatabases = _databaseRepository.Where(ForeignDatabasesQuery).OrderBy(e => e.Initials).ProjectToType<DatabaseTerraceDto>().ToList();
                    ProbationDatabase = _databaseRepository.Where(ProbationDatabaseQuery).OrderBy(e => e.Initials).ProjectToType<DatabaseTerraceDto>().ToList();
                    SelfBuiltDatabases = _databaseRepository.Where(SelfBuiltDatabasesQuery).OrderBy(e => e.Initials).ProjectToType<DatabaseTerraceDto>().ToList();
                    OtherDatabases = _databaseRepository.Where(OtherDatabasesQuery).OrderBy(e => e.Initials).ProjectToType<DatabaseTerraceDto>().ToList();
                    break;

                default:
                    break;
            }

            //组装访问链接
            //1.获取所有在用的链接
            var allUrls = this.GetAllUrl();

            this.AppendDirectUrl(ChineseDatabases, allUrls);
            this.AppendDirectUrl(ForeignDatabases, allUrls);
            this.AppendDirectUrl(ProbationDatabase, allUrls);
            this.AppendDirectUrl(SelfBuiltDatabases, allUrls);
            this.AppendDirectUrl(OtherDatabases, allUrls);

            return new DatabaseSinglePageVO
            {
                ChineseDatabases = ChineseDatabases,
                ForeignDatabases = ForeignDatabases,
                ProbationDatabase = ProbationDatabase,
                SelfBuiltDatabases = SelfBuiltDatabases,
                OtherDatabases = OtherDatabases
            };
        }

        /// <summary>
        /// 取所有的在用链接
        /// </summary>
        /// <returns></returns>
        public List<DatabaseAcessUrlDto> GetAllUrl()
        {
            var allUrls = _directUrlRepository.Where(e => !e.DeleteFlag).ProjectToType<DatabaseAcessUrlDto>().ToList();
            return allUrls;
        }

        /// <summary>
        /// 组装数据库导航与链接的关系
        /// </summary>
        /// <param name="databaseDtoList"></param>
        /// <param name="allUrls"></param>
        public void AppendDirectUrl(IEnumerable<DatabaseTerraceDto> databaseDtoList, List<DatabaseAcessUrlDto> allUrls)
        {
            foreach (var item in databaseDtoList)
            {
                var urls = allUrls.Where(f => f.DatabaseID == item.Id).ToList();
                item.DirectUrls = urls;
            }
        }

        public async Task<bool> IsCollected(Guid databaseId, string userKey)
        {

            return _subscriberRepository.Any(e => !e.DeleteFlag && e.DatabaseID == databaseId && e.UserKey == userKey);

        }

        public async Task<List<DatabaseTerraceDto>> GetDatabaseInPortal(int count, Guid columnId)
        {
            List<DatabaseTerraceDto> databases = await this.GetDatabaseColumnPreview(columnId);

            string baseUrl = await this.GetAppBaseUri("databaseguide");
            //拼接路由
            databases.ForEach(e => e.PortalUrl = $"{baseUrl}/#/web_dataBaseDetail?databaseid={e.Id}");

            return databases.Take(count).ToList();
        }

        public async Task<string> GetAppBaseUri(string appId)
        {
            AppBaseUriRequest request = new AppBaseUriRequest { AppRouteCode = appId };
            AppBaseUriReply reply = new AppBaseUriReply();
            try
            {
                //调用grpc，获取用户分组
                var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();

                reply = await grpcClient.GetAppBaseUriAsync(request);
                var result = reply.FrontUrl;
                return result;
            }
            catch (Exception ex)
            {

                throw Oops.Oh("应用中心异常").StatusCode(HttpStatusKeys.ExceptionCode);

            }
        }

        public async Task<List<OptionDto>> GetUserGroupKV()
        {

            try
            {
                //调用grpc，获取用户分组
                var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>();
                SimpleTableQuery query = new SimpleTableQuery { KeyWord = "", PageIndex = 1, PageSize = 100 };
                var grpcUserGroups = await grpcClient.GetUserGroupListAsync(query);
                var userGroups = grpcUserGroups.Items.ToList().Adapt<List<OptionDto>>();
                return userGroups;
            }
            catch (Exception ex)
            {

                throw Oops.Oh("用户中心异常").StatusCode(HttpStatusKeys.ExceptionCode);

            }

        }

        public async Task<List<OptionDto>> GetUserTypeKV()
        {
            try
            {
                //调用grpc，获取用户类型，
                var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>();
                SimpleTableQuery query = new SimpleTableQuery { KeyWord = "", PageIndex = 1, PageSize = 100 };
                var grpcUserTypes = await grpcClient.GetUserTypeListAsync(query);
                var userTypes = grpcUserTypes.Items.ToList().Adapt<List<OptionDto>>();

                return userTypes;
            }
            catch (Exception)
            {
                throw Oops.Oh("grpc服务异常，未找到对应的用户信息").StatusCode(HttpStatusKeys.ExceptionCode);

            }

        }


        private async Task<List<ProviderResourceItem>> GetProviderResourceItems()
        {
            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<ProviderResourceGrpcService.ProviderResourceGrpcServiceClient>();
            AvailableProviderResourceRequest request1 = new AvailableProviderResourceRequest { Owner = "" };
            AvailableProviderResourceResponse reply1 = new AvailableProviderResourceResponse();
            try
            {
                reply1 = await grpcClient1.GetAllAvailableProviderResourceAsync(request1);
            }
            catch (Exception)
            {
            }

            var providerResources = reply1.ProviderResourceItems.ToList();

            return providerResources;
        }

        /// <summary>
        /// 获取数据库供应上的列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetDatabaseFromCenter(string keyWord)
        {
            var providerResources = await this.GetProviderResourceItems();

            var groups = providerResources.Where(e => e.TerraceFullName.Contains(keyWord ?? "")).GroupBy(e => e.TerraceFullName);

            List<OptionDto> databaseResources = new List<OptionDto>();
            foreach (var item in groups)
            {
                var gid = Guid.NewGuid().ToString();

                var option = new OptionDto
                {
                    Key = item.Key,
                    Value = gid
                };


                option.Child = item.ToList().Adapt<List<OptionDto>>();
                if (item.Count() == 1)
                {
                    option.Child = new List<OptionDto>
                    {
                        new OptionDto
                        {
                            Key = item.Key,
                            Value=item.ToList().First().Provider,
                        }
                    };
                }
                databaseResources.Add(option);
            }

            databaseResources = databaseResources.Where(e => e.Key.Contains(keyWord ?? "")).ToList();
            return databaseResources;
        }
        /// <summary>
        /// 获取本馆选择的资源
        /// </summary>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetSelectedDatabase()
        {

            var resource = _providerResourceRepository.Where(e => !e.DeleteFlag).ToList();
            var groups = resource.GroupBy(e => e.TerraceFullName);

            List<OptionDto> databaseResources = new List<OptionDto>();
            foreach (var item in groups)
            {
                var option = new OptionDto
                {
                    Key = item.Key
                };

                option.Child = item.ToList().Adapt<List<OptionDto>>();
                databaseResources.Add(option);
            }

            return databaseResources;
        }


        /// <summary>
        /// 获取本馆选择的资源
        /// </summary>
        /// <returns></returns>
        public async Task SaveProviderResource(List<string> providers)
        {
            //数据中心取数据库资源
            var providerResources = await this.GetProviderResourceItems();
            providerResources = providerResources.Where(e => providers.Contains(e.Provider)).ToList();

            var providerResourcesDb = providerResources.Adapt<List<ProviderResource>>();

            await _providerResourceRepository.Context.BatchUpdate<ProviderResource>()
                 .Set(b => b.DeleteFlag, b => true)
                 .Where(e => !e.DeleteFlag && providers.Contains(e.DatabaseCode.ToString()))
                 .ExecuteAsync();

            await _providerResourceRepository.InsertAsync(providerResourcesDb);
        }

        /// <summary>
        /// 取中心站的数据作为新增模板
        /// </summary>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        public async Task<DatabaseTerraceDto> GetDatabaseFromCenterAsModel(string databaseId)
        {
            var providerResource = await this.GetProviderResourceItems();
            var ttt = providerResource.FirstOrDefault(e => e.Provider == databaseId)?.Adapt<DatabaseTerraceDto>();
            return providerResource.FirstOrDefault(e => e.Provider == databaseId)?.Adapt<DatabaseTerraceDto>();
        }

        /// <summary>
        /// 获取供应商列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetProviderDtos()
        {
            var peply = await this.GetProviderGrpc();
            var providers = peply.Adapt<List<OptionDto>>();
            return providers;
        }
        /// <summary>
        /// grpc方法，获取供应商列表
        /// </summary>
        /// <returns></returns>
        private async Task<List<DatabaseProviderItem>> GetProviderGrpc()
        {
            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<ProviderResourceGrpcService.ProviderResourceGrpcServiceClient>();
            GetAllDatabaseProviderRequest request1 = new GetAllDatabaseProviderRequest { Owner = "" };
            GetAllDatabaseProviderResponse reply1 = new GetAllDatabaseProviderResponse();
            try
            {
                reply1 = await grpcClient1.GetAllDatabaseProviderAsync(request1);
            }
            catch (Exception ex)
            {
            }

            var providerList = reply1.DatabaseProviderItems.ToList();

            return providerList;
        }


        /// <summary>
        /// grpc调用头尾模板的详情
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="footerId"></param>
        /// <returns></returns>
        public async Task<HeaderFooterReply> GetTemplateDetailGrpc(string headerId, string footerId)
        {
            //通过grpc服务获取尾部模板
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<SceneManageGrpcService.SceneManageGrpcServiceClient>();
            HeaderFooterRequest request = new HeaderFooterRequest { HeaderId = headerId, FooterId = footerId };
            try
            {
                var Reply = await grpcClient.GetHeaderFooterDetailAsync(request);
                return Reply;
            }
            catch (Exception ex)
            {
                throw Oops.Oh("场景中心出现异常").StatusCode(HttpStatusKeys.ExceptionCode);
            }
        }

        /// <summary>
        /// 获取新闻头尾模板
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<DatabaseTemplateDto>> GetDatabaseBodyTemplate(int type)
        {
            var reply = await this.GetTemplateListGrpc(type.ToString());
            var templateList = reply.HeaderFooterList.ToList().Adapt<List<DatabaseTemplateDto>>();
            return templateList;
        }

        /// <summary>
        /// grpc调用头尾模板的详情
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<HeaderFooterListReply> GetTemplateListGrpc(string type)
        {
            //通过grpc服务获取尾部模板
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<SceneManageGrpcService.SceneManageGrpcServiceClient>();
            HeaderFooterListRequest request = new HeaderFooterListRequest { Type = type };
            try
            {
                var Reply = await grpcClient.GetHeaderFooterListAsync(request);
                return Reply;
            }
            catch (Exception ex)
            {
                throw Oops.Oh("场景中心出现异常").StatusCode(HttpStatusKeys.ExceptionCode);
            }
        }
        /// <summary>
        /// 获取专辑
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetAlbumFromCenter(string provider)
        {
            var reply = await this.GetAlbumFromCenterGrpc(provider);
            var list = reply.Adapt<List<OptionDto>>();

            return list;
        }

        /// <summary>
        /// grpc调用专辑的列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<ResourceAlbumItem>> GetAlbumFromCenterGrpc(string provider)
        {
            //通过grpc服务获取尾部模板
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<ProviderResourceGrpcService.ProviderResourceGrpcServiceClient>();
            GetResourceAlbumByProviderRequest request = new GetResourceAlbumByProviderRequest { Provider = provider };
            try
            {
                var Reply = await grpcClient.GetResourceAlbumByProviderAsync(request);
                return Reply.ResourceAlbumItem.ToList();
            }
            catch (Exception ex)
            {
                throw Oops.Oh("场景中心出现异常").StatusCode(HttpStatusKeys.ExceptionCode);
            }


        }

        /// <summary>
        /// 获取数据库导航应用的默认模板字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<DatabaseDefaultTemplateDto>> GetDatabaseDefaultTemplateDtoList()
        {
            var list = _defaultTemplateRepository.Where(e => !e.DeleteFlag).ProjectToType<DatabaseDefaultTemplateDto>().ToList();
            return list;
        }

        /// <summary>
        /// 从数据中心获取资源类型- 系统自带类型 
        /// </summary>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetSysSourceTypeDto()
        {

            var sourceTypes = await this.GetAllSourceTypeGrpc();
            var sysList = sourceTypes.Where(e => string.IsNullOrEmpty(e.UserKey));
            return sysList.Adapt<List<OptionDto>>();

        }

        /// <summary>
        /// 从数据中心获取资源类型- 自定义类型 
        /// </summary>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetCusSourceTypeDto()
        {
            var sourceTypes = await this.GetAllSourceTypeGrpc();
            var cusList = sourceTypes.Where(e => !string.IsNullOrEmpty(e.UserKey));
            return cusList.Adapt<List<OptionDto>>();
        }


        /// <summary>
        /// 从数据中心获取资源类型- 自定义类型 
        /// </summary>
        /// <returns></returns>
        public async Task<List<SourceTypeItem>> GetAllSourceTypeGrpc()
        {
            try
            {
                var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<SourceTypeGrpcService.SourceTypeGrpcServiceClient>();
                AllSourceRequest request = new AllSourceRequest { Typs = "" };

                var reply = await grpcClient.GetAllSourceTypeAsync(request);
                return reply.SourceTypes.ToList();
            }
            catch (Exception ex)
            {

                throw Oops.Oh("数据中心grpc异常").StatusCode(HttpStatusKeys.ExceptionCode);
            }

        }

        /// <summary>
        /// 添加资源类型拓展项
        /// </summary>
        /// <param name="sourceName">拓展项名称</param>
        /// <returns></returns>


        public async Task<int> AddCustomSourceType(string sourceName, string userKey)
        {
            {
                try
                {
                    var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<SourceTypeGrpcService.SourceTypeGrpcServiceClient>();
                    AddSourceTypeRequest request = new AddSourceTypeRequest { Name = sourceName, UserKey = userKey };

                    var reply = await grpcClient.AddSourceTypeAsync(request);
                    return reply.Code;
                }
                catch (Exception ex)
                {

                    throw Oops.Oh("数据中心grpc异常").StatusCode(HttpStatusKeys.ExceptionCode);
                }
            }
        }

        public async Task<VisitCountInfo> IncreaseVisitCount(Guid databaseId)
        {
            VisitCountInfo count = new VisitCountInfo();
            var database = _databaseRepository.FirstOrDefault(e => e.Id == databaseId);
            count.MonthCount = ++database.MonthClickNum;
            count.TotalCount = ++database.TotalClickNum;
            _databaseRepository.Update(database);
            return count;
        }
    }
}