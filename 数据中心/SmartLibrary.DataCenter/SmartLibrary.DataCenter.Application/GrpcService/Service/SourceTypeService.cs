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
using SmartLibrary.DataCenter.Application.Services;
using SmartLibrary.DataCenter.EntityFramework.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application.GrpcService.Service
{
    public class SourceTypeService : SourceTypeGrpcService.SourceTypeGrpcServiceBase
    {
        private readonly ISourceTypeService _sourceTypeService;

        public SourceTypeService(ISourceTypeService sourceTypeService)
        {
            _sourceTypeService = sourceTypeService;
        }
        public async override Task<AllSourceReply> GetAllSourceType(AllSourceRequest request, ServerCallContext context)
        {
            var sourceTypes = await _sourceTypeService.GetSourceTypes();
            AllSourceReply reply = new AllSourceReply();
            reply.SourceTypes.AddRange(sourceTypes.Adapt<List<SourceTypeItem>>());
            return reply;
        }

        public async override Task<SourceTypeItem> AddSourceType(AddSourceTypeRequest request, ServerCallContext context)
        {
            var sourceTyoe = new SourceTypeDto()
            {
               CreateType=1,
               Name=request.Name,
               UserKey=request.UserKey,
            };
           var sourceTyoeDto = await  _sourceTypeService.AddSourceType(sourceTyoe);

            SourceTypeItem reply = new SourceTypeItem
            {
                Code = sourceTyoeDto.Code,
                Name = sourceTyoeDto.Name,
                UserKey = sourceTyoeDto.UserKey,
                Id = sourceTyoeDto.Id.ToString(),
            };
            return reply;
        }
    }
}
