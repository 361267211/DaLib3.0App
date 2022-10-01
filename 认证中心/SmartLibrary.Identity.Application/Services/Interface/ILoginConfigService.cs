using SmartLibrary.Identity.Application.Dtos.LoginConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services.Interface
{
    public interface ILoginConfigService
    {
        /// <summary>
        /// 获取登录设置列表
        /// </summary>
        /// <returns></returns>
        Task<List<LoginConfigSetDto>> QueryListData();

        /// <summary>
        /// 设置是否开启
        /// </summary>
        /// <param name="openSet"></param>
        /// <returns></returns>
        Task<bool> SetIsOpen(LoginSetOpenDto openSet);

        /// <summary>
        /// 获取Cas登录详细配置
        /// </summary>
        /// <returns></returns>
        Task<CasConfigDto> GetCasLoginConfig();
    }
}
