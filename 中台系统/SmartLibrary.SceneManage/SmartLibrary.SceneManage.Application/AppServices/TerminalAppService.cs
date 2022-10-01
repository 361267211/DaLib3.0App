/*********************************************************
 * 名    称：TerminalAppService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/22 22:21:10
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.DatabaseTerrace.Application.Filter;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using SmartLibrary.SceneManage.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Application.AppServices
{
    [Authorize(Policy = "DefaultPolicy")]
    public class TerminalAppService : IDynamicApiController
    {
        private ITerminalService _terminalService { get; set; }

        public TerminalAppService(ITerminalService terminalService)
        {
            _terminalService = terminalService;
        }

        /// <summary>
        /// 终端实例列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<TerminalInstanceDto>> GetTerminalInstanceList()
        {
            var result = await _terminalService.GetTerminalInstanceList();
            return result;
        }

        /// <summary>
        /// 获取终端实例详情
        /// </summary>
        /// <param name="terminalId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TerminalInstanceDto> GetTerminalInstanceDetail(string terminalId)
        {
            var result = await _terminalService.GetTerminalInstanceDetail(terminalId);
            return result;
        }


        /// <summary>
        /// 添加终端实例
        /// </summary>
        /// <param name="temrminal">终端实例</param>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork]
        public async Task<Guid> CreateTerminalInstance(TerminalInstanceDto temrminal)
        {
            var result = await _terminalService.CreateTerminalInstance(temrminal);
            return result;
        }

        /// <summary>
        /// 更新终端实例
        /// </summary>
        /// <param name="temrminalDto">终端实例</param>
        /// <returns></returns>
        [HttpPut]
        [UnitOfWork]
        public async Task<Guid> UpdateTerminalInstance([FromBody]TerminalInstanceDto temrminalDto)
        {
            var result = await _terminalService.UpdateTerminalInstance(temrminalDto);
            return result;
        }

        /// <summary>
        /// 删除终端实例
        /// </summary>
        /// <param name="terminalInstanceId">终端实例ID</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteTerminalInstance(string terminalInstanceId)
        {
            var result = await _terminalService.DeleteTerminalInstance(terminalInstanceId);
            return result;
        }

        /// <summary>
        /// 获取下拉框字典
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public  DictionaryViewModel GetDictionary()
        {
            var result =  _terminalService.GetDictionary();
            return result;
        }

    }
}
