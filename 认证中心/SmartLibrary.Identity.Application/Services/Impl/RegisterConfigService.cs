using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Identity.Application.Dtos.RegisterConfig;
using SmartLibrary.Identity.Application.Services.Interface;
using SmartLibrary.Identity.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services.Impl
{
    /// <summary>
    /// 注册配置服务
    /// </summary>
    public class RegisterConfigService : IRegisterConfigService, IScoped
    {
        private readonly IRepository<RegisterConfigSet> _registerConfigSetRepository;
        private readonly IRepository<UserRegisterProperty> _registerPropertyRepository;

        public RegisterConfigService(IRepository<RegisterConfigSet> registerConfigSetRepository
            , IRepository<UserRegisterProperty> registerPropertyRepository)
        {
            _registerConfigSetRepository = registerConfigSetRepository;
            _registerPropertyRepository = registerPropertyRepository;
        }

        /// <summary>
        /// 获取注册详情
        /// </summary>
        /// <returns></returns>
        public async Task<RegisterConfigSetDto> GetDetailInfo()
        {
            var configSet = await _registerConfigSetRepository.FirstOrDefaultAsync(x => !x.DeleteFlag);
            var configDto = configSet.Adapt<RegisterConfigSetDto>();
            var configProperties = await _registerPropertyRepository.Where(x => !x.DeleteFlag).OrderBy(x => x.Id).ProjectToType<RegisterPropertyDto>().ToListAsync();
            configDto.Properties = configProperties;
            return configDto;
        }

        /// <summary>
        /// 保存注册配置
        /// </summary>
        /// <param name="configSet"></param>
        /// <returns></returns>
        public async Task<bool> Save(RegisterConfigSetDto configSet)
        {
            var configSetEntity = await _registerConfigSetRepository.FirstOrDefaultAsync(x => !x.DeleteFlag);
            if (configSetEntity == null)
            {
                throw Oops.Oh("未找到注册配置数据");
            }
            configSetEntity = configSet.Adapt(configSetEntity);
            var existProperties = await _registerPropertyRepository.Where(x => !x.DeleteFlag).OrderBy(x => x.Id).ToListAsync();
            existProperties.ForEach(x =>
            {
                var mapProperty = configSet.Properties.FirstOrDefault(s => s.PropertyCode == x.PropertyCode);
                x.IsCheck = mapProperty.IsCheck;
            });
            await _registerConfigSetRepository.UpdateAsync(configSetEntity);
            await _registerPropertyRepository.UpdateAsync(existProperties);
            return true;
        }
    }

}
