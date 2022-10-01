using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.Open.EntityFramework.Core.Entitys;

namespace SmartLibrary.Open.Services.Search
{
    public interface  IOpacTemplateService
    {
        /// <summary>
        /// 创建或者修改opac模板
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Guid> CreateOrModifyOpacTemplateAsync(OpacTemplate model);
        /// <summary>
        /// 根据标识获取对应的模板
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        Task<OpacTemplate> FetchOpacTemplateBySymbolAsync(string symbol);
        /// <summary>
        /// 获取所有可用的模板
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<OpacTemplate>> FetchAllAsync();
    }
}
