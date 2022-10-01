
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
    public interface INewsSettingsService
    {
        #region 应用设置&栏目权限设置
        Task<ApiResultInfoModel> SaveNewsSettings(NewsSettingsDto model);
        Task<NewsSettingsDto> GetNewsSettings();
        Task<ApiResultInfoModel> SaveNewsColumnPermissions(NewsColumnPermissionsParam model);
        Task<List<ManagerParam>> GetNewsColumnPermissionsByColumnPerm(string columnID, int permission);
        Task<ApiResultInfoModel> DeleteNewsColumnPermissionsByColumnID(string columnID, bool isIncludeManage = false);
        Task<List<NewsSingleColumnPermissionsView>> GetNewsColumnPermissionsByColumnID(string columnID);
        Task<List<NewsColumnPermissionsView>> GetNewsColumnPermissionsList();

        bool CheckSensitiveWords(string checkStr);
        Task<Dictionary<int, string>> GetPermissionColumnList(string managerID);
        Task<List<AuditPowerEunm>> GetColumnAuditPowerListByUserKey(string columnID);

        Task<List<string>> GetColumnIDsByUserKey();
        Task<List<PermissionManagerInfo>> SearchPermissionManager(string searchKey);
        Task<List<KeyValuePair<string, string>>> GetColumnKVBuUserKey(string userKey);
        #endregion
    }
}