/*********************************************************
 * 名    称：AppRouteService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/15 15:53:56
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.Services.Dtos.AppRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.AppManagement
{
    public class AppRouteService : IScoped, IAppRouteService
    {
        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<MicroApplication> _microAppRepository;

        /// <summary>
        /// 应用分支数据仓储
        /// </summary>
        private readonly IRepository<AppBranch> _appBranchRepository;

        /// <summary>
        /// 应用部署环境数据仓储
        /// </summary>
        private readonly IRepository<Deployment> _deploymentRepository;

        /// <summary>
        /// 客户应用数据仓储
        /// </summary>
        private readonly IRepository<CustomerAppUsage> _customerAppUsageRepository;

        /// <summary>
        /// 客户数据仓储
        /// </summary>
        private readonly IRepository<Customer> _customerRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="microAppRepository"></param>
        /// <param name="appBranchRepository"></param>
        /// <param name="deploymentRepository"></param>
        /// <param name="customerAppUsageRepository"></param>
        /// <param name="customerRepository"></param>
        public AppRouteService(IRepository<MicroApplication> microAppRepository,
            IRepository<AppBranch> appBranchRepository,
            IRepository<Deployment> deploymentRepository,
            IRepository<CustomerAppUsage> customerAppUsageRepository,
            IRepository<Customer> customerRepository)
        {
            _microAppRepository = microAppRepository;
            _appBranchRepository = appBranchRepository;
            _deploymentRepository = deploymentRepository;
            _customerAppUsageRepository = customerAppUsageRepository;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// 查询应用数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<List<AppRouteViewModel>> GetAppRouteList(AppRouteQuery queryFilter)
        {
            var queryApplication = _microAppRepository.Where(p => !p.DeleteFlag).AsQueryable();
            var queryCustomerApp = _customerAppUsageRepository.Where(p => !p.DeleteFlag).AsQueryable();
            var queryAppBranch = _appBranchRepository.Where(p => !p.DeleteFlag).AsQueryable();
            var queryDeployment = _deploymentRepository.Where(p => !p.DeleteFlag).AsQueryable();
            var queryCustomer = _customerRepository.Where(p => !p.DeleteFlag).AsQueryable();

            var query = from app in queryApplication
                        join cua in queryCustomerApp on app.Id.ToString() equals cua.AppId
                        join cus in queryCustomer on cua.CustomerId equals cus.Id.ToString()
                        join appbr in queryAppBranch on cua.AppBranchId equals appbr.Id.ToString()
                        where (app.Status == 1 && cua.BeginDate < DateTime.Now && cua.ExpireDate >= DateTime.Now)
                        && (string.IsNullOrEmpty(queryFilter.TenantCode) || cus.Owner == queryFilter.TenantCode)
                        select new
                        {
                            TenantCode = cus.Owner,
                            AppRouter = (from dp in queryDeployment
                                          where dp.Id.ToString() == appbr.DeployeeId
                                          select new AppRouter
                                          {
                                              AppRouteCode = app.RouteCode,
                                              GrpcApiServerName = dp.GrpcGateway,
                                              RestApiServerName = dp.ApiGateway,
                                          }).FirstOrDefault()
                        };

            var table = await query.ToListAsync();
            var result = table.GroupBy(p => p.TenantCode).Select(g => new AppRouteViewModel { TenantCode = g.Key, AppRouters = g.Select(p => p.AppRouter).ToList() }).ToList();
            return result;
        }
    }
}
