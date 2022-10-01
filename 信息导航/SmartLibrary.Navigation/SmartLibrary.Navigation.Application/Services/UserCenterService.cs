/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Google.Protobuf.WellKnownTypes;
using SmartLibrary.Navigation.Application.Interceptors;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartLibrary.User.RpcService.UserPageData.Types;

namespace SmartLibrary.Navigation.Application.Services
{
    public class UserCenterService : IScoped, IUserCenterService
    {
        /// <summary>
        /// 获取所有用户分组
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetUserGroupsList(int pageIndex,int pageSize,string keyWord)
        {
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            SimpleTableQuery request = new SimpleTableQuery {PageIndex= pageIndex,PageSize=pageSize,KeyWord=keyWord ?? "" };
            DictList reply = new DictList();
            try
            {
                reply = await grpcClient.GetUserGroupListAsync(request);
                return reply.Items.ToList();
            }
            catch (Exception ex)
            {
                throw Oops.Oh($"应用中心调用异常{ex}");

            }
        }

        /// <summary>
        /// 获取所有用户类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictItem>> GetUserTypesList(int pageIndex, int pageSize, string keyWord)
        {
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            SimpleTableQuery request = new SimpleTableQuery { PageIndex = pageIndex, PageSize = pageSize, KeyWord = keyWord??"" };
            DictList reply = new DictList();
            try
            {
                reply = await grpcClient.GetUserTypeListAsync(request);
                return reply.Items.ToList();
            }
            catch (Exception ex)
            {
                throw Oops.Oh($"应用中心调用异常{ex}");

            }
        }

        /// <summary>
        /// 获取所有用户类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserListItem>> GetManagerList(int pageIndex, int pageSize, string keyWord)
        {
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            Empty request = new Empty { };
            UserPageData reply = new UserPageData();
            try
            {
                reply = await grpcClient.GetManagerListAsync(request);
                return reply.Items.ToList();
            }
            catch (Exception ex)
            {
                throw Oops.Oh($"应用中心调用异常{ex}");

            }
        }

    }
}
