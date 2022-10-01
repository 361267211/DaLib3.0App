using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.Services.SM.Internal;
using SmartLibrary.Open.Services.SM.Internal.Attributes;
using SmartLibrary.Search.EsSearchProxy.Core.Models;

namespace SmartLibrary.Open.Services.Search
{
   
    class SearchBoxItemService : ISearchBoxItemService, IScoped
    {
        private readonly IRepository<EntityFramework.Core.Entitys.SearchBoxTitleItem> _searchBoxTitleItemRepository;

        public SearchBoxItemService(IRepository<SearchBoxTitleItem> searchBoxTitleItemRepository)
        {
            _searchBoxTitleItemRepository = searchBoxTitleItemRepository;
        }

        private static IEnumerable<SearchBoxTitleItem> EnsureSeedData()
        {

            var arr = new[]
            {

                (ArticlesTypeEnum.All,SearchParameterType.U,"全部搜索"),
                (ArticlesTypeEnum.Literature,SearchParameterType.TY,"期刊"),
                (ArticlesTypeEnum.Books,SearchParameterType.TY,"图书"),
                (ArticlesTypeEnum.Degree,SearchParameterType.TY,"学位"),
                (ArticlesTypeEnum.Meeting,SearchParameterType.TY,"会议"),
                (ArticlesTypeEnum.Standard,SearchParameterType.TY,"标准"),
                (ArticlesTypeEnum.Patent,SearchParameterType.TY,"专利"),
                (ArticlesTypeEnum.Laws,SearchParameterType.TY,"法规"),
                (ArticlesTypeEnum.Achievements,SearchParameterType.TY,"成果"),
                (ArticlesTypeEnum.MultiMedia,SearchParameterType.TY,"多媒体"),
                (ArticlesTypeEnum.Newspaper,SearchParameterType.TY,"报纸"),

            };
            var dtNow = DateTimeOffset.Now;
            return arr.Select(x => new SearchBoxTitleItem
            {
                CreatedTime = dtNow,
                UpdatedTime = dtNow,
                DeleteFlag = false,
                Symbol = x.Item2.ToString(),
                Title = x.Item3,
                Value = Convert.ToInt32(x.Item1)
            });
        }




        [CachedMethod(nameof(GetAllAvailableTitleItemAsync), ExpireOn = 60 * 60 * 12)]
        public async Task<IReadOnlyList<SearchBoxTitleItem>> GetAllAvailableTitleItemAsync()
        {
            var temp = await this._searchBoxTitleItemRepository.Entities.Where(x => !x.DeleteFlag).ToArrayAsync();

            //如果没有，给默认值
            if (temp.Length == 0)
            {

                temp = EnsureSeedData().ToArray();
                await this._searchBoxTitleItemRepository.Entities.AddRangeAsync(temp);

            }

            await this._searchBoxTitleItemRepository.Context.SaveChangesAsync();
            return temp;
        }
    }
}
