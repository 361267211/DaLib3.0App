
using Grpc.Core;
using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.PageParam;
using SmartLibrary.News.Application.ViewModel;
using SmartLibraryNews;
using SmartLibraryUser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application
{
    public interface ILableInfoService
    {
        #region LableInfo ±Í«©π‹¿Ì
        Task<ApiResultInfoModel> AddLableInfo(LableInfoDto model);
        Task<List<LableInfoDto>> GetLableInfo(int type);
        Task<List<LableInfoDto>> GetLableInfo(string[] lableIDs);
        Task<ApiResultInfoModel> UpdateLableInfo(int type, List<LableUpdateParm> updateParmList);
        Task<ApiResultInfoModel> DeleteLableInfo(string lableID);
        Task<string> ProcessLablesFromLableStr(int type, string labels);
        #endregion
    }
}