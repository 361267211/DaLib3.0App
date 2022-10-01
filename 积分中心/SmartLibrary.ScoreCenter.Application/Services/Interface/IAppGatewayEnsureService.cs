/*********************************************************
* 名    称：IAppGatewayEnsureService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：App网关地址获取
* 更新历史：
*
* *******************************************************/
using SmartLibrary.ScoreCenter.Application.Dtos;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Interface
{
    /// <summary>
    /// App网关地址获取
    /// </summary>
    public interface IAppGatewayEnsureService
    {
        /// <summary>
        /// 通过appcode查询应用是否部署以及网关地址
        /// </summary>
        /// <param name="appcode"></param>
        /// <returns></returns>
        Task<AppGatewayResult> GetAppServiceAddress(string appcode);
    }
}
