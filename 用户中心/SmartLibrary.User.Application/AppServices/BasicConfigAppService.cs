/*********************************************************
* 名    称：BasicConfigAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：应用配置Api
* 更新历史：
*
* *******************************************************/
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.User.Application.Dtos.BasicConfigSet;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 应用配置功能
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class BasicConfigAppService : BaseAppService
    {
        private readonly IBasicConfigService _basicConfigService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="basicConfigService"></param>
        public BasicConfigAppService(IBasicConfigService basicConfigService)
        {
            _basicConfigService = basicConfigService;
        }

        /// <summary>
        /// 获取初始化数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await _basicConfigService.GetBasicConfigInitData();
        }

        /// <summary>
        /// 获取用户配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<BasicConfigSetOutput> GetBasicConfig()
        {
            var basicConfigDto = await _basicConfigService.GetBasicConfigSet();
            return basicConfigDto.Adapt<BasicConfigSetOutput>();
        }

        /// <summary>
        /// 配置应用设置
        /// </summary>
        /// <param name="configData">基础配置数据</param>
        /// <returns></returns>
        public async Task<bool> SetBasicConfig([FromBody] BasicConfigSetInput configData)
        {
            var configDto = configData.Adapt<BasicConfigSetDto>();
            var result = await _basicConfigService.SetBasicConfig(configDto);
            return result;
        }

        ///// <summary>
        ///// 设置敏感信息查看馆员
        ///// </summary>
        ///// <param name="staffIds">馆员信息Id</param>
        ///// <returns></returns>
        //public Task<bool> SetSensitiveVisibleStaff([FromBody] List<Guid> staffIds)
        //{
        //    return Task.FromResult(true);
        //}

        ///// <summary>
        ///// 查询敏感信息
        ///// </summary>
        ///// <param name="queryFilter"></param>
        ///// <returns></returns>
        //public Task<List<InfoPermitStaffOutput>> QuerySensitiveVisibleStaff([FromQuery] InfoPermitStaffTableQuery queryFilter)
        //{
        //    return Task.FromResult(new List<InfoPermitStaffOutput>());
        //}

        /// <summary>
        /// 查询可认领读者卡的用户1103
        /// </summary>
        /// <returns></returns>
        public async Task<List<InfoPermitReaderOutput>> QueryCardClaimReader()
        {
            var pageList = await _basicConfigService.GetCardClaimReader();
            var targetList = pageList.Adapt<List<InfoPermitReaderOutput>>();
            return targetList;
        }

        /// <summary>
        /// 设置认领读者卡用户1103
        /// </summary>
        /// <param name="infoPermitReaders">认领读者卡用户</param>
        /// <returns></returns>
        public async Task<bool> SetCardClaimReader([FromBody] List<InfoPermitReaderInput> infoPermitReaders)
        {
            var readerList = infoPermitReaders.Adapt<List<InfoPermitReaderDto>>();
            var result = await _basicConfigService.SetCardClaimReader(readerList);
            return result;
        }


        /// <summary>
        /// 查询可自己补全信息的用户1103
        /// </summary>
        /// <returns></returns>
        public async Task<List<InfoPermitReaderOutput>> QueryInfoAppendReader()
        {
            var pageList = await _basicConfigService.GetInfoAppendReader();
            var targetList = pageList.Adapt<List<InfoPermitReaderOutput>>();
            return targetList;
        }

        /// <summary>
        /// 设置补充读者信息的用户1103
        /// </summary>
        /// <param name="infoPermitReaders">认领读者卡用户</param>
        /// <returns></returns>
        public async Task<bool> SetInfoAppendReader([FromBody] List<InfoPermitReaderInput> infoPermitReaders)
        {
            var readerList = infoPermitReaders.Adapt<List<InfoPermitReaderDto>>();
            var result = await _basicConfigService.SetInfoAppendReader(readerList);
            return result;
        }

        /// <summary>
        /// 获取读者可编辑信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<ReaderEditPropertyOutput>> QueryReaderEditProperty()
        {
            var resultList = await _basicConfigService.QueryReaderEditProperty();
            return resultList.Adapt<List<ReaderEditPropertyOutput>>();
        }

        /// <summary>
        /// 设置可编辑属性
        /// </summary>
        /// <param name="editProperties"></param>
        /// <returns></returns>
        public async Task<bool> SetReaderEditProperty([FromBody] List<ReaderEditPropertyInput> editProperties)
        {
            var editData = editProperties.Adapt<List<ReaderEditPropertyDto>>();
            var result = await _basicConfigService.SetReaderEditProperty(editData);
            return result;
        }
    }
}
