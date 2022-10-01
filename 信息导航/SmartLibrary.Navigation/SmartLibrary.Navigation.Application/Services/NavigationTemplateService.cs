using DotNetCore.CAP;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using SmartLibrary.Navigation.Application.Interceptors;
using SmartLibrary.Navigation.Application.ViewModel;
using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using SmartLibrary.Navigation.EntityFramework.Core.Entitys;
using SmartLibrary.Navigation.EntityFramework.Core.Enum;
using SmartLibrary.Navigation.Utility;
using SmartLibrary.SceneManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    /// <summary>
    /// 名    称：NavigationTemplateService
    /// 作    者：张泽军
    /// 创建时间：2021/10/21 11:49:15
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationTemplateService : INavigationTemplateService, IScoped
    {
        private ICapPublisher _capPublisher;
        private IRepository<NavigationTemplate> _templateRepository;
        private IRepository<NavigationBodyTemplate> _bodyTemplateRepository;
        private TenantInfo _tenantInfo;

        public NavigationTemplateService(ICapPublisher capPublisher,
            IRepository<NavigationTemplate> templateRepository,
            IRepository<NavigationBodyTemplate> bodyTemplateRepository,
        TenantInfo tenantInfo)
        {
            _capPublisher = capPublisher;
            _templateRepository = templateRepository;
            _bodyTemplateRepository = bodyTemplateRepository;
            _tenantInfo = tenantInfo;
        }

        #region NavigationTemplate 信息导航模板管理
        /// <summary>
        /// 添加信息导航模板
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> AddNavigationTemplate(NavigationTemplateDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            model.Id = Time2KeyUtils.GetRandOnlyId();
            var temp = model.Adapt<NavigationTemplate>();
            temp.CreatedTime = DateTime.Now;
            var modelDB = await _templateRepository.InsertNowAsync(temp);
            return result;
        }

        /// <summary>
        /// 获取信息导航模板
        /// </summary>
        /// <param name="tempIDs"></param>
        /// <returns></returns>
        public async Task<List<NavigationTemplateDto>> GetNavigationTemplate(string[] tempIDs)
        {
            var result = _templateRepository.Entities.Where(d => !d.DeleteFlag && ((tempIDs == null || tempIDs.Count() == 0) ? true : tempIDs.Contains(d.Id))
            );
            return await Task.FromResult(result.Adapt<List<NavigationTemplateDto>>());
        }

        /// <summary>
        /// 获取信息导航模板
        /// </summary>
        /// <param name="tempID"></param>
        /// <returns></returns>
        public async Task<NavigationTemplateDto> GetSingleNavigationTemplate(string tempID)
        {
            var result = _templateRepository.Entities.FirstOrDefault(d => !d.DeleteFlag && tempID == d.Id
            );
            return await Task.FromResult(result.Adapt<NavigationTemplateDto>());
        }

        /// <summary>
        /// 更新信息导航模板
        /// </summary>
        /// <param name="templateDto"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> UpdateNavigationTemplate(NavigationTemplateDto templateDto)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var exists = _templateRepository.Any(d => d.Id == templateDto.Id);
            if (!exists)
            {
                result.Succeeded = false;
                result.Message = "信息导航模板不存在！";
                return result;
            }

            var model = templateDto.Adapt<NavigationTemplate>();
            model.UpdatedTime = DateTime.Now;
            var modelDB = await _templateRepository.UpdateAsync(model);
            return result;
        }

        /// <summary>
        /// 删除信息导航模板
        /// </summary>
        /// <param name="tempID"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNavigationTemplate(string tempID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var model = _templateRepository.FindOrDefault(tempID);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "信息导航模板不存在！";
            }
            model.DeleteFlag = true;
            model.UpdatedTime = DateTime.Now;
            await _templateRepository.UpdateAsync(model);
            return result;
        }

        #region NavigationBodyTemplate 信息导航头尾模板
        /// <summary>
        /// 添加信息导航头尾模板
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> AddNavigationBodyTemplate(NavigationBodyTemplateDto model)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            NavigationBodyTemplateTypeEnum? checkType = Converter.ToType<NavigationBodyTemplateTypeEnum?>(model.Type, null);
            if (checkType == null)
            {
                result.Succeeded = false;
                result.Message = "头尾模板只存在头部（1）；尾部（2）！";
                return result;
            }
            model.Id = Time2KeyUtils.GetRandOnlyId();
            var temp = model.Adapt<NavigationBodyTemplate>();
            temp.CreatedTime = DateTime.Now;
            var modelDB = await _bodyTemplateRepository.InsertNowAsync(temp);
            return result;
        }

        /// <summary>
        /// 获取信息导航头尾模板
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<NavigationBodyTemplateDto>> GetNavigationBodyTemplate(int type)
        {
            var result = _bodyTemplateRepository.Entities.Where(d => !d.DeleteFlag && d.Type == type
            );
            return await Task.FromResult(result.Adapt<List<NavigationBodyTemplateDto>>());
        }

        /// <summary>
        /// 获取信息导航头尾模板
        /// </summary>
        /// <param name="tempBodyIDs"></param>
        /// <returns></returns>
        public async Task<List<NavigationBodyTemplateDto>> GetNavigationBodyTemplate()
        {
            var result = _bodyTemplateRepository.Entities.Where(d => !d.DeleteFlag  );
            return    result.Adapt<List<NavigationBodyTemplateDto>>();
        }

        /// <summary>
        /// 更新信息导航头尾模板
        /// </summary>
        /// <param name="bodyTemplateDto"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> UpdateNavigationBodyTemplate(NavigationBodyTemplateDto bodyTemplateDto)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            NavigationBodyTemplateTypeEnum? checkType = Converter.ToType<NavigationBodyTemplateTypeEnum?>(bodyTemplateDto.Type, null);
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
                result.Message = "信息导航头尾模板不存在！";
                return result;
            }

            var model = bodyTemplateDto.Adapt<NavigationBodyTemplate>();
            model.UpdatedTime = DateTime.Now;
            var modelDB = await _bodyTemplateRepository.UpdateAsync(model);
            return result;
        }

        /// <summary>
        /// 删除信息导航头尾模板
        /// </summary>
        /// <param name="tempBodyID"></param>
        /// <returns></returns>
        public async Task<ApiResultInfoModel> DeleteNavigationBodyTemplate(string tempBodyID)
        {
            var result = new ApiResultInfoModel { Succeeded = true };
            var model = _bodyTemplateRepository.FindOrDefault(tempBodyID);
            if (model == null)
            {
                result.Succeeded = false;
                result.Message = "信息导航头尾模板不存在！";
            }
            model.DeleteFlag = true;
            model.UpdatedTime = DateTime.Now;
            await _bodyTemplateRepository.UpdateAsync(model);
            return result;
        }

        /// <summary>
        /// 获取所有头尾模板列表
        /// </summary>
        /// <returns></returns>
        public async Task<HeadFootTemplateListView> AllHeadFootTemplateGet()
        {
            HeadFootTemplateListView headFootTemplateView = new HeadFootTemplateListView();
            headFootTemplateView.HeadTemplateModels = (await HeadFootTemplateGrpcGet("2")).Adapt<List<NavigationTemplateDto>>();
            headFootTemplateView.FootTemplateModels = (await HeadFootTemplateGrpcGet("3")).Adapt<List<NavigationTemplateDto>>();
            return headFootTemplateView;
        }

        /// <summary>
        /// 获取所有头尾模板列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<HeaderFooterListSingle>> HeadFootTemplateGrpcGet(string type)
        {

            //通过grpc服务获取尾部模板
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<SceneManageGrpcService.SceneManageGrpcServiceClient>();
            HeaderFooterListRequest request = new HeaderFooterListRequest { Type = type };
            try
            {
                var Reply = await grpcClient.GetHeaderFooterListAsync(request);
                var result = Reply.HeaderFooterList.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw Oops.Oh("场景中心服务出现异常");
            }
        }

        public async Task<HeadFootTemplateDetailView> HeadFootTemplateDetailGet(string headerId, string footerId)
        {
            HeadFootTemplateDetailView headFootTemplateDetailView = new HeadFootTemplateDetailView()
            {
                HeadTemplateModel=new NavigationTemplateDto(),
                FootTemplateModel=new NavigationTemplateDto(),
            };

            HeaderFooterRequest request = new HeaderFooterRequest { HeaderId = headerId, FooterId = footerId };

            //通过grpc服务获取尾部模板
            var grpcClient = App.GetService<IGrpcClientResolver>().EnsureClient<SceneManageGrpcService.SceneManageGrpcServiceClient>();

            try
            {
                var Reply = await grpcClient.GetHeaderFooterDetailAsync(request);
                headFootTemplateDetailView.HeadTemplateModel.ApiRouter = Reply.ApiRouter;
                headFootTemplateDetailView.HeadTemplateModel.Router = Reply.HeaderRouter;
                headFootTemplateDetailView.HeadTemplateModel.TemplateCode = Reply.HeaderTemplateCode;

                headFootTemplateDetailView.FootTemplateModel.ApiRouter = Reply.ApiRouter;
                headFootTemplateDetailView.FootTemplateModel.Router = Reply.FooterRouter;
                headFootTemplateDetailView.FootTemplateModel.TemplateCode = Reply.FooterTemplateCode;
                return headFootTemplateDetailView;
            }
            catch (Exception ex)
            {
                throw Oops.Oh($"场景中心服务出现异常{ex}");
            }
        }
        #endregion
        #endregion
    }
}
