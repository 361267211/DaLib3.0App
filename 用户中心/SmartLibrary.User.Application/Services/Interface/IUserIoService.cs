/*********************************************************
* 名    称：IUserIoService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户数据导入服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 用户数据导入导出
    /// </summary>
    public interface IUserIoService
    {
        /// <summary>
        /// 验证数据有效性
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        List<UserImportTempDataDto> ValidateData(List<UserImportTempDataDto> userData);
        /// <summary>
        /// 验证编码
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        List<UserImportTempDataDto> ValidateUserCode(List<UserImportTempDataDto> userData);

        /// <summary>
        /// 映射编码名称
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        Task<List<UserImportTempDataDto>> MapPropertyCode(List<UserImportTempDataDto> userData);
        /// <summary>
        /// 用户临时数据导入
        /// </summary>
        /// <param name="importTempData"></param>
        /// <returns></returns>
        public Task<bool> ImportUserTempData(List<UserImportTempData> importTempData);

        /// <summary>
        /// 校验导入的临时数据
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public Task<bool> CheckUserTempData(Guid batchId);
        /// <summary>
        /// 查询临时导入用户数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<UserImportTempData>> QueryImportTempUserData(UserTempDataTableQuery queryFilter);
        /// <summary>
        /// 确认数据导入
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public Task<UserImportResultDto> ImportUserConfirm(Guid batchId);
        /// <summary>
        /// 查询待导出数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<ExportUserListItemDto>> QueryExportUserTableData(UserExportFilter queryFilter);
        /// <summary>
        /// 查询待导出数据简要信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<UserExportBriefDto> GetExportUserDataBriefInfo(UserExportFilter queryFilter);
        /// <summary>
        /// 查询待导出数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<ExportCardListItemDto>> QueryExportCardTableData(CardExportFilter queryFilter);
        /// <summary>
        /// 查询待导出数据简要信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<CardExportBriefDto> GetExportCardDataBriefInfo(CardExportFilter queryFilter);
        /// <summary>
        /// 获取需要合并的读者卡信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<MergeUserDto>> GetMergeInfo(Guid userId);
        /// <summary>
        /// 获取多用户合并信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public Task<List<MergeUserDto>> GetMergeInfo(List<Guid> userIds);
        /// <summary>
        /// 合并读者信息
        /// </summary>
        /// <param name="mergeInfo"></param>
        /// <returns></returns>
        public Task<Guid> MergeUserInfo(MergeUserInfo mergeInfo);
    }
}
