/*********************************************************
 * 名    称：BaseService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2022/1/10 16:17:43
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using Google.Protobuf.WellKnownTypes;
using SmartLibrary.AppCenter;
using SmartLibrary.SceneManage.Common;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.User.RpcService;
using SmartLibraryNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartLibrary.AppCenter.AppCenterGrpcService;
using static SmartLibrary.User.RpcService.UserGrpcService;
using static SmartLibraryNavigation.NavigationGrpcService;

namespace SmartLibrary.SceneManage.Service.Service
{
    /// <summary>
    /// 中台业务服务基类
    /// </summary>
    public class BaseService
    {

        /// <summary>
        /// grpc客户端生成器
        /// </summary>
        protected readonly IGrpcClientResolver GrpcClientResolver;



        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseService(IGrpcClientResolver grpcClientResolver)
        {
            GrpcClientResolver = grpcClientResolver;
        }

        #region 私有方法

        /// <summary>
        /// 获取当前应用的访问地址
        /// </summary>
        /// <returns></returns>
        protected async Task<AppBaseUriReply> GetAppBaseUrl()
        {
            try
            {
                var grpcClient = GrpcClientResolver.EnsureClient<AppCenterGrpcServiceClient>(nameof(AppCenterGrpcService));
                var result = await grpcClient.GetAppBaseUriAsync(new AppBaseUriRequest { AppRouteCode = SiteGlobalConfig.AppBaseConfig.AppRouteCode });
                return result;
            }
            catch(Exception ex)
            {
                return new AppBaseUriReply { FrontUrl = "192.168.21.46", BackUrl = "192.168.21.46" };
            }
        }

        /// <summary>
        /// 获取应用列表
        /// </summary>
        /// <returns></returns>
        protected async Task<AppListReply> GetAppList(AppListRequest appListRequest)
        {
            var grpcClient = GrpcClientResolver.EnsureClient<AppCenterGrpcServiceClient>(nameof(AppCenterGrpcService));
            var result = await grpcClient.GetAppListAsync(appListRequest);
            return result;
        }

        /// <summary>
        /// 获取应用模板
        /// </summary>
        /// <returns></returns>
        protected async Task<AppWidgetListReply> GetAppWidgetList(AppWidgetListRequest appWidgetListRequest)
        {
            var grpcClient = GrpcClientResolver.EnsureClient<AppCenterGrpcServiceClient>(nameof(AppCenterGrpcService));
            var result = await grpcClient.GetAppWidgetListAsync(appWidgetListRequest);
            return result;
        }

        /// <summary>
        /// 获取应用栏目
        /// </summary>
        /// <returns></returns>
        protected async Task<AppColumnListReply> GetAppColumnList(AppColumnListRequest appColumnListRequest)
        {
            var grpcClient = GrpcClientResolver.EnsureClient<AppCenterGrpcServiceClient>(nameof(AppCenterGrpcService));
            var result = await grpcClient.GetAppColumnListAsync(appColumnListRequest);
            return result;
        }


        /// <summary>
        /// 获取应用服务类型
        /// </summary>
        /// <returns></returns>
        protected async Task<ServiceTypeReply> GetServiceType()
        {
            var grpcClient = GrpcClientResolver.EnsureClient<AppCenterGrpcServiceClient>(nameof(AppCenterGrpcService));
            var result = await grpcClient.GetServiceTypeAsync(new Empty());
            return result;
        }


        /// <summary>
        /// 获取用户相关字典
        /// </summary>
        /// <returns></returns>
        protected async Task<DictList> GetUserDictionary(int dicType)
        {
            var grpcClient = GrpcClientResolver.EnsureClient<UserGrpcServiceClient>(nameof(UserGrpcService));
            var request = new SimpleTableQuery { PageIndex = 1, PageSize = 100 };

            switch (dicType)
            {
                case 1:
                    return await grpcClient.GetUserCollegeListAsync(request);
                case 2:
                    return await grpcClient.GetUserTypeListAsync(request);
                case 3:
                    return await grpcClient.GetUserGroupListAsync(request);
                default:
                    return await grpcClient.GetUserCollegeListAsync(request);
            }
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <returns></returns>
        protected async Task<UserData> GetUserByKey(string userKey)
        {
            var grpcClient = GrpcClientResolver.EnsureClient<UserGrpcServiceClient>(nameof(UserGrpcService));
            return await grpcClient.GetUserByKeyAsync(new StringValue { Value = userKey });
        }

        /// <summary>
        /// 获取首页顶部主导航
        /// </summary>
        /// <returns></returns>
        protected async Task<NavigationListReply> GetPortalMajorNavagationList(string id)
        {
            var grpcClient = GrpcClientResolver.EnsureClient<NavigationGrpcServiceClient>(nameof(NavigationGrpcService));
            return await grpcClient.GetNavigationListAsync(new NavigationListRequest { Id = id });
        }
        #endregion
    }
}
