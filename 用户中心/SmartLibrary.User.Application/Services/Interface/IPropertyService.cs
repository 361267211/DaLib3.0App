/*********************************************************
* 名    称：IPropertyService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using SmartLibrary.User.Application.Dtos.Property;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 属性服务
    /// </summary>
    public interface IPropertyService : IScoped
    {
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <returns></returns>
        public Task<List<PropertyListItemDto>> QueryPropertyList();

        /// <summary>
        /// 获取属性对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<PropertyDto> GetByID(Guid id);
        /// <summary>
        /// 输入数据预检查
        /// </summary>
        /// <param name="propertyDto"></param>
        /// <returns></returns>
        public Task PropertyPrecheck(PropertyDto propertyDto);
        /// <summary>
        /// 创建属性
        /// </summary>
        /// <param name="propertyDto"></param>
        /// <returns></returns>
        public Task<Guid> CreateProperty(PropertyDto propertyDto);
        /// <summary>
        /// 编辑属性
        /// </summary>
        /// <param name="propertyDto"></param>
        /// <returns></returns>
        public Task<Guid> UpdateProperty(PropertyDto propertyDto);
        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteProperty(Guid id);
        /// <summary>
        /// 设置是否可检索
        /// </summary>
        /// <param name="searchSet"></param>
        /// <returns></returns>
        public Task<bool> SetCanSearch(PropertySearchSetDto searchSet);
        /// <summary>
        /// 设置是否在列表显示
        /// </summary>
        /// <param name="showSet"></param>
        /// <returns></returns>
        public Task<bool> SetShowOnTable(PropertyShowSetDto showSet);

    }
}
