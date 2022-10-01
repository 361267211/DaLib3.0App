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
using SmartLibrary.Open.Services.Dtos.Deployee;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public interface IDeploymentService
    {
        Task<Guid> Create(DeploymentDto deploymentDto);
        Task<bool> Delete(string deploymentId);
        Task<DeploymentViewModel> GetByID(string deploymentId);
        Task<PagedList<DeploymentViewModel>> QueryTableData(DeploymentTableQuery queryFilter);
        Task<Guid> Update(DeploymentDto deploymentDto);
    }
}