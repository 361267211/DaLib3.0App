/*********************************************************
 * 名    称：Terminal
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/21 22:06:42
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Service.Service
{
    public interface ITerminalService
    {
        Task<Guid> CreateTerminalInstance(TerminalInstanceDto temrminal);
        Task<bool> DeleteTerminalInstance(string terminalInstanceId);
        DictionaryViewModel GetDictionary();
        Task<TerminalInstanceDto> GetTerminalInstanceDetail(string terminalId);
        Task<List<TerminalInstanceDto>> GetTerminalInstanceList();
        Task<Guid> UpdateTerminalInstance(TerminalInstanceDto temrminalDto);
    }
}