/*********************************************************
* 名    称：IDataApproveService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：数据审批服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using SmartLibrary.User.Application.Dtos.DataApprove;
using SmartLibrary.User.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 数据审批服务
    /// </summary>
    public interface IDataApproveService : IScoped
    {
        #region Property
        /// <summary>
        /// 创建属性变更记录
        /// </summary>
        /// <param name="changeLogDto"></param>
        /// <returns></returns>
        public Task<Guid> CreatePropertyChangeLog(PropertyChangeLogDto changeLogDto);
        /// <summary>
        /// 查询属性变更记录
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<PropertyChangeListDto>> QueryPropertyChangeLogTableData(PropertyChangeLogTableQuery queryFilter);

        /// <summary>
        /// 查询属性变更详情记录
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        public Task<List<PropertyChangeLogDetailItemDto>> GetPropertyChangeLogDetailItems(Guid logId);

        /// <summary>
        /// 查询属性变更详情记录
        /// </summary>
        /// <param name="logIds"></param>
        /// <returns></returns>
        public Task<List<PropertyChangeLogDetailItemDto>> GetPropertyGroupChangeLogDetailItems(List<Guid> logIds);
        /// <summary>
        /// 审批属性变更日志
        /// </summary>
        /// <param name="approvePropertyChange"></param>
        /// <returns></returns>
        public Task<bool> ApprovePropertyChange(ApproveLogChangeInput approvePropertyChange);
        #endregion

        #region User
        /// <summary>
        /// 创建员工属性变更记录
        /// </summary>
        /// <param name="changeLogDto"></param>
        /// <returns></returns>
        public Task<Guid> CreateUserChangeLog(UserChangeLogDto changeLogDto);
        /// <summary>
        /// 查询用户变更记录表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<UserChangeListDto>> QueryUserChangeLogTableData(UserChangeLogTableQuery queryFilter);
        /// <summary>
        /// 审批用户变更日志
        /// </summary>
        /// <param name="approveUserChange"></param>
        /// <returns></returns>
        public Task<bool> ApproveUserChange(ApproveLogChangeInput approveUserChange);
        /// <summary>
        /// 获取用户变更详情
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        public Task<UserChangeLogDetailInfoDto> GetUserChangeLogDetailInfo(Guid logId);
        /// <summary>
        /// 用户单个变更日志某用户数据变更详情
        /// </summary>
        /// <param name="logId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<UserChangeLogDetailItemDto>> GetUserChangeLogDetailItems(Guid logId, Guid userId);
        #endregion

        #region Register
        /// <summary>
        /// 用户注册列表查询
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<UserRegisterListItemDto>> QueryUserRegisterTableData(UserRegisterTableQuery queryFilter);
        /// <summary>
        /// 用户注册审批
        /// </summary>
        /// <param name="approveChange"></param>
        /// <returns></returns>
        public Task<bool> ApproveUserRegister(ApproveLogChangeInput approveChange);
        /// <summary>
        /// 获取用户注册详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<UserRegisterDetailDto> GetUserRegisterDetailInfo(Guid id);
        #endregion

        #region CardClaim
        /// <summary>
        /// 查询读者领卡数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<CardClaimListItemDto>> QueryCardClaimTableData(CardClaimTableQuery queryFilter);
        /// <summary>
        /// 读者领卡审批
        /// </summary>
        /// <param name="approveChange"></param>
        /// <returns></returns>
        public Task<bool> ApproveCardClaim(ApproveLogChangeInput approveChange);
        /// <summary>
        /// 取消读者领卡审核
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        public Task<bool> CancelCardConfirm(Guid claimId);
        /// <summary>
        /// 删除读者领卡记录
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        public Task<bool> DeleteCardClaim(Guid claimId);
        /// <summary>
        /// 读者卡领取
        /// </summary>
        /// <param name="cardClaim"></param>
        /// <returns></returns>
        public Task<int> ClaimReaderCard(CardCliamEditDto cardClaim);
        /// <summary>
        /// 获取读者领卡详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<CardClaimDetailDto> GetCardClaimDetailInfo(Guid id);
        #endregion
    }
}
