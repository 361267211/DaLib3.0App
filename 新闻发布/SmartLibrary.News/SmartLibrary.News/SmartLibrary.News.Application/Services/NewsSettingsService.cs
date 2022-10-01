using DotNetCore.CAP;
using EFCore.BulkExtensions;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.JsonSerialization;
using Grpc.Core;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartLibrary.AppCenter;
using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.Dtos.Cap;
using SmartLibrary.News.Application.Enums;
using SmartLibrary.News.Application.Interceptors;
using SmartLibrary.News.Application.PageParam;
using SmartLibrary.News.Application.ViewModel;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using SmartLibrary.News.Utility;
using SmartLibrary.News.Utility.EnumTool;
using SmartLibrary.User.RpcService;
using SmartLibraryNews;
//using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SmartLibrary.News.Application.Services
{
    /// <summary>
    /// 名    称：NewsService
    /// 作    者：张泽军
    /// 创建时间：2021/9/7 19:29:20
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsSettingsService : INewsSettingsService, IScoped
    {
        private ICapPublisher _capPublisher;
        private IRepository<NewsSettings> _settingsRepository;
        private IRepository<NewsColumnPermissions> _permissionRepository;
        private IRepository<NewsColumn> _newsColumnRepository;
        private TenantInfo _tenantInfo;

        public NewsSettingsService(ICapPublisher capPublisher,
            IRepository<NewsSettings> settingsRepository,
            IRepository<NewsColumnPermissions> permissionRepository,
            IRepository<NewsColumn> newsColumnRepository,
        TenantInfo tenantInfo)
        {
            _capPublisher = capPublisher;
            _settingsRepository = settingsRepository;
            _permissionRepository = permissionRepository;
            _newsColumnRepository = newsColumnRepository;
            _tenantInfo = tenantInfo;
        }

        #region 应用设置&栏目权限设置
        /// <summary>
        /// 保存应用设置
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> SaveNewsSettings(NewsSettingsDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };

            var modelOld = _settingsRepository.FirstOrDefault();
            if (modelOld != null)
            {
                modelOld.UpdatedTime = DateTime.Now;
                modelOld.SensitiveWords = model.SensitiveWords;
                modelOld.Comments = model.Comments;
                var modelDB = await _settingsRepository.UpdateNowAsync(modelOld);
            }
            else
            {
                model.Id = Time2KeyUtils.GetRandOnlyId();
                var settings = model.ToModel<NewsSettings>();
                settings.CreatedTime = DateTime.Now;
                var modelDB = await _settingsRepository.InsertNowAsync(settings);
            }
            return result;
        }

        /// <summary>
        /// 获取应用设置
        /// </summary>
        /// <returns></returns>
        public async Task<NewsSettingsDto> GetNewsSettings()
        {
            var model = await _settingsRepository.FirstOrDefaultAsync();
            if (model == null)
            {
                model = new NewsSettings();
            }
            return model.ToModel<NewsSettingsDto>();
        }

        /// <summary>
        /// 调用数据中心接口返回是否包含敏感词
        /// </summary>
        /// <param name="checkStr"></param>
        /// <returns></returns>
        public bool CheckSensitiveWords(string checkStr)
        {
            //TODO 调用数据中心接口返回是否包含敏感词
            return false;
        }

        /// <summary>
        /// 保存栏目权限管理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> SaveNewsColumnPermissions(NewsColumnPermissionsParam model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            AuditPowerEunm? temp = Converter.ToType<AuditPowerEunm?>(model.Permission, null);
            if (temp == null)
            {
                result.Succeeded = false;
                result.Message = "权限选项不存在";
                return result;
            }
            foreach (var item in model.ManagerList)
            {
                var permissions = _permissionRepository.FirstOrDefault(d => !d.DeleteFlag && d.ColumnID == model.ColumnID && d.Permission == model.Permission && d.ManagerID == item.ManagerID);
                if (permissions == null)
                {
                    permissions = new NewsColumnPermissions();
                    permissions.Id = Time2KeyUtils.GetRandOnlyId();
                    permissions.Permission = model.Permission;
                    permissions.ColumnID = model.ColumnID;
                    permissions.Manager = item.Manager;
                    permissions.ManagerID = item.ManagerID;
                    permissions.CreatedTime = DateTime.Now;
                    var modelDB = await _permissionRepository.InsertNowAsync
(permissions);
                }
                //else
                //{
                //    permissions.Manager = item.Manager;
                //    permissions.UpdatedTime = DateTime.Now;
                //    await _permissionRepository.UpdateNowAsync(permissions);
                //}
            }
            //删除不存在的
            string managerIDs = string.Join(";", model.ManagerList.Select(d => d.ManagerID));
            //var processResult = _permissionRepository.Where(d => !d.DeleteFlag && d.ColumnID == model.ColumnID && d.Permission == model.Permission && !managerIDs.Contains(d.ManagerID)).BatchUpdateAsync(new NewsColumnPermissions { DeleteFlag = true, UpdatedTime = DateTime.Now });
            _permissionRepository.Context.BatchUpdate<NewsColumnPermissions>()
                .Set(d => d.DeleteFlag, d => true)
                .Set(d => d.UpdatedTime, d => DateTime.Now)
                .Where(d => !d.DeleteFlag && d.ColumnID == model.ColumnID && d.Permission == model.Permission && !managerIDs.Contains(d.ManagerID/*.ToString()*/)).Execute();
            return result;
        }

        /// <summary>
        /// 获取栏目某一权限管理员列表
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public async Task<List<ManagerParam>> GetNewsColumnPermissionsByColumnPerm(string columnID, int permission)
        {
            var result = from perm in _permissionRepository.Where(d => !d.DeleteFlag && d.ColumnID == columnID && d.Permission == permission)
                         select new ManagerParam { Manager = perm.Manager, ManagerID = perm.ManagerID };
            return await Task.FromResult(result.ToList());
        }

        /// <summary>
        /// 删除某一栏目的权限管理
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="isIncludeManage"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNewsColumnPermissionsByColumnID(string columnID, bool isIncludeManage = false)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var temp = await _permissionRepository.Where(d => d.ColumnID == columnID && (isIncludeManage ? true : d.Permission != 0)).BatchUpdateAsync(new NewsColumnPermissions { UpdatedTime = DateTime.Now, DeleteFlag = true });
            return result;
        }

        /// <summary>
        /// 获取栏目对应的权限设置
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<List<NewsSingleColumnPermissionsView>> GetNewsColumnPermissionsByColumnID(string columnID)
        {
            List<NewsSingleColumnPermissionsView> result = new List<NewsSingleColumnPermissionsView>();
            var thisColumn = _newsColumnRepository.FindOrDefault(columnID);
            if (thisColumn != null)
            {
                var auditFlow = thisColumn.AuditFlow.Split(';');
                foreach (var item in auditFlow)
                {
                    NewsSingleColumnPermissionsView permissionsView = new NewsSingleColumnPermissionsView();
                    AuditProcessEnum process = Converter.ToType<AuditProcessEnum>(Converter.ObjectToInt(item));
                    switch (process)
                    {
                        case AuditProcessEnum.Edit:
                            {
                                permissionsView.AuditProcessStatus = ((int)AuditPowerEunm.Edit);
                                permissionsView.AuditProcessName = EnumUtils.GetName(AuditPowerEunm.Edit);
                                break;
                            }
                        case AuditProcessEnum.Submit://提交流程对应的权限同编辑流程
                            continue;
                        case AuditProcessEnum.PreliminaryAudit:
                            {
                                permissionsView.AuditProcessStatus = ((int)AuditPowerEunm.PreliminaryAudit);
                                permissionsView.AuditProcessName = EnumUtils.GetName(AuditPowerEunm.PreliminaryAudit);
                                break;
                            }
                        case AuditProcessEnum.PreliminaryCheck:
                            {
                                permissionsView.AuditProcessStatus = ((int)AuditPowerEunm.PreliminaryCheck);
                                permissionsView.AuditProcessName = EnumUtils.GetName(AuditPowerEunm.PreliminaryCheck);
                                break;
                            }
                        case AuditProcessEnum.SecondAudit:
                            {
                                permissionsView.AuditProcessStatus = ((int)AuditPowerEunm.SecondAudit);
                                permissionsView.AuditProcessName = EnumUtils.GetName(AuditPowerEunm.SecondAudit);
                                break;
                            }
                        case AuditProcessEnum.SecondCheck:
                            {
                                permissionsView.AuditProcessStatus = ((int)AuditPowerEunm.SecondCheck);
                                permissionsView.AuditProcessName = EnumUtils.GetName(AuditPowerEunm.SecondCheck);
                                break;
                            }
                        case AuditProcessEnum.FinallyAudit:
                            {
                                permissionsView.AuditProcessStatus = ((int)AuditPowerEunm.FinallyAudit);
                                permissionsView.AuditProcessName = EnumUtils.GetName(AuditPowerEunm.FinallyAudit);
                                break;
                            }
                        case AuditProcessEnum.FinallyCheck:
                            {
                                permissionsView.AuditProcessStatus = ((int)AuditPowerEunm.FinallyCheck);
                                permissionsView.AuditProcessName = EnumUtils.GetName(AuditPowerEunm.FinallyCheck);
                                break;
                            }
                        case AuditProcessEnum.Publish:
                            {
                                permissionsView.AuditProcessStatus = ((int)AuditPowerEunm.Publish);
                                permissionsView.AuditProcessName = EnumUtils.GetName(AuditPowerEunm.Publish);
                                break;
                            }
                        default:
                            break;
                    }
                    if (process == AuditProcessEnum.Edit)
                        process = AuditProcessEnum.Submit;
                    int thisPermission = ((int)process);
                    var listTemp = from perm in _permissionRepository.Where(d => !d.DeleteFlag && d.ColumnID == columnID && d.Permission == thisPermission)
                                   select new ManagerParam { Manager = perm.Manager, ManagerID = perm.ManagerID };
                    permissionsView.ListPermissions = listTemp.ToList();
                    result.Add(permissionsView);
                }
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取全部栏目及其管理权限的管理者
        /// </summary>
        /// <returns></returns>
        public async Task<List<NewsColumnPermissionsView>> GetNewsColumnPermissionsList()
        {
            List<NewsColumnPermissionsView> result = new List<NewsColumnPermissionsView>();
            var columnList = _newsColumnRepository.Entities.Where(d => !d.DeleteFlag).ToList();
            int i = 1;
            foreach (var item in columnList)
            {
                NewsColumnPermissionsView perm = new NewsColumnPermissionsView();
                perm.ColumnID = item.Id;
                perm.IndexNum = i;
                perm.Title = item.Title;
                List<KeyValuePair<string, string>> temp = new List<KeyValuePair<string, string>>();
                //var managerList = _permissionRepository.Entities.Where(d => d.ColumnID == item.Id && d.Permission==((int)AuditPowerEunm.Manage)).ToList();
                //foreach (var manager in managerList)
                //{
                //    if (!temp.Exists(d => d.Key == manager.ManagerID))
                //        temp.Add(new KeyValuePair<string, string>(manager.ManagerID, manager.Manager));
                //}
                var listTemp = from permission in _permissionRepository.Entities.Where(d =>!d.DeleteFlag&& d.ColumnID == item.Id && d.Permission == ((int)AuditPowerEunm.Manage))
                               select new ManagerParam { Manager = permission.Manager, ManagerID = permission.ManagerID };
                perm.ManagerList = listTemp.ToList();
                result.Add(perm);
            }
            return await Task.FromResult(result);
        }

        public async Task<Dictionary<int, string>> GetPermissionColumnList(string managerID)
        {
            var result = from perm in _permissionRepository.Where(d => !d.DeleteFlag && (string.IsNullOrEmpty(managerID) ? true : d.ManagerID == managerID))
                         group perm by new { perm.Permission, perm.ColumnID } into dataGropu
                         select new KeyValuePair<int, string>(dataGropu.Key.Permission, dataGropu.Key.ColumnID);
            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var item in result.ToList())
            {
                //管理权限对应该栏目所有设定的审核流程权限
                if (item.Key == 0)
                {
                    var column = _newsColumnRepository.Entities.FirstOrDefault(d => d.Id == item.Value && !d.DeleteFlag);
                    if (column != null && column.IsOpenAudit == 1)
                    {
                        var listPower = GetColumnAuditPowerList(column.Id);
                        foreach (var power in listPower)
                        {
                            if (dic.Keys.Contains(((int)power)))
                                dic[((int)power)] += ";" + item.Value;
                            else
                                dic.Add(((int)power), item.Value);
                        }
                    }
                }
                else
                {
                    if (dic.Keys.Contains(item.Key))
                        dic[item.Key] += ";" + item.Value;
                    else
                        dic.Add(item.Key, item.Value);
                }
            }
            return await Task.FromResult(dic);
        }

        /// <summary>
        /// 获取栏目对应的审核权限集合
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public List<AuditPowerEunm> GetColumnAuditPowerList(string columnID)
        {
            List<AuditPowerEunm> list = new List<AuditPowerEunm>();
            var thisColumn = _newsColumnRepository.FindOrDefault(columnID);
            if (thisColumn != null)
            {
                var auditFlow = thisColumn.AuditFlow.Split(';');
                foreach (var item in auditFlow)
                {
                    AuditProcessEnum process = Converter.ToType<AuditProcessEnum>(Converter.ObjectToInt(item));
                    switch (process)
                    {
                        case AuditProcessEnum.Edit:
                            {
                                list.Add(AuditPowerEunm.Edit);
                                break;
                            }
                        case AuditProcessEnum.Submit://提交流程对应的权限同编辑流程
                            continue;
                        case AuditProcessEnum.PreliminaryAudit:
                            {
                                list.Add(AuditPowerEunm.PreliminaryAudit);
                                break;
                            }
                        case AuditProcessEnum.PreliminaryCheck:
                            {
                                list.Add(AuditPowerEunm.PreliminaryCheck);
                                break;
                            }
                        case AuditProcessEnum.SecondAudit:
                            {
                                list.Add(AuditPowerEunm.SecondAudit);
                                break;
                            }
                        case AuditProcessEnum.SecondCheck:
                            {
                                list.Add(AuditPowerEunm.SecondCheck);
                                break;
                            }
                        case AuditProcessEnum.FinallyAudit:
                            {
                                list.Add(AuditPowerEunm.FinallyAudit);
                                break;
                            }
                        case AuditProcessEnum.FinallyCheck:
                            {
                                list.Add(AuditPowerEunm.FinallyCheck);
                                break;
                            }
                        case AuditProcessEnum.Publish:
                            {
                                list.Add(AuditPowerEunm.Publish);
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取当前登录用户某一栏目的权限关联列表
        /// </summary>
        /// <param name="columnID"></param>
        /// <returns></returns>
        public async Task<List<AuditPowerEunm>> GetColumnAuditPowerListByUserKey(string columnID)
        {
            List<AuditPowerEunm> list = new List<AuditPowerEunm>();
            var thisColumn = _newsColumnRepository.FindOrDefault(columnID);
            var columnPermissions = _permissionRepository.Where(d => !d.DeleteFlag && d.ColumnID == columnID && d.ManagerID == _tenantInfo.UserKey);
            foreach (var item in columnPermissions)
            {
                AuditPowerEunm auditPower = Converter.ToType<AuditPowerEunm>(item.Permission);
                list.Add(auditPower);
            }
            return await Task.FromResult(list);
        }

        /// <summary>
        /// 获取当前登录用户有权限的全部栏目ID
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetColumnIDsByUserKey()
        {
            var columnIDs = _permissionRepository.Where(d => !d.DeleteFlag && d.ManagerID == _tenantInfo.UserKey).Select(d => d.ColumnID);
            return await Task.FromResult(columnIDs.ToList());
        }

        /// <summary>
        /// 获取当前登录用户有权限的全部栏目KV
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetColumnKVBuUserKey(string userKey)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            if (string.IsNullOrEmpty(userKey))
            {
                var columnKV = from perm in _permissionRepository.Where(d => !d.DeleteFlag && d.ManagerID == userKey)
                               join col in _newsColumnRepository.AsQueryable() on perm.ColumnID equals col.Id
                               select new KeyValuePair<string, string>(perm.ColumnID, col.Title);
                result = columnKV.ToList();
            }
            else
            {
                var columnKV = from col in _newsColumnRepository.AsQueryable(d=>!d.DeleteFlag)
                               select new KeyValuePair<string, string>(col.Id, col.Title);
                result = columnKV.ToList();
            }
            return await Task.FromResult(result.Distinct().ToList());
        }

        /// <summary>
        /// 检索获取权限管理员
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task<List<PermissionManagerInfo>> SearchPermissionManager(string keyWord)
        {

            //调grpc 获取用户的名称及信息
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            ManagerListRequest request = new ManagerListRequest
            {
                RouteCode = "news",
            };

            var reply = await grpcClient.GetManagerListByCodeAsync(request);


            var managerList = reply.ManagerList;
            //关键词筛选
            var managers = managerList.Where(e => e.CardNo.Contains(keyWord) || e.Name.Contains(keyWord) || e.NickName.Contains(keyWord) || e.DepartName.Contains(keyWord) || e.CollegeDepartName.Contains(keyWord) || e.CollegeName.Contains(keyWord) || e.Email.Contains(keyWord) || e.Phone.Contains(keyWord)).ToList();

            return managers.Adapt<List<PermissionManagerInfo>>();
        }
        #endregion
    }
}
