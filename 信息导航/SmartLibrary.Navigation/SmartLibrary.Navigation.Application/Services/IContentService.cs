using SmartLibrary.Navigation.Application.PageParam;
using SmartLibrary.Navigation.Application.ViewModel;
using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibraryNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    public interface IContentService
    {
        Task<ApiResultInfoModel> AddContent(ContentDto model,string columnID);
        Task<ApiResultInfoModel> UpdateContent(ContentDto model,string columnID);
        Task<ContentDto> GetContent(string contentID);
        Task<PagedList<ContentDto>> GetContentList(int pageIndex, int pageSize, string columnID, string keywords, string cataID, bool? status);
        Task<ApiResultInfoModel> ChangeContentStatus(string[] contentIDList, bool isNormal);
        Task<ApiResultInfoModel> DeleteContent(string[] contentIDList);
        Task<ApiResultInfoModel> SortModel(string sourceID, int sortIndex);
        Task<ApiResultInfoModel> SortModel(string sourceID, string targetCataID, bool isUp);
        Task<PagedList<FrontContentListView>> GetProntContentListData(FrontContentListParm parm);

        Task<FrontContentView> GetProntContent(string contentID);
        Task<List<ContentProcessLog>> GetContentProcessLog(string contentID);
        Task<PagedList<ContentVo>> SearchNavigationContent(string keyWord,int pageIndex,int pageSize);
    }
}
