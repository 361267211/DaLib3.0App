
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
    public interface INewsTemplateService
    {
        #region NewsTemplate 新闻模板管理
        Task<ApiResultInfoModel> AddNewsTemplate(NewsTemplateDto model);
        Task<List<NewsTemplateDto>> GetNewsTemplate(string[] tempIDs);
        Task<NewsTemplateDto> GetSingleNewsTemplate(string tempID);
        Task<ApiResultInfoModel> UpdateNewsTemplate(NewsTemplateDto templateDto);
        Task<ApiResultInfoModel> DeleteNewsTemplate(string tempID);

        #region NewsBodyTemplate 新闻头尾模板
        Task<ApiResultInfoModel> AddNewsBodyTemplate(NewsBodyTemplateDto model);
        Task<List<NewsBodyTemplateDto>> GetNewsBodyTemplate(int type);
        Task<List<NewsBodyTemplateDto>> GetNewsBodyTemplate(string[] tempBodyIDs);
        Task<ApiResultInfoModel> UpdateNewsBodyTemplate(NewsBodyTemplateDto bodyTemplateDto);
        Task<ApiResultInfoModel> DeleteNewsBodyTemplate(string tempBodyID);
        Task<HeadFootTemplateView> GetTemplateDetailByColumnId(string columnID);

        #endregion
        #endregion
    }
}