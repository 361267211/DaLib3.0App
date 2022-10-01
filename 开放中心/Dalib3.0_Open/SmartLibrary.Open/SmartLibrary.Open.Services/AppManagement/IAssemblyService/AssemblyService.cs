/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.LinqBuilder;
using SmartLibrary.Open.Common.Dtos;
using SmartLibrary.Open.EntityFramework.Core.DbContexts;
using SmartLibrary.Open.EntityFramework.Core.Entitys.Assembly;
using SmartLibrary.Open.Services.Dtos.AssemblyDelivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SmartLibrary.Open.Services.AppManagement.IAssemblyService
{
    public class AssemblyService : IAssemblyService, IScoped
    {
        private readonly OpenDbContext _openDbContext;
        private readonly IRepository<AssemblyArticleColumn> _articleColumnRepository;
        private readonly IRepository<AssemblyBaseInfo> _assemblyRepository;
        private readonly IRepository<ArtColSearchThemes> _artColSearchThemesRepository;
        private readonly IRepository<ArtByImported> _artByImportedRepository;
        private readonly IRepository<ArtRetrievalExp> _artRetrievalExpRepository;
        private readonly IRepository<ArtByUpload> _artByUploadRepository;

        public AssemblyService(
            OpenDbContext openDbContext,
            IRepository<ArtByUpload> artByUploadRepository,
            IRepository<ArtRetrievalExp> artRetrievalExpRepository,
            IRepository<ArtByImported> artByImportedRepository,
            IRepository<ArtColSearchThemes> artColSearchThemesRepository,
            IRepository<AssemblyArticleColumn> articleColumnRepository,
            IRepository<AssemblyBaseInfo> assemblyRepository

            )
        {
            _articleColumnRepository = articleColumnRepository;
            _assemblyRepository = assemblyRepository;
            _artColSearchThemesRepository = artColSearchThemesRepository;
            _artByImportedRepository = artByImportedRepository;
            _artRetrievalExpRepository = artRetrievalExpRepository;
            _artByUploadRepository = artByUploadRepository;
        }

        public async Task<List<AssemblyTabsDto>> GetAssemblyFromOpenCenter(List<Guid> assemblyIds)
        {
            List<AssemblyTabsDto> tabs = new List<AssemblyTabsDto>();

            foreach (var item in assemblyIds)
            {
                tabs.Add(await this.GetOneAssembly(item));
            }
            return tabs;
        }

        /// <summary>
        /// 获取单个专题表集合
        /// </summary>
        /// <param name="assemblyId"></param>
        /// <returns></returns>
        private async Task<AssemblyTabsDto> GetOneAssembly(Guid assemblyId)
        {
            //查找改专题下关联的所有表
            var assemblys = _assemblyRepository.Where(e => !e.DeleteFlag && e.Id == assemblyId).ToList();
            var artColumns = _articleColumnRepository.Where(e => !e.DeleteFlag && e.AssemblyID == assemblyId).ToList();
            var artColumnIds = artColumns.Select(e => e.Id);
            var themeRule = _artColSearchThemesRepository.Where(e => !e.DeleteFlag && artColumnIds.Contains(e.AssemblyArticleColumnID)).ToList();
            var expRule = _artRetrievalExpRepository.Where(e => !e.DeleteFlag && artColumnIds.Contains(e.AssemblyArticleColumnID)).ToList();
            var importedArt = _artByImportedRepository.Where(e => !e.DeleteFlag && artColumnIds.Contains(e.ArtColumnId)).ToList();
            var uploadArt = _artByUploadRepository.Where(e => !e.DeleteFlag && artColumnIds.Contains(e.ArtColumnId)).ToList();


            //重新赋予id，并保持关联不变
            var newAssemblyId = Guid.NewGuid();
            var oldAssemblyId = assemblys.First().Id;
            var assembly = assemblys.First();

            assembly.Id = newAssemblyId;
            assembly.SharedId = oldAssemblyId;
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


            AssemblyTabsDto assemblyTabsDto = new AssemblyTabsDto
            {
                AssemblyTab = assemblys,
                ArtColumnTab = artColumns,
                RuleByThemsTab = themeRule,
                RuleByExpTab = expRule,
                RuleByUploadTab = uploadArt,
                ArtByImportedTab = importedArt
            };
            return assemblyTabsDto;
        }

        public async Task<PagedList<AssemblyBaseInfo>> GetSystemAssembly(string searchKey, string recommendType, int pageIndex, int pageSize, List<string> ids)
        {
            var lamda = LinqExpression.Create<AssemblyBaseInfo>(e => !e.DeleteFlag && e.OrgCode == "sys");
            lamda = lamda.AndIf(!string.IsNullOrEmpty(searchKey), e => e.AssemblyName.Contains(searchKey));
            lamda = lamda.AndIf(!string.IsNullOrEmpty(recommendType), e => e.RecommendType == recommendType);
            lamda = lamda.AndIf(ids.Count > 0, e => !ids.Contains(e.Id.ToString()));
            var systemAssenblyPage = _assemblyRepository.Where(lamda).ToPagedList(pageIndex, pageSize);

            return systemAssenblyPage;
        }

        public async Task SaveArtByImported(List<ArtByImported> artByImporteds)
        {
            _artByImportedRepository.InsertNow(artByImporteds);

        }

        /// <summary>
        /// 保存上传型文献栏目得数据
        /// </summary>
        /// <param name="artByUploads"></param>
        /// <returns></returns>
        public async Task SaveArtByUpload(List<ArtByUpload> artByUploads)
        {
            try
            {

                _artByUploadRepository.InsertNow(artByUploads);
            }
            catch (Exception ex)
            {

                throw;
            }
             

        }
        /// <summary>
        /// 保存主题型文献栏目得数据
        /// </summary>
        /// <param name="artColSearchThemes"></param>
        /// <returns></returns>
        public async Task SaveArtColSearchThemes(List<ArtColSearchThemes> artColSearchThemes)
        {
            _artColSearchThemesRepository.InsertNow(artColSearchThemes);
        }
        /// <summary>
        /// 保存表达式型文献栏目得数据
        /// </summary>
        /// <param name="artRetrievalExps"></param>
        /// <returns></returns>
        public async Task SaveArtRetrievalExp(List<ArtRetrievalExp> artRetrievalExps)
        {
            _artRetrievalExpRepository.InsertNow(artRetrievalExps);
        }
        /// <summary>
        /// 保存文献栏目
        /// </summary>
        /// <param name="assemblyArticleColumns"></param>
        /// <returns></returns>
        public async Task SaveAssemblyArticleColumn(List<AssemblyArticleColumn> assemblyArticleColumns)
        {
            _articleColumnRepository.InsertNow(assemblyArticleColumns);
        }
        /// <summary>
        /// 保存专题基本信息
        /// </summary>
        /// <param name="assemblyBaseInfos"></param>
        /// <returns></returns>
        public async Task SaveAssemblyBaseInfo(List<AssemblyBaseInfo> assemblyBaseInfos)
        {
            _assemblyRepository.InsertNow(assemblyBaseInfos);
        }

        /// <summary>
        /// 获取联盟已共享的专题（除开自己共享的）  --条件筛选--分页
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public async Task<PagedList<AssemblyBaseInfo>> GetUnionSharedAssemblyPage(string searchKey, int pageIndex, int pageSize, string orgCode, int sortRule, List<string> ids)
        {
            var lamda = LinqExpression.Create<AssemblyBaseInfo>(e => !e.DeleteFlag);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(searchKey), e => e.AssemblyName.Contains(searchKey));
            lamda = lamda.And(e => e.OrgCode != orgCode && e.OrgCode != "sys");
            lamda = lamda.AndIf(ids.Count > 0, e => !ids.Contains(e.Id.ToString()));



            var unionAssenblyPageQuery = _assemblyRepository.Where(lamda);

            PagedList<AssemblyBaseInfo> unionAssenblyPage = new PagedList<AssemblyBaseInfo>();

            switch (sortRule)
            {
                case 1:
                    unionAssenblyPage = unionAssenblyPageQuery.ToPagedList(pageIndex, pageSize);
                    break;
                case 2:
                    unionAssenblyPage = unionAssenblyPageQuery.OrderBy(e => e.SharedTime).ToPagedList(pageIndex, pageSize);
                    break;
                case 3:
                    unionAssenblyPage = unionAssenblyPageQuery.OrderBy(e => e.SharedCount).ToPagedList(pageIndex, pageSize);
                    break;
                default:
                    unionAssenblyPage = unionAssenblyPageQuery.ToPagedList(pageIndex, pageSize);
                    break;
            }

            return unionAssenblyPage;
        }
        /// <summary>
        /// 查找学校申请共享的专题
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orgCode"></param>
        /// <param name="auditType"></param>
        /// <returns></returns>
        public async Task<PagedList<AssemblyBaseInfo>> GetSharedLocalAssemblyPage(string searchKey, int pageIndex, int pageSize, string orgCode, int auditType)
        {
            var lamda = LinqExpression.Create<AssemblyBaseInfo>(e => !e.DeleteFlag && e.OrgCode == orgCode);
            lamda = lamda.AndIf(!string.IsNullOrEmpty(searchKey), e => e.AssemblyName.Contains(searchKey));
            lamda = lamda.AndIf(auditType != 0, e => e.AuditStatus == auditType);
            var query = _assemblyRepository.Where(lamda).OrderBy(e => e.SharedTime);

           var con= SiteGlobalConfig.Database.SqlConnection;
            try
            {
                var tst = query.ToList();
            }
            catch (Exception ex)
            {
                var eee = ex.Message;
                throw;
            }
             

            return await query.ToPagedListAsync();
        }

        /// <summary>
        /// 根据专题id，获取文献栏目列表
        /// </summary>
        /// <param name="assemblyId"></param>
        /// <returns></returns>
        public async Task<List<AssemblyArticleColumn>> GetArticleColumns(Guid assemblyId)
        {
            return _articleColumnRepository.Where(e => !e.DeleteFlag && e.AssemblyID == assemblyId).ToList();
        }

        public async Task<AssemblyArticleColumn> GetArticleColumnById(Guid artColumnId)
        {
            return await _articleColumnRepository.FirstOrDefaultAsync(e => !e.DeleteFlag && e.Id == artColumnId);
        }

        public async Task<ArtColSearchThemes> GetArtByThems(Guid artColumnId)
        {
            return await _artColSearchThemesRepository.FirstOrDefaultAsync(e => !e.DeleteFlag && e.AssemblyArticleColumnID == artColumnId);
        }

        public async Task<List<ArtByImported>> GetArtByImported(int pageIndex, int pageSize, Guid artColumnId)
        {
            return  _artByImportedRepository.Where(e => !e.DeleteFlag && e.ArtColumnId == artColumnId).ToList();

        }

        public async Task<ArtRetrievalExp> GetArtByExp(Guid artColumnId)
        {
            return await _artRetrievalExpRepository.FirstOrDefaultAsync(e => !e.DeleteFlag && e.AssemblyArticleColumnID == artColumnId);

        }

        public async Task<List<ArtByUpload>> GetArtByUpload(int pageIndex, int pageSize, Guid artColumnId)
        {
            return  _artByUploadRepository.Where(e => !e.DeleteFlag && e.ArtColumnId == artColumnId).ToList();
        }

        /// <summary>
        /// 取消共享
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task CancelSharedLocalAssembly(string value)
        {
            var assembly=await _assemblyRepository.FirstOrDefaultAsync(e => e.Id == new Guid(value));

            if (assembly == null)
                throw Oops.Oh("未找到此专题");

            assembly.DeleteFlag = true;
           await  _assemblyRepository.UpdateNowAsync(assembly);

        }
    }
}
