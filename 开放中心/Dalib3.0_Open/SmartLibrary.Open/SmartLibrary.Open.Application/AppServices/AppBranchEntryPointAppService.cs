using SmartLibrary.Open.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Application.Services;
using SmartLibrary.Open.Services;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 应用分支入口配置
    /// </summary>
    public class AppBranchEntryPointAppService : BaseAppService
    {
        /// <summary>
        /// 应用组件服务
        /// </summary>
        private IAppBranchEntryPointService _appBranchEntryPointService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppBranchEntryPointAppService(IAppBranchEntryPointService appBranchEntryPointService)
        {
            _appBranchEntryPointService = appBranchEntryPointService;
        }

        /// <summary>
        /// 获取应用分支所有入口
        /// </summary>
        /// <param name="appBranchId"></param>
        /// <returns></returns>
        [Route("[action]/{appBranchId}")]
        [HttpGet]
        public async Task<List<AppBranchEntryPointViewModel>> QueryListData(string appBranchId)
        {
            return await _appBranchEntryPointService.QueryListData(appBranchId);
        }

        /// <summary>
        /// 创建应用分支入口
        /// </summary>
        /// <param name="entryPointDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Guid> CreateEntryPoint([FromBody] AppBranchEntryPointDto entryPointDto)
        {
            return await _appBranchEntryPointService.Create(entryPointDto);
        }

        /// <summary>
        /// 修改应用分支入口
        /// </summary>
        /// <param name="entryPointDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<Guid> UpdateEntryPoint([FromBody] AppBranchEntryPointDto entryPointDto)
        {
            return await _appBranchEntryPointService.Update(entryPointDto);
        }

        /// <summary>
        /// 删除应用分支入口
        /// </summary>
        /// <param name="entryPointId">Id</param>
        /// <returns></returns>
        [Route("{entryPointId}")]
        [HttpDelete]
        public async Task<bool> DeleteEntryPoint(string entryPointId)
        {
            return await _appBranchEntryPointService.Delete(entryPointId);
        }


        /// <summary>
        /// 获取应用分支所有入口
        /// </summary>
        /// <param name="appBranchId"></param>
        /// <returns></returns>
        [Route("{appBranchId}")]
        [HttpGet]
        public async Task<List<AppBranchEntryPointViewModel>> GetAppEntranceList(string appBranchId)
        {
            return await _appBranchEntryPointService.GetAppEntranceList(appBranchId);
        }
    }
}
