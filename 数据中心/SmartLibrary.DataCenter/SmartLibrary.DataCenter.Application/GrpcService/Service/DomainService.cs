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
using Mapster;
using SmartLibrary.DataCenter.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application.GrpcService.Service
{
    public class DomainService : DomainInfoService.DomainInfoServiceBase
    {
        private readonly IDomainService _domainService;

        public DomainService(IDomainService domainService)
        {
            _domainService = domainService;
        }

        /// <summary>
        /// 获取所有的数据库商资源
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<AllDomainInfoTreeReply> GetAllDomainInfoTrees(AllDomainInfoTreeRequest request, ServerCallContext context)
        {
            AllDomainInfoTreeReply reply = new AllDomainInfoTreeReply();

            var list = await _domainService.GetAllDomainTrees(request.Type, request.Level);

           // var replylist = list.Adapt<List<DomainTreeItem>>();

            reply.DomainTrees.AddRange(list);

            return reply;
        }

    }
}
