/*********************************************************
 * 名    称：AppManageService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/12 17:19:12
 * 描    述：开放平台应用管理Grpc服务
 *
 * 更新历史：
 *
 * *******************************************************/
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Application.GrpcAppServices
{
    public interface IAppManageGrpcService
    {
        Task<AppListReply> GetAppList(Empty request, ServerCallContext context);
    }
}