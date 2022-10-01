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

using SmartLibrary.Open.EntityFramework.Core.Entitys.Assembly;
using SmartLibrary.Open.Services.Dtos.AssemblyDelivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.AppManagement.IAssemblyService
{
    public interface IAssemblyService
    {
        /// <summary>
        /// 保存专题得基本信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Task SaveAssemblyBaseInfo(List<AssemblyBaseInfo> assemblyBaseInfos);

        /// <summary>
        /// 保存文献栏目得基本信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Task SaveAssemblyArticleColumn(List<AssemblyArticleColumn> assemblyArticleColumns);

        /// <summary>
        /// 保存主题文献栏目得规则信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Task SaveArtColSearchThemes(List<ArtColSearchThemes> artColSearchThemes);

        /// <summary>
        /// 保存表达式文献栏目得规则信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Task SaveArtRetrievalExp(List<ArtRetrievalExp> artRetrievalExps);

        /// <summary>
        /// 保存导入型文献栏目得数据
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Task SaveArtByImported(List<ArtByImported> artByImporteds);

        /// <summary>
        /// 保存上传型文献栏目得数据
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Task SaveArtByUpload(List<ArtByUpload> artByUploads);
        Task<PagedList<AssemblyBaseInfo>> GetSystemAssembly(string searchKey, string recommendType,int pageIndex,int pageSize,List<string> ids);
        Task<List<AssemblyTabsDto>> GetAssemblyFromOpenCenter(List<Guid> assemblyIds);
        Task<PagedList<AssemblyBaseInfo>> GetUnionSharedAssemblyPage(string searchKey, int pageIndex, int pageSize, string orgCode,int sortRule, List<string> ids);
        Task<PagedList<AssemblyBaseInfo>> GetSharedLocalAssemblyPage(string searchKey, int pageIndex, int pageSize, string orgCode, int auditType);
        /// <summary>
        /// 根据专题id，获取文献栏目列表
        /// </summary>
        /// <param name="assemblyId"></param>
        /// <returns></returns>
        Task<List<AssemblyArticleColumn>> GetArticleColumns(Guid assemblyId);
        Task<AssemblyArticleColumn> GetArticleColumnById(Guid artColumnId);
        Task<ArtColSearchThemes> GetArtByThems(Guid artColumnId);
        Task<List<ArtByImported>> GetArtByImported(int pageIndex, int pageSize, Guid artColumnId);
        Task<ArtRetrievalExp> GetArtByExp(Guid artColumnId);
        Task<List<ArtByUpload>> GetArtByUpload(int pageIndex, int pageSize, Guid artColumnId);
        Task CancelSharedLocalAssembly(string value);
    }
}
