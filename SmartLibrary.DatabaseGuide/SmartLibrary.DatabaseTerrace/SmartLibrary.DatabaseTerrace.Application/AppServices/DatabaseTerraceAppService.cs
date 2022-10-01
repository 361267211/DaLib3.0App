
using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartLibrary.DatabaseTerrace.Application.Services.RemoteProxy;
using SmartLibrary.DatabaseTerrace.Common.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.ViewModels;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.DatabaseTerrace.Application.Filter;
using Furion.JsonSerialization;
using Furion;
using SmartLibrary.DatabaseTerrace.Application.Dto;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.ViewModels.Request;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.SysConst;
using System.Security.Claims;
using SmartLibrary.DatabaseTerrace.Common.Const;
using SmartLibrary.Assembly.Application.Attributes;

namespace SmartLibrary.DatabaseTerrace.Application
{
    [Authorize]
    /// <summary>
    /// 应用服务接口
    /// </summary>
   // [DatabaseActionFilter]
    public class DatabaseTerraceAppService : IDynamicApiController
    {

        public IConfiguration _configuration { get; }

        private readonly IDatabaseTerraceService _databaseTerraceService;
        public DatabaseTerraceAppService(IConfiguration configuration,
            IDatabaseTerraceService databaseTerraceService
            )
        {
            _configuration = configuration;
            _databaseTerraceService = databaseTerraceService;
        }



        #region 数据库管理
        /// <summary>
        /// 批量删除导航栏目
        /// </summary>
        /// <param name="ids">数据库id</param>
        /// <returns></returns>
        [HttpDelete]
        [QueryParameters]

        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        public async Task BatchDeleteDatabaseColumn([FromBody] List<Guid> ids)
        {
            await _databaseTerraceService.BatchDeleteDatabaseColumn(ids);

        }
        /// <summary>
        /// 获取数据库总导航的视图模型(按条件筛选)
        /// </summary>
        /// <param name="serachKey">搜索关键词</param>
        /// <param name="languageId">语言类型id</param>
        /// <param name="articleType">文献类型</param>
        /// <param name="domainEscs">学科类型</param>
        /// <param name="purchaseType">采购类型</param>
        /// <param name="status">启用状态</param>
        /// <param name="timeliness">按时效性分页签，1-效期内，2-效期外，3-隐藏</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">单页数量</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [QueryParameters]
        //  [Authorize]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]

        public async Task<DatabaseTerracesVO> GetDatabaseTerraceList(string serachKey, int languageId, string articleType, string domainEscs, string purchaseType, int status, int timeliness, string initials, int pageIndex, int pageSize)
        {
            var page = await _databaseTerraceService.GetDatabaseTerraceList(serachKey, languageId, articleType, domainEscs, purchaseType, status, timeliness, initials, sortType:1, label:"", pageIndex, pageSize,IsShow:false);
            var allUrls = _databaseTerraceService.GetAllUrl();
            _databaseTerraceService.AppendDirectUrl(page.Items.ToList(), allUrls);

            var databaseVO = new DatabaseTerracesVO
            {
                StatisticsCount = await _databaseTerraceService.GetStatisticsCount(serachKey, languageId, articleType, domainEscs, purchaseType, status, isShow: false),
                DatabaseTerraces = page


            };
            return databaseVO;
        }
        /// <summary>
        /// 批量推荐平台/数据库
        /// </summary>
        /// <param name="ids">id列表，字符数组</param>
        [HttpPut]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]

        public async Task BatchRecommendDatabaseTerrace([FromBody] List<Guid> ids)
        {
            await _databaseTerraceService.BatchRecommendDatabaseTerrace(ids);
        }

