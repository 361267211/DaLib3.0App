/*********************************************************
 * 名    称：AppBranchEntryPointService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/7 10:11:50
 * 描    述：应用分支入口地址
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
    public interface IAppBranchEntryPointService
    {
        Task<Guid> Create(AppBranchEntryPointDto entryPointDto);
        Task<bool> Delete(string entryPointId);
        Task<List<AppBranchEntryPointViewModel>> GetAppEntranceList(string appBranchId);
        Task<List<AppBranchEntryPointViewModel>> QueryListData(string appBranchId);
        Task<Guid> Update(AppBranchEntryPointDto entryPointDto);
    }
}