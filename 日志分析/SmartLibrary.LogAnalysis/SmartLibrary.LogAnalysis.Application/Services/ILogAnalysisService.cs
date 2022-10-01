using SmartLibrary.LogAnalysis.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.LogAnalysis.Application.Services
{
    /// <summary>
    /// 统计服务
    /// </summary>
    public  interface ILogAnalysisService
    {
        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<DisplayLogAnalysisResponseDto>> GetDisplayLogAnalysesAsync(DisplayLogAnalysisRequestDto requestDto);
        /// <summary>
        /// 首页数据统计接口
        /// </summary>
        /// <returns></returns>
        Task<IndexStatisticsDto> GetIndexStatisticsAsync();
        /// <summary>
        /// 获取热门下载的文献
        /// </summary>
        /// <returns></returns>
        Task<List<ILibTitleInfo>> GetRecommendHotBookAsync();
        /// <summary>
        /// 获取热门借阅的文献
        /// </summary>
        /// <returns></returns>
        Task<List<ILibTitleInfo>> GetRecommendHotBorrowBookAsync();
    }
}
