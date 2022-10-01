/*********************************************************
 * 名    称：AppRouteGrpcService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/28 19:19:58
 * 描    述：应用路由Grpc服务
 *
 * 更新历史：
 *
 * *******************************************************/
using Grpc.Core;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Application.GrpcAppServices.Service
{
    public interface IAppRouteGrpcAppService
    {
        Task<AppRouteListReply> GetAppRouteList(AppRouteListRequest request, ServerCallContext context);
    }
}