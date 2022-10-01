using DotNetCore.CAP;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Grpc.Core;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.SceneManage.Application.Dtos.Cap;
using SmartLibraryUser;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Application
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : UserGrpcService.UserGrpcServiceBase, IUserService, IScoped
    {
        private ICapPublisher _capPublisher;
        private TenantInfo _tenantInfo;

        public UserService(ICapPublisher capPublisher,
            TenantInfo tenantInfo)
        {
            _capPublisher = capPublisher;
            _tenantInfo = tenantInfo;
        }

        /// <summary>
        /// 获取用户姓名
        /// </summary>
        /// <param name="request"></param>
        /// <param name="callContext"></param>
        /// <returns></returns>
        [Authorize]
        public async override Task<UserReply> GetUserName(UserRequest request, ServerCallContext callContext = null)
        {
            var result = new UserReply
            {
                Id = request.Id,
                UserName = "Grpc-Test"
            };

            //var x= SiteGlobalConfig.KrsInfo.KrsSiteId;
            return await Task.FromResult(result);
        }
        /// <summary>
        /// 模拟添加员工同时，发布投递添加员工消息
        /// </summary>
        /// <returns></returns>
        public async Task<int> AddOnePerson()
        {
            
                return 1;

        }

    }
}