using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibrary.Navigation.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    /// <summary>
    /// 名    称：INavigationColumnService
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 14:20:59
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public interface INavigationColumnService
    {
        #region NavigationColumn 栏目管理
        Task<List<LableNavigationColumnView>> GetLableNavigationColumnList();
        Task<NavigationColumnDto> GetNavigationColumn(string columnID);

        Task<ApiResultInfoModel> AddNavigationColumn(NavigationColumnDto NavigationColumn);

        Task<ApiResultInfoModel> UpdateNavigationColumn(NavigationColumnDto model);

        Task<ApiResultInfoModel> DeleteNavigationColumn(string columnID);
        Task<ApiResultInfoModel> DeleteNavigationColumn(string[] columnIDs);

        Task<List<ProntNavigationColumnListView>> GetProntNavigationColumnList(string columnID);

        /// <summary>
        /// 获取底部链接列表
        /// </summary>
        /// <param name="plateSign"></param>
        /// <param name="count"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        Task<List<ContentInfoDto>> GetPlatListAsync(string plateSign, int count, int itemType);

        /// <summary>
        /// 获取底部链接列表
        /// </summary>
        /// <param name="plateSign"></param>
        /// <param name="count"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
         Task<string> GetAllianceCertifyUrlAsync();
        Task<GetColumnLinkInfoReply> GetColumnLinkInfo(string columnId);
        #endregion
    }
}
