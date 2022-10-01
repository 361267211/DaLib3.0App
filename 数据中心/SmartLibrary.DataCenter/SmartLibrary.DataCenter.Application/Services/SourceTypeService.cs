/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.LinqBuilder;
using Mapster;
using SmartLibrary.DataCenter.EntityFramework.Core.Dto;
using SmartLibrary.DataCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application.Services
{
    public class SourceTypeService : IScoped, ISourceTypeService
    {
        private readonly IRepository<SourceType> _sourceTypeRepository;
        public SourceTypeService(
            IRepository<SourceType> sourceTypeRepository

            )
        {
            _sourceTypeRepository = sourceTypeRepository;
        }

        /// <summary>
        /// 获取资源类型字典
        /// </summary>
        /// <returns></returns>
        public async Task<List<SourceTypeDto>> GetSourceTypes()
        {
            var lamda = LinqExpression.Create<SourceType>(e => !e.DeleteFlag);

            var list= _sourceTypeRepository.AsQueryable(lamda).ProjectToType<SourceTypeDto>().ToList();

            return _sourceTypeRepository.AsQueryable(lamda).ProjectToType<SourceTypeDto>().ToList();
        }

        /// <summary>
        /// 添加资源类型字典
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<SourceTypeDto> AddSourceType(SourceTypeDto sourceType)
        {

            var sourceTypeEntity = sourceType.Adapt<SourceType>();
            sourceTypeEntity.CreatedTime = DateTimeOffset.UtcNow;
            sourceTypeEntity.Id = Guid.NewGuid();
            sourceTypeEntity.Code = _sourceTypeRepository.Where(e => e.CreateType == 1 && !e.DeleteFlag).Max(e => e.Code)+1;

           var result= await _sourceTypeRepository.InsertNowAsync(sourceTypeEntity);
            return result.Entity.Adapt<SourceTypeDto>();
        }
    }
}
