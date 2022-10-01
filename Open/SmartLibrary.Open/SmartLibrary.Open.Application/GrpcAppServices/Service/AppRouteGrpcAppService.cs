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
using Furion.DependencyInjection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SmartLibrary.Open.Services;
using SmartLibrary.Open.Services.AppManagement;
using SmartLibrary.Open.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartLibrary.Open.AppRouteGrpcService;

namespace SmartLibrary.Open.Application.GrpcAppServices.Service
{
    public class AppRouteGrpcAppService : AppRouteGrpcServiceBase, IScoped, IAppRouteGrpcAppService
    {
        /// <summary>
        /// 应用路由服务
        /// </summary>
        private IAppRouteService _appRouteService { get; set; }
        private readonly ICustomerService _CustomerService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appRouteService"></param>
        /// <param name="customerService"></param>
        public AppRouteGrpcAppService(IAppRouteService appRouteService,
                                      ICustomerService customerService)
        {
            _appRouteService = appRouteService;
            _CustomerService = customerService;
        }

        /// <summary>
        /// 应用列表检索
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppRouteListReply> GetAppRouteList(AppRouteListRequest request, ServerCallContext context)
        {
            var query = new AppRouteQuery
            {
                TenantCode = request.TenantCode
            };
            var result = new AppRouteListReply();
            try
            {
                var list = await _appRouteService.GetAppRouteList(query);

                result.TenantRouteList.AddRange(list.Select(p => new AppRouteListSingle
                {
                    TenantCode = p.TenantCode
                }));
                foreach (var item in result.TenantRouteList)
                {
                    item.AppRouteList.AddRange(list.FirstOrDefault(p => p.TenantCode == item.TenantCode).AppRouters.Select(r => new AppRouteListSingleItem
                    {
                        AppRouteCode = r.AppRouteCode,
                        GrpcApiGateway = r.GrpcApiServerName,
                        RestApiGateway = r.RestApiServerName
                    }));
                }
            }
            catch (Exception)
            {

            }
            return result;

        }

        /// <summary>
        /// 获取所有租户列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CustomerListReply> GetCustomerList(Empty request, ServerCallContext context)
        {
            var result = new CustomerListReply();

            var list = await _CustomerService.GetCustomerCredentials();

            list?.ForEach(c =>
            {
                result.CustomerList.Add(new CustomerSingleItem
                {
                    OrgId = c.Key,
                    OrgSecret = c.Secret
                });
            });

            return result;
        }
    }
}
