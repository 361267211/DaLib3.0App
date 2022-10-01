using Furion.RemoteRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.RemoteProxy
{
    public interface IHttp : IHttpDispatchProxy
    {
        [Get("http://localhost:42480/api/getCurrentBorrowList?t={t}&query={query}")]
        Task<int> GetCurrentBorrowListAsync(int t, string query);
    }
}
