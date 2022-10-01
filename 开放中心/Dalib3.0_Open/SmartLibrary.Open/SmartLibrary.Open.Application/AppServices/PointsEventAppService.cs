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
    /// 查询应用积分事件接口
    /// </summary>
    public class PointsEventAppService : BaseAppService
    {
        /// <summary>
        /// 查询积分事件表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<TableQueryResult<PointsEventViewModel>> QueryTableData([FromQuery] PointsEventTableQuery queryFilter)
        {
            return Task.FromResult(new TableQueryResult<PointsEventViewModel>());
        }

        /// <summary>
        /// 获取事件详情
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <returns></returns>
        [Route("{eventId}")]
        public Task<PointsEventViewModel> GetByID(Guid eventId)
        {
            return Task.FromResult(new PointsEventViewModel());
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventDto">事件数据</param>
        /// <returns></returns>
        public Task<Guid> Create([FromBody] PointsEventDto eventDto)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        /// <summary>
        /// 修改事件
        /// </summary>
        /// <param name="eventDto"></param>
        /// <returns></returns>
        public Task<Guid> Update([FromBody] PointsEventDto eventDto)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [Route("{eventId}")]
        public Task<bool> Delete(Guid eventId)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 审批事件
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [Route("[action]/{eventId}")]
        [HttpPost]
        public Task<bool> Confirm(Guid eventId)
        {
            return Task.FromResult(true);
        }
    }
}
