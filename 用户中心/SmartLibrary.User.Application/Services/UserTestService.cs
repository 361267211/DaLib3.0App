using DotNetCore.CAP;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Grpc.Core;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.User.Application.Dtos.Cap;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserTestService : UserGrpcService.UserGrpcServiceBase, IUserTestService, IScoped
    {
        private ICapPublisher _capPublisher;
        private TenantInfo _tenantInfo;

        public UserTestService(ICapPublisher capPublisher,
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
            return 0;
            //var newP = new Person
            //{
            //    Name = "测试人员",
            //    Age = 20,
            //    Address = "测试地址"
            //};

            ////添加事务确保当前服务数据操作与消息投递一致性
            //using (var tran = _personRepository.Database.BeginTransaction(_capPublisher, true))
            //{
            //    var newPerson = await _personRepository.InsertNowAsync(newP);
            //    var msg = new UserServicePublishEvent.NewPersonMsg
            //    {
            //        TenantName = _tenantInfo?.Name,
            //        PersonId = newPerson.Entity.Id,
            //    };
            //    _capPublisher.Publish(UserServicePublishEvent.NewPerson, msg);
            //    return newPerson.Entity.Id;
            //}

        }

    }
}