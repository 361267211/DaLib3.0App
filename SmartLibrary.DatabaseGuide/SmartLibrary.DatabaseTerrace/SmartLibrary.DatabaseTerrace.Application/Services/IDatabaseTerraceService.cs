
using Grpc.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartLibrary.DatabaseTerrace.Application.Dto;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.ViewModels;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Application
{
    public interface IDatabaseTerraceService
    {
        Task<PagedList<DatabaseTerraceDto>> GetDatabaseTerraceList(string serachKey, int languageId, string articleType, string domainEscs, string purchaseType, int status, int timeliness, string initials, int sortType,string label, int pageIndex, int pageSize, bool? IsShow);
        Task<List<DatabaseTerraceDto>> GetAllDatabaseTerrace();
        Task<StatisticsCountDto> GetStatisticsCount(string serachKey, int languageId, string articleType, string domainEscs, string purchaseType, int status,bool isShow);
        Task<DatabaseTerraceDto> GetDatabaseTerrace(Guid databaseID);
        Task InsertDatabaseTerrace(DatabaseTerraceDto databaseTerraceDto);
        /// <summary>
        /// 获取现存数据库总数
        /// </summary>
        /// <returns></returns>
        Task<int> GetDatabaseTotalCount();
        Task UpdateDatabaseTerrace(DatabaseTerraceDto databaseTerraceDto);

        Task BatchRecommendDatabaseTerrace(List<Guid> ids);
        Task BatchDeleteTerrace(List<Guid> ids);
        Task<List<OptionDto>> GetDomainEscDtos();
        Task<List<OptionDto>> GetLabelDtos();
        Task<PagedList<DatabaseColumnDto>> GetDatabaseColumns(int pageIndex, int pageSize);
        Task<DatabaseColumnDto> GetDatabaseColumn(Guid columnID);
        Task<List<DatabaseTerraceDto>> GetDatabaseColumnPreview(Guid columnID);
        Task<DatabaseTerraceSettingsDto> GetDatabaseSettings();
        Task SaveDatabaseSettings(DatabaseTerraceSettingsDto databaseTerraceSettings);
        Task CollectionDatabase(Guid databaseId);

        /// <summary>
        /// 获取已经收藏的数据库列表
        /// </summary>
        /// <returns></returns>
        Task<List<DatabaseTerraceDto>> GetSubscribeDatabase();

        Task DeleteCollectionDatabase(Guid databaseId);
        Task<List<DatabaseSubscriberDto>> GetCollectionDatabase();
        Task<List<DatabaseTerraceDto>> GetDatabasesOrderByTotalClick();
        Task<List<DatabaseTerraceDto>> GetDatabasesOrderByMonthClick();
        Task BatchRecoverDatabaseTerrace(List<Guid> ids);
        Task InsertDatabaseColumn(DatabaseColumnDto databaseColumnDto);
        Task UpdateDatabaseColumn(DatabaseColumnDto databaseColumnDto);
        Task UpdateDatabaseColumnRules(DatabaseColumnDto databaseColumnDto, EntityEntry<DatabaseColumn> databaseColumn);
        Task BatchDeleteDatabaseColumn(List<Guid> ids);

        /// <summary>
        /// 批量删除导航栏目
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task BatchDeleteLabel(List<Guid> ids);

        Task SaveDomainEscDtos(List<string> domainEscs);
        Task<List<OptionDto>> GetDomainTree();
        Task<PagedList<OptionDto>> GetCoustomLabels(int pageIndex, int pageSize);
        Task<List<string>> GetDomainEsc();
        Task<List<string>> InsertCustomLabels(List<string> databaseLabels);
        Task<PagedList<OptionDto>> GetAcessUrlName(int pageIndex, int pageSize);
        Task BatchDeleteAcessUrlName(List<Guid> ids);
        Task SaveCustomLabels(List<OptionDto> labels);
        Task SortDatabaseByDrag(int sourceArtIndex, int destinationIndex, bool isUp);
        Task SortSourceFromImportByDestination(int sourceArtIndex, int absoluteIndex);
        Task<DatabaseSinglePageVO> GetDatabaseSinglePageList(string artType, string domain, int languagId, int sortType,string searchKey,string purchaseType,int status,string initials,string label);
        /// <summary>
        /// 取所有的在用链接
        /// </summary>
        /// <returns></returns>
         List<DatabaseAcessUrlDto> GetAllUrl();
        /// <summary>
        /// 组装数据库导航与链接的关系
        /// </summary>
        /// <param name="databaseDtoList"></param>
        /// <param name="allUrls"></param>
        public void AppendDirectUrl(IEnumerable<DatabaseTerraceDto> databaseDtoList, List<DatabaseAcessUrlDto> allUrls);
        Task<bool> IsCollected(Guid databaseId, string userKey);
        Task<List<DatabaseTerraceDto>> GetDatabaseInPortal(int count, Guid columnId);
        Task<List<OptionDto>> GetUserGroupKV();
        Task<List<OptionDto>> GetUserTypeKV();
        Task<List<OptionDto>> GetDatabaseFromCenter(string keyWord);
        Task<List<OptionDto>> GetSelectedDatabase();
        Task SaveProviderResource(List<string> providers);
        Task<DatabaseTerraceDto> GetDatabaseFromCenterAsModel(string databaseId);
        Task<List<OptionDto>> GetProviderDtos();
        Task<List<DatabaseTemplateDto>> GetDatabaseBodyTemplate(int type);
        Task<List<OptionDto>> GetAlbumFromCenter(string provider);
        Task<string> GetAppBaseUri(string uri);
        /// <summary>
        /// 获取数据库导航应用的默认模板字典
        /// </summary>
        /// <returns></returns>
        Task<List<DatabaseDefaultTemplateDto>> GetDatabaseDefaultTemplateDtoList();
        /// <summary>
        /// 从数据中心获取资源类型- 系统自带类型 
        /// </summary>
        /// <returns></returns>
        Task<List<OptionDto>> GetSysSourceTypeDto();

        /// <summary>
        /// 从数据中心获取资源类型- 自定义类型 
        /// </summary>
        /// <returns></returns>
        Task<List<OptionDto>> GetCusSourceTypeDto();
        /// <summary>
        /// 添加资源类型拓展项
        /// </summary>
        /// <param name="sourceName">拓展项名称</param>
        /// <returns></returns>


        Task<int> AddCustomSourceType(string sourceName, string userKey);
        Task<VisitCountInfo> IncreaseVisitCount(Guid databaseId);
    }
}