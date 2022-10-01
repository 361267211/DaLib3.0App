using Microsoft.AspNetCore.Authorization;
using SmartLibrary.User.Application.Dtos.Common;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class UserTagAnalysisAppService : BaseAppService
    {
    }
}
