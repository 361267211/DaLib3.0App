
using Grpc.Core;
using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.PageParam;
using SmartLibrary.News.Application.ViewModel;
using SmartLibraryNews;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application
{
    public interface INewsContentService
    {
        #region NewsContentExpend 新闻内容扩展字段
        void ProcessNewsContentExpend(string columnID, string newExtension, string oldExtension = "");
        Task<ApiResultInfoModel> AddNewsContentExpend(NewsContentExpendDto model);
        Task<List<NewsContentExpendDto>> GetNewsContentExpend(string columnID);
        Task<List<NewsContentExpendDto>> AddNewsContentExpend(string[] expendIDs);
        Task<ApiResultInfoModel> UpdateNewsContentExpend(string columnID, string filed, string name);
        Task<ApiResultInfoModel> DeleteNewsContentExpend(string columnID, string filed);
        #endregion
        #region NewsContent 新闻内容
        Task<ApiResultInfoModel> AddNewsContent(NewsContentDto model);
        Task<ApiResultInfoModel> UpdateNewsContent(NewsContentDto model);
        Task<ApiResultInfoModel> UpdateNewsContentAuditStatus(NewsContentAuditStatusParm parm);
        Task<NewsContentsManageViewModel> GetNewsContent(NewsContentByColumnParm pageParm);
        Task<NewsColumnContentManage> GetNewsColumnContentManage(string columnID);
        Task<PagedList<NewsContentsForSearchView>> GetNewsContentBySearch(NewsContentBySearchKeyParm pageParm);
        Task<ApiResultInfoModel> OffShelfNewsContent(string[] contentIDs);
        Task<ApiResultInfoModel> PublishOffShelfNewsContent(string columnID);
        Task<ApiResultInfoModel> DeleteNewsContent(string[] contentIDs);
        Task<NewsContentManageView> GetNewsContentManage(string contentID);
        Task<ApiResultInfoModel> ResetAuditStatusNewsContent(string columnID);
        Task<PagedList<FrontNewsContentListView>> GetProntNewsListData(FrontNewsListParm parm);
        Task<FrontNewsContentView> GetProntNewsContent(string contentID);
        Task<List<KeyValuePair<string, string>>> GetPublishNewsContentList(string managerID, int counts);
        Task<int> GetAllNewsContentCount();
        Task<WorkbenchAuditPageView> GetWorkbenchAudit(WorkbenchAuditNewsListParam param);
        /// <summary>
        /// 根据内容id取栏目id
        /// </summary>
        /// <returns></returns>
        Task<string> GetNewsColumnIdByContentId(string contentId);
        Task<ApiResultInfoModel> SortModel(string sourceID, int sortIndex);
        Task<ApiResultInfoModel> SortModel(string sourceID, string targetCataID, bool isUp);
        Task<List<ContentProcessLog>> GetContentProcessLog(string contentID);
        Task<ProntScenesNewsView> GetProntScenesNews(string columnID, int topNum, string sortField, bool isAsc);

        Task<string> GrpcBaseUriGet(string appId);
        #endregion
    }
}