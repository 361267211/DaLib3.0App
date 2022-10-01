/*********************************************************
 * 名    称：SceneManageGrpcAppService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/12/6 13:27:57
 * 描    述：中台grpc服务
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DependencyInjection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using SmartLibrary.SceneManage.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Application.Grpc.AppService
{
    public class SceneManageGrpcAppService : SceneManageGrpcService.SceneManageGrpcServiceBase, IScoped
    {
        /// <summary>
        /// 场景管理服务
        /// </summary>
        private ISceneManageService _sceneManageService;

        public SceneManageGrpcAppService(ISceneManageService sceneManageService)
        {
            _sceneManageService = sceneManageService;
        }

        /// <summary>
        /// 获取默认头尾模板
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<HeaderFooterReply> GetDefaultHeaderFooter(Empty request, ServerCallContext context)
        {
            var hf = await _sceneManageService.GetDefaultTemplateList();

            return new HeaderFooterReply
            {
                ApiRouter = hf.ApiRouter,
                FooterRouter = hf?.FooterTemplate?.Router ?? "",
                FooterTemplateCode = hf?.FooterTemplate?.TemplateCode ?? "",
                HeaderRouter = hf?.HeaderTemplate?.Router ?? "",
                HeaderTemplateCode = hf?.HeaderTemplate?.TemplateCode ?? "",
                ThemeColor = hf?.ThemeColor,
                UserCenterName = hf?.UserCenterName 
            };
        }

        /// <summary>
        /// 获取头尾模板列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<HeaderFooterListReply> GetHeaderFooterList(HeaderFooterListRequest request, ServerCallContext context)
        {
            var list = await _sceneManageService.GetTemplateList(new TemplateListQuery { Type = int.Parse(request.Type) });

            var result = new HeaderFooterListReply();
            result.HeaderFooterList.AddRange(list.Items.Select(p => new HeaderFooterListSingle
            {
                Cover = p.Cover,
                Id = p.Id,
                Name = p.Name
            }));
            return result;
        }

        /// <summary>
        /// 获取头尾模板详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<HeaderFooterReply> GetHeaderFooterDetail(HeaderFooterRequest request, ServerCallContext context)
        {
            var header = string.IsNullOrEmpty(request.HeaderId) ? null : await _sceneManageService.GetTemplateDetail(request.HeaderId);
            var footer = string.IsNullOrEmpty(request.FooterId) ? null : await _sceneManageService.GetTemplateDetail(request.FooterId);

            return new HeaderFooterReply
            {
                ApiRouter = SiteGlobalConfig.AppBaseConfig.AppRouteCode,
                FooterRouter = footer?.Router ?? "",
                FooterTemplateCode = footer?.TemplateCode ?? "",
                HeaderRouter = header?.Router ?? "",
                HeaderTemplateCode = header?.TemplateCode ?? "",
                FooterContent=footer?.Content??"",
                FooterJsPath=footer?.Content ?? "",
                HeaderLogo=header?.Logo ?? "",
                HeaderNavList=header?.DisplayNavColumn ?? "",
            };
        }

        /// <summary>
        /// 获取栏目使用情况
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<ColumnUseageListReply> GetColumnUseageList(ColumnUseageListRequest request, ServerCallContext context)
        {
            var list = await _sceneManageService.GetSceneUseageByColumnId(request.ColumIdList.ToList());

            var result = new ColumnUseageListReply();
            result.ColumnUseageList.AddRange(list.Select(p => new ColumnUseageSingle
            {
                ColumId=p.Key,
                SceneCount=p.Value
            }));
            return result;
        }
    }
}
