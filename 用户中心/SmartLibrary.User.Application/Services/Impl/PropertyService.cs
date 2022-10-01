/*********************************************************
* 名    称：PropertyService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性管理服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos.Property;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 属性服务
    /// </summary>
    public class PropertyService : IPropertyService
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<Property> _propertyRepository;
        private readonly IRepository<PropertyGroup> _propertyGroupRepository;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="idGenerator"></param>
        /// <param name="propertyRepository"></param>
        /// <param name="propertyGroupRepository"></param>
        public PropertyService(
             IDistributedIDGenerator idGenerator
            , IRepository<Property> propertyRepository
            , IRepository<PropertyGroup> propertyGroupRepository)
        {
            _idGenerator = idGenerator;
            _propertyRepository = propertyRepository;
            _propertyGroupRepository = propertyGroupRepository;
        }

        /// <summary>
        /// 获取属性列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyListItemDto>> QueryPropertyList()
        {
            var propertyQuery = _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag).OrderBy(x => x.CreateTime).ProjectToType<PropertyListItemDto>();
            var propertyList = await propertyQuery.ToListAsync();
            var sortNo = 1;
            propertyList.ForEach(x =>
            {
                x.Sort = sortNo++;
            });
            return propertyList;
        }

        /// <summary>
        /// 获取属性对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PropertyDto> GetByID(Guid id)
        {
            var propertyEntity = await _propertyRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (propertyEntity == null)
            {
                throw Oops.Oh("未找到属性对象").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (propertyEntity.DeleteFlag)
            {
                throw Oops.Oh("属性对象已删除").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var propertyDto = propertyEntity.Adapt<PropertyDto>();
            return propertyDto;
        }

        public async Task PropertyPrecheck(PropertyDto propertyDto)
        {
            if (!propertyDto.ForReader && !propertyDto.ForCard)
            {
                throw Oops.Oh("描述对象不能为空").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var code = propertyDto.Code;
            if (string.IsNullOrWhiteSpace(code))
            {
                throw Oops.Oh("属性标识不能为空").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (await _propertyRepository.AnyAsync(x => x.Code.ToLower() == code.ToLower() && x.Id != propertyDto.ID))
            {
                throw Oops.Oh($"属性标识:{code}已被使用").StatusCode(Consts.Consts.ExceptionStatus);
            }
        }

        /// <summary>
        /// 创建属性
        /// </summary>
        /// <param name="propertyDto"></param>
        /// <returns></returns>
        public async Task<Guid> CreateProperty(PropertyDto propertyDto)
        {
            //数据校验
            await this.PropertyPrecheck(propertyDto);
            propertyDto.ID = _idGenerator.CreateGuid(propertyDto.ID);
            var propertyEntity = propertyDto.Adapt<Property>();
            if (propertyEntity.Type == (int)EnumPropertyType.属性组)
            {
                var propertyGroup = new PropertyGroup
                {

                    Id = _idGenerator.CreateGuid(),
                    Name = propertyDto.Name,
                    Code = propertyDto.Code,
                    Type = (int)EnumGroupType.扩展,
                    SysBuildIn = false
                };
                await _propertyGroupRepository.InsertAsync(propertyGroup);
                propertyEntity.PropertyGroupID = propertyGroup.Id;
            }
            var entityEntry = await _propertyRepository.InsertAsync(propertyEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 修改属性
        /// </summary>
        /// <param name="propertyDto"></param>
        /// <returns></returns>
        public async Task<Guid> UpdateProperty(PropertyDto propertyDto)
        {
            //数据校验
            await this.PropertyPrecheck(propertyDto);
            var propertyEntity = await _propertyRepository.FirstOrDefaultAsync(x => x.Id == propertyDto.ID);
            if (propertyEntity == null)
            {
                throw Oops.Oh("未找到属性对象");
            }
            if (propertyEntity.DeleteFlag)
            {
                throw Oops.Oh("属性已删除，不能修改");
            }
            if (propertyEntity.SysBuildIn)
            {
                throw Oops.Oh("不能修改系统内置属性");
            }
            //if (propertyEntity.ApproveStatus == (int)EnumPropertyApproveStatus.待审批)
            //{
            //    throw Oops.Oh("不能修改待审批属性").StatusCode(Consts.Consts.ExceptionStatus);
            //}
            propertyEntity.Name = propertyDto.Name;
            propertyEntity.Intro = propertyDto.Intro;
            propertyEntity.Required = propertyDto.Required;
            propertyEntity.Unique = propertyDto.Unique;
            propertyEntity.ShowOnTable = propertyDto.ShowOnTable;
            propertyEntity.CanSearch = propertyDto.CanSearch;
            propertyEntity.ForReader = propertyDto.ForReader;
            propertyEntity.ForCard = propertyDto.ForCard;
            propertyEntity.ApproveStatus = propertyDto.ApproveStatus;
            var entityEntry = await _propertyRepository.UpdateAsync(propertyEntity);
            return entityEntry.Entity.Id;
        }


        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProperty(Guid id)
        {
            var propertyEntity = await _propertyRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (propertyEntity == null)
            {
                throw Oops.Oh("未找到属性对象").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (propertyEntity.DeleteFlag)
            {
                return true;
            }
            if (propertyEntity.SysBuildIn)
            {
                throw Oops.Oh("不能删除系统内置属性");
            }

            if (propertyEntity.Type == (int)EnumPropertyType.属性组)
            {
                var groupEntity = await _propertyGroupRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Code == propertyEntity.Code);
                if (groupEntity != null)
                {
                    groupEntity.DeleteFlag = true;
                    await _propertyGroupRepository.UpdateAsync(groupEntity);
                }
            }
            propertyEntity.DeleteFlag = true;
            propertyEntity.UpdateTime = DateTime.Now;
            await _propertyRepository.UpdateAsync(propertyEntity);
            return true;
        }
        /// <summary>
        /// 设置属性是否可检索
        /// </summary>
        /// <param name="searchSet"></param>
        /// <returns></returns>
        public async Task<bool> SetCanSearch(PropertySearchSetDto searchSet)
        {
            var propertyEntity = await _propertyRepository.FirstOrDefaultAsync(x => x.Id == searchSet.ID);
            if (propertyEntity == null)
            {
                throw Oops.Oh("未找到属性对象");
            }
            if (propertyEntity.DeleteFlag)
            {
                throw Oops.Oh("属性对象已删除");
            }
            if (!propertyEntity.SysBuildIn)
            {
                throw Oops.Oh("非系统属性，不支持检索");
            }
            propertyEntity.CanSearch = searchSet.CanSearch;
            propertyEntity.UpdateTime = DateTime.Now;
            await _propertyRepository.UpdateAsync(propertyEntity);
            return true;
        }

        /// <summary>
        /// 设置属性是否在列表显示
        /// </summary>
        /// <param name="showSet"></param>
        /// <returns></returns>
        public async Task<bool> SetShowOnTable(PropertyShowSetDto showSet)
        {
            var propertyEntity = await _propertyRepository.FirstOrDefaultAsync(x => x.Id == showSet.ID);
            if (propertyEntity == null)
            {
                throw Oops.Oh("未找到属性对象");
            }
            if (propertyEntity.DeleteFlag)
            {
                throw Oops.Oh("属性对象已删除");
            }
            propertyEntity.ShowOnTable = showSet.ShowOnTable;
            propertyEntity.UpdateTime = DateTime.Now;
            await _propertyRepository.UpdateAsync(propertyEntity);
            return true;
        }
    }
}
