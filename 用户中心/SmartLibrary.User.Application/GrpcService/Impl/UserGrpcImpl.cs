/*********************************************************
* 名    称：UserGrpcImpl.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户信息获取服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.User.Application.GrpcService.Interface;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartLibrary.User.RpcService;
using UserTableQuery = SmartLibrary.User.RpcService.UserTableQuery;
using static SmartLibrary.User.RpcService.UserPageData.Types;
using SmartLibrary.User.Application.Dtos.User;

namespace SmartLibrary.User.Application.GrpcService
{
    [Authorize]
    public class UserGrpcImpl : UserGrpcService.UserGrpcServiceBase, IUsersGrpcService, IScoped
    {
        private readonly IUserService _UserService;
        private readonly IPropertyGroupService _PropertyGroupService;
        private readonly IUserGroupService _UserGroupService;

        public UserGrpcImpl(IUserService userService,
                            IPropertyGroupService propertyGroupService,
                            IUserGroupService userGroupService)
        {
            _UserService = userService;
            _PropertyGroupService = propertyGroupService;
            _UserGroupService = userGroupService;
        }


        #region 用户信息查询

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<UserData> GetUserByKey(StringValue request, ServerCallContext context)
        {
            return new UserData { 
            IdCard="111"
            };

            //var userDto = await _UserService.GetByUserKey(request.Value);
            //var userData = userDto.Adapt<UserData>();
            //userData.ShowStatus = ((EnumUserStatus)userDto.Status).ToString();
            //userData.ShowSourceFrom = ((EnumUserSourceFrom)userDto.SourceFrom).ToString();
            //userData.ShowCardStatus = userData.CardStatus.HasValue ? ((EnumCardStatus)userDto.CardStatus).ToString() : "";
            //userData.GroupIds.AddRange(userDto.GroupIds.Select(x => x.ToString()));
            //return userData;
        }

        /// <summary>
        /// 获取用户表格数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<UserPageData> GetUserTableData(UserTableQuery request, ServerCallContext context)
        {
            var queryFilter = new ViewModels.UserTableQuery
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize > 100 ? 100 : request.PageSize,
                Keyword = request.KeyWord
            };
            var pagedList = await _UserService.QuerySimpleInfoByKeyword(queryFilter);
            var userPageData = new UserPageData { TotalCount = pagedList.TotalCount };
            pagedList.Items.ToList().ForEach(x =>
            {
                var item = x.Adapt<UserListItem>();
                userPageData.Items.Add(item);
            });
            return userPageData;
        }

        /// <summary>
        /// 通过用户Id集合获取用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<UserListData> GetUserListByIds(UserIdList request, ServerCallContext context)
        {
            var userKeys = new List<string>();
            foreach (var id in request.Ids)
            {
                try
                {
                    var idGuid = id;
                    userKeys.Add(idGuid);
                }
                catch
                {
                    //忽略
                }
            }
            var list = await _UserService.QuerySimpleInfoListByUserKeys(userKeys);
            var userListData = new UserListData();
            list.ForEach(x =>
            {
                var item = x.Adapt<UserListItem>();
                userListData.Items.Add(item);
            });

            return userListData;
        }

        /// <summary>
        /// 获取所有馆员列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<UserPageData> GetManagerList(Empty request, ServerCallContext context)
        {
            var queryFilter = new ViewModels.UserTableQuery
            {
                PageIndex = 1,
                PageSize = 1000,
                Keyword = "",
                IsStaff = true
            };
            var pagedList = await _UserService.QuerySimpleInfoByKeyword(queryFilter);
            var userPageData = new UserPageData { TotalCount = pagedList.TotalCount };
            pagedList.Items.ToList().ForEach(x =>
            {
                var item = x.Adapt<UserListItem>();
                userPageData.Items.Add(item);
            });

            return userPageData;
        }

        /// <summary>
        /// 获取用户类型字典
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<DictList> GetUserTypeList(SimpleTableQuery request, ServerCallContext context)
        {
            var queryFilter = new PropertyGroupItemTableQuery
            {
                Keyword = request.KeyWord,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize > 1000 ? 1000 : request.PageSize,
                GroupCode = "User_Type"
            };
            var pageData = await _PropertyGroupService.QueryGroupItemListByKeyword(queryFilter);
            var listItems = pageData.Items.Select(x => new DictItem { Key = x.Name, Value = x.Code.ToString() }).ToList();
            var dictListData = new DictList();
            dictListData.Items.AddRange(listItems);
            return dictListData;
        }
        /// <summary>
        /// 获取用户学院字典
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<DictList> GetUserCollegeList(SimpleTableQuery request, ServerCallContext context)
        {
            var queryFilter = new PropertyGroupItemTableQuery
            {
                Keyword = request.KeyWord,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize > 100 ? 100 : request.PageSize,
                GroupCode = "User_College"
            };
            var pageData = await _PropertyGroupService.QueryGroupItemListByKeyword(queryFilter);
            var listItems = pageData.Items.Select(x => new DictItem { Key = x.Name, Value = x.ID.ToString() }).ToList();
            var dictListData = new DictList();
            dictListData.Items.AddRange(listItems);
            return dictListData;
        }

        /// <summary>
        /// 获取用户组字典
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<DictList> GetUserGroupList(SimpleTableQuery request, ServerCallContext context)
        {
            var queryFilter = new GroupTableQuery
            {
                Keyword = request.KeyWord,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize > 1000 ? 1000 : request.PageSize,
            };
            var pageData = await _UserGroupService.QueryTableQuery(queryFilter);
            var listItems = pageData.Items.Select(x => new DictItem { Key = x.Name, Value = x.ID.ToString() }).ToList();
            var dictListData = new DictList();
            dictListData.Items.AddRange(listItems);
            return dictListData;
        }

        /// <summary>
        /// 通过用户组查询用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<UserPageData> GetUserListByGroups(SimpleTableQuery request, ServerCallContext context)
        {

            var queryFilter = new SimpleUserTableQuery
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                GroupIds = GetGuidList(request.GroupIds.ToList())
            };
            var pagedList = await _UserService.QuerySimpleUserByGroupIds(queryFilter);
            var userPageData = new UserPageData { TotalCount = pagedList.TotalCount };
            pagedList.Items.ToList().ForEach(x =>
            {
                var item = x.Adapt<UserListItem>();
                userPageData.Items.Add(item);
            });
            return userPageData;
        }

        private List<Guid> GetGuidList(List<string> idList)
        {
            var guidList = new List<Guid>();
            idList.ForEach(x =>
            {
                try
                {
                    var guidx = new Guid(x);
                    guidList.Add(guidx);
                }
                catch
                {
                    //ignore
                }
            });
            return guidList;
        }

        /// <summary>
        /// 通过用户类型查询用户
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<UserPageData> GetUserListByTypes(SimpleTableQuery request, ServerCallContext context)
        {
            var queryFilter = new SimpleUserTableQuery
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                UserTypeCodes = request.UserTypeCodes.ToList()
            };
            var pagedList = await _UserService.QuerySimpleUserByUserTypes(queryFilter);
            var userPageData = new UserPageData { TotalCount = pagedList.TotalCount };
            pagedList.Items.ToList().ForEach(x =>
            {
                var item = x.Adapt<UserListItem>();
                userPageData.Items.Add(item);
            });
            return userPageData;
        }

        /// <summary>
        /// 获取精准用户查询条件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<SearchPropertyList> GetUserSearchProperties(Empty request, ServerCallContext context)
        {
            var canSearchProperties = await _UserService.GetCanSearchPropertyList();
            var canSearchPropertyItems = canSearchProperties.Adapt<List<SearchPropertyItem>>();
            var searchPropertyList = new SearchPropertyList();
            searchPropertyList.Items.AddRange(canSearchPropertyItems);
            return searchPropertyList;
        }

        /// <summary>
        /// 精准查询用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<UserPageData> GetUserListByConditions(UserSearchTableQuery request, ServerCallContext context)
        {
            var userTableQuery = request.Adapt<ViewModels.UserTableQuery>();
            var pagedList = await _UserService.QuerySimpleInfoTableData(userTableQuery);
            var userPageData = new UserPageData { TotalCount = pagedList.TotalCount };
            pagedList.Items.ToList().ForEach(x =>
            {
                var item = x.Adapt<UserListItem>();
                userPageData.Items.Add(item);
            });
            return userPageData;
        }

        /// <summary>
        /// 获取属性组选项
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<GroupSelectReply> GetUserPropertyGroupSelect(Empty request, ServerCallContext context)
        {
            var groupSelect = await _PropertyGroupService.GetPropertyGroupSelect();
            var selectReply = new GroupSelectReply();
            foreach (var groupItem in groupSelect)
            {
                var selectItem = new GroupSelectItem { GroupCode = groupItem.GroupCode };
                var groupItemList = groupItem.GroupItems.Select(x => new DictItem { Key = x.Key, Value = x.Value });
                selectItem.Items.AddRange(groupItemList);
                selectReply.Items.Add(selectItem);
            }
            return selectReply;
        }

        /// <summary>
        /// 通过导入信息查询用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<UserListImportSearchReply> GetUserListBySearchInfo(UserListImportSearchRequest request, ServerCallContext context)
        {
            var dataSearch = new List<UserImportSearchDto>();
            foreach (var item in request.Items)
            {
                var dataSearchItem = item.Adapt<UserImportSearchDto>();
                dataSearch.Add(dataSearchItem);
            }
            var userList = await _UserGroupService.QueryUserListBySearchInfo(dataSearch);
            var reply = new UserListImportSearchReply();
            foreach (var item in userList)
            {
                var userItem = item.Adapt<UserListItem>();
                reply.Items.Add(userItem);
            }
            return reply;
        }
        #endregion

    }
}
