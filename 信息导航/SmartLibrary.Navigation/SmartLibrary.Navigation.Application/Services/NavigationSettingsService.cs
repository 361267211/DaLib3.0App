using DotNetCore.CAP;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibrary.Navigation.Application.Enums;
using SmartLibrary.Navigation.Application.PageParam;
using SmartLibrary.Navigation.Application.ViewModel;
using SmartLibrary.Navigation.EntityFramework.Core.Entitys;
using SmartLibrary.Navigation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Mapster;
using SmartLibrary.Navigation.Application.Interceptors;
using SmartLibrary.User.RpcService;
using Furion;
using SmartLibrary.AppCenter;

namespace SmartLibrary.Navigation.Application.Services
{
    /// <summary>
    /// 名    称：NavigationSettingsService
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 11:27:35
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationSettingsService : INavigationSettingsService, IScoped
    {
        private ICapPublisher _capPublisher;
        private IRepository<NavigationSettings> _settingsRepository;
        private IRepository<NavigationColumnPermissions> _permissionRepository;
        private IRepository<NavigationColumn> _navigationColumnRepository;
        private TenantInfo _tenantInfo;

        public NavigationSettingsService(ICapPublisher capPublisher,
            IRepository<NavigationSettings> settingsRepository,
            IRepository<NavigationColumnPermissions> permissionRepository,
            IRepository<NavigationColumn> navigationColumnRepository,
        TenantInfo tenantInfo)
        {
            _capPublisher = capPublisher;
            _settingsRepository = settingsRepository;
            _permissionRepository = permissionRepository;
            _navigationColumnRepository = navigationColumnRepository;
            _tenantInfo = tenantInfo;
        }

        #region 应用设置&栏目权限设置
        /// <summary>
        /// 保存应用设置
        /// </summary>
        /// <returns></returns>
        
        public async Task<ApiResultInfoModel> SaveNavigationSettings(NavigationSettingsDto model)
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
                var settings = model.Adapt<NavigationSettings>();
                settings.CreatedTime = DateTime.Now;
                var modelDB = await _settingsRepository.InsertNowAsync(settings);
            }
            return result;
        }

        /// <summary>
        /// 获取应用设置
        /// </summary>
        /// <returns></returns>
        
        public async Task<NavigationSettingsDto> GetNavigationSettings()
        {
            var model = await _settingsRepository.FirstOrDefaultAsync();
            if (model == null)
            {
                model = new NavigationSettings();
            }
            return model.Adapt<NavigationSettingsDto>();
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
        
        public async Task<ApiResultInfoModel> SaveNavigationColumnPermissions(NavigationColumnPermissionsParam model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            AuditPowerEunm? temp = Converter.ToType<AuditPowerEunm?>(model.Permission, null);
            if (temp == null)
            {
                result.Succeeded = false;
                result.Message = "权限选项不存在";
                return result;
            }
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var item in model.ManagerList)
                    {
                        var permissions = _permissionRepository.FirstOrDefault(d => !d.DeleteFlag && d.ColumnID == model.ColumnID && d.Permission == model.Permission && d.ManagerID == item.ManagerID);
                        if (permissions == null)
                        {
                            permissions = new NavigationColumnPermissions();
                            permissions.Id = Time2KeyUtils.GetRandOnlyId();
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
                    var processResult = _permissionRepository.Context.BatchUpdate<NavigationColumnPermissions>()
                       .Set(b => b.DeleteFlag, b => true)
                       .Set(b => b.UpdatedTime, b => DateTime.Now)
                       .Where(d => !d.DeleteFlag && d.ColumnID == model.ColumnID && d.Permission == model.Permission && !managerIDs.Contains(d.ManagerID)).Execute();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    result.Succeeded = false;
                    result.Message = ex.Message;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取栏目某一权限管理员列表
        /// </summary>
        /// <param name="columnID"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        
        public async Task<List<ManagerParam>> GetNavigationColumnPermissionsByColumnPerm(string columnID, int permission)
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
        
        public async Task<ApiResultInfoModel> DeleteNavigationColumnPermissionsByColumnID(string columnID, bool isIncludeManage = false)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var temp = await _permissionRepository.Context.BatchUpdate<NavigationColumnPermissions>()
                      .Set(b => b.DeleteFlag, b => true)
                      .Set(b => b.UpdatedTime, b => DateTime.Now)
                      .Where(d => d.ColumnID == columnID && (isIncludeManage ? true : d.Permission != 0)).ExecuteAsync();
            return result;
        }

        /// <summary>
        /// 获取全部栏目及其管理权限的管理者
        /// </summary>
        /// <returns></returns>
        public async Task<List<NavigationColumnPermissionsView>> GetNavigationColumnPermissionsList()
        {
            List<NavigationColumnPermissionsView> result = new List<NavigationColumnPermissionsView>();
            var columnList = _navigationColumnRepository.Entities.Where(d => !d.DeleteFlag).ToList();
            int i = 1;
            foreach (var item in columnList)
            {
                NavigationColumnPermissionsView perm = new NavigationColumnPermissionsView();
                perm.ColumnID = item.Id;
                perm.IndexNum = i;
                perm.Title = item.Title;
                List<KeyValuePair<string, string>> temp = new List<KeyValuePair<string, string>>();
                var managerList = _permissionRepository.Entities.Where(d => d.ColumnID == item.Id && d.Permission == ((int)AuditPowerEunm.Manage)).ToList();
                foreach (var manager in managerList)
                {
                    if (!temp.Exists(d => d.Key == manager.ManagerID))
                        temp.Add(new KeyValuePair<string, string>(manager.ManagerID, manager.Manager));
                }
                perm.ManagerList = temp;
                result.Add(perm);
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取当前登录用户有权限的全部栏目KV
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetColumnKVByUserKey(string userKey)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(userKey))
            {
                var columnKV = from perm in _permissionRepository.Where(d => !d.DeleteFlag && d.ManagerID == userKey)
                               join col in _navigationColumnRepository.AsQueryable() on perm.ColumnID equals col.Id
                               select new KeyValuePair<string, string>(perm.ColumnID, col.Title);
                result = columnKV.ToList();
            }
            else
            {
                var columnKV = from col in _navigationColumnRepository.AsQueryable(d => !d.DeleteFlag)
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


            //TODO:调grpc 获取用户的名称及信息
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            ManagerListRequest request = new ManagerListRequest
            {
                RouteCode = "assembly",
            };

            var reply = await grpcClient.GetManagerListByCodeAsync(request);


            var managerList = reply.ManagerList;
            //关键词筛选
            var managers = managerList.Where(e => e.CardNo.Contains(keyWord) || e.Name.Contains(keyWord) || e.NickName.Contains(keyWord) || e.DepartName.Contains(keyWord) || e.CollegeDepartName.Contains(keyWord) || e.CollegeName.Contains(keyWord) || e.Email.Contains(keyWord)).ToList();

            return managers.Adapt<List<PermissionManagerInfo>>();


        }
        #endregion
    }
}
