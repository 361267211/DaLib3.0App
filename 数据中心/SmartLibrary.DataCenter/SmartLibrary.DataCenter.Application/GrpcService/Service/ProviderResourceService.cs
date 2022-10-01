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

using Grpc.Core;
using SmartLibrary.DataCenter.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application.GrpcService.Service
{
    public class ProviderResourceService: ProviderResourceGrpcService.ProviderResourceGrpcServiceBase
    {
        private readonly IDatabaseService _databaseService;

        public ProviderResourceService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// 获取所有的数据库商资源
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<AvailableProviderResourceResponse> GetAllAvailableProviderResource(AvailableProviderResourceRequest request, ServerCallContext context)
        {          
            AvailableProviderResourceResponse reply = new AvailableProviderResourceResponse();
            reply.ProviderResourceItems.AddRange(await _databaseService.GetAllAvailableProviderResource());

            return reply;
        }

        public async override Task<GetAllDatabaseProviderResponse> GetAllDatabaseProvider(GetAllDatabaseProviderRequest request, ServerCallContext context)
        {
            GetAllDatabaseProviderResponse reply = new GetAllDatabaseProviderResponse();

          //  var ttt = await _databaseService.GetAllDatabaseProvider();
            reply.DatabaseProviderItems.AddRange(await _databaseService.GetAllDatabaseProvider());

            return reply;
        }

        public async override Task<GetResourceAlbumByProviderResponse> GetResourceAlbumByProvider(GetResourceAlbumByProviderRequest request, ServerCallContext context)
        {
            GetResourceAlbumByProviderResponse reply = new GetResourceAlbumByProviderResponse();

       //     var ttt = await _databaseService.GetResourceAlbum(request.Provider);
            reply.ResourceAlbumItem.AddRange(await _databaseService.GetResourceAlbum(request.Provider));

            return reply;
        }
    }
}