        /// <summary>
        /// 批量恢复平台/数据库的状态
        /// </summary>
        /// <param name="ids">id列表，字符数组</param>
        [HttpPut]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]


        public async Task BatchRecoverDatabaseTerrace([FromBody] List<Guid> ids)
        {
            await _databaseTerraceService.BatchRecoverDatabaseTerrace(ids);
        }

        /// <summary>
        /// 批量删除平台/数据库 逻辑删
        /// </summary>
        /// <param name="ids">id列表，字符数组</param>
        [HttpDelete]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]

        public async Task BatchDeleteDatabaseTerrace([FromBody] List<Guid> ids)
        {
            await _databaseTerraceService.BatchDeleteTerrace(ids);
        }

        #endregion

        #region 数据库编辑
        /*        /// <summary>
                /// 保存供应商资源
                /// </summary>
                /// <returns></returns>
                [HttpPost]
                public async Task SaveProviderResource([FromBody] List<string> providers)
                {
                    await _databaseTerraceService.SaveProviderResource(providers);
                }*/

        /// <summary>
        /// 根据Id获取平台/数据库 表单的数据
        /// </summary>
        /// <param name="databaseID">数据库id</param>
        /// <returns></returns>
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]

        [QueryParameters]
        [HttpGet]
        public async Task<DatabaseTerraceDto> GetDatabaseTerrace(Guid databaseID)
        {
            return await _databaseTerraceService.GetDatabaseTerrace(databaseID);
        }

        /// <summary>
        /// 保存数据库平台
        /// </summary>
        /// <param name="databaseTerraceDto">待保存的数据库表单对象</param>
        /// <returns></returns>
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]

        [HttpPost]
        public async Task SaveDatabaseTerrace([FromBody] DatabaseTerraceDto databaseTerraceDto)
        {
            if (databaseTerraceDto.Id == Guid.Empty)
            {
                await _databaseTerraceService.InsertDatabaseTerrace(databaseTerraceDto);

            }
            else
            {
                await _databaseTerraceService.UpdateDatabaseTerrace(databaseTerraceDto);
            }
        }

        /// <summary>
        /// 从中心站获取数据库的结构数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        [UnitOfWork]
        [QueryParameters]
        public async Task<DatabaseOptionVO> GetDatabaseFromCenter(string keyWord)
        {
            DatabaseOptionVO database = new DatabaseOptionVO();
            database.AllDatabase = await _databaseTerraceService.GetDatabaseFromCenter(keyWord);
            database.SelectedDatabase = await _databaseTerraceService.GetSelectedDatabase();
            return database;
        }
        /// <summary>
        /// 获取专辑信息
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        [QueryParameters]
        public async Task<List<OptionDto>> GetAlbumFromCenter(string provider)
        {
            List<OptionDto> Album = await _databaseTerraceService.GetAlbumFromCenter(provider);
            return Album;
        }

        /// <summary>
        /// 从中心站获取数据库的结构数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]

        public async Task<DatabaseTerraceDto> GetDatabaseFromCenterAsModel(string databaseId)
        {
            DatabaseTerraceDto database = await _databaseTerraceService.GetDatabaseFromCenterAsModel(databaseId);
            return database;
        }

        /// <summary>
        /// 获取用户分组/类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserGroupInfosVO> GetUserGroupInfos()
        {
            UserGroupInfosVO groupKeyValue = new UserGroupInfosVO();
            groupKeyValue.UserGroupKV = await _databaseTerraceService.GetUserGroupKV();
            groupKeyValue.UserTypeKV = await _databaseTerraceService.GetUserTypeKV();
            return groupKeyValue;
        }
        #endregion

        #region 杂项
        /// <summary>
        /// 从数据中心获取学科分类树
        /// </summary>
        /// <returns></returns>


        [HttpGet]
        public async Task<List<OptionDto>> GetDomainEscTree()
        {
            return await _databaseTerraceService.GetDomainTree();
        }

        /// <summary>
        /// 获取数据库导航应用的下拉列表
        /// </summary>
        /// <returns></returns>


        public async Task<DatabaseInitializedModel> GetDatabaseInitializedModel()
        {
            var domains = await _databaseTerraceService.GetDomainEscDtos();
            var labels = await _databaseTerraceService.GetLabelDtos();
            List<OptionDto> providers = await _databaseTerraceService.GetProviderDtos();
            List<OptionDto> sysSourceTypes = await _databaseTerraceService.GetSysSourceTypeDto();
            List<OptionDto> cusSourceTypes = await _databaseTerraceService.GetCusSourceTypeDto();


            List<OptionDto> SourceTypes = new List<OptionDto>();

            SourceTypes.AddRange(sysSourceTypes);
            SourceTypes.AddRange(cusSourceTypes);



            return new DatabaseInitializedModel
            {
                //ArticleTypeDtos = new List<OptionDto>
                // {
                //     new OptionDto{ Value=((int)ArticleTypeEnum.期刊).ToString(), Key=Enum.GetName(ArticleTypeEnum.期刊)},
                //     new OptionDto{ Value=((int)ArticleTypeEnum.学位论文).ToString(), Key=Enum.GetName(ArticleTypeEnum.学位论文)},
                //     new OptionDto{ Value=((int)ArticleTypeEnum.图书).ToString(), Key=Enum.GetName(ArticleTypeEnum.图书)}
                // },

                ArticleTypeDtos = SourceTypes,
                DomainEscDtos = domains,
                languageDtos = new List<OptionDto>
                 {
                     new OptionDto(){ Value="1", Key="中文"},
                     new OptionDto(){ Value="2", Key="英文"},
                 },//TODO:从数据中心取
                PurchaseTypeDtos = new List<OptionDto>()
                 {
                     new OptionDto{ Value="1", Key="订购"},
                     new OptionDto{ Value="2", Key="试用"},
                     new OptionDto{ Value="3", Key="免费"},
                     new OptionDto{ Value="4", Key="自建"},
                     new OptionDto{ Value="5", Key="其他"},
                 },
                StatusDtos = new List<OptionDto>
                {
                    new OptionDto{ Value="1", Key="正常"},
                    new OptionDto{ Value="2", Key="推荐"},
                    new OptionDto{ Value="3", Key="隐藏"},
                },
                Labels = labels,
                DatabaseProviderDtos = providers,
                OtherIntroduceDtos = new List<OptionDto>
                {
                     new OptionDto{ Value=((int)OtherIntroducsEnum.使用帮助).ToString(), Key=Enum.GetName(OtherIntroducsEnum.使用帮助)},
                     new OptionDto{ Value=((int)OtherIntroducsEnum.资源统计).ToString(), Key=Enum.GetName(OtherIntroducsEnum.资源统计)},
                },
                OrderRuleDtos = new List<OptionDto>
                {
                     new OptionDto{ Value=((int)OrderRuleEnum.默认排序).ToString(), Key=Enum.GetName(OrderRuleEnum.默认排序)},
                     new OptionDto{ Value=((int)OrderRuleEnum.月点击量).ToString(), Key=Enum.GetName(OrderRuleEnum.月点击量)},
                     new OptionDto{ Value=((int)OrderRuleEnum.总点击量).ToString(), Key=Enum.GetName(OrderRuleEnum.总点击量)},
                     new OptionDto{ Value=((int)OrderRuleEnum.创建时间).ToString(), Key=Enum.GetName(OrderRuleEnum.创建时间)},
                }
            };

        }
        /// <summary>
        /// 获取本馆在用学科分类 字符数组
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<List<string>> GetDomainEscs()
        {
            return await _databaseTerraceService.GetDomainEsc();
        }

        /// <summary>
        /// 保存学科分类
        /// </summary>
        /// <param name="domainEscs"></param>
        /// <returns></returns>

        [UnitOfWork]
        [HttpPost]
        public async Task SaveDomainEscDtos([FromBody] List<string> domainEscs)
        {
            await _databaseTerraceService.SaveDomainEscDtos(domainEscs);
        }


        /// <summary>
        /// 批量删除自定义标签
        /// </summary>
        /// <param name="ids">数据库id</param>
        /// <returns></returns>

        [HttpDelete]
        public async Task BatchDeleteCoustomLabel([FromBody] List<Guid> ids)
        {
            await _databaseTerraceService.BatchDeleteLabel(ids);
        }

        /// <summary>
        /// 获取在用自定义标签
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页数量</param>
        /// <returns></returns>

        [HttpGet]
        public async Task<PagedList<OptionDto>> GetCoustomLabels(int pageIndex, int pageSize)
        {
            return await _databaseTerraceService.GetCoustomLabels(pageIndex, pageSize);
        }

        /// <summary>
        /// 获取链接名称
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">单页数量</param>
        /// <returns></returns>

        [HttpGet]
        public async Task<PagedList<OptionDto>> GetAcessUrlName(int pageIndex, int pageSize)
        {
            return await _databaseTerraceService.GetAcessUrlName(pageIndex, pageSize);
        }

        /// <summary>
        /// 批量删除链接名称
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>

        [HttpDelete]
        public async Task BatchDeleteAcessUrlName([FromBody] List<Guid> ids)
        {
            await _databaseTerraceService.BatchDeleteAcessUrlName(ids);
        }

        /// <summary>
        /// 批量编辑自定义标签，全删后新增
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]

        [UnitOfWork]
        public async Task SaveCustomLabels([FromBody] List<OptionDto> Labels)
        {
            await _databaseTerraceService.SaveCustomLabels(Labels);
        }

        /// <summary>
        /// 获取头尾模板列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<List<DatabaseTemplateDto>> GetDatabaseBodyTemplate(int type)
        {
            return _databaseTerraceService.GetDatabaseBodyTemplate(type);
        }

        public Task<List<DatabaseDefaultTemplateDto>> GetDatabaseDefaultTemplateDtoList()
        {
            return _databaseTerraceService.GetDatabaseDefaultTemplateDtoList();
        }

        #endregion

        #region 数据库栏目
        /// <summary>
        /// 获取所有的数据库栏目(总列表)
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">单页数量</param>
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        [QueryParameters]
        [HttpGet]
        public async Task<PagedList<DatabaseColumnDto>> GetDatabaseColumns(int pageIndex, int pageSize)
        {
            return await _databaseTerraceService.GetDatabaseColumns(pageIndex, pageSize);
        }

        /// <summary>
        /// 根据ID获取数据库栏目
        /// </summary>
        /// <param name="columnID">栏目id</param>
        /// <returns></returns>
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]

        [HttpGet]
        public async Task<DatabaseColumnDto> GetDatabaseColumn(Guid columnID)
        {
            return await _databaseTerraceService.GetDatabaseColumn(columnID);
        }
        /// <summary>
        /// 保存数据库导航栏目
        /// </summary>
        /// <param name="databaseColumnDto">待保存的栏目表单对象</param>
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]

        [UnitOfWork]
        [HttpPost]
        public async Task SaveDatabaseColumn([FromBody] DatabaseColumnDto databaseColumnDto)
        {
            if (databaseColumnDto.Id == Guid.Empty)
            {
                //id为空时新增
                await _databaseTerraceService.InsertDatabaseColumn(databaseColumnDto);
            }
            else
            {
                //id非空时保存
                await _databaseTerraceService.UpdateDatabaseColumn(databaseColumnDto);
            }
        }

        /// <summary>
        /// 获取数据库栏目的预览视图
        /// </summary>
        /// <param name="columnID">栏目id</param>
        /// <returns></returns>
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]

        [HttpGet]
        public async Task<DatabaseColumnPreviewVO> GetDatabaseColumnPreview(Guid columnID)
        {
            return new DatabaseColumnPreviewVO
            {
                DatabaseColumnPreviewDtos = await _databaseTerraceService.GetDatabaseColumnPreview(columnID)
            };
        }

        /// <summary>
        /// 前端通过拖拽方法排序
        /// </summary>
        /// <param name="sourceArtIndex"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="isUp"></param>        
        /// <returns></returns>
        [HttpPost]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]

        [UnitOfWork]
        public async Task SortDatabaseByDrag(int sourceArtIndex, int destinationIndex, bool isUp)
        {
            await _databaseTerraceService.SortDatabaseByDrag(sourceArtIndex, destinationIndex, isUp);
        }

        /// <summary>
        /// 通过弹窗输入绝对序号实现排序
        /// </summary>
        /// <param name="sourceArtIndex">源的序号</param>
        /// <param name="absoluteIndex">绝对序号</param>
        /// <returns></returns>
        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        [HttpPost]
        //// [ApiDescriptionSettings(Tag = "编辑文献操作")]
        public async Task SortSourceFromImportByDestination(int sourceArtIndex, int absoluteIndex)
        {
            await _databaseTerraceService.SortSourceFromImportByDestination(sourceArtIndex, absoluteIndex);
        }

        #endregion

        #region 应用设置

        /// <summary>
        /// 获取数据库导航的应用设置信息
        /// </summary>
        /// <returns></returns>

        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        [HttpGet]
        public Task<DatabaseTerraceSettingsDto> GetDatabaseSettings()
        {
            return _databaseTerraceService.GetDatabaseSettings();
        }
        /// <summary>
        /// 保存数据库导航的应用设置
        /// </summary>
        /// <param name="databaseTerraceSettings">应用设置的表单对象</param>
        /// <returns></returns>

        [UnitOfWork]
        [AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        [HttpPost]
        public async Task SaveDatabaseSettings(DatabaseTerraceSettingsDto databaseTerraceSettings)
        {
            await _databaseTerraceService.SaveDatabaseSettings(databaseTerraceSettings);
        }

        /// <summary>
        /// 添加资源类型拓展项
        /// </summary>
        /// <param name="sourceName">拓展项名称</param>
        /// <returns></returns>


        //[AuthorizeMultiplePolicy(PolicyKey.StaffAuth, false)]
        [HttpPost]
        public async Task<int> AddCustomSourceType([FromBody] string sourceName)
        {

            var userKey = App.User?.FindFirstValue(Const.userKey);

            var sourceCode = await _databaseTerraceService.AddCustomSourceType(sourceName, userKey);
            return sourceCode;
        }





        #endregion

        #region 门户前台
        [HttpGet]
        [QueryParameters]
        [ApiDescriptionSettings(Tag = "门户前台")]
        [PermissionObjAttribute("Database")]
        [AuthorizeMultiplePolicy(PolicyKey.PortalColumn, false)] 

        public async Task<VisitCountInfo> VisitDatabases(Guid databaseId)
        {
            var count = await _databaseTerraceService.IncreaseVisitCount(databaseId);
            return count;
        }


        /// <summary>
        /// 获取推荐的数据库
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ApiDescriptionSettings(Tag = "门户前台")]

        public async Task<RecommendDatabasesVO> GetRecommendDatabases()
        {
            var mo = await _databaseTerraceService.GetDatabasesOrderByMonthClick();
            var to = await _databaseTerraceService.GetDatabasesOrderByTotalClick();

            var my = await _databaseTerraceService.GetCollectionDatabase();

            var urls = _databaseTerraceService.GetAllUrl();
            _databaseTerraceService.AppendDirectUrl(mo, urls);
            _databaseTerraceService.AppendDirectUrl(to, urls);

            foreach (var item in my)
            {
                item.DirectUrls = urls.Where(f => f.DatabaseID == item.DatabaseID).ToList();
            }

            List<Guid> strs = new List<Guid>();
            my.ForEach(e => strs.Add(e.DatabaseID));

            var monthlyHotDatabase = from a in mo
                                     where !strs.Contains(a.Id)
                                     select new DatabaseSubscriberDto
                                     {
                                         TypeName = "最近热门",
                                         Title = a.Title,
                                         DatabaseID = a.Id,
                                         Introduction = a.Introduction,
                                         TotalCount = a.TotalClickNum,
                                         DirectUrls = a.DirectUrls,
                                         Cover = a.Cover

                                     };

            var totalityHotDatabase = from a in to
                                      where !strs.Contains(a.Id)
                                      select new DatabaseSubscriberDto
                                      {
                                          TypeName = "经常访问",
                                          Title = a.Title,
                                          DatabaseID = a.Id,
                                          Introduction = a.Introduction,
                                          TotalCount = a.TotalClickNum,
                                          DirectUrls = a.DirectUrls,
                                          Cover = a.Cover
                                      };

            return new RecommendDatabasesVO
            {
                MonthlyHotDatabase = monthlyHotDatabase.ToList(),
                TotalityHotDatabase = totalityHotDatabase.ToList(),
                MySubscriberDatabase = my,
            };
        }

        /// <summary>
        /// 获取推荐的数据库
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ApiDescriptionSettings(Tag = "门户前台")]
        [AuthorizeMultiplePolicy(PolicyKey.UnAuthKey, false)]

        public async Task<MyFavoriteDatabasesReply> GetMyFavoriteDatabases(int count)
        {
            var reply = new MyFavoriteDatabasesReply();
            if (App.User?.FindFirstValue(Const.userKey) == null)
            {
                return new MyFavoriteDatabasesReply();
            }

            var mo = await _databaseTerraceService.GetDatabasesOrderByMonthClick();
            var to = await _databaseTerraceService.GetDatabasesOrderByTotalClick();
            var my = await _databaseTerraceService.GetSubscribeDatabase();

            var urls = _databaseTerraceService.GetAllUrl();

            //添加直接链接
            _databaseTerraceService.AppendDirectUrl(mo, urls);
            _databaseTerraceService.AppendDirectUrl(to, urls);
            _databaseTerraceService.AppendDirectUrl(my, urls);

            var moVo = mo.Adapt<List<DatabaseItemViewModel>>();
            var toVo = to.Adapt<List<DatabaseItemViewModel>>();
            var myVo = my.Adapt<List<DatabaseItemViewModel>>();

            foreach (var item in toVo)
            {
                item.Type = 2;
            }

            foreach (var item in myVo)
            {
                item.Type = 1;
            }

            var favorite = new List<DatabaseItemViewModel>();
            favorite.AddRange(myVo);
            favorite.AddRange(toVo.Where(e => !favorite.Any(f => f.Id == e.Id)));

            reply.Databases = favorite.Take(count).ToList();

            string baseUrl = await _databaseTerraceService.GetAppBaseUri(uri: "databaseguide");
            //拼接路由
            reply.MoreUrl = $"{baseUrl}/#/web_dataBaseHome";
            reply.Databases.ForEach(e => e.PortalUrl = $"{baseUrl}/#/web_dataBaseDetail?databaseid={e.Id}");

            return reply;
        }

        /// <summary>
        ///  查数据库导航详情--前台
        /// </summary>
        [HttpGet]
        [QueryParameters]
        [PermissionObjAttribute("Database")]
        [ApiDescriptionSettings(Tag = "门户前台")]
        [AuthorizeMultiplePolicy(PolicyKey.PortalColumn, false)]

        public async Task<DatabaseTerraceDto> GetDatabaseTerracePortal(Guid databaseId)
        {
            var database = await _databaseTerraceService.GetDatabaseTerrace(databaseId);

            var userKey = App.HttpContext.User.FindFirst("UserKey")?.Value;
            if (!string.IsNullOrEmpty(userKey))
            {
                //是否已经收藏该专题
                database.IsCollected = await _databaseTerraceService.IsCollected(databaseId, App.HttpContext.User.FindFirst("UserKey").Value);
            }
            return database;
        }

        /// <summary>
        /// 获取当前用户收藏的数据库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiDescriptionSettings(Tag = "门户前台")]
        [AuthorizeMultiplePolicy(PolicyKey.UnAuthKey, false)]

        public async Task<List<DatabaseSubscriberDto>> GetCollectionDatabase()
        {
            var urls = _databaseTerraceService.GetAllUrl();
            var subs = await _databaseTerraceService.GetCollectionDatabase();
            foreach (var item in subs)
            {
                item.DirectUrls = urls.Where(e => !e.DeleteFlag && e.DatabaseID == item.DatabaseID).ToList();
            }
            return subs;
        }


        /// <summary>
        /// 收藏数据库
        /// </summary>
        /// <param name="databaseId">数据库id</param>
        /// <returns></returns>
        [HttpGet]
        [PermissionObjAttribute("Database")]
        [ApiDescriptionSettings(Tag = "门户前台")]
        public async Task CollectionDatabase(Guid databaseId)
        {
            await _databaseTerraceService.CollectionDatabase(databaseId);
        }

        /// <summary>
        /// 删除该条收藏
        /// </summary>
        /// <param name="databaseId">数据库id</param>
        /// <returns></returns>
        [HttpDelete]
        [QueryParameters]
        [ApiDescriptionSettings(Tag = "门户前台")]
        [AuthorizeMultiplePolicy(PolicyKey.UnAuthKey, false)]

        public async Task DeleteCollectionDatabase(Guid databaseId)
        {
            await _databaseTerraceService.DeleteCollectionDatabase(databaseId);
        }

        /// <summary>
        /// 获取数据库总导航的视图模型(按条件筛选)
        /// </summary>
        /// <param name="serachKey">搜索关键词</param>
        /// <param name="languageId">语言类型id</param>
        /// <param name="articleType">文献类型</param>
        /// <param name="domainEscs">学科类型</param>
        /// <param name="purchaseType">采购类型</param>
        /// <param name="status">启用状态</param>
        /// <param name="initials"></param>
        /// <param name="sortType"></param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">单页数量</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [QueryParameters]
        [ApiDescriptionSettings(Tag = "门户前台")]
        public async Task<DatabaseTerracesVO> GetDatabaseTerraceListPortal(string serachKey, int languageId, string articleType, string domainEscs, string purchaseType, int status, string initials, int sortType, string label, int pageIndex = 1, int pageSize = 10)
        {
            //取列表合集
            var databaseVO = new DatabaseTerracesVO
            {
                DatabaseTerraces = await _databaseTerraceService.GetDatabaseTerraceList(serachKey, languageId, articleType, domainEscs, purchaseType, status: 9, timeliness: 1, initials, sortType, label, pageIndex, pageSize, IsShow: true)
            };

            //拼装外部链接
            var urls = _databaseTerraceService.GetAllUrl();
            _databaseTerraceService.AppendDirectUrl(databaseVO.DatabaseTerraces.Items, urls);
            return databaseVO;
        }

        /// <summary>
        /// 获取数据库导航的应用设置信息--前台
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [QueryParameters]
        [ApiDescriptionSettings(Tag = "门户前台")]
        public async Task<DatabaseTerraceSettingsDto> GetDatabaseSettingsPortal()
        {
            return await _databaseTerraceService.GetDatabaseSettings();
        }


        /// <summary>
        /// 获取单页模式下的数据库列表--前台
        /// </summary>
        /// <param name="artType">文献类型</param>
        /// <param name="domain">学科分类</param>
        /// <param name="languagId">语言id</param>
        /// <param name="sortType">1-默认排序，2-总量倒叙，3-字母排序</param>
        /// <returns></returns>
        [HttpGet]
        [QueryParameters]
        [ApiDescriptionSettings(Tag = "门户前台")]
        public async Task<DatabaseSinglePageVO> GetDatabaseSinglePageListPortal(string serachKey, int languagId, string artType, string domain, string purchaseType, int status, string initials, int sortType, string label)
        {
            return await _databaseTerraceService.GetDatabaseSinglePageList(artType, domain, languagId, sortType, serachKey, purchaseType, status, initials, label);
        }

        /// <summary>
        /// 查找门户首页的数据库列表
        /// </summary>
        /// <param name="count">匹配数量</param>
        /// <param name="columnId">绑定栏目的id</param>
        /// <returns></returns>
        [HttpPost]
        [ApiDescriptionSettings(Tag = "门户首页")]      

        public async Task<List<PortalColumnVo<DatabaseTerraceDto>>> GetDatabaseInPortal([FromBody] List<ColumnRequest> requests)
        {
            List<PortalColumnVo<DatabaseTerraceDto>> reply = new List<PortalColumnVo<DatabaseTerraceDto>>();
            foreach (var request in requests)
            {
                var columnId = request.ColumnId;
                var column = await _databaseTerraceService.GetDatabaseColumn(new Guid(request.ColumnId));
                var allUri = _databaseTerraceService.GetAllUrl();
                var list = await _databaseTerraceService.GetDatabaseInPortal(request.Count, new Guid(request.ColumnId));
                //添加链接
                _databaseTerraceService.AppendDirectUrl(list, allUri);
                PortalColumnVo<DatabaseTerraceDto> keyValue = new PortalColumnVo<DatabaseTerraceDto>
                {
                    ColumnId = request.ColumnId,
                    ColumnName = column.ColumnName,
                    List = list
                };
                reply.Add(keyValue);
            }
            return reply;
        }
        #endregion

    }
}

