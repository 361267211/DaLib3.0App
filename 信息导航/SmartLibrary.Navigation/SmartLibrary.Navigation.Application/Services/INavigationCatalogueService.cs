using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibrary.Navigation.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SmartLibrary.Navigation.Application.Services
{
    public interface INavigationCatalogueService
    {
        Task<ApiResultInfoModel> AddNavigationCatalogue(NavigationCatalogueDto model);
        Task<ApiResultInfoModel> UpdateNavigationCatalogue(NavigationCatalogueDto model);

        Task<NavigationCatalogueDto> GetNavigationCatalogue(string catalogueID);
        Task<List<NavigationCatalogueView>> GetNavigationCatalogueList(string columnID);
        Task<List<NavigationCatalogueTreeView>> GetAllNavigationCatalogueTreeList(string parentCataID, string columnID = "");
        Task<ApiResultInfoModel> ChangeNavigationCatalogueStatus(string[] cataIDList, bool isNormal);
        Task<ApiResultInfoModel> DeleteNavigationCatalogue(string[] cataIDList);

        Task<ApiResultInfoModel> SortModel(string sourceID, int sortIndex);
        Task<ApiResultInfoModel> SortModel(string sourceID, string targetCataID, bool isUp);
        Task<List<ProntNavigationCatalogue>> GetNavigationCatalogueListForPront(string columnID, bool isContent);
        Task<List<ContentProcessLog>> GetCatalogueProcessLog(string cataID);
        Task<ProntScenesNavaigationView> GetProntScenesCatalogue(string columnID, int topNum, string sortField, bool isAsc, bool isOnlyParentCata = true);

        /// <summary>
        /// 根据目录id获取导航
        /// </summary>
        /// <param name="colId"></param>
        /// <returns></returns>
        public List<NavigationCatalogueDto> GetCataListByColId(string colId);
    }
}
