/*********************************************************
* 名    称：AppBranchAppService.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210912
* 描    述：应用分支管理
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Application.Services;
using System.Collections.Generic;
using SmartLibrary.Open.Services;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 应用分支管理接口
    /// </summary>
    public class AppBranchAppService : BaseAppService
    {
        private IAppBranchService _appBranchService { get; set; }

        public AppBranchAppService(IAppBranchService appBranchService)
        {
            _appBranchService = appBranchService;
        }

        /// <summary>
        /// 查询应用分支表格数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<AppBranchViewModel>> QueryTableData([FromBody] AppBranchTableQuery queryFilter)
        {
            var result = await _appBranchService.QueryTableData(queryFilter);
            return result;
        }

        /// <summary>
        /// 获取应用分支详情
        /// </summary>
        /// <param name="appBranchId">应用分支Id</param>
        /// <returns></returns>
        [Route("{AppBranchId}")]
        [HttpGet]
        public async Task<AppBranchViewModel> GetByID(Guid appBranchId)
        {
            var result = await _appBranchService.GetByID(appBranchId);
            return result;
        }

        /// <summary>
        /// 创建应用分支
        /// </summary>
        /// <param name="appBranchDto">应用分支数据</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> Create([FromBody] AppBranchDto appBranchDto)
        {
            var result = await _appBranchService.Create(appBranchDto);
            return result;
        }

        /// <summary>
        /// 修改应用分支
        /// </summary>
        /// <param name="appBranchDto">应用分支数据</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Guid> Update([FromBody] AppBranchDto appBranchDto)
        {
            var result = await _appBranchService.Update(appBranchDto);
            return result;
        }

        /// <summary>
        /// 删除应用分支
        /// </summary>
        /// <param name="appBranchId">Id</param>
        /// <returns></returns>
        [Route("{appBranchId}")]
        [HttpDelete]
        public async Task<bool> Delete(Guid appBranchId)
        {
            var result = await _appBranchService.Delete(appBranchId);
            return result;
        }


    }
}
