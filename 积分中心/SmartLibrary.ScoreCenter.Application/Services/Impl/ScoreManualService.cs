/*********************************************************
* 名    称：ScoreManualService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分奖惩服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using Mapster;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Services.Enum;
using SmartLibrary.ScoreCenter.Application.Services.Extensions;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.Common.Dtos;
using SmartLibrary.ScoreCenter.Common.Services;
using SmartLibrary.ScoreCenter.Common.Utils;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 手动创建
    /// </summary>
    public class ScoreManualService : IScoreManualService, IScoped
    {
        private readonly IRepository<ScoreManualProcess> _manualProcessRepository;
        private readonly IRepository<ScoreRecieveRule> _recieveRuleRepository;
        private readonly IRepository<ScoreRecieveUser> _recieveUserRepository;
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IGrpcClientResolver _grpcClientResolver;

        public ScoreManualService(IRepository<ScoreManualProcess> manualProcessRepository
            , IRepository<ScoreRecieveRule> recieveRuleRepository
            , IRepository<ScoreRecieveUser> recieveUserRepository
            , IDistributedIDGenerator idGenerator
            , IGrpcClientResolver grpcClientResolver)
        {
            _manualProcessRepository = manualProcessRepository;
            _recieveRuleRepository = recieveRuleRepository;
            _recieveUserRepository = recieveUserRepository;
            _idGenerator = idGenerator;
            _grpcClientResolver = grpcClientResolver;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var userGrpcClient = _grpcClientResolver.EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            var queryFilter = new SimpleTableQuery { PageIndex = 1, PageSize = 10000 };
            var userGroup = await userGrpcClient.GetUserGroupListAsync(queryFilter);
            var userType = await userGrpcClient.GetUserTypeListAsync(queryFilter);
            var userSearchProperties = await userGrpcClient.GetUserSearchPropertiesAsync(new Google.Protobuf.WellKnownTypes.Empty());
            var groupSelect = await userGrpcClient.GetUserPropertyGroupSelectAsync(new Google.Protobuf.WellKnownTypes.Empty());
            var initData = new
            {
                data = new ScoreManualProcessDto
                {
                    ID = _idGenerator.CreateGuid()
                },
                UserGroupList = userGroup.Items.ToList(),
                UserTypeList = userType.Items.ToList(),
                UserSearchConditions = userSearchProperties.Items.ToList(),
                UserGroupSelect = groupSelect.Items.ToList(),
                ValidateTerms = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumValidTerm)),
            };
            return await Task.FromResult(initData);
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ScoreManualListItemDto>> QueryTableData(ScoreManualTableQuery queryFilter)
        {
            var processQuery = from manualProcess in _manualProcessRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                               .Where(queryFilter.Type.HasValue, x => x.Type == queryFilter.Type)
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Keyword), x => x.Desc != null && x.Desc != "" && x.Desc.Contains(queryFilter.Keyword))
                               .Where(queryFilter.OperatorStartTime.HasValue, x => x.OperatorTime >= queryFilter.OperatorStartTime)
                               .Where(queryFilter.OperatorCompareEndTime.HasValue, x => x.OperatorTime < queryFilter.OperatorCompareEndTime)
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Operator), x => x.OperatorName != null && x.OperatorName != "" && x.OperatorName.Contains(queryFilter.Operator))
                               select new ScoreManualListItemDto
                               {
                                   ID = manualProcess.Id,
                                   Sort = 0,
                                   Desc = manualProcess.Desc,
                                   Type = manualProcess.Type,
                                   Score = manualProcess.Score,
                                   UserCount = _recieveUserRepository.DetachedEntities.Count(u => !u.DeleteFlag && u.ProcessID == manualProcess.Id),
                                   Status = manualProcess.Status,
                                   OperatorName = manualProcess.OperatorName,
                                   OperatorTime = manualProcess.OperatorTime,
                                   SourceFrom = manualProcess.SourceFrom,
                                   CreateTime = manualProcess.CreateTime
                               };
            var pagedList = await processQuery.OrderBy(x => x.Status).ThenByDescending(x => x.CreateTime).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pagedList;
        }

        /// <summary>
        /// 任务创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Guid> Create(ScoreManualProcessDto input)
        {
            var id = _idGenerator.CreateGuid();
            var manualProcessEntity = new ScoreManualProcess
            {
                Id = id,
                Desc = input.Desc,
                Type = input.Type,
                Score = input.Score,
                ValidTerm = input.ValidTerm,
                Status = (int)EnumManualProcessStatus.待处理,
                SourceFrom = input.SourceFrom,
                OperatorName = input.OperatorName,
                OperatorTime = DateTime.Now,
                OperatorUserKey = input.OperatorUserKey
            };
            await _manualProcessRepository.InsertAsync(manualProcessEntity);
            switch (input.SourceFrom)
            {
                case (int)EnumSourceFrom.直接添加:
                    var users = input.Users.ToList();
                    var scoreRecieveUser = users.Select(x => new ScoreRecieveUser
                    {
                        Id = _idGenerator.CreateGuid(),
                        ProcessID = id,
                        UserKey = x.UserKey,
                        SourceFrom = input.SourceFrom,
                    }).ToList();
                    await _recieveUserRepository.InsertAsync(scoreRecieveUser);
                    break;
                case (int)EnumSourceFrom.规则查询:
                    var rules = input.Rules.ToList();
                    var scoreRecieveRule = rules.Select(x => new ScoreRecieveRule
                    {
                        Id = _idGenerator.CreateGuid(),
                        ProcessID = id,
                        RuleType = x.RuleType,
                        PropertyCode = x.PropertyCode,
                        OperateType = x.OperateType,
                        PropertyValue = x.PropertyValue
                    });
                    await _recieveRuleRepository.InsertAsync(scoreRecieveRule);
                    break;
            }
            return id;
        }


        /// <summary>
        /// 获取用户数据列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SimpleUserListItemDto>> GetUserListByGroups(GroupUserTableQuery queryFilter)
        {
            var simpleUserList = new List<SimpleUserListItemDto>();
            var userGrpcClient = _grpcClientResolver.EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            var groupUserQueryFilter = new SimpleTableQuery { PageIndex = queryFilter.PageIndex, PageSize = queryFilter.PageSize };
            groupUserQueryFilter.GroupIds.AddRange(queryFilter.GroupIds);
            var userList = await userGrpcClient.GetUserListByGroupsAsync(groupUserQueryFilter);
            foreach (var item in userList.Items)
            {
                var mapItem = item.Adapt<SimpleUserListItemDto>();
                simpleUserList.Add(mapItem);
            }
            return new PagedList<SimpleUserListItemDto>
            {
                PageIndex = queryFilter.PageIndex,
                PageSize = queryFilter.PageSize,
                TotalCount = userList.TotalCount,
                Items = simpleUserList
            };
        }

        /// <summary>
        /// 获取用户数据列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SimpleUserListItemDto>> GetUserListByUserType(TypeUserTableQuery queryFilter)
        {
            var simpleUserList = new List<SimpleUserListItemDto>();
            var userGrpcClient = _grpcClientResolver.EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            var typeUserQueryFilter = new SimpleTableQuery { PageIndex = queryFilter.PageIndex, PageSize = queryFilter.PageSize };
            typeUserQueryFilter.UserTypeCodes.AddRange(queryFilter.UserTypes);
            var userList = await userGrpcClient.GetUserListByTypesAsync(typeUserQueryFilter);
            foreach (var item in userList.Items)
            {
                var mapItem = item.Adapt<SimpleUserListItemDto>();
                simpleUserList.Add(mapItem);
            }
            return new PagedList<SimpleUserListItemDto>
            {
                PageIndex = queryFilter.PageIndex,
                PageSize = queryFilter.PageSize,
                TotalCount = userList.TotalCount,
                Items = simpleUserList
            };
        }

        /// <summary>
        /// 获取用户数据列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SimpleUserListItemDto>> GetUserListByConditions(ViewModels.UserTableQuery queryFilter)
        {
            var simpleUserList = new List<SimpleUserListItemDto>();
            var userGrpcClient = _grpcClientResolver.EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            var conditionQuery = queryFilter.Adapt<UserSearchTableQuery>();
            var userList = await userGrpcClient.GetUserListByConditionsAsync(conditionQuery);
            foreach (var item in userList.Items)
            {
                var mapItem = item.Adapt<SimpleUserListItemDto>();
                simpleUserList.Add(mapItem);
            }
            return new PagedList<SimpleUserListItemDto>
            {
                PageIndex = queryFilter.PageIndex,
                PageSize = queryFilter.PageSize,
                TotalCount = userList.TotalCount,
                Items = simpleUserList
            };
        }


    }
}
