using DotNetCore.CAP;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Grpc.Core;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.Core.Entitys;
using SmartLibrary.FileServer.Application.Dtos.Cap;
using SmartLibrary.FileServer.EntityFramework.Core.Entitys;
using SmartLibraryUser;
using System.Threading.Tasks;

namespace SmartLibrary.FileServer.Application
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : UserGrpcService.UserGrpcServiceBase, IUserService, IScoped
    {
        private ICapPublisher _capPublisher;
        private IRepository<Person> _personRepository;
        private IRepository<Asset> _assetRepository;
        private TenantInfo _tenantInfo;

        public UserService(ICapPublisher capPublisher,
            IRepository<Person> personRepository, IRepository<Asset> assetRepository,
            TenantInfo tenantInfo)
        {
            _capPublisher = capPublisher;
            _personRepository = personRepository;
            _assetRepository = assetRepository;
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
            var asset = _assetRepository.Entities.Find(1);

            var result = new UserReply
            {
                Id = request.Id,
                UserName = $"Grpc-Test-{asset.Title}"
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
            var newP = new Person
            {
                Name = "测试人员",
                Age = 20,
                Address = "测试地址"
            };

            //添加事务确保当前服务数据操作与消息投递一致性
            using (var tran = _personRepository.Database.BeginTransaction(_capPublisher, true))
            {
                var newPerson = await _personRepository.InsertNowAsync(newP);
                var msg = new UserServicePublishEvent.NewPersonMsg
                {
                    TenantName = _tenantInfo?.Name,
                    PersonId = newPerson.Entity.Id,
                };
                _capPublisher.Publish(UserServicePublishEvent.NewPerson, msg);
                return newPerson.Entity.Id;
            }

        }

    }
}