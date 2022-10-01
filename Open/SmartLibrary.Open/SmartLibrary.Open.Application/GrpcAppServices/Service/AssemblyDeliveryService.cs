using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furion.DependencyInjection;
using Grpc.Core;
using Mapster;
using SmartLibrary.Open.EntityFramework.Core.Entitys.Assembly;
using Furion;
using SmartLibrary.Assembly.Common.Extensions;
using SmartLibrary.Open.Services.AppManagement.IAssemblyService;
using SmartLibrary.Open.EntityFramework.Core.DbContexts;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Open.Services.Dtos.AssemblyDelivery;
using SmartLibrary.Assembly.Application.Protos;
using Google.Protobuf.WellKnownTypes;

namespace SmartLibrary.Open.Application.GrpcAppServices.Service
{
    public class AssemblyDeliveryService : AssemblySharedGrpcService.AssemblySharedGrpcServiceBase, IScoped
    {

        private readonly OpenDbContext _openDbContext;

        private readonly IAssemblyService _assemblyService;

        public AssemblyDeliveryService(IAssemblyService assemblyService,
            OpenDbContext openDbContext
            )
        {
            _openDbContext = openDbContext;
            _assemblyService = assemblyService;
        }

        /// <summary>
        /// 保存各个学校传递过来的表数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<DeliverySharedAssemblyReply> DeliverySharedAssembly(DeliverySharedAssemblyRequest request, ServerCallContext context)
        {
            _openDbContext.Database.Migrate();

            //var owner= context.GetHttpContext().EnsureOwner();
            var owner = App.HttpContext.EnsureOwner();

            var aaa = request.AssemblyTab.ToList();
            try
            {
                var tst = aaa.Adapt<List<AssemblyBaseInfo>>();
            }
            catch (Exception ex)
            {

                throw;
            }
        

            var assemblys = request.AssemblyTab.ToList().Adapt<List<AssemblyBaseInfo>>();
            var artColumns = request.ArtColumnTab.ToList().Adapt<List<AssemblyArticleColumn>>();
            var importedArt = request.RuleByImportedTab.ToList().Adapt<List<ArtByImported>>();
            var uploadArt = request.RuleByUploadTab.ToList().Adapt<List<ArtByUpload>>();
            var themeRule = request.RuleByThemsTab.ToList().Adapt<List<ArtColSearchThemes>>();
            var expRule = request.RuleByExpTab.ToList().Adapt<List<ArtRetrievalExp>>();

            //判断是否已经共享过了


            //重新赋予id，建立关联
            var newAssemblyId = Guid.NewGuid();
            var oldAssemblyId = assemblys.First().Id;
            var assembly = assemblys.First();

            assembly.Id = newAssemblyId;
            assembly.SharedId = oldAssemblyId;

            assembly.AuditStatus = 1;
            assembly.SharedTime = DateTime.Now;
            assembly.CreatedTime = DateTime.Now;
            assembly.OrgCode = owner;
            artColumns.ForEach(e => e.AssemblyID = newAssemblyId);
            List<AssemblyArticleColumn> newArticleColumns = new List<AssemblyArticleColumn>();

            // List<ArtByImported> newImportedArt = new List<ArtByImported>();
            //重新生成文献栏目id
            foreach (var item in importedArt.GroupBy(e => e.ArtColumnId))
            {
                var newArtColumnId = Guid.NewGuid();
                artColumns.FirstOrDefault(e => e.Id == item.Key).Id = newArtColumnId;
                item.ToList().ForEach(e => { e.Id = Guid.NewGuid(); e.ArtColumnId = newArtColumnId; e.UpdatedTime = DateTime.Now; });
            }

            foreach (var item in uploadArt.GroupBy(e => e.ArtColumnId))
            {
                var newArtColumnId = Guid.NewGuid();
                artColumns.FirstOrDefault(e => e.Id == item.Key).Id = newArtColumnId;
                item.ToList().ForEach(e => { e.Id = Guid.NewGuid(); e.ArtColumnId = newArtColumnId; e.UpdatedTime = DateTime.Now; });
            }

            foreach (var item in themeRule.GroupBy(e => e.AssemblyArticleColumnID))
            {
                var newArtColumnId = Guid.NewGuid();
                artColumns.FirstOrDefault(e => e.Id == item.Key).Id = newArtColumnId;
                item.ToList().ForEach(e => { e.Id = Guid.NewGuid(); e.AssemblyArticleColumnID = newArtColumnId; e.UpdatedTime = DateTime.Now; });
            }

            foreach (var item in expRule.GroupBy(e => e.AssemblyArticleColumnID))
            {
                var newArtColumnId = Guid.NewGuid();
                artColumns.FirstOrDefault(e => e.Id == item.Key).Id = newArtColumnId;
                item.ToList().ForEach(e => { e.Id = Guid.NewGuid(); e.AssemblyArticleColumnID = newArtColumnId; e.UpdatedTime = DateTime.Now; });
            }



            await _assemblyService.SaveAssemblyBaseInfo(assemblys);

            await _assemblyService.SaveAssemblyArticleColumn(artColumns);

            await _assemblyService.SaveArtByImported(importedArt);

            await _assemblyService.SaveArtByUpload(uploadArt);

            await _assemblyService.SaveArtColSearchThemes(themeRule);

            await _assemblyService.SaveArtRetrievalExp(expRule);


            return new DeliverySharedAssemblyReply() { Message = "成功" };
        }


