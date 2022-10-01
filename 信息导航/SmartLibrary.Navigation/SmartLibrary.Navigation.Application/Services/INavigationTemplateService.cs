using SmartLibrary.Navigation.Application.ViewModel;
using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    /// <summary>
    /// 名    称：INavigationTemplateService
    /// 作    者：张泽军
    /// 创建时间：2021/10/21 11:50:00
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public interface INavigationTemplateService
    {
        #region NavigationTemplate 新闻模板管理
        Task<ApiResultInfoModel> AddNavigationTemplate(NavigationTemplateDto model);
        Task<List<NavigationTemplateDto>> GetNavigationTemplate(string[] tempIDs);
        Task<NavigationTemplateDto> GetSingleNavigationTemplate(string tempID);
        Task<ApiResultInfoModel> UpdateNavigationTemplate(NavigationTemplateDto templateDto);
        Task<ApiResultInfoModel> DeleteNavigationTemplate(string tempID);

        #region NavigationBodyTemplate 新闻头尾模板
        Task<ApiResultInfoModel> AddNavigationBodyTemplate(NavigationBodyTemplateDto model);
        Task<List<NavigationBodyTemplateDto>> GetNavigationBodyTemplate(int type);
        Task<List<NavigationBodyTemplateDto>> GetNavigationBodyTemplate();
        Task<ApiResultInfoModel> UpdateNavigationBodyTemplate(NavigationBodyTemplateDto bodyTemplateDto);
        Task<ApiResultInfoModel> DeleteNavigationBodyTemplate(string tempBodyID);
        Task<HeadFootTemplateListView> AllHeadFootTemplateGet();
        Task<HeadFootTemplateDetailView> HeadFootTemplateDetailGet(string headerId, string footerId);
        #endregion
        #endregion
    }
}
