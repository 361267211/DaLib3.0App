/*********************************************************
* 名    称：BasicConfigService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：基础配置服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos.BasicConfigSet;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 基础配置服务
    /// </summary>
    public class BasicConfigService : IBasicConfigService
    {
        private readonly IRepository<BasicConfigSet> _configSetRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<UserGroup> _userGroupRepository;
        private readonly IRepository<EntityFramework.Core.Entitys.User> _userRepository;
        private readonly IRepository<PropertyGroup> _propertyGroupRepository;
        private readonly IRepository<PropertyGroupItem> _propertyGroupItemRepository;
        private readonly IRepository<ReaderEditProperty> _readerEditPropertyRepository;
        private readonly IRepository<Property> _propertyRepository;
        private readonly IRepository<InfoPermitReader> _infoPermitReaderRepository;
        private readonly IDistributedIDGenerator _idGenerator;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="configSetRepository"></param>
        /// <param name="groupRepository"></param>
        /// <param name="userGroupRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="propertyGroupRepository"></param>
        /// <param name="propertyGroupItemRepository"></param>
        /// <param name="readerEditPropertyRepository"></param>
        /// <param name="propertyRepository"></param>
        /// <param name="infoPermitReaderRepository"></param>
        /// <param name="idGenerator"></param>
        public BasicConfigService(IRepository<BasicConfigSet> configSetRepository
            , IRepository<Group> groupRepository
            , IRepository<UserGroup> userGroupRepository
            , IRepository<EntityFramework.Core.Entitys.User> userRepository
            , IRepository<PropertyGroup> propertyGroupRepository
            , IRepository<PropertyGroupItem> propertyGroupItemRepository
            , IRepository<ReaderEditProperty> readerEditPropertyRepository
            , IRepository<Property> propertyRepository
            , IRepository<InfoPermitReader> infoPermitReaderRepository
            , IDistributedIDGenerator idGenerator)
        {
            _configSetRepository = configSetRepository;
            _groupRepository = groupRepository;
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
            _propertyGroupRepository = propertyGroupRepository;
            _propertyGroupItemRepository = propertyGroupItemRepository;
            _readerEditPropertyRepository = readerEditPropertyRepository;
            _propertyRepository = propertyRepository;
            _infoPermitReaderRepository = infoPermitReaderRepository;
            _idGenerator = idGenerator;
        }

        /// <summary>
        /// 获取配置初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetBasicConfigInitData()
        {
            var basicConfigData = new BasicConfigSetDto();
            var userGroupQuery = from userGroup in _userGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                 join user in _userRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumUserStatus.正常) on userGroup.UserID equals user.Id into users
                                 from user in users
                                 select new UserGroupDto
                                 {
                                     GroupId = userGroup.GroupID,
                                     UserId = user.Id
                                 };

            var groupQuery = from groupInfo in _groupRepository.DetachedEntities.Where(x => !x.DeleteFlag).OrderBy(x => x.CreateTime)
                             select new GroupListDto
                             {
                                 ID = groupInfo.Id,
                                 Name = groupInfo.Name,
                                 Count = userGroupQuery.Where(x => x.GroupId == groupInfo.Id).Select(x => x.UserId).Distinct().Count()
                             };
            var groupList = await groupQuery.ToListAsync();
            var userTypeList = new List<UserTypeDto>();
            var userTypeGroup = await _propertyGroupRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Code == "User_Type");
            if (userTypeGroup != null)
            {
                userTypeList = await _propertyGroupItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.GroupID == userTypeGroup.Id).Select(x => new UserTypeDto
                {
                    GroupItemId = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Count = _userRepository.DetachedEntities.Where(u => !u.DeleteFlag && u.Status == (int)EnumUserStatus.正常 && u.Type == x.Name).Count()
                }).ToListAsync();
            }

            return new
            {
                basicConfigData,
                groupList,
                userTypeList
            };
        }

        /// <summary>
        /// 获取基础配置
        /// </summary>
        /// <returns></returns>
        public async Task<BasicConfigSetDto> GetBasicConfigSet()
        {
            var basicConfigEntity = await _configSetRepository.FirstOrDefaultAsync();
            if (basicConfigEntity == null)
            {
                throw Oops.Oh("未找到配置信息");
            }
            return basicConfigEntity.Adapt<BasicConfigSetDto>();
        }

        /// <summary>
        /// 修改基础配置
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public async Task<bool> SetBasicConfig(BasicConfigSetDto configData)
        {
            var basicConfigEntity = await _configSetRepository.FirstOrDefaultAsync();
            if (basicConfigEntity == null)
            {
                throw Oops.Oh("未找到配置信息");
            }
            basicConfigEntity = configData.Adapt(basicConfigEntity);
            await _configSetRepository.UpdateAsync(basicConfigEntity);
            return true;
        }

        /// <summary>
        /// 获取读者可编辑配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<ReaderEditPropertyDto>> QueryReaderEditProperty()
        {
            var resultList = new List<ReaderEditPropertyDto>();
            var editProperties = await _readerEditPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag).ToListAsync();
            var forUserProperties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForReader && x.SysBuildIn).ToListAsync();
            editProperties.ForEach(x =>
            {
                var mapUserProperty = forUserProperties.FirstOrDefault(p => p.Code == x.PropertyCode);
                if (mapUserProperty != null)
                {
                    var readerProperty = new ReaderEditPropertyDto
                    {
                        ID = x.Id,
                        PropertyCode = x.PropertyCode,
                        PropertyName = mapUserProperty.Name,
                        IsEnable = x.IsEnable,
                        IsCheck = x.IsCheck,
                        IsRequired = mapUserProperty.Required,
                        IsUnique = mapUserProperty.Unique,
                    };
                    resultList.Add(readerProperty);
                }
            });
            return resultList;
        }

        /// <summary>
        /// 设置读者可编辑字段
        /// </summary>
        /// <param name="editProperties"></param>
        /// <returns></returns>
        public async Task<bool> SetReaderEditProperty(List<ReaderEditPropertyDto> editProperties)
        {
            var existEditProperties = await _readerEditPropertyRepository.Where(x => !x.DeleteFlag).ToListAsync();
            existEditProperties.ForEach(x =>
            {
                var mapEditProperty = editProperties.FirstOrDefault(p => p.PropertyCode == x.PropertyCode);
                if (mapEditProperty != null)
                {
                    x.IsCheck = mapEditProperty.IsCheck;
                }
            });
            await _readerEditPropertyRepository.UpdateAsync(existEditProperties);
            return true;
        }

        /// <summary>
        /// 获取读者领卡用户
        /// </summary>
        /// <returns></returns>
        public async Task<List<InfoPermitReaderDto>> GetCardClaimReader()
        {
            var infoPermitReaders = await _infoPermitReaderRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ConfigType == (int)EnumConfigConfigType.读者领卡).OrderBy(x => x.CreateTime)
                .ProjectToType<InfoPermitReaderDto>().ToListAsync();
            return infoPermitReaders;
        }

        /// <summary>
        /// 设置读者领卡用户
        /// </summary>
        /// <param name="cardClaimReaders"></param>
        /// <returns></returns>
        public async Task<bool> SetCardClaimReader(List<InfoPermitReaderDto> cardClaimReaders)
        {
            var newReaderDatas = cardClaimReaders.Where(x => x.ConfigType == (int)EnumConfigConfigType.读者领卡).ToList();
            var updateBuilder = _infoPermitReaderRepository.Context.BatchUpdate<InfoPermitReader>();
            //删除所有以前的领卡读者
            updateBuilder.Set(b => b.DeleteFlag, b => true)
                .Where(x => !x.DeleteFlag && x.ConfigType == (int)EnumConfigConfigType.读者领卡);
            await updateBuilder.ExecuteAsync();
            //添加新的领卡读者
            var insertData = newReaderDatas.Adapt<List<InfoPermitReader>>();
            insertData.ForEach(x => x.Id = _idGenerator.CreateGuid());
            await _infoPermitReaderRepository.InsertAsync(insertData);
            return true;
        }

        /// <summary>
        /// 获取读者用户信息完善
        /// </summary>
        /// <returns></returns>
        public async Task<List<InfoPermitReaderDto>> GetInfoAppendReader()
        {
            var infoPermitReaders = await _infoPermitReaderRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ConfigType == (int)EnumConfigConfigType.信息完善).OrderBy(x => x.CreateTime)
               .ProjectToType<InfoPermitReaderDto>().ToListAsync();
            return infoPermitReaders;
        }

        /// <summary>
        /// 设置读者用户信息完善
        /// </summary>
        /// <param name="infoAppendReaders"></param>
        /// <returns></returns>
        public async Task<bool> SetInfoAppendReader(List<InfoPermitReaderDto> infoAppendReaders)
        {
            var newReaderDatas = infoAppendReaders.Where(x => x.ConfigType == (int)EnumConfigConfigType.信息完善).ToList();
            var updateBuilder = _infoPermitReaderRepository.Context.BatchUpdate<InfoPermitReader>();
            //删除所有以前的信息完善读者
            updateBuilder.Set(b => b.DeleteFlag, b => true)
                .Where(x => !x.DeleteFlag && x.ConfigType == (int)EnumConfigConfigType.信息完善);
            await updateBuilder.ExecuteAsync();
            //添加新的信息完善读者
            var insertData = newReaderDatas.Adapt<List<InfoPermitReader>>();
            insertData.ForEach(x => x.Id = _idGenerator.CreateGuid());
            await _infoPermitReaderRepository.InsertAsync(insertData);
            return true;
        }
    }
}
