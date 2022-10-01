/*********************************************************
* 名    称：IUserGroupService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Dtos.UserGroup;
using SmartLibrary.User.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 用户组服务
    /// </summary>
    public interface IUserGroupService : IScoped
    {
        /// <summary>
        /// 获取初始化数据
        /// </summary>
        /// <returns></returns>
        public Task<object> GetInitData();
        /// <summary>
        /// 查询用户组定义列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<GroupListItemOutput>> QueryTableQuery(GroupTableQuery queryFilter);
        /// <summary>
        /// 通过Id获取用户组数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<GroupInfoDto> GetByID(Guid id);
        /// <summary>
        /// 通过Id获取用户组简要信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<GroupBriefInfoOutput> GetBriefInfoByID(Guid id);

        /// <summary>
        /// 通过导入信息查询用户数据
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public Task<List<SimpleUserListItemDto>> QueryUserListBySearchInfo(List<UserImportSearchDto> searchInfo);

        /// <summary>
        /// 创建用户组
        /// </summary>
        /// <param name="groupEditData"></param>
        /// <returns></returns>
        public Task<Guid> Create(GroupEditDto groupEditData);
        /// <summary>
        /// 修改用户组
        /// </summary>
        /// <param name="groupEditData"></param>
        /// <returns></returns>
        public Task<Guid> Update(GroupEditDto groupEditData);
        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public Task<bool> Delete(Guid groupId);
        /// <summary>
        /// 添加用户到用户组
        /// </summary>
        /// <param name="userGroupAddData"></param>
        /// <returns></returns>
        public Task<bool> AddUserToGroup(UserGroupAddDto userGroupAddData);
        /// <summary>
        /// 从用户组删除用户
        /// </summary>
        /// <param name="userGroupDelData"></param>
        /// <returns></returns>
        public Task<bool> DeleteGroupUsers(UserGroupDelDto userGroupDelData);
    }
}
