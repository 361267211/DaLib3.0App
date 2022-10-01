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
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public class AppBranchService : IScoped, IAppBranchService
    {
        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<MicroApplication> _microAppRepository;

        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<AppBranch> _appBranchRepository;

        /// <summary>
        /// 服务包数据仓储
        /// </summary>
        private readonly IRepository<AppServicePack> _appServicePackRepository;

        /// <summary>
        /// 服务类型数据仓储
        /// </summary>
        private readonly IRepository<AppServiceType> _appServiceTypeRepository;

        /// <summary>
        /// 应用字典数据仓储
        /// </summary>
        private readonly IRepository<AppDictioanry> _appDictioanryRepository;

        /// <summary>
        /// 应用用户授权数据仓储
        /// </summary>
        private readonly IRepository<AppSpecificCustomer> _appSpecificCustomerRepository;

        /// <summary>
        /// 客户应用数据仓储
        /// </summary>
        private readonly IRepository<CustomerAppUsage> _customerAppUsageRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppBranchService(IRepository<MicroApplication> microAppRepository,
            IRepository<AppBranch> appBranchRepository,
            IRepository<AppServicePack> appServicePackRepository,
            IRepository<AppServiceType> appServiceTypeRepository,
            IRepository<AppDictioanry> appDictioanryRepository,
            IRepository<AppSpecificCustomer> appSpecificCustomerRepository,
            IRepository<CustomerAppUsage> customerAppUsageRepository
            )
        {
            _microAppRepository = microAppRepository;
            _appBranchRepository = appBranchRepository;
            _appServicePackRepository = appServicePackRepository;
            _appServiceTypeRepository = appServiceTypeRepository;
            _appDictioanryRepository = appDictioanryRepository;
            _appSpecificCustomerRepository = appSpecificCustomerRepository;
            _customerAppUsageRepository = customerAppUsageRepository;

        }


        /// <summary>
        /// 查询应用分支表格数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<PagedList<AppBranchViewModel>> QueryTableData(AppBranchTableQuery queryFilter)
        {
            var query = _appBranchRepository.Where(p => !p.DeleteFlag
            && (!queryFilter.IsMaster.HasValue || p.IsMaster == queryFilter.IsMaster)
            && (!queryFilter.AppID.HasValue || p.AppId == queryFilter.AppID.Value.ToString())).Select(p=>new AppBranchViewModel { 
               Id=p.Id,
               AppID=p.AppId,
               CreateTime=p.CreatedTime.LocalDateTime,
               DeployeeID=p.DeployeeId,
               Icon=p.Icon,
               IsMaster=p.IsMaster,
               Name=p.Name,
               Remark=p.Remark,
               UpdateTime=p.UpdatedTime.HasValue? p.UpdatedTime.Value.LocalDateTime:null,
               Version=p.Version
            });
            var table = await query.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return table;
        }

        /// <summary>
        /// 获取应用分支详情
        /// </summary>
        /// <param name="appBranchId">应用分支Id</param>
        /// <returns></returns>
        public async Task<AppBranchViewModel> GetByID(Guid appBranchId)
        {
            var query = await _appBranchRepository.FirstOrDefaultAsync(p => !p.DeleteFlag
             && p.Id == appBranchId);
            var result = new AppBranchViewModel
            {
                Id = query.Id,
                AppID = query.AppId,
                CreateTime = query.CreatedTime.LocalDateTime,
                DeployeeID = query.DeployeeId,
                Icon = query.Icon,
                IsMaster = query.IsMaster,
                Name = query.Name,
                Remark = query.Remark,
                UpdateTime = query.UpdatedTime.HasValue ? query.UpdatedTime.Value.LocalDateTime : null,
                Version = query.Version
            };
            return result;
        }

        /// <summary>
        /// 创建应用分支
        /// </summary>
        /// <param name="appBranchDto">应用分支数据</param>
        /// <returns></returns>
        public async Task<Guid> Create(AppBranchDto appBranchDto)
        {
            var entity = new AppBranch
            {
                Id = Guid.NewGuid(),
                AppId = appBranchDto.AppId.ToString(),
                CreatedTime = DateTimeOffset.UtcNow,
                DeployeeId = appBranchDto.DeployeeId.ToString(),
                Icon = appBranchDto.Icon,
                IsMaster = appBranchDto.IsMaster,
                Name = appBranchDto.Name,
                Remark = appBranchDto.Remark,
                Version = appBranchDto.Version,
            };
            var result = await _appBranchRepository.InsertAsync(entity);
            
            return result.Entity.Id;
        }

        /// <summary>
        /// 修改应用分支
        /// </summary>
        /// <param name="appBranchDto">应用分支数据</param>
        /// <returns></returns>
        public async Task<Guid> Update(AppBranchDto appBranchDto)
        {
            var entity = _appBranchRepository.FirstOrDefault(p => p.Id == appBranchDto.Id && !p.DeleteFlag);  

            entity.UpdatedTime = DateTimeOffset.UtcNow;
            entity.DeployeeId = appBranchDto.DeployeeId.ToString();
            entity.Icon = appBranchDto.Icon;
            entity.IsMaster = appBranchDto.IsMaster;
            entity.Name = appBranchDto.Name;
            entity.Remark = appBranchDto.Remark;
            entity.Version = appBranchDto.Version;
            var result = await _appBranchRepository.UpdateAsync(entity);

            return result.Entity.Id;
        }

        /// <summary>
        /// 删除应用分支
        /// </summary>
        /// <param name="appBranchId">Id</param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid appBranchId)
        {
            var entity = _appBranchRepository.FirstOrDefault(p => p.Id == appBranchId && !p.DeleteFlag);

            entity.UpdatedTime = DateTimeOffset.UtcNow;
            entity.DeleteFlag = true;
            var result = await _appBranchRepository.UpdateAsync(entity);

            return result.Entity != null;
        }
    }
}
