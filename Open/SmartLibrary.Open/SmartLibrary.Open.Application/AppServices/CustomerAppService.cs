/*********************************************************
* 名    称：CustomerAppService.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210909
* 描    述：客户管理服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Common.Dtos;
using System.Collections.Generic;
using SmartLibrary.Open.Services;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 客户管理服务
    /// </summary>
    public class CustomerAppService : BaseAppService
    {
        private ICustomerService _customerService;
        public CustomerAppService(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        /// <summary>
        /// 查询客户Table数据
        /// </summary>
        /// <param name="queryFilter">列表查询条件</param>
        /// <returns></returns>
        public async Task<PagedList<CustomerTableViewModel>> QueryTableData([FromQuery] CustomerTableQuery queryFilter)
        {
            var pageList = await _customerService.QueryTableData(queryFilter);
            return pageList;
        }

        /// <summary>
        /// 查询客户数据详情
        /// </summary>
        /// <param name="customerId">应用Id</param>
        /// <returns></returns>
        [Route("{customerId}")]
        public async Task<CustomerTableViewModel> GetById(Guid customerId)
        {
            var result = await _customerService.GetById(customerId);
            return result;
        }

        /// <summary>
        /// 客户凭据信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [Route("[action]/{customerId}")]
        public async Task<CustomerCredentialViewModel> GetCredentialById(Guid customerId)
        {
            var result = await _customerService.GetCredentialById(customerId);
            return result;
        }

    }
}
