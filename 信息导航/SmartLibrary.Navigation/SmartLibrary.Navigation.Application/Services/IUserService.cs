
using Grpc.Core;
using SmartLibraryUser;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application
{
    public interface IUserService
    {
        Task<UserReply> GetUserName(UserRequest request, ServerCallContext callContext = null);

        Task<int> AddOnePerson();
    }
}