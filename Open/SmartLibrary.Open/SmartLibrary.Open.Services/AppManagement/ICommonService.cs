using SmartLibrary.Open.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public interface ICommonService
    {
        /// <summary>
        /// 添加服务类型
        /// </summary>
        /// <param name="dictDto"></param>
        /// <returns></returns>
        Task<Guid> CreateServiceType(AppDictionaryDto dictDto);
        /// <summary>
        /// 添加服务包
        /// </summary>
        /// <param name="dictDto"></param>
        /// <returns></returns>
        Task<Guid> CreateServicePack(AppDictionaryDto dictDto);
        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        Task<SysDictModel> GetSysDictInfo(Guid dictId);
        /// <summary>
        /// 更新字典值
        /// </summary>
        /// <param name="dictDto"></param>
        /// <returns></returns>
        Task<bool> UpdateSysDict(AppDictionaryDto dictDto);
        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        Task<bool> DeleteSysDict(Guid dictId);
    }
}
