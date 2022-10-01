using DotNetCore.CAP;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.Navigation.Application.PageParam;
using SmartLibrary.Navigation.EntityFramework.Core.Entitys;
using SmartLibrary.Navigation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SmartLibrary.Navigation.Application.Services
{
    /// <summary>
    /// 名    称：NavigationLableInfoService
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 16:33:20
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationLableInfoService: INavigationLableInfoService, IScoped
    {
        private ICapPublisher _capPublisher;
        private IRepository<NavigationLableInfo> _lableRepository;
        private IRepository<NavigationColumn> _columnRepository;
        private TenantInfo _tenantInfo;

        public NavigationLableInfoService(ICapPublisher capPublisher,
            IRepository<NavigationLableInfo> lableRepository,
            IRepository<NavigationColumn> columnRepository,
        TenantInfo tenantInfo)
        {
            _capPublisher = capPublisher;
            _lableRepository = lableRepository;
            _columnRepository = columnRepository;
            _tenantInfo = tenantInfo;
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <returns></returns>
        
        public async Task<List<LableUpdateParm>> GetLableList()
        {
            var lableList = from lab in _lableRepository.Where(d => !d.DeleteFlag)
                            select new LableUpdateParm { Id = lab.Id, Name = lab.Title };
            return await Task.FromResult(lableList.ToList());
        }

        /// <summary>
        /// 保存标签
        /// </summary>
        /// <param name="updateParmList"></param>
        /// <returns></returns>
        
        public async Task<ApiResultInfoModel> SaveLableInfo(List<LableUpdateParm> updateParmList)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            List<string> updateIds = updateParmList.Select(d => d.Id).ToList();
            var deleteLables = _lableRepository.Where(d => !d.DeleteFlag && !updateIds.Contains(d.Id)).ToList();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var item in updateParmList)
                    {
                        string lableID = item.Id;
                        string name = item.Name;
                        var model = _lableRepository.FindOrDefault(lableID);
                        if (model == null)//不存在则新增 存在则不处理
                        {
                            _lableRepository.InsertNow(new NavigationLableInfo { Id = Time2KeyUtils.GetRandOnlyId(), Title = name, CreatedTime = DateTime.Now });
                        }
                        else
                        {
                            model.Id = lableID;
                            model.Title = name;
                            model.UpdatedTime = DateTime.Now;
                            var newsColumn = await _lableRepository.UpdateAsync(model);
                        }
                    }
                    //删除提交之外的标签
                    //_lableRepository.Where(d => !updateIds.Contains(d.Id)).BatchUpdate(d => new LableInfo { DeleteFlag = true });
                    foreach (var item in deleteLables)
                    {
                        await DeleteLableInfo(item.Id);
                    }
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
        /// 删除标签
        /// </summary>
        /// <param name="lableID"></param>
        /// <returns></returns>
        
        public async Task<ApiResultInfoModel> DeleteLableInfo(string lableID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var model = _lableRepository.FindOrDefault(lableID);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "标签不存在！";
            }
            model.DeleteFlag = true;
            model.UpdatedTime = DateTime.Now;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _lableRepository.UpdateAsync(model);
                    var colList = _columnRepository.Where(a => !a.DeleteFlag && a.Label.Contains(model.Id)).ToList();
                    foreach (var item in colList)
                    {
                        var labs = item.Label.Split(';').ToList();
                        labs.Remove(model.Id);
                        item.Label = string.Join(";", labs);
                        await _columnRepository.UpdateAsync(item);
                    }
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
        /// 传入标签组字符串，返回对应标签主键
        /// </summary>
        /// <param name="labels"></param>
        /// <returns></returns>
        public async Task<string> ProcessLablesFromLableStr(string labels)
        {
            List<string> lableIDs = new List<string>();
            foreach (var lab in labels.Split(';').Distinct())
            {
                if (!string.IsNullOrEmpty(lab))
                {
                    var labModel = _lableRepository.FirstOrDefault(d => !d.DeleteFlag && d.Title == lab);
                    if (labModel == null)
                    {
                        labModel = new NavigationLableInfo { Id = Time2KeyUtils.GetRandOnlyId(), Title = lab,CreatedTime = DateTime.Now };
                        var labDB = await _lableRepository.InsertNowAsync(labModel);
                        lableIDs.Add(labDB.Entity.Id);
                    }
                    else
                        lableIDs.Add(labModel.Id);
                }
            }
            return await Task.FromResult(string.Join(";", lableIDs));
        }
    }
}
