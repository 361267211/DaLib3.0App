using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartLibrary.Identity.Application.Dtos.LoginConfig;
using SmartLibrary.Identity.Application.Services.Enum;
using SmartLibrary.Identity.Application.Services.Interface;
using SmartLibrary.Identity.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services.Impl
{
    public class LoginConfigService : ILoginConfigService, IScoped
    {
        private readonly IRepository<LoginConfigSet> _loginSetRepository;

        public LoginConfigService(IRepository<LoginConfigSet> loginSetRepository)
        {
            _loginSetRepository = loginSetRepository;
        }
        /// <summary>
        /// 获取登录配置
        /// </summary>
        /// <returns></returns>
        public async Task<List<LoginConfigSetDto>> QueryListData()
        {
            var loginList = await _loginSetRepository.Where(x => !x.DeleteFlag).OrderBy(x => x.Sort).ProjectToType<LoginConfigSetDto>().ToListAsync();
            return loginList;
        }
        /// <summary>
        /// 设置开启
        /// </summary>
        /// <param name="openSet"></param>
        /// <returns></returns>
        public async Task<bool> SetIsOpen(LoginSetOpenDto openSet)
        {
            var loginSet = await _loginSetRepository.FirstOrDefaultAsync(x => x.Id == openSet.ID);
            if (loginSet == null)
            {
                throw Oops.Oh("未找到设置信息");
            }
            loginSet.IsOpen = openSet.IsOpen;
            await _loginSetRepository.UpdateAsync(loginSet);
            return true;
        }
        /// <summary>
        /// 获取Cas登录详细配置
        /// </summary>
        /// <returns></returns>
        public async Task<CasConfigDto> GetCasLoginConfig()
        {
            return new CasConfigDto();
            //var casLogin = await _loginSetRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.LoginType == (int)EnumLoginType.学校统一认证);
            //if (casLogin == null || !casLogin.IsOpen)
            //{
            //    return null;
            //}
            //try
            //{
            //    return JsonConvert.DeserializeObject<CasConfigDto>(casLogin.LoginConfig);
            //}
            //catch
            //{
            //    return null;
            //}


        }
    }
}
