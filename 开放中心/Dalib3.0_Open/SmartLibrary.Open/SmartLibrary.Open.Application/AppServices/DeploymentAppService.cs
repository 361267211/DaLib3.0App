using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Application.Services;
using SmartLibrary.Open.Services;
using SmartLibrary.Open.Services.Dtos.Deployee;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 应用部署环境信息管理
    /// </summary>
    public class DeploymentAppService : BaseAppService
    {

        /// <summary>
        /// 应用组件服务
        /// </summary>
        private IDeploymentService _deploymentService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deploymentService"></param>
        public DeploymentAppService(IDeploymentService deploymentService)
        {
            _deploymentService = deploymentService;
        }

        /// <summary>
        /// 查询应用部署信息表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<DeploymentViewModel>> QueryTableData([FromQuery] DeploymentTableQuery queryFilter)
        {
            return await _deploymentService.QueryTableData(queryFilter);
        }

        /// <summary>
        /// 获取部署信息详情
        /// </summary>
        /// <param name="deploymentId">部署信息ID</param>
        /// <returns></returns>
        [Route("{deploymentId}")]
        [HttpGet]
        public async Task<DeploymentViewModel> GetByID(string deploymentId)
        {
            return await _deploymentService.GetByID(deploymentId);
        }

        /// <summary>
        /// 创建应用部署环境信息
        /// </summary>
        /// <param name="deploymentDto">部署环境信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> Create([FromBody]DeploymentDto deploymentDto)
        {
            return await _deploymentService.Create(deploymentDto);
        }


        /// <summary>
        /// 修改应用部署环境信息
        /// </summary>
        /// <param name="deploymentDto">部署环境信息</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Guid> Update([FromBody]DeploymentDto deploymentDto)
        {
            return await _deploymentService.Update(deploymentDto);
        }

        /// <summary>
        /// 删除应用部署环境信息
        /// </summary>
        /// <param name="deploymentId">Id</param>
        /// <returns></returns>
        [Route("{deploymentId}")]
        [HttpDelete]
        public async Task<bool> Delete(string deploymentId)
        {
            return await _deploymentService.Delete(deploymentId);
        }
    }
}
