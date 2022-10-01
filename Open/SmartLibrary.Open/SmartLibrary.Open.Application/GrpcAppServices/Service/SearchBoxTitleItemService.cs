using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.Services.Search;
using SearchBoxTitleItem = SmartLibrary.Open.EntityFramework.Core.Entitys.SearchBoxTitleItem;

namespace SmartLibrary.Open.Application.GrpcAppServices.Service
{
    public class SearchBoxTitleItemService : SearchBoxTitleItemGrpcService.SearchBoxTitleItemGrpcServiceBase, IScoped
    {
        private readonly ISearchBoxItemService _searchBoxItemService;

        public SearchBoxTitleItemService(ISearchBoxItemService searchBoxItemService)
        {
            _searchBoxItemService = searchBoxItemService;
        }

        public override async Task<SearchBoxTitleItemResponse> GetAllAvailableTitleItem(SearchBoxTitleItemRequest request, ServerCallContext context)
        {
            var temp = await this._searchBoxItemService.GetAllAvailableTitleItemAsync();

            return new SearchBoxTitleItemResponse
            {
                SearchBoxTitleItems = { temp.Select(x => new SearchBoxTitleItem
                {
                    Id = x.Id.ToString(),
                    Symbol = x.Symbol,
                    Title = x.Title,
                    Value = x.Value
                })}
            };
        }
    }
}
