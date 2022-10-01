using SmartLibrary.Identity.Application.Dtos.RegisterConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services.Interface
{
    /// <summary>
    /// 注册配置服务
    /// </summary>
    public interface IRegisterConfigService
    {
        /// <summary>
        /// 获取注册配置信息
        /// </summary>
        /// <returns></returns>
        Task<RegisterConfigSetDto> GetDetailInfo();
        /// <summary>
        /// 保存注册配置
        /// </summary>
        /// <param name="configSet"></param>
        /// <returns></returns>
        Task<bool> Save(RegisterConfigSetDto configSet);
    }
}
