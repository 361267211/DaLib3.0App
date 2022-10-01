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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos
{
    public class DatabaseColumnDto : BaseDto
    {
        /// <summary>
        /// 主键
        /// </summary>

        public Guid Id { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>

        /// <summary>
        /// 栏目名称
        /// </summary>

        public string ColumnName { get; set; }

        /// <summary>
        /// 规则字符串
        /// </summary>

        public string Rule{ get; set; }

        /// <summary>
        /// 匹配数量
        /// </summary>

        public int MatchCount { get; set; }


        /// <summary>
        /// 状态列表
        /// </summary>
        public List<string> StatusDtos { get; set; }

        /// <summary>
        /// 语言列表
        /// </summary>
        public List<string> languageDtos { get; set; }

        /// <summary>
        /// 文献类型列表
        /// </summary>
        public List<string> ArticleTypeDtos { get; set; }
        /// <summary>
        /// 学科类型列表
        /// </summary>
        public List<string> DomainEscDtos { get; set; }

        /// <summary>
        /// 采购类型列表
        /// </summary>
        public List<string> PurchaseTypeDtos { get; set; }
        /// <summary>
        /// 标签列表
        /// </summary>
        public List<string> Labels { get; set; }
        /// <summary>
        /// 排序规则 1-默认排序，2-月点击量，3-总点击量，4-创建时间
        /// </summary>
        public string OrderRule { get; set; }
    }
}
