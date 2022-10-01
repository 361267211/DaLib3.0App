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
using SmartLibrary.Open.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public interface IAppWidgetService
    {
        Task<Guid> Create(AppWidgetDto appWidgetDto);
        Task<bool> Delete(string appWidgetId);
        Task<AppWidgetViewModel> GetByID(Guid appWidgetId);
        Task<PagedList<AppWidgetViewModel>> QueryTableData(AppWidgetTableQuery queryFilter);
        Task<Guid> Update(AppWidgetDto appWidgetDto);
    }
}