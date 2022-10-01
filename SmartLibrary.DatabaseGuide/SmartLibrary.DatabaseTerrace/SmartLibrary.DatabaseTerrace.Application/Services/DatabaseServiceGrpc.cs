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
using SmartLibraryDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Application.Services
{
    public class DatabaseServiceGrpc : DatabaseGrpcService.DatabaseGrpcServiceBase
    {
        private readonly IDatabaseTerraceService _databaseTerraceService;
        public DatabaseServiceGrpc(IDatabaseTerraceService databaseTerraceService)
        {
            _databaseTerraceService = databaseTerraceService;
        }
        public async override Task<GetDatabaseGuideStatCountReply> GetDatabaseGuideStatCount(Empty request, ServerCallContext context)
        {
            GetDatabaseGuideStatCountReply reply = new GetDatabaseGuideStatCountReply();
            var list = await _databaseTerraceService.GetStatisticsCount(serachKey:"",languageId:0,articleType:"",domainEscs:"",purchaseType:"",status:0,isShow:true);
            int count = list.EffectiveCount;
            reply.Count = count ;
            return reply;
        }
    }
}
