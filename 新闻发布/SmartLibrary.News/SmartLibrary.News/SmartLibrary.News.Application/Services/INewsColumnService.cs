
using Grpc.Core;
using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.Enums;
using SmartLibrary.News.Application.PageParam;
using SmartLibrary.News.Application.ViewModel;
using SmartLibraryNews;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application
{
    public interface INewsColumnService
    {
        #region NewsColumn À¸Ä¿¹ÜÀí
        //Task<NewsColumnReply> GetNewsColumn(NewsColumnRequest request, ServerCallContext callContext = null);
        Task<List<LableNewsColumnView>> GetLableNewsColumnList();
        Task<NewsColumnDto> GetNewsColumn(string columnID);
        Task<List<KeyValuePair<string, string>>> GetDeliveryColumnList(string columnID);
        Task<ApiResultInfoModel> AddNewsColumn(NewsColumnDto newsColumn);

        Task<ApiResultInfoModel> UpdateNewsColumn(NewsColumnDto model);

        Task<ApiResultInfoModel> DeleteNewsColumn(string columnID);
        Task<ApiResultInfoModel> DeleteNewsColumn(string[] columnIDs);

        Task<List<ProntNewsColumnListView>> GetProntNewsColumnList(string columnID);

        Task<List<KeyValuePair<string, string>>> GetPublishColumnList(string managerID, int counts);
        Task<int> GetAllNewsColumnCount();
        Task<List<KeyValuePair<string, string>>> GetUserPermissionList(int type);
        Task<GetColumnLinkInfoReply> GetColumnLinkInfo(string columnId);
        #endregion
    }
}