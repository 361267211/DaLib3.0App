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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos.AssemblyDelivery
{
    /// <summary>
    /// 直接表传递
    /// </summary>
   public class AssemblyTabsDto
    {
        public List<AssemblyBaseInfo> AssemblyTab { get; set; }
        public List<AssemblyArticleColumn> ArtColumnTab { get; set; }
        public List<ArtColSearchThemes> RuleByThemsTab { get; set; }
        public List<ArtRetrievalExp> RuleByExpTab { get; set; }
        public List<ArtByImported> ArtByImportedTab { get; set; }
        public List<ArtByUpload> RuleByUploadTab { get; set; }
    }
}
