/*********************************************************
* 名    称：DeveloperAppService.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210913
* 描    述：供应商管理接口
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Common.Dtos;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 供应商管理接口
    /// </summary>
    public class DeveloperAppService : BaseAppService
    {
        /// <summary>
        /// 供应商表格数据查询
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<TableQueryResult<DeveloperViewModel>> QueryTableData([FromQuery] DeveloperTableQuery queryFilter)
        {
            return Task.FromResult(new TableQueryResult<DeveloperViewModel>());
        }

        /// <summary>
        /// 查询供应商详情
        /// </summary>
        /// <param name="developerId"></param>
        /// <returns></returns>
        [Route("{developerId}")]
        public Task<DeveloperViewModel> GetByID(Guid developerId)
        {
            return Task.FromResult(new DeveloperViewModel());
        }

        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="developerDto"></param>
        /// <returns></returns>
        public Task<Guid> Create([FromBody] DeveloperDto developerDto)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="developerDto"></param>
        /// <returns></returns>
        public Task<Guid> Update([FromBody] DeveloperDto developerDto)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        /// <summary>
        /// 删除供应商
        /// </summary>
        /// <param name="developerId"></param>
        /// <returns></returns>
        [Route("{developerId}")]
        public Task<bool> Delete(Guid developerId)
        {
            return Task.FromResult(true);
        }
    }
}
