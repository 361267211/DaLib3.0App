using DotNetCore.CAP;
using EFCore.BulkExtensions;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.JsonSerialization;
using Grpc.Core;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.Dtos.Cap;
using SmartLibrary.News.Application.Enums;
using SmartLibrary.News.Application.PageParam;
using SmartLibrary.News.Application.ViewModel;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using SmartLibrary.News.Utility;
using SmartLibrary.News.Utility.EnumTool;
using SmartLibraryNews;
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
    public class LableInfoService : NewsGrpcService.NewsGrpcServiceBase, ILableInfoService, IScoped
    {
        private ICapPublisher _capPublisher;
        private IRepository<LableInfo> _lableRepository;
        private IRepository<NewsColumn> _newsColumnRepository;
        private TenantInfo _tenantInfo;

        public LableInfoService(ICapPublisher capPublisher,
            IRepository<LableInfo> lableRepository,
            IRepository<NewsColumn> newsColumnRepository,
        TenantInfo tenantInfo)
        {
            _capPublisher = capPublisher;
            _lableRepository = lableRepository;
            _newsColumnRepository = newsColumnRepository;
            _tenantInfo = tenantInfo;
        }

        #region LableInfo 标签管理
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> AddLableInfo(LableInfoDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var lable = model.ToModel<LableInfo>();
            lable.CreatedTime = DateTime.Now;
            var modelDB = await _lableRepository.InsertNowAsync(lable);
            return result;
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<LableInfoDto>> GetLableInfo(int type)
        {
            var result = _lableRepository.Entities.Where(d => !d.DeleteFlag && d.Type == type
            );
            return await Task.FromResult(result.ToModelList<LableInfoDto>());
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="lableIDs"></param>
        /// <returns></returns>
        public async Task<List<LableInfoDto>> GetLableInfo(string[] lableIDs)
        {
            var result = _lableRepository.Entities.Where(d => !d.DeleteFlag && lableIDs.Contains(d.Id)
            );
            return await Task.FromResult(result.ToModelList<LableInfoDto>());
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="type"></param>
        /// <param name="updateParmList"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> UpdateLableInfo(int type, List<LableUpdateParm> updateParmList)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            List<string> updateIds = updateParmList.Select(d => d.Id).ToList();
            var deleteLables = _lableRepository.Where(d =>!d.DeleteFlag && d.Type== type && !updateIds.Contains(d.Id)).ToList();
           
            foreach (var item in updateParmList)
            {
                string lableID = item.Id;
                string name = item.Name;
                var model = _lableRepository.FindOrDefault(lableID);
                if (model == null)
                {
                    result.Succeeded = false;
                    result.Message = "标签不存在！";
                    return result;
                }

                model.Id = lableID;
                model.Name = name;
                model.UpdatedTime = DateTime.Now;
                var newsColumn = await _lableRepository.UpdateAsync(model);
            }
            //删除提交之外的标签
            //_lableRepository.Where(d => !updateIds.Contains(d.Id)).BatchUpdate(d => new LableInfo { DeleteFlag = true });
            foreach (var item in deleteLables)
            {
                await DeleteLableInfo(item.Id);
            }
            return result;
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="lableID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> UpdateLableInfo(string lableID, string name)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var model = _lableRepository.FindOrDefault(lableID);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "标签不存在！";
                return result;
            }

            model.Id = lableID;
            model.Name = name;
            model.UpdatedTime = DateTime.Now;
            var newsColumn = await _lableRepository.UpdateAsync(model);
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
            await _lableRepository.UpdateAsync(model);
            var colList = _newsColumnRepository.Where(a => !a.DeleteFlag && a.Label.Contains(model.Id)).ToList();
            foreach (var item in colList)
            {
                var labs = item.Label.Split(';').ToList();
                labs.Remove(model.Id);
                item.Label = string.Join(";", labs);
                await _newsColumnRepository.UpdateAsync(item);
            }
            return result;
        }

        /// <summary>
        /// 传入标签组字符串，返回对应标签主键
        /// </summary>
        /// <param name="type">1:新闻栏目，2:新闻内容</param>
        /// <param name="labels"></param>
        /// <returns></returns>
        public async Task<string> ProcessLablesFromLableStr(int type, string labels)
        {
            List<string> lableIDs = new List<string>();
            foreach (var lab in (labels??"").Split(';',StringSplitOptions.RemoveEmptyEntries).Distinct())
            {
                if (!string.IsNullOrEmpty(lab))
                {
                    var labModel = _lableRepository.FirstOrDefault(d => !d.DeleteFlag && d.Type == type && d.Name == lab);
                    if (labModel == null)
                    {
                        labModel = new LableInfo { Id = Time2KeyUtils.GetRandOnlyId(), Name = lab, Type = type, CreatedTime = DateTime.Now };
                        var labDB = await _lableRepository.InsertNowAsync(labModel);
                        lableIDs.Add(labDB.Entity.Id);
                    }
                    else
                        lableIDs.Add(labModel.Id);
                }
            }
            return await Task.FromResult( string.Join(";", lableIDs));
        }
        #endregion
    }
}
