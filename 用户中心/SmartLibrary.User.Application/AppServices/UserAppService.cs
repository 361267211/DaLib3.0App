/*********************************************************
* 名    称：UserAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：用户管理Api
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SmartLibrary.AppCenter;
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Dtos.DataApprove;
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Services.Consts;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Dtos;
using SmartLibrary.User.Common.Services;
using SmartLibrary.User.Common.Utils;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class UserAppService : BaseAppService
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IBasicConfigService _basicConfigService;
        private readonly IUserService _userService;
        private readonly IDataApproveService _dataApproveService;
        private readonly IPropertyService _propertyService;
        private readonly ICardService _cardService;
        private readonly ILogger<UserAppService> _logger;
        private readonly IUserIoService _userIoService;
        private readonly IGrpcClientResolver _grpcClientResolver;

        public UserAppService(
            IDistributedIDGenerator idGenerator
            , IBasicConfigService basicConfigService
            , IUserService userService
            , IDataApproveService dataApproveService
            , IPropertyService propertyService
            , ICardService cardService
            , ILogger<UserAppService> logger
            , IUserIoService userIoService
            , IGrpcClientResolver grpcClientResolver)
        {
            _idGenerator = idGenerator;
            _basicConfigService = basicConfigService;
            _userService = userService;
            _dataApproveService = dataApproveService;
            _propertyService = propertyService;
            _cardService = cardService;
            _logger = logger;
            _userIoService = userIoService;
            _grpcClientResolver = grpcClientResolver;
        }

        /// <summary>
        /// 用户初始化数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await _userService.GetUserInitData();
        }

        /// <summary>
        /// 获取属性组可选项
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyGroupSelectDto>> GetGroupSelectItem()
        {
            return await _userService.GetGroupSelectItem();
        }

        /// <summary>
        /// 获取用户表格数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<PagedList<UserListItemOutput>> QueryTableData([FromQuery] UserTableQuery queryFilter)
        {
            var pageDtoList = await _userService.QueryTableData(queryFilter);
            var targetPageList = pageDtoList.Adapt<PagedList<UserListItemOutput>>();
            if (CurUserPermission == null || !CurUserPermission.SeeSensitiveInfo)
            {
                targetPageList = AdaptEncoder.SensitiveEncode<UserListItemOutput, SensitiveUserListItemOutput>(targetPageList);
            }
            return targetPageList;
        }

        /// <summary>
        /// 获取基础用户表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SimpleUserListItemOutput>> QueryBasicUserTableData([FromQuery] UserTableQuery queryFilter)
        {
            var pageDtoList = await _userService.QuerySimpleInfoTableData(queryFilter);
            var targetPageList = pageDtoList.Adapt<PagedList<SimpleUserListItemOutput>>();
            if (CurUserPermission == null || !CurUserPermission.SeeSensitiveInfo)
            {
                targetPageList = AdaptEncoder.SensitiveEncode<SimpleUserListItemOutput, SensitiveUserListItemOutput>(targetPageList);
            }
            return targetPageList;
        }

        /// <summary>
        /// 获取用户下拉选择数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SimpleUserListItemOutput>> QueryUserSelectListData([FromQuery] UserTableQuery queryFilter)
        {
            var pageDtoList = await _userService.QuerySimpleInfoByKeyword(queryFilter);
            var targetPageList = pageDtoList.Adapt<PagedList<SimpleUserListItemOutput>>();
            if (CurUserPermission == null || !CurUserPermission.SeeSensitiveInfo)
            {
                targetPageList = AdaptEncoder.SensitiveEncode<SimpleUserListItemOutput, SensitiveUserListItemOutput>(targetPageList);
            }
            return targetPageList;
        }

        #region 一期暂不实现
        /// <summary>
        /// 获取获取积分详情数据
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public Task<UserPointTableQueryResult<UserPointsListItemOutput>> QueryUserPointsTableData([FromQuery] UserPointsTableQuery queryFilter)
        {
            return Task.FromResult(new UserPointTableQueryResult<UserPointsListItemOutput>
            {
                TotalPoints = 2170,
                ConsumePoints = 1000,
                ExpirePoints = 200,
                Items = new List<UserPointsListItemOutput> {
                    new UserPointsListItemOutput { Points = "+5", ChangeTime = DateTime.Parse("2021-07-09 09:50"), EventName = "登录门户网站" },
                    new UserPointsListItemOutput { Points = "+5", ChangeTime = DateTime.Parse("2021-07-09 08:09"), EventName = "登录门户网站" },
                    new UserPointsListItemOutput { Points = "+5", ChangeTime = DateTime.Parse("2021-07-06 08:00"), EventName = "登录门户网站" },
                    new UserPointsListItemOutput { Points = "+5", ChangeTime = DateTime.Parse("2021-07-03 07:45"), EventName = "登录门户网站" },
                }
            });
        }

        /// <summary>
        /// 获取用户借阅明细记录
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public Task<UserBorrowTableQueryResult<UserBorrowListItemOutput>> QueryUserBorrowTableData([FromQuery] UserBorrowTableQuery queryFilter)
        {
            return Task.FromResult(new UserBorrowTableQueryResult<UserBorrowListItemOutput>
            {
                TotalBorrowCount = 56,
                NowBorrowCount = 6,
                Items = new List<UserBorrowListItemOutput> {
                    new UserBorrowListItemOutput { Title = "计算机导论", SearchNo = "No100001",CollectPlace="A区图书馆",RenewApply=1,RenewCount=3,BorrowTime=DateTime.Parse("2021-05-08 08:30"), ShowReturnTime=DateTime.Parse("2021-08-08 08:30") }
                }
            });
        }

        /// <summary>
        /// 获取用户授权应用信息
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public async Task<List<UserAuthAppListItemOutput>> QueryUserAuthAppListData([FromQuery] UserAuthAppTableQuery queryFilter)
        {
            var totalAuthList = new List<UserAuthAppListItemOutput>();
            var userData = await _userService.GetByID(queryFilter.UserID);
            if (userData == null)
            {
                return await Task.FromResult(totalAuthList);
            }
            var appCenterGrpcClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            var appAuthListResult = await appCenterGrpcClient.GetUserAppAuthListAsync(new UserAppAuthRequest { UserId = userData.UserKey });
            var userAuthTypeRequest = new UserAppAuthByTypeRequest { UserTypeId = userData.Type };
            userAuthTypeRequest.UserGroupIds.AddRange(userData.GroupIds.Select(x => x.ToString()));
            var backendAuthList = appAuthListResult.UserAppAuthList.Select(x => new UserAuthAppListItemOutput { AppName = x.AppName, Icon = x.Icon, Type = (int)EnumUserAuthAppType.馆员授权 });
            var appFrontAuthListResult = await appCenterGrpcClient.GetUserAppAuthListByTypeAsync(userAuthTypeRequest);
            var frontAuthList = appFrontAuthListResult.UserAppAuthList.Select(x => new UserAuthAppListItemOutput { AppName = x.AppName, Icon = x.Icon, Type = (int)EnumUserAuthAppType.应用授权 });
            totalAuthList.AddRange(backendAuthList);
            totalAuthList.AddRange(frontAuthList);
            return totalAuthList;
        }

        /// <summary>
        /// 获取用户行为日志
        /// </summary>
        /// <param name="queryFilter">查询条件</param>
        /// <returns></returns>
        public Task<PagedList<UserLogListItemOutput>> QueryUserLogTableData([FromQuery] UserLogTableQuery queryFilter)
        {
            return Task.FromResult(new PagedList<UserLogListItemOutput>
            {
                PageIndex = 1,
                PageSize = 50,
                TotalCount = 2,
                TotalPages = 1,
                HasPrevPages = false,
                HasNextPages = false,
                Items = new List<UserLogListItemOutput> {
                    new UserLogListItemOutput { Sort=1,EventTime=DateTime.Parse("2021-01-03 12:30:30"),EventName="登陆",LogFrom="门户系统",LogDesc="登录系统" },
                    new UserLogListItemOutput { Sort=2,EventTime=DateTime.Parse("2020-08-03 12:30:30"),EventName="入馆",LogFrom="导入",LogDesc="门禁刷卡" },
                }

            });
        }
        #endregion

        /// <summary>
        /// 获取用户数据用于编辑展示，不掩饰敏感信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDetailOutput> GetByIdForEdit(Guid userId)
        {
            var targetUser = await _userService.GetByID(userId);
            return targetUser;
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [Route("{userId}")]
        public async Task<UserDetailOutput> GetByID(Guid userId)
        {
            var targetUser = await _userService.GetByID(userId);
            if (CurUserPermission == null || !CurUserPermission.SeeSensitiveInfo)
            {
                targetUser = AdaptEncoder.SensitiveEncode<UserDetailOutput, SensitiveUserDetailOutput>(targetUser);
            }
            return targetUser;
        }

        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <param name="userData">用户数据</param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> Create([FromBody] UserCreateInput userData)
        {

            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.UserInfoConfirm;
            userData.UserData.ID = _idGenerator.CreateGuid(userData.UserData.ID);
            userData.CardData.ID = _idGenerator.CreateGuid(userData.CardData.ID);
            var userDto = userData.UserData.Adapt<UserDto>();
            userDto = await _userService.MapPropertyName(userDto);
            var cardDto = userData.CardData.Adapt<CardDto>();
            cardDto = await _cardService.MapPropertyName(cardDto);
            cardDto.UserID = userDto.ID;
            userDto.Status = (int)EnumUserStatus.正常;
            userDto.SourceFrom = (int)EnumUserSourceFrom.后台新增;
            cardDto.Status = (int)EnumCardStatus.正常;
            cardDto.IsPrincipal = true;
            cardDto.IssueDate = cardDto.IssueDate == null ? DateTime.Now : cardDto.IssueDate;
            cardDto.ExpireDate = cardDto.ExpireDate == null ? cardDto.IssueDate.Value.AddYears(3) : cardDto.ExpireDate;
            var changeLog = await CompareUserDiffAsync(EnumLogDiffType.新增, null, userDto);
            if (needApprove)
            {
                changeLog.Status = (int)EnumUserLogStatus.待审批;
                userDto.Status = (int)EnumUserStatus.未激活;
                cardDto.Status = (int)EnumCardStatus.停用;
            }
            //创建用户
            var userId = await _userService.Create(userDto);
            //创建读者卡
            await _cardService.Create(cardDto);
            //创建审批记录
            await _dataApproveService.CreateUserChangeLog(changeLog);
            return userId;
        }

        /// <summary>
        /// 创建馆员
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost]
        public async Task<Guid> CreateStaff([FromBody] UserCreateInput userData)
        {
            var needApprove = false;//馆员无需审核
            userData.UserData.ID = _idGenerator.CreateGuid(userData.UserData.ID);
            userData.CardData.ID = _idGenerator.CreateGuid(userData.CardData.ID);
            var userDto = userData.UserData.Adapt<UserDto>();
            userDto = await _userService.MapPropertyName(userDto);
            var cardDto = userData.CardData.Adapt<CardDto>();
            cardDto = await _cardService.MapPropertyName(cardDto);
            cardDto.UserID = userDto.ID;
            userDto.Status = (int)EnumUserStatus.正常;
            userDto.SourceFrom = (int)EnumUserSourceFrom.后台新增;
            userDto.IsStaff = true;
            userDto.StaffStatus = (int)EnumStaffStatus.正式;
            userDto.StaffBeginTime = DateTime.Now;
            cardDto.Status = (int)EnumCardStatus.正常;
            cardDto.IsPrincipal = true;
            cardDto.IssueDate = cardDto.IssueDate == null ? DateTime.Now : cardDto.IssueDate;
            cardDto.ExpireDate = cardDto.ExpireDate == null ? cardDto.IssueDate.Value.AddYears(3) : cardDto.ExpireDate;
            var changeLog = await CompareUserDiffAsync(EnumLogDiffType.新增, null, userDto);
            if (needApprove)
            {
                changeLog.Status = (int)EnumUserLogStatus.待审批;
                userDto.Status = (int)EnumUserStatus.未激活;
                cardDto.Status = (int)EnumCardStatus.停用;
            }
            //创建用户
            var userId = await _userService.Create(userDto);
            //创建读者卡
            await _cardService.Create(cardDto);
            //创建审批记录
            await _dataApproveService.CreateUserChangeLog(changeLog);
            return userId;
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="userData">用户数据</param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> Update([FromBody] UserInput userData)
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.UserInfoConfirm;
            var userDto = userData.Adapt<UserDto>();
            userDto = await _userService.MapPropertyName(userDto);
            await _userService.UserEditValidate(userDto, false);
            var preUser = await _userService.GetByID(userData.ID);
            var preUserDto = preUser.Adapt<UserDto>();
            var changeLog = await CompareUserDiffAsync(EnumLogDiffType.修改, preUserDto, userDto);
            if (!changeLog.ItemChangeLogs.Any())
            {
                return userData.ID;
            }
            userDto.Status = (int)EnumUserStatus.正常;
            if (needApprove)
            {
                changeLog.Status = (int)EnumUserLogStatus.待审批;
                await _dataApproveService.CreateUserChangeLog(changeLog);
            }
            else
            {
                await _userService.Update(userDto);
            }
            return userData.ID;
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="userData">用户数据</param>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost]
        [Route("[action]")]
        public async Task<UserUpdateMergeOutput> UpdateWithMerge([FromBody] UserInput userData)
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.UserInfoConfirm;
            var userDto = userData.Adapt<UserDto>();
            userDto = await _userService.MapPropertyName(userDto);
            //读者数据校验
            try
            {
                await _userService.UserEditValidate(userDto, false);
            }
            catch (AppFriendlyException ex)
            {
                //重复情况下，前端返回空ID,代表需要合并
                if (ex.Message.Contains("身份证号已存在"))
                {
                    return new UserUpdateMergeOutput { ID = userData.ID, Success = false, RepeateIdCard = true };
                }
                if (ex.Message.Contains("联系电话已存在"))
                {
                    return new UserUpdateMergeOutput { ID = userData.ID, Success = false, RepeatePhone = true };
                }
                throw Oops.Oh(ex.Message).StatusCode(Consts.ExceptionStatus);
            }

            var preUser = await _userService.GetByID(userData.ID);
            var preUserDto = preUser.Adapt<UserDto>();
            var changeLog = await CompareUserDiffAsync(EnumLogDiffType.修改, preUserDto, userDto);
            if (!changeLog.ItemChangeLogs.Any())
            {
                return new UserUpdateMergeOutput { ID = userData.ID, Success = true };
            }
            userDto.Status = (int)EnumUserStatus.正常;
            if (needApprove)
            {
                changeLog.Status = (int)EnumUserLogStatus.待审批;
                await _dataApproveService.CreateUserChangeLog(changeLog);
            }
            else
            {
                try
                {
                    await _userService.Update(userDto);
                }
                catch (AppFriendlyException ex)
                {
                    //重复情况下，前端返回空ID,代表需要合并
                    if (ex.Message.Contains("身份证号已存在"))
                    {
                        return new UserUpdateMergeOutput { ID = userData.ID, Success = false, RepeateIdCard = true };
                    }
                    if (ex.Message.Contains("联系电话已存在"))
                    {
                        return new UserUpdateMergeOutput { ID = userData.ID, Success = false, RepeatePhone = true };
                    }
                    throw Oops.Oh(ex.Message).StatusCode(Consts.ExceptionStatus);
                }
            }

            return new UserUpdateMergeOutput { ID = userData.ID, Success = true };
        }

        /// <summary>
        /// 删除用户数据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [Route("{userId}")]
        [UnitOfWork]
        public async Task<bool> Delete(Guid userId)
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.UserInfoConfirm;
            var userData = await _userService.GetByID(userId);
            var userDto = userData.Adapt<UserDto>();
            var changeLog = await CompareUserDiffAsync(EnumLogDiffType.删除, userDto, null);
            if (needApprove)
            {
                changeLog.Status = (int)EnumUserLogStatus.待审批;
                //创建变更日志
                await _dataApproveService.CreateUserChangeLog(changeLog);
            }
            else
            {
                //逻辑删除用户
                await _userService.Delete(userId);
            }
            return true;
        }



        /// <summary>
        /// 用户信息批量修改
        /// </summary>
        /// <param name="userData">用户数据</param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        [UnitOfWork]
        public async Task<bool> BatchUpdate([FromBody] UserBatchEditInput userData)
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.UserInfoConfirm;
            userData = await _userService.MapBatchPropertyName(userData);
            var userBatchEditData = userData.Adapt<UserBatchEditDto>();

            var preUserDtos = await _userService.QuerySimpleInfoListByIds(userData.UserIDList);
            var preUserDatas = preUserDtos.Adapt<List<SimpleUserListItemOutput>>();
            var userNames = preUserDatas.OrderBy(x => x.CreateTime).Select(x => x.Name).ToList();
            var forUserProperties = (await _propertyService.QueryPropertyList()).Where(x => x.ForReader).ToList();
            var fieldsName = forUserProperties.Where(x => userData.Fields.Select(f => $"User_{f}".ToLower()).Contains(x.Code.ToLower())).Select(x => x.Name).ToList();
            var xProperties = forUserProperties.Where(x => userData.Fields.Select(f => $"User_{f}".ToLower()).Contains(x.Code.ToLower())).ToList();
            var userChangeLog = new UserChangeLogDto
            {
                ID = new Guid(_idGenerator.Create().ToString()),
                ChangeType = (int)EnumUserLogType.批量修改,
                Status = (int)EnumUserLogStatus.无需审批,
                ChangeTime = DateTime.Now,
                ChangeUserID = CurrentUser?.UserID ?? Guid.Empty,
                ChangeUserName = CurrentUser?.UserName ?? "",
                ChangeUserPhone = CurrentUser?.UserPhone ?? "",
                Content = $"读者:{string.Join(',', userNames)} 变更字段:{string.Join(',', fieldsName)}"
            };
            var itemChangeLogs = new List<UserChangeItemDto>();
            var userType = typeof(SimpleUserListItemOutput);
            var userProperties = userType.GetProperties();
            var editType = typeof(UserBatchEditInput);
            var editProperties = editType.GetProperties();
            xProperties.ForEach(x =>
            {
                preUserDatas.ForEach(u =>
                {
                    var fieldCode = x.Code.Replace("User_", "");
                    var mapProperty = userProperties.FirstOrDefault(up => up.Name.ToLower() == fieldCode.ToLower());
                    var mapEditProperty = editProperties.FirstOrDefault(up => up.Name.ToLower() == fieldCode.ToLower());
                    if (mapProperty != null && mapEditProperty != null)
                    {
                        var preValue = (mapProperty.GetValue(u) ?? "").ToString();
                        var newValue = (mapEditProperty.GetValue(userData) ?? "").ToString();
                        if (preValue != newValue)
                        {
                            var itemLog = new UserChangeItemDto
                            {
                                LogID = userChangeLog.ID,
                                UserID = u.ID,
                                IsField = true,
                                PropertyType = x.Type,
                                PropertyCode = x.Code,
                                PropertyName = x.Name,
                                OldValue = preValue,
                                NewValue = newValue
                            };
                            itemChangeLogs.Add(itemLog);
                        }
                    }
                });
            });
            userChangeLog.ItemChangeLogs = itemChangeLogs;
            if (!userChangeLog.ItemChangeLogs.Any())
            {
                return true;
            }
            if (needApprove)
            {
                userChangeLog.Status = (int)EnumUserLogStatus.待审批;
                await _dataApproveService.CreateUserChangeLog(userChangeLog);
            }
            else
            {
                await _userService.BatchUpdate(userBatchEditData);
            }

            return true;
        }

        /// <summary>
        /// 批量设置用户为馆员
        /// </summary>
        /// <param name="userIds">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<bool> BatchSetUserAsStaff([FromBody] List<Guid> userIds)
        {
            var result = await _userService.BatchSetUserAsStaff(userIds);
            return result;
        }

        /// <summary>
        /// 用户数据导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DownloadUserImportTemplate()
        {

            var fileStream = new MemoryStream();
            byte[] content = null;
            try
            {
                var path = FileHelper.GetAbsolutePath("/Template/UserImportDataTemp.xls");
                return new FileStreamResult(new FileStream(path, FileMode.Open), "application/octet-stream") { FileDownloadName = "用户导入元数据模板.xls" };

            }
            catch (Exception ex)
            {
                _logger.LogError($"用户元数据导出错误：{ex.Message}");
                throw Oops.Oh("用户元数据模板导出错误");
            }
            finally
            {
                await fileStream.FlushAsync();
                fileStream.Dispose();
                fileStream.Close();
            }

        }

        /// <summary>
        /// 用户数据导入
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [UnitOfWork]
        public async Task<Guid> ImportUser(IFormFile file)
        {
            var batchId = _idGenerator.CreateGuid();
            if (file == null || string.IsNullOrWhiteSpace(file.FileName))
            {
                throw Oops.Oh("未获取到文件信息");
            }
            var strExtName = Path.GetExtension(file.FileName).ToLower();
            if (strExtName != ".xls" && strExtName != ".xlsx")
            {
                throw Oops.Oh("请上传Excel文件");
            }
            var dt = await LoadToDataTable(file);
            if (dt.Rows.Count <= 0)
            {
                throw Oops.Oh("未从文件中获取到数据信息");
            }
            var dataList = DataToList(dt, batchId);
            dataList = _userIoService.ValidateData(dataList);
            dataList = await _userIoService.MapPropertyCode(dataList);
            dataList = _userIoService.ValidateUserCode(dataList);
            var batchInsertTempData = dataList.Adapt<List<UserImportTempData>>();
            await _userIoService.ImportUserTempData(batchInsertTempData);
            await _userIoService.CheckUserTempData(batchId);
            return batchId;
        }

        ///// <summary>
        ///// 测试检测导入数据
        ///// </summary>
        ///// <param name="batchId"></param>
        ///// <returns></returns>
        //public async Task<bool> ImportCheckTest(Guid batchId)
        //{
        //    await _userIoService.CheckUserTempData(batchId);
        //    return true;
        //}

        private async Task<DataTable> LoadToDataTable(IFormFile file)
        {
            var dt = new DataTable();
            var fileStream = new MemoryStream();
            try
            {
                await file.CopyToAsync(fileStream);
                fileStream.Position = 0;
                var workbook = new XSSFWorkbook(fileStream);
                if (workbook.NumberOfSheets <= 0)
                {
                    throw Oops.Oh("未找到Excel文件内容");
                }
                var sheet = workbook.GetSheetAt(0);
                dt.Columns.AddRange(new[] {
                    new DataColumn("User_Name",typeof(string)),
                    new DataColumn("User_Gender",typeof(string)),
                    new DataColumn("User_Phone",typeof(string)),
                    //new DataColumn("User_Type",typeof(string)),
                    new DataColumn("User_TypeName",typeof(string)),
                    new DataColumn("User_StudentNo",typeof(string)),
                    new DataColumn("User_Unit",typeof(string)),
                    new DataColumn("User_Edu",typeof(string)),
                    //new DataColumn("User_College",typeof(string)),
                    new DataColumn("User_CollegeName",typeof(string)),
                    //new DataColumn("User_CollegeDepart",typeof(string)),
                    new DataColumn("User_CollegeDepartName",typeof(string)),
                    new DataColumn("User_Major",typeof(string)),
                    new DataColumn("User_Grade",typeof(string)),
                    new DataColumn("User_Class",typeof(string)),
                    new DataColumn("User_IdCard",typeof(string)),
                    new DataColumn("User_Email",typeof(string)),
                    new DataColumn("User_Birthday",typeof(string)),
                    new DataColumn("User_Addr",typeof(string)),
                    new DataColumn("User_AddrDetail",typeof(string)),
                    new DataColumn("Card_No",typeof(string)),
                    //new DataColumn("Card_Type",typeof(string)),
                    new DataColumn("Card_TypeName",typeof(string)),
                });
                //写入内容
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                while (rows.MoveNext())
                {
                    IRow row = (XSSFRow)rows.Current;
                    if (row.RowNum == sheet.FirstRowNum)
                    {
                        continue;
                    }

                    DataRow dr = dt.NewRow();
                    foreach (ICell item in row.Cells)
                    {
                        switch (item.CellType)
                        {
                            case CellType.Boolean:
                                dr[item.ColumnIndex] = item.BooleanCellValue;
                                break;
                            case CellType.Error:
                                dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                break;
                            case CellType.Formula:
                                switch (item.CachedFormulaResultType)
                                {
                                    case CellType.Boolean:
                                        dr[item.ColumnIndex] = item.BooleanCellValue;
                                        break;
                                    case CellType.Error:
                                        dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(item))
                                        {
                                            dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                        }
                                        else
                                        {
                                            dr[item.ColumnIndex] = item.NumericCellValue;
                                        }
                                        break;
                                    case CellType.String:
                                        string str = item.StringCellValue;
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            dr[item.ColumnIndex] = str.ToString();
                                        }
                                        else
                                        {
                                            dr[item.ColumnIndex] = null;
                                        }
                                        break;
                                    case CellType.Unknown:
                                    case CellType.Blank:
                                    default:
                                        dr[item.ColumnIndex] = string.Empty;
                                        break;
                                }
                                break;
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(item))
                                {
                                    dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                }
                                else
                                {
                                    dr[item.ColumnIndex] = item.NumericCellValue;
                                }
                                break;
                            case CellType.String:
                                string strValue = item.StringCellValue;
                                if (!string.IsNullOrEmpty(strValue))
                                {
                                    dr[item.ColumnIndex] = strValue.ToString();
                                }
                                else
                                {
                                    dr[item.ColumnIndex] = null;
                                }
                                break;
                            case CellType.Unknown:
                            case CellType.Blank:
                            default:
                                dr[item.ColumnIndex] = string.Empty;
                                break;
                        }
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
            finally
            {
                fileStream.Dispose();
                fileStream.Close();
            }
        }

        private List<UserImportTempDataDto> DataToList(DataTable dt, Guid batchId)
        {
            var listTempReader = new List<UserImportTempDataDto>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var tempReader = new UserImportTempDataDto();
                    tempReader.Id = _idGenerator.CreateGuid();
                    tempReader.BatchId = batchId;
                    tempReader.UserName = dt.Rows[i]["User_Name"].ToString();
                    tempReader.UserGender = dt.Rows[i]["User_Gender"].ToString();
                    tempReader.UserPhone = dt.Rows[i]["User_Phone"].ToString();
                    tempReader.UserType = "";// dt.Rows[i]["User_Type"].ToString();
                    tempReader.UserTypeName = dt.Rows[i]["User_TypeName"].ToString();
                    tempReader.StudentNo = dt.Rows[i]["User_StudentNo"].ToString();
                    tempReader.Unit = dt.Rows[i]["User_Unit"].ToString();
                    tempReader.Edu = dt.Rows[i]["User_Edu"].ToString();
                    tempReader.College = "";// dt.Rows[i]["User_College"].ToString();
                    tempReader.CollegeName = dt.Rows[i]["User_CollegeName"].ToString();
                    tempReader.CollegeDepart = "";// dt.Rows[i]["User_CollegeDepart"].ToString();
                    tempReader.CollegeDepartName = dt.Rows[i]["User_CollegeDepartName"].ToString();
                    tempReader.Major = dt.Rows[i]["User_Major"].ToString();
                    tempReader.Grade = dt.Rows[i]["User_Grade"].ToString();
                    tempReader.Class = dt.Rows[i]["User_Class"].ToString();
                    tempReader.IdCard = dt.Rows[i]["User_IdCard"].ToString();
                    tempReader.Email = dt.Rows[i]["User_Email"].ToString();
                    tempReader.Birthday = DataConverter.ToNumableDateTime(dt.Rows[i]["User_Birthday"].ToString());
                    tempReader.Addr = dt.Rows[i]["User_Addr"].ToString();
                    tempReader.AddrDetail = dt.Rows[i]["User_AddrDetail"].ToString();
                    tempReader.CardNo = dt.Rows[i]["Card_No"].ToString();
                    tempReader.CardType = "";// dt.Rows[i]["Card_Type"].ToString();
                    tempReader.CardTypeName = dt.Rows[i]["Card_TypeName"].ToString();
                    tempReader.ExpireTime = DateTime.Now.AddMinutes(30);
                    if (!string.IsNullOrWhiteSpace(tempReader.UserName))
                    {
                        listTempReader.Add(tempReader);
                    }

                }
            }
            return listTempReader;
        }

        /// <summary>
        /// 获取待导入数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<PagedList<UserTempDataListItemDto>> QueryImportTempUserData([FromQuery] UserTempDataTableQuery queryFilter)
        {
            var pageList = await _userIoService.QueryImportTempUserData(queryFilter);
            var targetList = pageList.Adapt<PagedList<UserTempDataListItemDto>>();
            return targetList;
        }

        /// <summary>
        /// 用户数据导入确认
        /// </summary>
        /// <param name="batchId">用户数据</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]/{batchId}")]
        [UnitOfWork]
        public async Task<UserImportResultDto> ImportUserConfirm(Guid batchId)
        {
            var result = await _userIoService.ImportUserConfirm(batchId);
            return result;
        }

        /// <summary>
        /// 获取待导出简要信息1103
        /// </summary>
        /// <param name="exportFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<UserExportBriefOutput> GetExportUserDataBriefInfo([FromBody] UserExportFilter exportFilter)
        {
            var briefInfoDto = await _userIoService.GetExportUserDataBriefInfo(exportFilter);
            var targetBriefInfo = briefInfoDto.Adapt<UserExportBriefOutput>();
            return targetBriefInfo;
        }

        /// <summary>
        /// 用户数据导出
        /// </summary>
        /// <param name="exportFilter">导出筛选</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ExportUserData([FromBody] UserExportFilter exportFilter)
        {
            var groupSelects = await _userService.GetGroupSelectItem();
            var properties = exportFilter.Properties.ToList();
            switch (exportFilter.ExportType)
            {
                case 0:
                    exportFilter.PageSize = 5000;
                    break;
                default:
                    break;
            }
            var wb = new XSSFWorkbook();
            var sheet = wb.CreateSheet("sheet1");//创建工作簿
            var dateCellStyle = wb.CreateCellStyle();
            var dataFormat = wb.CreateDataFormat();
            dateCellStyle.DataFormat = dataFormat.GetFormat("yyyy-MM-dd");
            //查询导出数据
            var userData = await _userIoService.QueryExportUserTableData(exportFilter);
            var userDataItems = userData.Items.ToList();
            var userList = userDataItems.Adapt<List<ExportUserListItemOutput>>();
            var rowIndex = 0;
            //添加表头
            var titleRow = sheet.CreateRow(rowIndex);
            var titleRowCells = new Dictionary<int, ICell>();
            for (var cellIndex = 0; cellIndex < properties.Count(); cellIndex++)
            {
                var property = properties[cellIndex];
                titleRowCells.Add(cellIndex, titleRow.CreateCell(cellIndex));
                titleRowCells[cellIndex].SetCellValue(property.PropertyName);
            }
            rowIndex++;
            //添加数据
            foreach (var rowData in userList)
            {
                var dataRow = sheet.CreateRow(rowIndex);
                var dataRowCells = new Dictionary<int, ICell>();
                for (var cellIndex = 0; cellIndex < properties.Count(); cellIndex++)
                {
                    var property = properties[cellIndex];
                    dataRowCells.Add(cellIndex, dataRow.CreateCell(cellIndex));
                    var cellValue = GetPropertyValue(rowData, property, groupSelects);
                    dataRowCells[cellIndex].SetCellValue(cellValue);
                }
                rowIndex++;
            }

            var fileStream = new MemoryStream();
            byte[] content = null;
            try
            {
                wb.Write(fileStream);
                content = fileStream.ToArray();
                return await Task.FromResult(new FileContentResult(content, "application/octet-stream") { FileDownloadName = $"用户导出数据{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"用户数据导出错误：{ex.Message}");
                throw Oops.Oh("用户数据导出错误");
            }
            finally
            {
                //await fileStream.FlushAsync();
                fileStream.Dispose();
                fileStream.Close();
            }
        }

        /// <summary>
        /// 获取用户数据需要合并信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<MergeUserOutput>> GetMergeInfo(Guid userId)
        {
            var mergeUserInfos = await _userIoService.GetMergeInfo(userId);
            var mergeUserInfoOutput = mergeUserInfos.Adapt<List<MergeUserOutput>>();
            return mergeUserInfoOutput;
        }

        /// <summary>
        /// 获取用户数据需要合并信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<MergeUserOutput>> GetMergeInfo([FromBody] List<Guid> userIds)
        {
            var mergeUserInfos = await _userIoService.GetMergeInfo(userIds);
            var mergeUserInfoOutput = mergeUserInfos.Adapt<List<MergeUserOutput>>();
            return mergeUserInfoOutput;
        }

        /// <summary>
        /// 合并读者消息
        /// </summary>
        /// <param name="mergeInput"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> MergeUserInfo([FromBody] MergeUserInput mergeInput)
        {
            var mergeInfo = mergeInput.Adapt<MergeUserInfo>();
            var result = await _userIoService.MergeUserInfo(mergeInfo);
            return result;
        }



        #region private method

        private string ShowAddrName(string addr)
        {
            var addrArray = (addr ?? "").Split('|');
            if (addrArray.Length >= 1)
            {
                return addrArray[0];
            }
            return "";
        }

        private string GetPropertyValue(ExportUserListItemOutput userData, ExportPropertyInput property, List<PropertyGroupSelectDto> groupSelects)
        {
            var fieldProperties = typeof(ExportUserListItemOutput).GetProperties().ToList();
            var extenalProperties = userData.Properties.ToList();
            if (!property.External)
            {
                var fieldCode = property.PropertyCode;
                if (property.PropertyCode.ToLower().Contains("user"))
                {
                    fieldCode = fieldCode.Replace("User_", "");
                }
                else
                {
                    fieldCode = fieldCode.Replace("_", "");
                }
                var appendNamesFields = new[] { "Depart", "College", "CollegeDepart", "Type", "CardType" };
                if (appendNamesFields.Contains(fieldCode))
                {
                    fieldCode = $"{fieldCode}Name";
                }
                var field = fieldProperties.FirstOrDefault(x => x.Name.ToLower() == fieldCode.ToLower());
                if (field != null)
                {
                    var fieldValue = DataConverter.ObjectToString(field.GetValue(userData), field.PropertyType);
                    if (property.PropertyType == (int)EnumPropertyType.属性组 && (field.PropertyType == typeof(int) || field.PropertyType == typeof(int?)))
                    {
                        var mapItems = groupSelects.Where(g => g.GroupCode == property.PropertyCode).SelectMany(g => g.GroupItems).ToList();
                        var mapItem = mapItems.FirstOrDefault(i => i.Value == fieldValue);
                        fieldValue = mapItem != null ? mapItem.Key : fieldValue;
                    }
                    if (property.PropertyType == (int)EnumPropertyType.是非)
                    {
                        fieldValue = !string.IsNullOrWhiteSpace(fieldValue) ? ((DataConverter.ToNullableBoolean(fieldValue) ?? false) ? "是" : "否") : "";
                    }
                    if (property.PropertyType == (int)EnumPropertyType.地址)
                    {
                        fieldValue = this.ShowAddrName(fieldValue);
                    }
                    return fieldValue;
                }
                return "";
            }
            else
            {
                var fieldCode = property.PropertyCode;
                var mapProperty = extenalProperties.FirstOrDefault(x => x.PropertyCode == fieldCode);
                if (mapProperty != null)
                {
                    var fieldValue = mapProperty.PropertyValue;
                    if (property.PropertyType == (int)EnumPropertyType.是非)
                    {
                        fieldValue = !string.IsNullOrWhiteSpace(fieldValue) ? ((DataConverter.ToNullableBoolean(fieldValue) ?? false) ? "是" : "否") : "";
                    }
                    if (property.PropertyType == (int)EnumPropertyType.地址)
                    {
                        fieldValue = this.ShowAddrName(fieldValue);
                    }
                    return fieldValue;
                }
                return "";
            }
        }

        /// <summary>
        /// 属性编辑，用户信息新旧对比
        /// </summary>
        /// <param name="diffType"></param>
        /// <param name="preData"></param>
        /// <param name="newData"></param>
        /// <returns></returns>
        private async Task<UserChangeLogDto> CompareUserDiffAsync(EnumLogDiffType diffType, UserDto preData, UserDto newData)
        {
            var xType = typeof(UserDto);
            var xProperties = xType.GetProperties().ToList();
            var logProperties = xType.GetProperties().Where(x => x.GetCustomAttributes(typeof(LogPropertyAttribute), false).Any()).Select(x => (LogPropertyAttribute)(x.GetCustomAttributes(typeof(LogPropertyAttribute), false).FirstOrDefault())).ToList();
            var forUserProperties = (await _propertyService.QueryPropertyList()).Where(x => x.ForReader).ToList();
            var fieldNames = new List<string>();
            var userChangeLog = new UserChangeLogDto
            {
                ID = new Guid(_idGenerator.Create().ToString()),
                ChangeType = 0,
                Status = (int)EnumUserLogStatus.无需审批,
                ChangeTime = DateTime.Now,
                ChangeUserID = CurrentUser?.UserID ?? Guid.Empty,
                ChangeUserName = CurrentUser?.UserName ?? "",
                ChangeUserPhone = CurrentUser?.UserPhone ?? "",
                Content = ""
            };
            var itemChangeLogs = new List<UserChangeItemDto>();
            switch (diffType)
            {
                case EnumLogDiffType.新增:
                    {
                        if (newData == null)
                        {
                            throw Oops.Oh("属性对象不能为空");
                        }
                        userChangeLog.ChangeType = (int)EnumPropertyLogType.新增;
                        //更字段记录
                        logProperties.OrderBy(x => x.Sort).ToList().ForEach(x =>
                        {
                            var mapP = xProperties.FirstOrDefault(p => p.Name.ToLower() == x.Code.ToLower());
                            if (mapP == null)
                            {
                                throw Oops.Oh("属性映射对比失败");
                            }
                            var mapUserProperty = forUserProperties.FirstOrDefault(p => p.Code == $"User_{x.Code}");
                            if (mapUserProperty != null)
                            {
                                var preValue = "";
                                var newValue = (mapP.GetValue(newData) ?? "").ToString();
                                var itemLog = new UserChangeItemDto
                                {
                                    LogID = userChangeLog.ID,
                                    UserID = newData.ID,
                                    IsField = true,
                                    PropertyType = mapUserProperty.Type,
                                    PropertyCode = mapUserProperty.Code,
                                    PropertyName = x.Name,
                                    OldValue = preValue,
                                    NewValue = newValue
                                };
                                itemChangeLogs.Add(itemLog);
                            }
                        });
                        //变更扩展属性记录
                        var extProperties = newData.Properties;
                        extProperties.ForEach(x =>
                        {
                            var mapUserProperty = forUserProperties.FirstOrDefault(p => p.ID == x.PropertyID);
                            if (mapUserProperty != null)
                            {
                                var preValue = "";
                                var newValue = x.PropertyValue.ToString();
                                var itemLog = new UserChangeItemDto
                                {
                                    LogID = userChangeLog.ID,
                                    UserID = newData.ID,
                                    IsField = false,
                                    PropertyType = mapUserProperty.Type,
                                    PropertyCode = mapUserProperty.Code,
                                    PropertyName = mapUserProperty.Name,
                                    OldValue = preValue,
                                    NewValue = newValue
                                };
                                itemChangeLogs.Add(itemLog);
                            }

                        });
                        fieldNames = itemChangeLogs.Select(x => x.PropertyName).ToList();
                        userChangeLog.Content = $"读者:{newData.Name} 变更字段:{string.Join(";", fieldNames)}";
                        userChangeLog.ItemChangeLogs = itemChangeLogs;
                    }
                    break;
                case EnumLogDiffType.修改:
                    {
                        if (preData == null || newData == null)
                        {
                            throw Oops.Oh("属性对象不能为空");
                        }
                        userChangeLog.ChangeType = (int)EnumPropertyLogType.修改;
                        logProperties.OrderBy(x => x.Sort).ToList().ForEach(x =>
                        {
                            var mapP = xProperties.FirstOrDefault(p => p.Name.ToLower() == x.Code.ToLower());
                            if (mapP == null)
                            {
                                throw Oops.Oh("属性映射对比失败");
                            }
                            var mapUserProperty = forUserProperties.FirstOrDefault(p => p.Code == $"User_{x.Code}");
                            if (mapUserProperty != null)
                            {
                                var preValue = (mapP.GetValue(preData) ?? "").ToString();
                                var newValue = (mapP.GetValue(newData) ?? "").ToString();
                                if (preValue != newValue)
                                {
                                    var itemLog = new UserChangeItemDto
                                    {
                                        LogID = userChangeLog.ID,
                                        UserID = preData.ID,
                                        IsField = true,
                                        PropertyType = mapUserProperty.Type,
                                        PropertyCode = mapUserProperty.Code,
                                        PropertyName = mapUserProperty.Name,
                                        OldValue = preValue,
                                        NewValue = newValue
                                    };
                                    itemChangeLogs.Add(itemLog);
                                }
                            }
                        });
                        var preExtProperties = preData.Properties;
                        var newExtProperties = newData.Properties;
                        //插入新增扩展属性记录
                        var insertProperties = GetInsertProperties(preExtProperties, newExtProperties);
                        insertProperties.ForEach(x =>
                        {
                            var mapUserProperty = forUserProperties.FirstOrDefault(p => p.ID == x.PropertyID);
                            if (mapUserProperty != null)
                            {
                                var preValue = "";
                                var newValue = x.PropertyValue.ToString();
                                if (preValue != newValue)
                                {
                                    var itemLog = new UserChangeItemDto
                                    {
                                        LogID = userChangeLog.ID,
                                        UserID = newData.ID,
                                        IsField = false,
                                        PropertyType = mapUserProperty.Type,
                                        PropertyCode = mapUserProperty.Code,
                                        PropertyName = mapUserProperty.Name,
                                        OldValue = preValue,
                                        NewValue = newValue
                                    };
                                    itemChangeLogs.Add(itemLog);
                                }
                            }

                        });
                        var updateProperties = GetUpdateProperties(preExtProperties, newExtProperties);
                        updateProperties.ForEach(x =>
                        {
                            var mapUserProperty = forUserProperties.FirstOrDefault(p => p.ID == x.PropertyID);
                            if (mapUserProperty != null)
                            {
                                var preProperty = preExtProperties.FirstOrDefault(p => p.PropertyID == mapUserProperty.ID);
                                var preValue = preProperty?.PropertyValue ?? "";
                                var newValue = x.PropertyValue;
                                if (preValue != newValue)
                                {
                                    var itemLog = new UserChangeItemDto
                                    {
                                        LogID = userChangeLog.ID,
                                        UserID = newData.ID,
                                        IsField = false,
                                        PropertyType = mapUserProperty.Type,
                                        PropertyCode = mapUserProperty.Code,
                                        PropertyName = mapUserProperty.Name,
                                        OldValue = preValue,
                                        NewValue = newValue
                                    };
                                    itemChangeLogs.Add(itemLog);
                                }
                            }
                        });
                        var deleteProperties = GetDelProperties(preExtProperties, newExtProperties);
                        deleteProperties.ForEach(x =>
                        {
                            var mapUserProperty = forUserProperties.FirstOrDefault(p => p.ID == x.PropertyID);
                            if (mapUserProperty != null)
                            {
                                var preProperty = preExtProperties.FirstOrDefault(p => p.PropertyID == mapUserProperty.ID);
                                var preValue = preProperty?.PropertyValue ?? "";
                                var newValue = "";
                                if (preValue != newValue)
                                {
                                    var itemLog = new UserChangeItemDto
                                    {
                                        LogID = userChangeLog.ID,
                                        UserID = newData.ID,
                                        IsField = false,
                                        PropertyType = mapUserProperty.Type,
                                        PropertyCode = mapUserProperty.Code,
                                        PropertyName = mapUserProperty.Name,
                                        OldValue = preValue,
                                        NewValue = newValue
                                    };
                                    itemChangeLogs.Add(itemLog);
                                }
                            }
                        });
                        fieldNames = itemChangeLogs.Select(x => x.PropertyName).ToList();
                        userChangeLog.Content = $"读者:{preData.Name} 变更字段:{string.Join(";", fieldNames)}";
                        userChangeLog.ItemChangeLogs = itemChangeLogs;
                    }

                    break;
                case EnumLogDiffType.删除:
                    {
                        if (preData == null)
                        {
                            throw Oops.Oh("属性对象不能为空");
                        }
                        userChangeLog.ChangeType = (int)EnumPropertyLogType.删除;
                        logProperties.OrderBy(x => x.Sort).ToList().ForEach(x =>
                        {
                            var mapP = xProperties.FirstOrDefault(p => p.Name.ToLower() == x.Code.ToLower());
                            if (mapP == null)
                            {
                                throw Oops.Oh("属性映射对比失败");
                            }
                            var mapUserProperty = forUserProperties.FirstOrDefault(p => p.Code == $"User_{x.Code}");
                            if (mapUserProperty != null)
                            {
                                var preValue = (mapP.GetValue(preData) ?? "").ToString();
                                var newValue = "";
                                if (preValue != newValue)
                                {
                                    var itemLog = new UserChangeItemDto
                                    {
                                        LogID = userChangeLog.ID,
                                        UserID = preData.ID,
                                        IsField = false,
                                        PropertyType = mapUserProperty.Type,
                                        PropertyCode = mapUserProperty.Code,
                                        PropertyName = mapUserProperty.Name,
                                        OldValue = preValue,
                                        NewValue = newValue
                                    };
                                    itemChangeLogs.Add(itemLog);
                                }
                            }
                        });
                        //变更扩展属性记录
                        var extProperties = preData.Properties;
                        extProperties.ForEach(x =>
                        {
                            var mapUserProperty = forUserProperties.FirstOrDefault(p => p.ID == x.PropertyID);
                            if (mapUserProperty != null)
                            {
                                var preValue = x.PropertyValue.ToString();
                                var newValue = "";
                                var itemLog = new UserChangeItemDto
                                {
                                    LogID = userChangeLog.ID,
                                    UserID = preData.ID,
                                    IsField = false,
                                    PropertyType = mapUserProperty.Type,
                                    PropertyCode = mapUserProperty.Code,
                                    PropertyName = mapUserProperty.Name,
                                    OldValue = preValue,
                                    NewValue = newValue
                                };
                                itemChangeLogs.Add(itemLog);
                            }

                        });
                        fieldNames = itemChangeLogs.Select(x => x.PropertyName).ToList();
                        userChangeLog.Content = $"读者:{preData.Name} 变更字段:{string.Join(";", fieldNames)}";
                        userChangeLog.ItemChangeLogs = itemChangeLogs;
                    }
                    break;
                default:
                    break;
            }
            return userChangeLog;
        }

        private List<UserPropertyDto> GetInsertProperties(List<UserPropertyDto> extProperties, List<UserPropertyDto> newProperties)
        {
            var insertUserProperties = newProperties.Where(x => !extProperties.Any(p => p.PropertyID == x.PropertyID)).ToList();
            return insertUserProperties;
        }

        private List<UserPropertyDto> GetUpdateProperties(List<UserPropertyDto> extProperties, List<UserPropertyDto> newProperties)
        {
            var updateUserProperties = newProperties.Where(x => extProperties.Any(p => p.PropertyID == x.PropertyID)).ToList();
            return updateUserProperties;
        }

        private List<UserPropertyDto> GetDelProperties(List<UserPropertyDto> extProperties, List<UserPropertyDto> newProperties)
        {
            var delUserProperties = extProperties.Where(x => !newProperties.Any(p => p.PropertyID == x.PropertyID)).ToList();
            return delUserProperties;
        }
        #endregion
    }
}
