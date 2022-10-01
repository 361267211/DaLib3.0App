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

using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.ViewModels
{
    /// <summary>
    /// 数据库平台通用的下拉列表模型dto
    /// </summary>
    public class DatabaseInitializedModel
    {
        /// <summary>
        /// 状态列表
        /// </summary>
        public List<OptionDto> StatusDtos { get; set; }

        /// <summary>
        /// 语言列表
        /// </summary>
        public List<OptionDto> languageDtos { get; set; }

        /// <summary>
        /// 文献类型列表
        /// </summary>
        public List<OptionDto> ArticleTypeDtos { get; set; }
        /// <summary>
        /// 学科类型列表
        /// </summary>
        public List<OptionDto> DomainEscDtos { get; set; }

        /// <summary>
        /// 采购类型列表
        /// </summary>
        public List<OptionDto> PurchaseTypeDtos { get; set; }
        /// <summary>
        /// 标签列表
        /// </summary>
        public List<OptionDto> Labels { get; set; }
        /// <summary>
        /// 排序规则
        /// </summary>
        public List<OptionDto> OrderRuleDtos { get; set; }
        /// <summary>
        /// 其他介绍
        /// </summary>
        public List<OptionDto> OtherIntroduceDtos { get; set; }
        /// <summary>
        /// 供应商信息
        /// </summary>
        public List<OptionDto> DatabaseProviderDtos { get; set; }

        /// <summary>
        /// 排序规则字典
        /// </summary>
        public List<OptionDto> SortRules { get; set; }

    }
}
