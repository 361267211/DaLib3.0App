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
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmartLibrary.Open.Services
{
    /// <summary>
    /// 应用分支入口地址
    /// </summary>
    public class AppBranchEntryPointService : IScoped, IAppBranchEntryPointService
    {
        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<AppBranchEntryPoint> _appBranchEntryPointRepository;

        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<AppBranch> _appRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appBranchEntryPointRepository"></param>
        /// <param name="appRepository"></param>
        public AppBranchEntryPointService(IRepository<AppBranchEntryPoint> appBranchEntryPointRepository
            , IRepository<AppBranch> appRepository)
        {
            _appBranchEntryPointRepository = appBranchEntryPointRepository;
            _appRepository = appRepository;
        }

        /// <summary>
        /// 获取应用分支所有入口
        /// </summary>
        /// <param name="appBranchId"></param>
        /// <returns></returns>
        public Task<List<AppBranchEntryPointViewModel>> QueryListData(string appBranchId)
        {
            var query = _appBranchEntryPointRepository.Where(p => !p.DeleteFlag && p.AppBranchId == appBranchId).Select(p => new AppBranchEntryPointViewModel
            {
                Id = p.Id,
                CreateTime = p.CreatedTime.LocalDateTime,
                VisitUrl = p.VisitUrl,
                Name = p.Name,
                Code = p.Code,
                AppId = p.AppId,
                AppBranchId = p.AppBranchId,
                IsSystem = p.IsSystem,
                IsDefault=p.IsDefault,
                UpdateTime = p.UpdatedTime.HasValue ? p.UpdatedTime.Value.LocalDateTime : null
            });
            var table = query.ToList();
            return Task.FromResult(table);
        }

        /// <summary>
        /// 创建应用分支入口
        /// </summary>
        /// <param name="entryPointDto"></param>
        /// <returns></returns>
        public async Task<Guid> Create(AppBranchEntryPointDto entryPointDto)
        {
            var entity = new AppBranchEntryPoint
            {
                Id = Guid.NewGuid(),
                VisitUrl = entryPointDto.VisitUrl,
                Name = entryPointDto.Name,
                Code = entryPointDto.Code,
                AppId = entryPointDto.AppId,
                AppBranchId = entryPointDto.AppBranchId,
                IsSystem = false,
                IsDefault = false,
                CreatedTime = DateTimeOffset.UtcNow
            };
            var result = await _appBranchEntryPointRepository.InsertAsync(entity);

            return result.Entity.Id;
        }

        /// <summary>
        /// 修改应用分支入口
        /// </summary>
        /// <param name="entryPointDto"></param>
        /// <returns></returns>
        public async Task<Guid> Update(AppBranchEntryPointDto entryPointDto)
        {
            var entity = _appBranchEntryPointRepository.FirstOrDefault(p => p.Id == entryPointDto.ID && !p.DeleteFlag);

            entity.UpdatedTime = DateTimeOffset.UtcNow;
            entity.VisitUrl = entryPointDto.VisitUrl;
            entity.Name = entryPointDto.Name;
            entity.Code = entryPointDto.Code;
            entity.AppId = entryPointDto.AppId;
            entity.AppBranchId = entryPointDto.AppBranchId;
            var result = await _appBranchEntryPointRepository.UpdateAsync(entity);

            return result.Entity.Id;
        }

        /// <summary>
        /// 删除应用分支入口
        /// </summary>
        /// <param name="entryPointId">Id</param>
        /// <returns></returns>
        public async Task<bool> Delete(string entryPointId)
        {
            var entity = _appBranchEntryPointRepository.FirstOrDefault(p => p.Id.ToString() == entryPointId && !p.DeleteFlag);

            entity.UpdatedTime = DateTimeOffset.UtcNow;
            entity.DeleteFlag = true;
            var result = await _appBranchEntryPointRepository.UpdateAsync(entity);

            return result.Entity != null;
        }

        /// <summary>
        /// 获取应用分支所有入口
        /// </summary>
        /// <param name="appBranchId"></param>
        /// <returns></returns>
        public async Task<List<AppBranchEntryPointViewModel>> GetAppEntranceList(string appBranchId)
        {
            var query = _appBranchEntryPointRepository.Include(p=>p.AppEvents).Where(p => !p.DeleteFlag && p.AppBranchId == appBranchId).Select(p => new AppBranchEntryPointViewModel
            {
                Id = p.Id,
                CreateTime = p.CreatedTime.LocalDateTime,
                VisitUrl = p.VisitUrl,
                Name = p.Name,
                Code = p.Code,
                AppId = p.AppId,
                AppBranchId = p.AppBranchId,
                IsSystem = p.IsSystem,
                IsDefault = p.IsDefault,
                UpdateTime = p.UpdatedTime.HasValue ? p.UpdatedTime.Value.LocalDateTime : null
            });
            var table = await query.ToListAsync();
            return table;
        }
    }
}
