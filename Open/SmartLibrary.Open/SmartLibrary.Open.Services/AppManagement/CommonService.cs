using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using SmartLibrary.Open.Common.Dtos;
using SmartLibrary.Open.Common.Extensions;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public class CommonService : ICommonService, IScoped
    {
        private readonly IRepository<AppDictioanry> _appDictRepository;
        private readonly IDistributedIDGenerator _idGenerator;

        public CommonService(IRepository<AppDictioanry> appDictRepository
            , IDistributedIDGenerator idGenerator)
        {
            _appDictRepository = appDictRepository;
            _idGenerator = idGenerator;
        }
        /// <summary>
        /// 创建服务包
        /// </summary>
        /// <param name="dictDto"></param>
        /// <returns></returns>
        public async Task<Guid> CreateServicePack(AppDictionaryDto dictDto)
        {
            if (_appDictRepository.Any(x => !x.DeleteFlag && x.DictType == EnumSysDictType.ServicePack.ToString() && x.Value == dictDto.Value))
            {
                throw Oops.Oh("字典值重复");
            }
            var dictEntity = new AppDictioanry
            {
                Id = _idGenerator.CreateGuid(),
                DictType = EnumSysDictType.ServicePack.ToString(),
                Name = dictDto.Key,
                Value = dictDto.Value,
                Desc = dictDto.Desc
            };
            await _appDictRepository.InsertAsync(dictEntity);
            return dictEntity.Id;
        }

        /// <summary>
        /// 创建服务类型
        /// </summary>
        /// <param name="dictDto"></param>
        /// <returns></returns>
        public async Task<Guid> CreateServiceType(AppDictionaryDto dictDto)
        {
            if (_appDictRepository.Any(x => !x.DeleteFlag && x.DictType == EnumSysDictType.ServiceType.ToString() && x.Value == dictDto.Value))
            {
                throw Oops.Oh("字典值重复");
            }
            var dictEntity = new AppDictioanry
            {
                Id = _idGenerator.CreateGuid(),
                DictType = EnumSysDictType.ServiceType.ToString(),
                Name = dictDto.Key,
                Value = dictDto.Value,
                Desc = dictDto.Desc
            };
            await _appDictRepository.InsertAsync(dictEntity);
            return dictEntity.Id;
        }

        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSysDict(Guid dictId)
        {
            var sysDict = await _appDictRepository.FirstOrDefaultAsync(x => x.Id == dictId);
            if (sysDict == null)
            {
                throw Oops.Oh("未找到字典数据");
            }
            sysDict.DeleteFlag = true;
            sysDict.UpdatedTime = DateTime.Now;
            await _appDictRepository.UpdateAsync(sysDict);
            return true;
        }
        /// <summary>
        /// 获取字典信息
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        public async Task<SysDictModel> GetSysDictInfo(Guid dictId)
        {
            var sysDict = await _appDictRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == dictId);
            if (sysDict == null)
            {
                throw Oops.Oh("未找到字典数据");
            }
            var sysDictModel = new SysDictModel
            {
                Key = sysDict.Name,
                Value = sysDict.Value
            };
            return sysDictModel;
        }

        /// <summary>
        /// 更新字典值
        /// </summary>
        /// <param name="dictDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSysDict(AppDictionaryDto dictDto)
        {

            var sysDict = await _appDictRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == dictDto.ID);
            if (sysDict == null)
            {
                throw Oops.Oh("未找到字典数据");
            }
            if (_appDictRepository.Any(x => !x.DeleteFlag && x.DictType == sysDict.DictType && x.Value == dictDto.Value && x.Id != dictDto.ID))
            {
                throw Oops.Oh("字典值重复");
            }
            sysDict.Name = sysDict.Name;
            sysDict.Value = sysDict.Value;
            sysDict.Desc = sysDict.Desc;
            sysDict.UpdatedTime = DateTime.Now;
            await _appDictRepository.UpdateAsync(sysDict);
            return true;
        }
    }
}
