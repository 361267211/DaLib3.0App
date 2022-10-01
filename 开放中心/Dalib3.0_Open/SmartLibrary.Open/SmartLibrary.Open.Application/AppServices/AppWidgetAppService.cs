using SmartLibrary.Open.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Application.Services;
using SmartLibrary.Open.Services;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 应用组件管理接口
    /// </summary>
    public class AppWidgetAppService : BaseAppService
    {
        /// <summary>
        /// 应用组件服务
        /// </summary>
        private IAppWidgetService _appWidgetService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appWidgetService"></param>
        public AppWidgetAppService(IAppWidgetService appWidgetService)
        {
            _appWidgetService = appWidgetService;
        }


        /// <summary>
        /// 查询应用组件表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<AppWidgetViewModel>> QueryTableData([FromQuery] AppWidgetTableQuery queryFilter)
        {
            return await _appWidgetService.QueryTableData(queryFilter);
        }

        /// <summary>
        /// 获取应用组件详情
        /// </summary>
        /// <param name="appWidgetId">应用组件ID</param>
        /// <returns></returns>
        [Route("{appWidgetId}")]
        [HttpGet]
        public async Task<AppWidgetViewModel> GetByID(Guid appWidgetId)
        {
            return await _appWidgetService.GetByID(appWidgetId);
        }

        /// <summary>
        /// 创建应用组件
        /// </summary>
        /// <param name="appWidgetDto">应用组件数据</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> Create([FromBody] AppWidgetDto appWidgetDto)
        {
            return await _appWidgetService.Create(appWidgetDto);
        }

        /// <summary>
        /// 修改应用组件
        /// </summary>
        /// <param name="appWidgetDto">应用组件数据</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Guid> Update([FromBody] AppWidgetDto appWidgetDto)
        {
            return await _appWidgetService.Update(appWidgetDto);
        }

        /// <summary>
        /// 删除应用组件
        /// </summary>
        /// <param name="appWidgetId">应用组件ID</param>
        /// <returns></returns>
        [Route("{appWidgetId}")]
        [HttpPut]
        public async Task<bool> Delete(string appWidgetId)
        {
            return await _appWidgetService.Delete(appWidgetId);
        }

    }
}
