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

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mapster;
using SmartLibraryAppAppColumn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Application.Services
{
   public class DatabaseColumnGrpcService: DatabaseAppColumnGrpcService.DatabaseAppColumnGrpcServiceBase
    {
        private readonly IDatabaseTerraceService _databaseTerraceService;
        public DatabaseColumnGrpcService(
            IDatabaseTerraceService databaseTerraceService
            )
        {
            _databaseTerraceService = databaseTerraceService;
        }

        public async override Task<AppColumnListReply> GetAppColumnList(Empty request, ServerCallContext context)
        {

           var columnPage= await _databaseTerraceService.GetDatabaseColumns(1, 100);
            var columns = columnPage.Items.Adapt<List<AppColumnListSingleItem>>();

            AppColumnListReply reply = new AppColumnListReply();
            reply.AppColumnList.AddRange(columns);
            return reply;
        }
    }
}
