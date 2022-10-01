/*********************************************************
* 名    称：IPropertyGroupService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：2021
* 描    述：属性组服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 属性组服务
    /// </summary>
    public interface IPropertyGroupService : IScoped
    {
        #region 属性组
        /// <summary>
        /// 获取属性组列表
        /// </summary>
        /// <returns></returns>
        Task<List<PropertyGroupListItemDto>> QueryListData();
        /// <summary>
        /// 获取属性组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<PropertyGroupListItemDto> GetGroupById(Guid groupId);
        #endregion

        #region 属性组选项
        /// <summary>
        /// 获取属性组可选项列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<PropertyGroupDto> QueryGroupItemList(Guid groupId);
        /// <summary>
        /// 查询属性组选项
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<PropertyGroupItemDto>> QueryGroupItemListByKeyword(PropertyGroupItemTableQuery queryFilter);
        /// <summary>
        /// 查询所有属性组选项
        /// </summary>
        /// <returns></returns>
        Task<List<PropertyGroupSelectDto>> GetPropertyGroupSelect();
        /// <summary>
        /// 获取选项详情
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task<PropertyGroupItemDto> GetGroupItemById(Guid itemId);
        /// <summary>
        /// 创建属性组选项
        /// </summary>
        /// <param name="groupItem"></param>
        /// <returns></returns>
        Task<Guid> CreateGroupItem(PropertyGroupItemDto groupItem);

        /// <summary>
        /// 修改属性组选项
        /// </summary>
        /// <param name="groupItem"></param>
        /// <returns></returns>
        Task<Guid> UpdateGroupItem(PropertyGroupItemDto groupItem);

        /// <summary>
        /// 删除属性组选项
        /// </summary>
        /// <param name="groupItemId"></param>
        /// <returns></returns>
        Task<bool> DeleteGroupItem(Guid groupItemId);
        /// <summary>
        /// 获取映射编码属性选项
        /// </summary>
        /// <returns></returns>
        Task<List<PropertyGroupDto>> GetMapCodePropertyGroupList();
        #endregion

    }
}
