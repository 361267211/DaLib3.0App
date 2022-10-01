/*********************************************************
* 名    称：StaffAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：馆员信息管理
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 馆员信息管理
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class StaffAppService : BaseAppService
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IUserService _userService;
        private readonly ICardService _cardService;
        private readonly ISysOrgService _sysOrgService;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="idGenerator"></param>
        /// <param name="cardService"></param>
        /// <param name="sysOrgService"></param>
        public StaffAppService(IDistributedIDGenerator idGenerator,
                               IUserService userService,
                               ICardService cardService,
                               ISysOrgService sysOrgService)
        {
            _userService = userService;
            _idGenerator = idGenerator;
            _cardService = cardService;
            _sysOrgService = sysOrgService;
        }

        /// <summary>
        /// 馆员数据初始化
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await _userService.GetStaffInitData();
        }

        /// <summary>
        /// 获取馆员表格数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<PagedList<StaffListItemOutput>> QueryTableData([FromQuery] StaffTableQuery queryFilter)
        {
            var pageDtoList = await _userService.QueryStaffTableData(queryFilter);
            var targetPageList = pageDtoList.Adapt<PagedList<StaffListItemOutput>>();
            var items = targetPageList.Items as List<StaffListItemOutput>;
            var sysorglist = await _sysOrgService.GetOrgList();
            items.ForEach(c =>
            {
                c.DepartName = sysorglist.Find(x => x.FullPath == c.Depart)?.FullName;
            });
            return targetPageList;
        }

        /// <summary>
        /// 批量设置用户
        /// </summary>
        /// <param name="departEditData">批量设置部门</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<bool> BatchSetDepartment([FromBody] StaffDepartEditInput departEditData)
        {
            departEditData = await _userService.MapStaffPropertyName(departEditData);
            var staffDepartSetDto = departEditData.Adapt<StaffDepartSetDto>();
            var result = await _userService.BatchSetDepartment(staffDepartSetDto);
            return result;
        }

        /// <summary>
        /// 删除馆员身份
        /// </summary>
        /// <param name="staffIds">馆员Id</param>
        /// <returns></returns>
        public async Task<bool> Delete([FromBody] List<Guid> staffIds)
        {
            var result = await _userService.BatchSetUserAsReader(staffIds);
            return result;
        }

        /// <summary>
        /// 添加临时馆员
        /// </summary>
        /// <param name="staffData">馆员信息</param>
        /// <returns></returns>
        [Route("[action]")]
        [UnitOfWork]
        public async Task<Guid> CreateTempStaff([FromBody] TempStaffInput staffData)
        {
            var userData = new UserDto
            {
                ID = _idGenerator.CreateGuid(),
                StudentNo = staffData.StudentNo,
                Name = staffData.Name,
                Unit = staffData.Unit,
                Edu = staffData.Edu,
                Depart = staffData.Depart,
                College = staffData.College,
                CollegeDepart = staffData.CollegeDepart,
                Title = staffData.Title,
                Phone = staffData.Phone,
                IdCard = staffData.IdCard,
                Gender = staffData.Gender,
                Status = staffData.Status,
                IsStaff = true,
                StaffBeginTime = DateTime.Now,
                Type = "",
                TypeName = "",
                SourceFrom = (int)EnumUserSourceFrom.后台新增,
                StaffStatus = (int)EnumStaffStatus.临时,
            };
            userData = await _userService.MapPropertyName(userData);
            var cardData = new CardDto
            {
                ID = _idGenerator.CreateGuid(),
                UserID = userData.ID,
                No = staffData.Account,
                Secret = staffData.Password,
                IssueDate = staffData.IssueDate,
                ExpireDate = staffData.ExpireDate,
                Type = "tempmanager",//需要换成编码
                TypeName = "临时官员卡",
                Usage = (int)EnumCardUsage.临时馆员登陆,
                Status = (int)EnumCardStatus.正常,
                SysBuildIn = true,
                IsPrincipal = true
            };
            cardData = await _cardService.MapPropertyName(cardData);
            var userId = await _userService.Create(userData);
            cardData.UserID = userId;
            await _cardService.Create(cardData);
            return userId;
        }

    }
}
