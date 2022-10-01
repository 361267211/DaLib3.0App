using SmartLibrary.AppCenter.Application.Dtos.Application;
using SmartLibrary.AppCenter.Application.Dtos.BaseInfo;
using SmartLibrary.SceneManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.Common
{
    /// <summary>
    /// 通用服务，获取图标，终端类型，应用类型等
    /// </summary>
    public interface IGeneralService
    {
        /// <summary>
        /// 获取应用类型
        /// </summary>
        /// <returns></returns>
        Task<List<DictionaryDto>> GetAppType();

        /// <summary>
        /// 获取业务类型 馆员工作台
        /// </summary>
        /// <returns></returns>
        Task<List<DictionaryDto>> GetBusinessType();

        /// <summary>
        /// 获取采购类型
        /// </summary>
        /// <returns></returns>
        Task<List<DictionaryDto>> GetPurchaseType();

        /// <summary>
        /// 获取终端类型
        /// </summary>
        /// <returns></returns>
        Task<List<DictionaryDto>> GetTerminalType();

        /// <summary>
        /// 获取用户类型列表
        /// </summary>
        /// <returns></returns>
        Task<List<DictionaryDto>> GetUserTypeList();

        /// <summary>
        /// 获取用户分组列表
        /// </summary>
        /// <returns></returns>
        Task<List<DictionaryDto>> GetUserGroupList();
        
        /// <summary>
        /// 获取当前机构信息
        /// </summary>
        /// <returns></returns>
        Task<OrgInfo> GetCurrentOrgInfo();
        
        /// <summary>
        /// 获取当前头部底部信息
        /// </summary>
        /// <returns></returns>
        Task<HeaderFooterReply> GetCurrentHeaderFooterInfo();

        /// <summary>
        /// 获取当前用户的权限类型（后台）
        /// </summary>
        /// <returns></returns>
        Task<int> GetPermissionType();
    }
}
