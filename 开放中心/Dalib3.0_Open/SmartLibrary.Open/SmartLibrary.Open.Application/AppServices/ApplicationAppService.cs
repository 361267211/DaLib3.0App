/*********************************************************
* 名    称：ApplicationAppService.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210908
* 描    述：应用管理Api接口
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Common.Dtos;
using SmartLibrary.Open.Application.Services;
using SmartLibrary.Open.Services;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 应用管理接口
    /// </summary>
    public class ApplicationAppService : BaseAppService
    {
        /// <summary>
        /// 应用服务
        /// </summary>
        private IApplicationService _applicationService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationService"></param>
        public ApplicationAppService(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }


        /// <summary>
        /// 查询App表格数据
        /// </summary>
        /// <param name="queryFilter">App表格查询条件</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<AppTableViewModel>> QueryTableData([FromQuery] AppTableQuery queryFilter)
        {
            return await _applicationService.QueryTableData(queryFilter);
        }

        /// <summary>
        /// 获取应用信息系详情
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        [Route("{appId}")]
        [HttpGet]
        public async Task<AppViewModel> GetById(Guid appId)
        {
            return await _applicationService.GetById(appId);
        }


        /// <summary>
        /// 查询App版本变更数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<AppDynamicInfoViewModel>> QueryVersionRecordData([FromQuery] VersionTableQuery queryFilter)
        {
            return await _applicationService.QueryVersionRecordData(queryFilter);
        }

        /// <summary>
        /// 查询应用客户数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<AppCustomerTableViewModel>> QueryAppCustomerTableData([FromQuery] AppCustomerTableQuery queryFilter)
        {
            return await _applicationService.QueryAppCustomerTableData(queryFilter);
        }

        /// <summary>
        /// 添加应用信息
        /// </summary>
        /// <param name="appDto">应用创建数据</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> Create([FromBody] ApplicationDto appDto)
        {
            return await _applicationService.Create(appDto);
        }

        /// <summary>
        /// 更新应用信息
        /// </summary>
        /// <param name="appDto">应用修改数据</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Guid> Update([FromBody] ApplicationDto appDto)
        {
            return await _applicationService.Update(appDto);
        }

        /// <summary>
        /// 删除应用
        /// </summary>
        /// <param name="appID">应用ID</param>
        /// <returns></returns>
        [Route("{appID}")]
        [HttpDelete]
        public async Task<bool> Delete(Guid appID)
        {
            return await _applicationService.Delete(appID);
        }

        /// <summary>
        /// 批量上架应用
        /// </summary>
        /// <param name="appIds">上架应用Id集合</param>
        /// <returns></returns>
        [HttpPut]
        public Task<bool> EnableApplication([FromBody] List<Guid> appIds)
        {
            return Task.FromResult(_applicationService.EnableApplication(appIds));
        }

        /// <summary>
        /// 批量下架应用
        /// </summary>
        /// <param name="appIds">下架应用Id集合</param>
        /// <returns></returns>
        [HttpPut]
        public Task<bool> DisableApplication([FromBody] List<Guid> appIds)
        {
            return Task.FromResult(_applicationService.DisableApplication(appIds));
        }
    }
}