        /// <summary>
        /// 获取在用的系统专题分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<GetSystemAssemblyReply> GetSystemAssemblyPage(GetSystemAssemblyRequest request, ServerCallContext context)
        {
            PagedList<AssemblyBaseInfo> assemblyPage = await _assemblyService.GetSystemAssembly(request.SearchKey, request.RecommendType, request.PageIndex, request.PageSize, request.AcquiredIds.ToList());
            GetSystemAssemblyReply reply = new GetSystemAssemblyReply();
            reply.AssemblyPage = assemblyPage.Adapt<SysAssemblyPagedList>();

            foreach (var item in assemblyPage.Items)
            {
                reply.AssemblyPage.Items.Add(item.Adapt<AssemblyBaseInfoTab>());
            }

            return reply;
        }

        /// <summary>
        /// 从开放中心获取专题
        /// </summary>
        /// <param name="getAssemblyFromOpenCenterRequest"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<GetAssemblyFromOpenCenterReply> GetAssemblyFromOpenCenter(GetAssemblyFromOpenCenterRequest getAssemblyFromOpenCenterRequest, ServerCallContext context)
        {
            var assemblyIdStrs = getAssemblyFromOpenCenterRequest.AssemblyIds.Adapt<List<string>>();

            var assemblyIds = new List<Guid>();
            assemblyIdStrs.ForEach(e => assemblyIds.Add(new Guid(e)));
            //获取专题的表
            List<AssemblyTabsDto> assemblyTabsDtos = await _assemblyService.GetAssemblyFromOpenCenter(assemblyIds);

            var reply = new GetAssemblyFromOpenCenterReply();

            //组装到grpc 的reply中
            foreach (var assembly in assemblyTabsDtos)
            {
                var assemblytab = new DeliverySharedAssemblyRequest();
                //专题表
                foreach (var item in assembly.AssemblyTab)
                {
                    assemblytab.AssemblyTab.Add(item.Adapt<AssemblyBaseInfoTab>());
                }

                //文献栏目表
                foreach (var item in assembly.ArtColumnTab)
                {
                    assemblytab.ArtColumnTab.Add(item.Adapt<AssemblyArticleColumnTab>());
                }

                //主题词规则表
                foreach (var item in assembly.RuleByThemsTab)
                {
                    assemblytab.RuleByThemsTab.Add(item.Adapt<ArtColSearchThemesTab>());
                }

                //检索规则表
                foreach (var item in assembly.RuleByExpTab)
                {
                    assemblytab.RuleByExpTab.Add(item.Adapt<ArtRetrievalExpTab>());
                }

                //导入规则栏目表
                foreach (var item in assembly.ArtByImportedTab)
                {
                    assemblytab.RuleByImportedTab.Add(item.Adapt<ArtByImportedTab>());
                }

                //上传规则表
                foreach (var item in assembly.RuleByUploadTab)
                {
                    assemblytab.RuleByUploadTab.Add(item.Adapt<ArtByUploadTab>());
                }
                reply.Assemblys.Add(assemblytab);
            }

            return reply;
        }

