using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using Grpc.Core;
using Mapster;
using SmartLibrary.Open.Services.Search;
using SmartLibrary.Open;

namespace SmartLibrary.Open.Application.GrpcAppServices.Service
{
    public class OpacTemplateService : SmartLibrary.Open.OpacTemplateGrpcService.OpacTemplateGrpcServiceBase, IScoped
    {


        private readonly IOpacTemplateService _opacTemplateService;

        public OpacTemplateService(IOpacTemplateService opacTemplateService)
        {
            _opacTemplateService = opacTemplateService;
        }
        /// <summary>
        /// 手工匹配
        /// </summary>
        /// <returns></returns>
        private static OpacTemplateGrpcResponse ManulMap(EntityFramework.Core.Entitys.OpacTemplate x)
        {
            return new OpacTemplateGrpcResponse
            {

                Name = x.Name,


                Symbol = x.Symbol,
                AppointmentSupport = x.AppointmentSupport,
                DllLink = x.DllLink
            };
        }
        public override async Task<OpacTemplateGrpcResponse> FetchOpacTemplateBySymbol(OpacTemplateGrpcRequest request, ServerCallContext context)
        {
            var x = await this._opacTemplateService.FetchOpacTemplateBySymbolAsync(request.Symbol);
            var result = ManulMap(x);
            return result;
        }


        public override async Task<OpacTemplateListGrpcResponse> FetchOpacTemplateList(OpacTemplateListGrpcRequest request, ServerCallContext context)
        {
            var temp = await this._opacTemplateService.FetchAllAsync();
            return new OpacTemplateListGrpcResponse
            {
                List = { temp.Select(ManulMap) }
            };
        }
    }
}
