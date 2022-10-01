/*********************************************************
* 名    称：DataApproveAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：数据审批Api
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 数据审批服务
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class DataApproveAppService : BaseAppService
    {
        private readonly IDataApproveService _dataApproveService;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dataApproveService"></param>
        public DataApproveAppService(IDataApproveService dataApproveService)
        {
            _dataApproveService = dataApproveService;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await Task.FromResult(new
            {
                LogPropertyType = EnumHelper.GetEnumDictionaryItems(typeof(EnumLogPropertyType)),
                PropertyLogStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumPropertyLogStatus)),
                PropertyLogType = EnumHelper.GetEnumDictionaryItems(typeof(EnumPropertyLogType)),
                CardStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumCardStatus)),
                UserRegisterStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserRegisterStatus)),
                CardClaimStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumCardClaimStatus)),
            });
        }

        /// <summary>
        /// 获取属性变更日志记录
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ApprovePropertyChangeListOutput>> QueryPropertyChangeLogTableData([FromQuery] PropertyChangeLogTableQuery queryFilter)
        {
            var changeLogPagedList = await _dataApproveService.QueryPropertyChangeLogTableData(queryFilter);
            var targetPagedList = changeLogPagedList.Adapt<PagedList<ApprovePropertyChangeListOutput>>();
            return targetPagedList;
        }

        /// <summary>
        /// 获取属性变更详情
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        public async Task<List<PropertyChangeLogDetailItem>> GetPropertyChangeLogDetailItems(Guid logId)
        {
            var changeDetails = await _dataApproveService.GetPropertyChangeLogDetailItems(logId);
            var targetDetails = changeDetails.Adapt<List<PropertyChangeLogDetailItem>>();
            return targetDetails;
        }
        /// <summary>
        /// 审批变更日志
        /// </summary>
        /// <param name="approvePropertyChange"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<bool> ApprovePropertyChange([FromBody] ApproveLogChangeInput approvePropertyChange)
        {
            var result = await _dataApproveService.ApprovePropertyChange(approvePropertyChange);
            return result;
        }

        /// <summary>
        /// 获取读者审批变更日志记录
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ApproveUserChangeListOutput>> QueryUserChangeLogTableData([FromQuery] UserChangeLogTableQuery queryFilter)
        {
            var changeLogPagedList = await _dataApproveService.QueryUserChangeLogTableData(queryFilter);
            var targetPagedList = changeLogPagedList.Adapt<PagedList<ApproveUserChangeListOutput>>();
            return targetPagedList;
        }

        /// <summary>
        /// 审批读者修改日志
        /// </summary>
        /// <param name="approveUserChange"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<bool> ApproveUserChange([FromBody] ApproveLogChangeInput approveUserChange)
        {
            var result = await _dataApproveService.ApproveUserChange(approveUserChange);
            return result;
        }

        /// <summary>
        /// 获取读者信息变更详情
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        public async Task<UserChangeLogDetailInfo> GetUserChangeLogDetailInfo(Guid logId)
        {
            var detailInfo = await _dataApproveService.GetUserChangeLogDetailInfo(logId);
            var targetDetailInfo = detailInfo.Adapt<UserChangeLogDetailInfo>();
            return targetDetailInfo;
        }

        /// <summary>
        /// 获取读者变更详情
        /// </summary>
        /// <param name="logId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserChangeLogDetailItem>> GetUserChangeLogDetailItems(Guid logId, Guid userId)
        {
            var detailItems = await _dataApproveService.GetUserChangeLogDetailItems(logId, userId);
            var targetItems = detailItems.Adapt<List<UserChangeLogDetailItem>>();
            return targetItems;
        }

        /// <summary>
        /// 获取注册审批列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<UserRegisterListItemOutput>> QueryUserRegisterTableData([FromQuery] UserRegisterTableQuery queryFilter)
        {
            var pagedList = await _dataApproveService.QueryUserRegisterTableData(queryFilter);
            var targetPagedList = pagedList.Adapt<PagedList<UserRegisterListItemOutput>>();
            return targetPagedList;
        }

        /// <summary>
        /// 审批用户注册信息
        /// </summary>
        /// <param name="approveRegister"></param>
        /// <returns></returns>
        public async Task<bool> ApproveUserRegister([FromBody] ApproveLogChangeInput approveRegister)
        {
            var result = await _dataApproveService.ApproveUserRegister(approveRegister);
            return result;
        }

        /// <summary>
        /// 获取注册信息详情1103
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserRegisterDetailOutput> GetUserRegisterDetailInfo(Guid id)
        {
            var detailInfo = await _dataApproveService.GetUserRegisterDetailInfo(id);
            var targetInfo = detailInfo.Adapt<UserRegisterDetailOutput>();
            return targetInfo;
        }

        /// <summary>
        /// 获取读者领卡列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<CardClaimListItemOutput>> QueryCardClaimTableData([FromQuery] CardClaimTableQuery queryFilter)
        {
            var pagedList = await _dataApproveService.QueryCardClaimTableData(queryFilter);
            var targetPagedList = pagedList.Adapt<PagedList<CardClaimListItemOutput>>();
            return targetPagedList;
        }

        /// <summary>
        /// 审批读者领卡信息
        /// </summary>
        /// <param name="approveRegister"></param>
        /// <returns></returns>
        public async Task<bool> ApproveCardClaim([FromBody] ApproveLogChangeInput approveRegister)
        {
            var result = await _dataApproveService.ApproveCardClaim(approveRegister);
            return result;
        }
        /// <summary>
        /// 读者领卡详情1103
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CardClaimDetailOutput> GetCardClaimDetailInfo(Guid id)
        {
            var detailInfo = await _dataApproveService.GetCardClaimDetailInfo(id);
            var targetInfo = detailInfo.Adapt<CardClaimDetailOutput>();
            return targetInfo;
        }

    }
}
