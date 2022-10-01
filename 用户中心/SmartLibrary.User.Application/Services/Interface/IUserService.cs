/*********************************************************
* 名    称：IUserService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 获取读者高级查询条件
        /// </summary>
        /// <returns></returns>
        Task<List<SearchPropertyDto>> GetCanSearchPropertyList();
        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        Task<Tuple<List<UserProperty>, List<UserProperty>>> UserEditValidate(UserDto userData, bool isAdd);
        /// <summary>
        /// 映射编码名称
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        Task<UserDto> MapPropertyName(UserDto userData);
        /// <summary>
        /// 映射编码名称
        /// </summary>
        /// <param name="staffData"></param>
        /// <returns></returns>
        Task<StaffDepartEditInput> MapStaffPropertyName(StaffDepartEditInput staffData);
        /// <summary>
        /// 映射编码名称
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        Task<UserBatchEditInput> MapBatchPropertyName(UserBatchEditInput userData);
        /// <summary>
        /// 获取用户初始数据
        /// </summary>
        /// <returns></returns>
        public Task<object> GetUserInitData();
        /// <summary>
        /// 获取读者初始数据
        /// </summary>
        /// <returns></returns>
        public Task<object> GetReaderInitData();
        /// <summary>
        /// 获取属性组可选项
        /// </summary>
        /// <returns></returns>
        public Task<List<PropertyGroupSelectDto>> GetGroupSelectItem();
        /// <summary>
        /// 获取馆员初始数据
        /// </summary>
        /// <returns></returns>
        public Task<object> GetStaffInitData();
        /// <summary>
        /// 用户列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<UserListItemDto>> QueryTableData(UserTableQuery queryFilter);
        /// <summary>
        /// 基础用户列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<SimpleUserListItemDto>> QuerySimpleInfoTableData(UserTableQuery queryFilter);

        /// <summary>
        /// 通过关键字基础用户列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<SimpleUserListItemDto>> QuerySimpleInfoByKeyword(UserTableQuery queryFilter);
        /// <summary>
        /// 通过用户Id获取用户信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public Task<List<SimpleUserListItemDto>> QuerySimpleInfoListByIds(List<Guid> userIds);
        /// <summary>
        /// 通过用户UserKey获取用户信息
        /// </summary>
        /// <param name="userKeys"></param>
        /// <returns></returns>
        public Task<List<SimpleUserListItemDto>> QuerySimpleInfoListByUserKeys(List<string> userKeys);
        /// <summary>
        /// 馆员列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<StaffListItemDto>> QueryStaffTableData(StaffTableQuery queryFilter);
        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<UserDetailOutput> GetByID(Guid userId);
        /// <summary>
        /// 通过UserKey获取用户详情
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public Task<UserDetailOutput> GetByUserKey(string userKey);
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public Task<Guid> Create(UserDto userData);
        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public Task<Guid> Update(UserDto userData);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<bool> Delete(Guid userId);
        /// <summary>
        /// 批量修改员工信息
        /// </summary>
        /// <param name="batchEditData"></param>
        /// <returns></returns>
        public Task<bool> BatchUpdate(UserBatchEditDto batchEditData);
        /// <summary>
        /// 批量设置用户为馆员
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public Task<bool> BatchSetUserAsStaff(List<Guid> userIds);
        /// <summary>
        /// 批量设置馆员为用户
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public Task<bool> BatchSetUserAsReader(List<Guid> userIds);
        /// <summary>
        /// 批量设置馆员部门
        /// </summary>
        /// <param name="staffDepartSetData"></param>
        /// <returns></returns>
        public Task<bool> BatchSetDepartment(StaffDepartSetDto staffDepartSetData);
        /// <summary>
        /// 编辑读者信息
        /// </summary>
        /// <param name="readerData"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<bool> UpdateReaderInfo(ReaderEditDto readerData, Guid userId);
        /// <summary>
        /// 通过卡账号密码登录
        /// </summary>
        /// <param name="accountInfo"></param>
        /// <returns></returns>
        public Task<LoginResultDto> LoginByAccountPwd(AccountInfoDto accountInfo);
        /// <summary>
        /// 通过身份证号密码登录
        /// </summary>
        /// <param name="idCardInfo"></param>
        /// <returns></returns>
        public Task<LoginResultDto> LoginByIdCardPwd(IdCardInfoDto idCardInfo);
        /// <summary>
        /// 通过手机号码登录
        /// </summary>
        /// <param name="phoneInfo"></param>
        /// <returns></returns>
        public Task<LoginResultDto> LoginByPhone(PhoneInfoDto phoneInfo);
        /// <summary>
        /// 通过卡号查询用户卡
        /// </summary>
        /// <param name="cardSearch"></param>
        /// <returns></returns>
        public Task<CardSearchResultDto> SearchCardByNo(CardSearchDto cardSearch);
        /// <summary>
        /// 变更卡密码
        /// </summary>
        /// <param name="CardToken"></param>
        /// <returns></returns>
        public Task<SimpleResultDto> ChangeCardPwd(CardTokenInfoDto CardToken);
        /// <summary>
        /// 变更卡密码，需要验证旧密码
        /// </summary>
        /// <param name="cardChangePwd"></param>
        /// <returns></returns>
        Task<SimpleResultDto> ChangeCardPwdEx(CardChangePwdDto cardChangePwd);
        /// <summary>
        /// 根据userkey获取卡列表
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<List<CardSingleInfo>> GetCardListByUserKey(string userKey);
        /// <summary>
        /// 检查手机号是否已存在
        /// </summary>
        /// <param name="phoneInfo"></param>
        /// <returns></returns>
        public Task<SimpleResultDto> CheckUniquePhone(PhoneInfoDto phoneInfo);
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public Task<RegisterResultDto> RegisterUser(RegisterUserInfoDto userInfo);
        /// <summary>
        /// 通过用户组Id获取用户信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<SimpleUserListItemDto>> QuerySimpleUserByGroupIds(SimpleUserTableQuery queryFilter);
        /// <summary>
        /// 通过用户类型获取用户信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<SimpleUserListItemDto>> QuerySimpleUserByUserTypes(SimpleUserTableQuery queryFilter);
    }
}
