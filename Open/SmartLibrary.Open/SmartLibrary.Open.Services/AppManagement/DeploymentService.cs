/*********************************************************
 * 名    称：DeploymentService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/8 16:32:50
 * 描    述：部署环境服务
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using SmartLibrary.Open.Services.Dtos.Deployee;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    /// <summary>
    /// 部署环境服务
    /// </summary>
    public class DeploymentService : IScoped, IDeploymentService
    {

        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<Deployment> _deploymentRepository;

        /// <summary>
        /// 应用数据仓储
        /// </summary>
        private readonly IRepository<MicroApplication> _appRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deploymentRepository"></param>
        /// <param name="appRepository"></param>
        public DeploymentService(IRepository<Deployment> deploymentRepository
            , IRepository<MicroApplication> appRepository)
        {
            _deploymentRepository = deploymentRepository;
            _appRepository = appRepository;
        }



        /// <summary>
        /// 查询应用部署信息表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<DeploymentViewModel>> QueryTableData(DeploymentTableQuery queryFilter)
        {
            var query = _deploymentRepository.Where(p => !p.DeleteFlag
            && (string.IsNullOrEmpty(queryFilter.CustomerId) || p.CustomerId == queryFilter.CustomerId)).Select(p => new DeploymentViewModel
            {
                ID = p.Id,
                CreateTime = p.CreatedTime.LocalDateTime,
                Name = p.Name,
                UpdateTime = p.UpdatedTime.HasValue ? p.UpdatedTime.Value.LocalDateTime : null,
                ApiGateway = p.ApiGateway,
                Code = p.Code,
                Desc = p.Desc,
                GrpcGateway = p.GrpcGateway
            });
            var table = await query.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return table;
        }

        /// <summary>
        /// 获取部署信息详情
        /// </summary>
        /// <param name="deploymentId">部署信息ID</param>
        /// <returns></returns>
        public async Task<DeploymentViewModel> GetByID(string deploymentId)
        {
            var deployment = await _deploymentRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == deploymentId);

            var result = new DeploymentViewModel
            {
                ID = deployment.Id,
                CreateTime = deployment.CreatedTime.LocalDateTime,
                Name = deployment.Name,
                UpdateTime = deployment.UpdatedTime.HasValue ? deployment.UpdatedTime.Value.LocalDateTime : null,
                ApiGateway = deployment.ApiGateway,
                Code = deployment.Code,
                Desc = deployment.Desc,
                GrpcGateway = deployment.GrpcGateway
            };
            return result;
        }

        /// <summary>
        /// 创建应用部署环境信息
        /// </summary>
        /// <param name="deploymentDto">部署环境信息</param>
        /// <returns></returns>
        public async Task<Guid> Create(DeploymentDto deploymentDto)
        {
            var deployment = new Deployment
            {
                Id = Guid.NewGuid(),
                CreatedTime = DateTimeOffset.UtcNow,
                Name = deploymentDto.Name,
                ApiGateway = deploymentDto.ApiGateway,
                Code = deploymentDto.Code,
                Desc = deploymentDto.Desc,
                GrpcGateway = deploymentDto.GrpcGateway
            };
            var result = await _deploymentRepository.InsertAsync(deployment);
            return result.Entity.Id;
        }


        /// <summary>
        /// 修改应用部署环境信息
        /// </summary>
        /// <param name="deploymentDto">部署环境信息</param>
        /// <returns></returns>
        public async Task<Guid> Update(DeploymentDto deploymentDto)
        {
            var deployment = await _deploymentRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id == deploymentDto.ID);
            deployment.UpdatedTime = DateTimeOffset.UtcNow;
            deployment.Name = deploymentDto.Name;
            deployment.ApiGateway = deploymentDto.ApiGateway;
            deployment.Code = deploymentDto.Code;
            deployment.Desc = deploymentDto.Desc;
            deployment.GrpcGateway = deploymentDto.GrpcGateway;
            var result = await _deploymentRepository.UpdateAsync(deployment);
            return result.Entity.Id;
        }

        /// <summary>
        /// 删除应用部署环境信息
        /// </summary>
        /// <param name="deploymentId">Id</param>
        /// <returns></returns>
        public async Task<bool> Delete(string deploymentId)
        {
            var deployment = await _deploymentRepository.FirstOrDefaultAsync(p => !p.DeleteFlag && p.Id.ToString() == deploymentId);
            deployment.UpdatedTime = DateTimeOffset.UtcNow;
            deployment.DeleteFlag = true;
            var result = await _deploymentRepository.UpdateAsync(deployment);
            return result.Entity != null;
        }
    }
}
