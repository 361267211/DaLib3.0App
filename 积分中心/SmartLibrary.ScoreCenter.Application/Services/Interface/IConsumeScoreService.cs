/*********************************************************
* 名    称：IConsumeScoreService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分消费服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SqlSugar;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Interface
{
    /// <summary>
    /// 积分消费服务
    /// </summary>
    public interface IConsumeScoreService
    {
        Task<bool> ConsumeTry(SqlSugarClient dbClient, ConsumeScoreInput scoreInput);
        Task<bool> ConsumeConfirm(SqlSugarClient dbClient, ConsumeScoreInput scoreInput);
        Task<bool> ConsumeCancel(SqlSugarClient dbClient, ConsumeScoreInput scoreInput);
    }
}
