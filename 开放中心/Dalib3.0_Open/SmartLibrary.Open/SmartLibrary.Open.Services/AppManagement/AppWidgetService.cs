/*********************************************************
 * 名    称：AppWidgetService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/7 15:11:20
 * 描    述：应用组件
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.Common.Utility;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public class AppWidgetService : IScoped, IAppWidgetService
    {
        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<AppWidget> _appWidgetRepository;
        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<MicroApplication> _appRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appWidgetRepository"></param>
        /// <param name="appRepository"></param>
        public AppWidgetService(IRepository<AppWidget> appWidgetRepository
            , IRepository<MicroApplication> appRepository)
        {
            _appWidgetRepository = appWidgetRepository;
            _appRepository = appRepository;
        }

        /// <summary>
        /// 查询应用组件表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<AppWidgetViewModel>> QueryTableData(AppWidgetTableQuery queryFilter)
        {
            var queryApplication = _appRepository.Where(p => !p.DeleteFlag);
            var queryAppWidget = _appWidgetRepository.Where(p => !p.DeleteFlag);

            var query = from wgt in queryAppWidget
                        join app in queryApplication on wgt.AppId equals app.Id.ToString()
                        where (string.IsNullOrEmpty(queryFilter.AppID) || app.Id.ToString() == queryFilter.AppID)
            && (string.IsNullOrEmpty(queryFilter.Keyword) || wgt.Name.Contains(queryFilter.Keyword))
                        select new AppWidgetViewModel
                        {
                            Id = wgt.Id,
                            CreateTime = wgt.CreatedTime.LocalDateTime,
                            UpdateTime = wgt.UpdatedTime.HasValue ? wgt.UpdatedTime.Value.LocalDateTime : null,
                            AppName = app.Name,
                            AppId = app.Id.ToString(),
                            Desc = wgt.Desc,
                            Name = wgt.Name,
                            Target = wgt.Target,
                            AvailableConfig = wgt.AvailableConfig,
                            MaxTopCount = wgt.MaxTopCount,
                            TopCountInterval = wgt.TopCountInterval
                        };

            var table = await query.ApplyOrder(queryFilter.SortField, queryFilter.IsAsc).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return table;
        }

        /// <summary>
        /// 获取应用组件详情
        /// </summary>
        /// <param name="appWidgetId">应用组件ID</param>
        /// <returns></returns>
        public async Task<AppWidgetViewModel> GetByID(Guid appWidgetId)
        {
            var appWidget = await _appWidgetRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id == appWidgetId);
            var app = await _appRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == appWidget.AppId);

            var result = new AppWidgetViewModel
            {
                Id = appWidget.Id,
                Desc = appWidget.Desc,
                Name = appWidget.Name,
                AppId = app.Id.ToString(),
                AppName = app.Name,
                CreateTime = appWidget.CreatedTime.LocalDateTime,
                Target = appWidget.Target,
                UpdateTime = appWidget.UpdatedTime.HasValue ? appWidget.UpdatedTime.Value.LocalDateTime : null
            };
            return result;
        }

        /// <summary>
        /// 创建应用组件
        /// </summary>
        /// <param name="appWidgetDto">应用组件数据</param>
        /// <returns></returns>
        public async Task<Guid> Create(AppWidgetDto appWidgetDto)
        {
            var appWidget = new AppWidget
            {
                Id = Guid.NewGuid(),
                Name = appWidgetDto.Name,
                CreatedTime = DateTimeOffset.UtcNow,
                AppId = appWidgetDto.AppID.ToString(),
                Desc = appWidgetDto.Desc,
                Target = appWidgetDto.Target
            };
            var result = await _appWidgetRepository.InsertAsync(appWidget);
            return result.Entity.Id;
        }

        /// <summary>
        /// 修改应用组件
        /// </summary>
        /// <param name="appWidgetDto">应用组件数据</param>
        /// <returns></returns>
        public async Task<Guid> Update(AppWidgetDto appWidgetDto)
        {
            var appWidget = await _appWidgetRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id == appWidgetDto.ID);
            appWidget.Name = appWidgetDto.Name;
            appWidget.UpdatedTime = DateTimeOffset.UtcNow;
            appWidget.AppId = appWidgetDto.AppID.ToString();
            appWidget.Desc = appWidgetDto.Desc;
            appWidget.Target = appWidgetDto.Target;
            var result = await _appWidgetRepository.UpdateAsync(appWidget);
            return result.Entity.Id;
        }

        /// <summary>
        /// 删除应用组件
        /// </summary>
        /// <param name="appWidgetId">应用组件ID</param>
        /// <returns></returns>
        public async Task<bool> Delete(string appWidgetId)
        {
            var appWidget = await _appWidgetRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == appWidgetId);
            appWidget.UpdatedTime = DateTimeOffset.UtcNow;
            appWidget.DeleteFlag = true;
            var result = await _appWidgetRepository.UpdateAsync(appWidget);
            return result.Entity != null;
        }
    }
}
