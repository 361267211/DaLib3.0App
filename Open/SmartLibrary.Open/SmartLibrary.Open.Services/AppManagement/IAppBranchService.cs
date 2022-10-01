/*********************************************************
 * 名    称：AppBranchService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/2 15:40:21
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public interface IAppBranchService
    {
        Task<Guid> Create(AppBranchDto appBranchDto);
        Task<bool> Delete(Guid appBranchId);
        Task<AppBranchViewModel> GetByID(Guid appBranchId);
        Task<PagedList<AppBranchViewModel>> QueryTableData(AppBranchTableQuery queryFilter);
        Task<Guid> Update(AppBranchDto appBranchDto);
    }
}