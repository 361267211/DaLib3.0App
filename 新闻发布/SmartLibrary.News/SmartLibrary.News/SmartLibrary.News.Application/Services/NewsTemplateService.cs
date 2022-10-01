using DotNetCore.CAP;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.JsonSerialization;
using Grpc.Core;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.Dtos.Cap;
using SmartLibrary.News.Application.Enums;
using SmartLibrary.News.Application.Interceptors;
using SmartLibrary.News.Application.PageParam;
using SmartLibrary.News.Application.ViewModel;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using SmartLibrary.News.Utility;
using SmartLibrary.News.Utility.EnumTool;
using SmartLibrary.SceneManage;
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
    public class NewsTemplateService : INewsTemplateService, IScoped
    {
        private ICapPublisher _capPublisher;
        private IRepository<NewsTemplate> _templateRepository;
        private IRepository<NewsBodyTemplate> _bodyTemplateRepository;
        private IRepository<NewsColumn> _newsColumnRepository;
        private TenantInfo _tenantInfo;

        public NewsTemplateService(ICapPublisher capPublisher,
            IRepository<NewsTemplate> templateRepository,
            IRepository<NewsBodyTemplate> bodyTemplateRepository,
            IRepository<NewsColumn> newsColumnRepository,
        TenantInfo tenantInfo)
        {
            _capPublisher = capPublisher;
            _templateRepository = templateRepository;
            _bodyTemplateRepository = bodyTemplateRepository;
            _newsColumnRepository = newsColumnRepository;
            _tenantInfo = tenantInfo;
        }

        #region NewsTemplate 新闻模板管理
        /// <summary>
        /// 添加新闻模板
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> AddNewsTemplate(NewsTemplateDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            model.Id = Time2KeyUtils.GetRandOnlyId();
            var temp = model.ToModel<NewsTemplate>();
            temp.CreatedTime = DateTime.Now;
            var modelDB = await _templateRepository.InsertNowAsync(temp);
            return result;
        }

        /// <summary>
        /// 获取新闻模板
        /// </summary>
        /// <param name="tempIDs"></param>
        /// <returns></returns>
        public async Task<List<NewsTemplateDto>> GetNewsTemplate(string[] tempIDs)
        {
            var result = _templateRepository.Entities.Where(d => !d.DeleteFlag && ((tempIDs == null || tempIDs.Count() == 0) ? true : tempIDs.Contains(d.Id))
            );
            return await Task.FromResult(result.ToModelList<NewsTemplateDto>());
        }

        /// <summary>
        /// 获取新闻模板
        /// </summary>
        /// <param name="tempID"></param>
        /// <returns></returns>
        public async Task<NewsTemplateDto> GetSingleNewsTemplate(string tempID)
        {
            var result = _templateRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && tempID == d.Id
            );
            return await Task.FromResult(result.ToModel<NewsTemplateDto>());
        }

        /// <summary>
        /// 更新新闻模板
        /// </summary>
        /// <param name="templateDto"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> UpdateNewsTemplate(NewsTemplateDto templateDto)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var exists = _templateRepository.Any(d => d.Id == templateDto.Id);
            if (!exists)
            {
                result.Succeeded = false;
                result.Message = "新闻模板不存在！";
                return result;
            }

            var model = templateDto.ToModel<NewsTemplate>();
            model.UpdatedTime = DateTime.Now;
            var modelDB = await _templateRepository.UpdateAsync(model);
            return result;
        }

        /// <summary>
        /// 删除新闻模板
        /// </summary>
        /// <param name="tempID"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNewsTemplate(string tempID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var model = _templateRepository.FindOrDefault(tempID);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "新闻模板不存在！";
            }
            model.DeleteFlag = true;
            model.UpdatedTime = DateTime.Now;
            await _templateRepository.UpdateAsync(model);
            return result;
        }

        #region NewsBodyTemplate 新闻头尾模板
        /// <summary>
        /// 添加新闻头尾模板
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> AddNewsBodyTemplate(NewsBodyTemplateDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            NewsBodyTemplateTypeEnum? checkType = Converter.ToType<NewsBodyTemplateTypeEnum?>(model.Type, null);
            if (checkType == null)
            {
                result.Succeeded = false;
                result.Message = "头尾模板只存在头部（1）；尾部（2）！";
                return result;
            }
            model.Id = Time2KeyUtils.GetRandOnlyId();
            var temp = model.ToModel<NewsBodyTemplate>();
            temp.CreatedTime = DateTime.Now;
            var modelDB = await _bodyTemplateRepository.InsertNowAsync(temp);
            return result;
        }

        /// <summary>
        /// 获取新闻头尾模板
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<NewsBodyTemplateDto>> GetNewsBodyTemplate(int type)
        {
            var result = _bodyTemplateRepository.Entities.Where(d => !d.DeleteFlag && d.Type == type);

            var reply = await this.GetTemplateListGrpc(type.ToString());
            var templateList = reply.HeaderFooterList.ToList().Adapt<List<NewsBodyTemplateDto>>();


            return await Task.FromResult(templateList);
        }

        /// <summary>
        /// 获取新闻头尾模板
        /// </summary>
        /// <param name="tempBodyIDs"></param>
        /// <returns></returns>
        public async Task<List<NewsBodyTemplateDto>> GetNewsBodyTemplate(string[] tempBodyIDs)
        {
            var result = _bodyTemplateRepository.Entities.Where(d => !d.DeleteFlag && tempBodyIDs.Contains(d.Id)
            );
            return await Task.FromResult(result.ToModelList<NewsBodyTemplateDto>());
        }

        /// <summary>
        /// 更新新闻头尾模板
        /// </summary>
        /// <param name="bodyTemplateDto"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> UpdateNewsBodyTemplate(NewsBodyTemplateDto bodyTemplateDto)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            NewsBodyTemplateTypeEnum? checkType = Converter.ToType<NewsBodyTemplateTypeEnum?>(bodyTemplateDto.Type, null);
            if (checkType == null)
            {
                result.Succeeded = false;
                result.Message = "头尾模板只存在头部（1）；尾部（2）！";
                return result;
            }
            var exists = _bodyTemplateRepository.Any(d => d.Id == bodyTemplateDto.Id);
            if (!exists)
            {
                result.Succeeded = false;
                result.Message = "新闻头尾模板不存在！";
                return result;
            }

            var model = bodyTemplateDto.ToModel<NewsBodyTemplate>();
            model.UpdatedTime = DateTime.Now;
            var modelDB = await _bodyTemplateRepository.UpdateAsync(model);
            return result;
        }

        /// <summary>
        /// 删除新闻头尾模板
        /// </summary>
        /// <param name="tempBodyID"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNewsBodyTemplate(string tempBodyID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var model = _bodyTemplateRepository.FindOrDefault(tempBodyID);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "新闻头尾模板不存在！";
            }
            model.DeleteFlag = true;
            model.UpdatedTime = DateTime.Now;
            await _bodyTemplateRepository.UpdateAsync(model);
            return result;
        }

        /// <summary>
        /// 根据栏目Id取对应的头尾模板详情
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public async Task<HeadFootTemplateView> GetTemplateDetailByColumnId(string columnId)
        {
            var column = _newsColumnRepository.Find(columnId);
            var template = await GetTemplateDetailGrpc(column.HeadTemplate, column.FootTemplate);
            HeadFootTemplateView result = new HeadFootTemplateView();
            result.HeadTemplateModel = new NewsHeadFootTemplateDto
            {
                ApiRouter = template.ApiRouter,
                HeaderRouter = template.HeaderRouter,
                HeaderTemplateCode = template.HeaderTemplateCode,
            };
            result.FootTemplateModel = new NewsHeadFootTemplateDto
            {
                ApiRouter = template.ApiRouter,
                FooterRouter = template.FooterRouter,
                FooterTemplateCode = template.FooterTemplateCode,
            };

            return result;
        }

        /// <summary>
        /// grpc调用头尾模板的详情
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="footerId"></param>
        /// <returns></returns>
        public async Task<HeaderFooterReply> GetTemplateDetailGrpc(string headerId, string footerId)
        {
            //通过grpc服务获取尾部模板
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<SceneManageGrpcService.SceneManageGrpcServiceClient>();
            HeaderFooterRequest request = new HeaderFooterRequest { HeaderId = headerId, FooterId = footerId };
            try
            {
                var Reply = await grpcClient.GetHeaderFooterDetailAsync(request);
                return Reply;
            }
            catch (Exception ex)
            {
                throw Oops.Oh("场景中心出现异常");
            }
        }

        /// <summary>
        /// grpc调用头尾模板的详情
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<HeaderFooterListReply> GetTemplateListGrpc(string type)
        {
            //通过grpc服务获取尾部模板
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<SceneManageGrpcService.SceneManageGrpcServiceClient>();
            HeaderFooterListRequest request = new HeaderFooterListRequest { Type = type };
            try
            {
                var Reply = await grpcClient.GetHeaderFooterListAsync(request);
                return Reply;
            }
            catch (Exception ex)
            {
                throw Oops.Oh("场景中心出现异常");
            }
        }

        #endregion
        #endregion
    }
}
