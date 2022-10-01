using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    public interface IUserCenterService
    {
        Task<List<DictItem>> GetUserGroupsList(int pageIndex, int pageSize, string keyWord);
        Task<List<DictItem>> GetUserTypesList(int pageIndex, int pageSize, string keyWord);
    }
}
