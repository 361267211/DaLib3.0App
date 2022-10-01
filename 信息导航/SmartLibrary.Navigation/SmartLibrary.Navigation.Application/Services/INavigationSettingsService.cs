using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibrary.Navigation.Application.PageParam;
using SmartLibrary.Navigation.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    /// <summary>
    /// 名    称：INavigationSettingsService
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 11:15:12
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public interface INavigationSettingsService
    {
        #region 应用设置&栏目权限设置
        Task<ApiResultInfoModel> SaveNavigationSettings(NavigationSettingsDto model);
        Task<NavigationSettingsDto> GetNavigationSettings();
        Task<ApiResultInfoModel> SaveNavigationColumnPermissions(NavigationColumnPermissionsParam model);
        Task<List<ManagerParam>> GetNavigationColumnPermissionsByColumnPerm(string columnID, int permission);
        Task<ApiResultInfoModel> DeleteNavigationColumnPermissionsByColumnID(string columnID, bool isIncludeManage = false);
        Task<List<NavigationColumnPermissionsView>> GetNavigationColumnPermissionsList();

        bool CheckSensitiveWords(string checkStr);
        Task<List<KeyValuePair<string, string>>> GetColumnKVByUserKey(string userKey);
        Task<List<PermissionManagerInfo>> SearchPermissionManager(string searchKey);
        #endregion
    }
}
