/*********************************************************
* 名    称：IReaderActionEventService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者行为事件监听服务
* 更新历史：
*
* *******************************************************/
using DotNetCore.CAP;
using SmartLibrary.ScoreCenter.Application.Dtos.Cap;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Interface
{
    /// <summary>
    /// 读者行为事件监听处理
    /// </summary>
    public interface IReaderActionEventService
    {
        /// <summary>
        /// 处理读者行为事件
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="capHeader"></param>
        /// <returns></returns>
        Task<bool> ProcessReaderActionEvent(SubscribeEvent.ActionEventMsg msg, CapHeader capHeader);
    }
}
