using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.Open.EntityFramework.Core.Entitys;

namespace SmartLibrary.Open.Services.Search
{
    public  interface ISearchBoxItemService
    {

        Task<IReadOnlyList<SearchBoxTitleItem>> GetAllAvailableTitleItemAsync();
    }
}