        /// <summary>
        /// 获取联盟已经共享的专题
        /// </summary>
        /// <param name="getUnionSharedAssemblyPageRequest"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<GetUnionSharedAssemblyReply> GetUnionSharedAssemblyPage(GetUnionSharedAssemblyPageRequest request, ServerCallContext context)
        {
            var OrgCode = App.HttpContext.EnsureOwner();
            PagedList<AssemblyBaseInfo> assemblyPage = await _assemblyService.GetUnionSharedAssemblyPage(request.SearchKey, request.PageIndex, request.PageSize, OrgCode, request.SortRule, request.AcquiredIds.ToList());
            GetUnionSharedAssemblyReply reply = new GetUnionSharedAssemblyReply();
            reply.AssemblyPage = assemblyPage.Adapt<SysAssemblyPagedList>();

            foreach (var item in assemblyPage.Items)
            {
                reply.AssemblyPage.Items.Add(item.Adapt<AssemblyBaseInfoTab>());
            }
            return reply;
        }


        /// <summary>
        /// 查找申请共享的专题
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<GetSharedLocalAssemblyPageReply> GetSharedLocalAssemblyPage(GetSharedLocalAssemblyPageRequest request, ServerCallContext context)
        {
            var OrgCode = App.HttpContext.EnsureOwner();
            PagedList<AssemblyBaseInfo> assemblyPage = await _assemblyService.GetSharedLocalAssemblyPage(request.SearchKey, request.PageIndex, request.PageSize, OrgCode, request.AuditType);
            GetSharedLocalAssemblyPageReply reply = new GetSharedLocalAssemblyPageReply();
            reply.AssemblyPage = assemblyPage.Adapt<SysAssemblyPagedList>();
            reply.AssemblyPage.Items.AddRange(assemblyPage.Items.Adapt<List<AssemblyBaseInfoTab>>());
            return reply;
        }
        /// <summary>
        /// 根据专题id 预览文献栏目
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<PreviewArticleColumnReply> PreviewArticleColumn(PreviewArticleColumnRequest request, ServerCallContext context)
        {
            List<AssemblyArticleColumn> artColumns = await _assemblyService.GetArticleColumns(new Guid(request.AssemblyId));
            PreviewArticleColumnReply reply = new PreviewArticleColumnReply();
            reply.ArtColumnTabs.AddRange(artColumns.Adapt<List<AssemblyArticleColumnTab>>());

            return reply;
        }

        /// <summary>
        /// 根据文献栏目id 获取预览数据表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<PreviewArticleColumnByIdReply> PreviewArticleColumnById(PreviewArticleColumnByIdRequest request, ServerCallContext context)
        {
            PreviewArticleColumnByIdReply reply = new PreviewArticleColumnByIdReply();

            var artColumn = await _assemblyService.GetArticleColumnById(new Guid(request.ArtColumnId));
            reply.ArtColumnTab = artColumn.Adapt<AssemblyArticleColumnTab>();

            switch (artColumn.ArtBindType)
            {
                case 1:
                    ArtColSearchThemes artColSearchThemes = await _assemblyService.GetArtByThems(new Guid(request.ArtColumnId));
                    reply.RuleByThemsTab = artColSearchThemes.Adapt<ArtColSearchThemesTab>();
                    break;
                case 2:
                    List<ArtByImported> artByImported = await _assemblyService.GetArtByImported(request.PageIndex, request.PageSize, new Guid(request.ArtColumnId));
                    reply.RuleByImportedTab.AddRange(artByImported.Adapt<List<ArtByImportedTab>>());
                    break;
                case 3:
                    ArtRetrievalExp artRetrievalExp = await _assemblyService.GetArtByExp(new Guid(request.ArtColumnId));
                    reply.RuleByExpTab = artRetrievalExp.Adapt<ArtRetrievalExpTab>();
                    break;
                case 4:
                    List<ArtByUpload> artByUploads = await _assemblyService.GetArtByUpload(request.PageIndex, request.PageSize, new Guid(request.ArtColumnId));
                    reply.RuleByUploadTab.AddRange(artByUploads.Adapt<List<ArtByUploadTab>>());
                    break;
                default:
                    break;
            }

            return reply;
        }

        public async override Task<Empty> CancelSharedLocalAssembly(StringValue request, ServerCallContext context)
        {
            await _assemblyService.CancelSharedLocalAssembly(request.Value);

            Empty empty = new Empty();
            return empty;
        }
    }
}
