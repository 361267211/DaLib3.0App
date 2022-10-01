using SmartLibrary.Open.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Common.Dtos;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 客户应用使用情况
    /// </summary>
    public class CustomerAppUsageAppService : BaseAppService
    {
        /// <summary>
        /// 查询客户应用使用情况表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<TableQueryResult<CustomerAppUsageViewModel>> QueryTableData([FromQuery] CustomerAppUsageTableQuery queryFilter)
        {
            return Task.FromResult(new TableQueryResult<CustomerAppUsageViewModel>());
        }

        /// <summary>
        /// 查询客户应用使用情况列表，不分页
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<List<CustomerAppUsageViewModel>> QueryListData([FromQuery] CustomerAppUsageTableQuery queryFilter)
        {
            return Task.FromResult(new List<CustomerAppUsageViewModel>());
        }

        /// <summary>
        /// 添加客户应用使用记录
        /// </summary>
        /// <param name="usageDto"></param>
        /// <returns></returns>
        public Task<Guid> Create([FromBody] CustomerAppUsageDto usageDto)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        /// <summary>
        /// 删除客户应用使用情况
        /// </summary>
        /// <param name="usageId"></param>
        /// <returns></returns>
        [Route("{usageId}")]
        public Task<bool> Delete(Guid usageId)
        {
            return Task.FromResult(true);
        }
    }
}
