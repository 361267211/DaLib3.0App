using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Grpc.Core;
using SmartLibrary.Open.EntityFramework.Core.Entitys;

namespace SmartLibrary.Open.Application.GrpcAppServices.Service
{
    public class BookDonationTemplateGrpcService : DonationTemplateGrpcService.DonationTemplateGrpcServiceBase, IScoped
    {
        private readonly IRepository<BookDonationTemplate> _bookDonationTemplateRepository;

        public BookDonationTemplateGrpcService(IRepository<BookDonationTemplate> bookDonationTemplateRepository)
        {

            _bookDonationTemplateRepository = bookDonationTemplateRepository;
        }

        public override async Task<DonationTemplateListGrpcResponse> FetchDonationTemplateBySymbol(DonationTemplateGrpcRequest request, ServerCallContext context)
        {
            var temp = await this._bookDonationTemplateRepository.FirstOrDefaultAsync(x =>
                !x.DeleteFlag && x.Symbol == request.Symbol);
            if (temp == null)
                temp = new BookDonationTemplate { DllLink = String.Empty, Name = String.Empty, Symbol = String.Empty };
            return new DonationTemplateListGrpcResponse
            {
                Result = new DonationTemplateGrpcResponse
                {
                    DllLink = temp.DllLink,
                    Name = temp.Name,
                    Symbol = temp.Symbol
                }
            };
        }
    }
}
