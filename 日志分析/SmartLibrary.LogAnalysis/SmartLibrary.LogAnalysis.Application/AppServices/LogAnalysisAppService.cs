using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.LogAnalysis.Application.Dtos;
using SmartLibrary.LogAnalysis.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.LogAnalysis.Application.AppServices
{
    public class LogAnalysisAppService:IDynamicApiController
    {
        private readonly ILogAnalysisService _logAnalysisService;

        public LogAnalysisAppService(ILogAnalysisService logAnalysisService)
        {
            _logAnalysisService = logAnalysisService;
        }
        [HttpGet]
        public Task<IReadOnlyList<DisplayLogAnalysisResponseDto>> GetDisplayLogAnalyses([FromQuery]DisplayLogAnalysisRequestDto requestDto) => this._logAnalysisService.GetDisplayLogAnalysesAsync(requestDto);
        [HttpGet] 
        public Task<IndexStatisticsDto> GetIndexStatisticsAsync() => this._logAnalysisService.GetIndexStatisticsAsync();

        [HttpGet] 
        public Task<List<ILibTitleInfo>> GetRecommendHotBookAsync() => this._logAnalysisService.GetRecommendHotBookAsync();

        [HttpGet]
        public Task<List<ILibTitleInfo>> GetRecommendHotBorrowBookAsync() => this._logAnalysisService.GetRecommendHotBorrowBookAsync();
    }
}
