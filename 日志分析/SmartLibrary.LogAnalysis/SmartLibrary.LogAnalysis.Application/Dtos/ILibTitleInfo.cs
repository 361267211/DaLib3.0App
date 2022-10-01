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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartLibrary.LogAnalysis.Application.Dtos
{
    /// <summary>
    /// 智图元数据实体，与智图底层数据（对应表名：ilib_title_info）格式一一对应
    /// </summary>
    [Serializable]
    public class ILibTitleInfo
    {
        /// <summary>
        /// 标识
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        /// <summary>
        /// 题名
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// 英文题名
        /// </summary>
        [JsonProperty(PropertyName = "title_alternative")]
        public string EnglishTitle { get; set; }

        /// <summary>
        /// 版本说明/版次
        /// </summary>
        [JsonProperty(PropertyName = "title_edition")]
        public string TitleEdition { get; set; }

        /// <summary>
        ///丛书名
        /// </summary>
        [JsonProperty(PropertyName = "title_series")]
        public string TitleSeries { get; set; }

        /// <summary>
        /// ISBN（数字）
        /// </summary>
        [JsonProperty(PropertyName = "identifier_eisbn")]
        public string EISBN { get; set; }

        /// <summary>
        /// ISBN（纸本）
        /// </summary>
        [JsonProperty(PropertyName = "identifier_pisbn")]
        public string PISBN { get; set; }

        /// <summary>
        /// ISSN
        /// </summary>
        [JsonProperty(PropertyName = "identifier_issn")]
        public string ISSN { get; set; }

        /// <summary>
        /// 国内统一刊号
        /// </summary>
        [JsonProperty(PropertyName = "identifier_cnno")]
        public string CNNO { get; set; }

        /// <summary>
        /// 标准编号
        /// </summary>
        [JsonProperty(PropertyName = "identifier_standard")]
        public string Standard { get; set; }

        /// <summary>
        /// DOI
        /// </summary>
        [JsonProperty(PropertyName = "identifier_doi")]
        public string DOI { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [JsonProperty(PropertyName = "creator")]
        public string Author { get; set; }

        /// <summary>
        /// 作者英文名
        /// </summary>
        [JsonProperty(PropertyName = "creator_en")]
        public string EnglishAuthor { get; set; }

        /// <summary>
        /// 作者单位/提出单位
        /// </summary>
        [JsonProperty(PropertyName = "creator_institution")]
        public string Institution { get; set; }

        /// <summary>
        /// 作者专业
        /// </summary>
        [JsonProperty(PropertyName = "creator_discipline")]
        public string Discipline { get; set; }

        /// <summary>
        /// 学位级别
        /// </summary>
        [JsonProperty(PropertyName = "creator_degree")]
        public string Degree { get; set; }

        /// <summary>
        /// 起草单位
        /// </summary>
        [JsonProperty(PropertyName = "creator_drafting")]
        public string Drafting { get; set; }

        /// <summary>
        /// 发布单位
        /// </summary>
        [JsonProperty(PropertyName = "creator_release")]
        public string Release { get; set; }

        /// <summary>
        /// 导师
        /// </summary>
        [JsonProperty(PropertyName = "contributor")]
        public string Contributor { get; set; }

        /// <summary>
        /// 出版物名称
        /// </summary>
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        /// <summary>
        /// 出版物首字母
        /// </summary>
        [JsonProperty(PropertyName = "source_fl")]
        public string SourceFL { get; set; }

        /// <summary>
        /// 出版物英文名称
        /// </summary>
        [JsonProperty(PropertyName = "source_en")]
        public string EnglishSource { get; set; }

        /// <summary>
        /// 出版物ID
        /// </summary>
        [JsonProperty(PropertyName = "source_id")]
        public string SourceID { get; set; }

        /// <summary>
        /// 学位授予单位
        /// </summary>
        [JsonProperty(PropertyName = "source_institution")]
        public string SourceInstitution { get; set; }

        /// <summary>
        /// 出版社/出版者
        /// </summary>
        [JsonProperty(PropertyName = "publisher")]
        public string Publisher { get; set; }
        /// <summary>
        /// 出版地
        /// </summary>
        [JsonProperty(PropertyName = "pub_place")]
        public string PubPlace { get; set; }

        /// <summary>
        /// 出版年/年/学位授予年度
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        /// <summary>
        /// 卷
        /// </summary>
        [JsonProperty(PropertyName = "volume")]
        public string Volume { get; set; }

        /// <summary>
        /// 期
        /// </summary>
        [JsonProperty(PropertyName = "issue")]
        public string Issue { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// 摘要英文
        /// </summary>
        [JsonProperty(PropertyName = "description_en")]
        public string DescriptionEn { get; set; }

        /// <summary>
        /// 基金
        /// </summary>
        [JsonProperty(PropertyName = "description_fund")]
        public string Fund { get; set; }

        /// <summary>
        /// 核心收录
        /// </summary>
        [JsonProperty(PropertyName = "description_core")]
        public string Core { get; set; }

        /// <summary>
        /// 出版周期
        /// </summary>
        [JsonProperty(PropertyName = "description_cycle")]
        public string Cycle { get; set; }

        /// <summary>
        /// 标准类型
        /// </summary>
        [JsonProperty(PropertyName = "description_type")]
        public string StandardType { get; set; }

        /// <summary>
        /// 主题词
        /// </summary>
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public string Page { get; set; }

        /// <summary>
        /// 中图分类号/中图分类号号
        /// </summary>
        [JsonProperty(PropertyName = "subject_clc")]
        public string SubjectCLCJson { get; set; }

        /// <summary>
        /// 中图分类号/中图分类号号
        /// </summary>
        public string SubjectCLC => string.IsNullOrWhiteSpace(SubjectCLCJson) ? "" : Regex.Replace(Regex.Replace(SubjectCLCJson ?? "", @"\(.*\)", ""), @"\（.*\）", "");

        /// <summary>
        /// 教育部学科分类号
        /// </summary>
        [JsonProperty(PropertyName = "subject_esc")]
        public string SubjectESCJson { get; set; }

        /// <summary>
        /// 教育部学科分类号
        /// </summary>
        public string SubjectESC => string.IsNullOrWhiteSpace(SubjectESCJson) ? "" : Regex.Replace(Regex.Replace(SubjectESCJson ?? "", @"\(.*\)", ""), @"\（.*\）", "");

        /// <summary>
        /// 学位学科
        /// </summary>
        [JsonProperty(PropertyName = "subject_dsa")]
        public string SubjectDSA { get; set; }

        /// <summary>
        /// 中国标准分类号
        /// </summary>
        [JsonProperty(PropertyName = "subject_csc")]
        public string SubjectCSC { get; set; }

        /// <summary>
        /// 国际标准分类号
        /// </summary>
        [JsonProperty(PropertyName = "subject_isc")]
        public string SubjectISC { get; set; }

        /// <summary>
        /// 实施日期
        /// </summary>
        [JsonProperty(PropertyName = "date_impl")]
        public string DateImpl { get; set; }

        /// <summary>
        /// 出版日期/学位授予日期/发布日期
        /// </summary>
        [JsonProperty(PropertyName = "date_created")]
        public string DateCreated { get; set; }

        /// <summary>
        /// 封面路径
        /// </summary>
        [JsonProperty(PropertyName = "cover")]
        public string Cover { get; set; }

        /// <summary>
        /// 封面路径
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// 默认封面css类名
        /// </summary>
        public string DefaultCoverCssClass { get; set; }

        /// <summary>
        /// 语种
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        /// <summary>
        /// 国别
        /// </summary>
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        /// <summary>
        /// 来源数据库
        /// </summary>
        [JsonProperty(PropertyName = "provider")]
        public string Provider { get; set; }

        /// <summary>
        /// 来源路径
        /// </summary>
        [JsonProperty(PropertyName = "provider_url")]
        public string ProviderUrl { get; set; }

        /// <summary>
        /// 来源ID
        /// </summary>
        [JsonProperty(PropertyName = "provider_id")]
        public string ProviderID { get; set; }

        /// <summary>
        /// 来源书号
        /// </summary>
        [JsonProperty(PropertyName = "provider_bn")]
        public string ProviderBN { get; set; }

        /// <summary>
        /// 来源书号
        /// </summary>
        [JsonProperty(PropertyName = "provider_owner")]
        public string ProviderOwner { get; set; }

        /// <summary>
        /// 所属者
        /// </summary>
        [JsonProperty(PropertyName = "owner")]
        public string Owner { get; set; }


        /// <summary>
        /// 文献类型
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }


        /// <summary>
        /// 载体
        /// </summary>
        [JsonProperty(PropertyName = "medium")]
        public string Medium { get; set; }

        /// <summary>
        /// 载体
        /// </summary>
        [JsonProperty(PropertyName = "medium_owner")]
        public string MediumOwner { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [JsonProperty(PropertyName = "batch")]
        public string Batch { get; set; }

        /// <summary>
        /// 被合并ID
        /// </summary>
        [JsonProperty(PropertyName = "combined")]
        public string Combined { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "dio")]
        public string DIO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "coverhtml")]
        public string CoverHtml { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "provider_combined")]
        public string ProviderCombined { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "folio_size")]
        public string FolioSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "title_sub")]
        public string TitleSub { get; set; }

        /// <summary>
        /// 作者简介
        /// </summary>
        [JsonProperty(PropertyName = "creator_bio")]
        public string CreatorBio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 数据库所属者
        /// </summary>
        public string DBOwner { get; set; }

        /// <summary>
        /// ISSN/专利申请号
        /// </summary>
        [JsonProperty(PropertyName = "identifier_pissn")]
        public string PISSN { get; set; }


        /// <summary>
        /// ISSN
        /// </summary>
        [JsonProperty(PropertyName = "identifier_eissn")]
        public string EISSN { get; set; }

        /// <summary>
        /// 申请人/主编
        /// </summary>
        [JsonProperty(PropertyName = "creator_cluster")]
        public string CreatorCluster { get; set; }

        /// <summary>
        /// 英文关键词
        /// </summary>
        [JsonProperty(PropertyName = "subject_en")]
        public string SubjectEn { get; set; }

        /// <summary>
        /// 是否有html全文
        /// </summary>
        [JsonProperty(PropertyName = "if_html_fulltex")]
        public string IfHtmlFulltex { get; set; }

        /// <summary>
        /// 是否有pdf全文
        /// </summary>
        [JsonProperty(PropertyName = "if_pdf_fulltext")]
        public string IfPdfFulltex { get; set; }

        /// <summary>
        /// 是否优先出版
        /// </summary>
        [JsonProperty(PropertyName = "if_pub1st")]
        public string IfPub1st { get; set; }

        /// <summary>
        /// 引文量
        /// </summary>
        [JsonProperty(PropertyName = "ref_cnt")]
        public string RefCnt { get; set; }

        /// <summary>
        /// 被引量
        /// </summary>
        [JsonProperty(PropertyName = "cited_cnt")]
        public string CitedCnt { get; set; }

        /// <summary>
        /// 原始类型
        /// </summary>
        [JsonProperty(PropertyName = "rawtype")]
        public string Rawtype { get; set; }

        /// <summary>
        /// 代理人
        /// </summary>
        [JsonProperty(PropertyName = "agents")]
        public string Agents { get; set; }

        /// <summary>
        /// 代理机构
        /// </summary>
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        /// <summary>
        /// 法律状态
        /// </summary>
        [JsonProperty(PropertyName = "legal_status")]
        public string LegalStatus { get; set; }

        /// <summary>
        /// 国别省市代码
        /// </summary>
        [JsonProperty(PropertyName = "province_code")]
        public string ProvinceCode { get; set; }

        /// <summary>
        /// 来源专辑/分类
        /// </summary>
        [JsonProperty(PropertyName = "provider_subject")]
        public string ProviderSubject { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty(PropertyName = "mark")]
        public string Mark { get; set; }

        /// <summary>
        /// 汇编资源id
        /// </summary>
        [JsonProperty(PropertyName = "asmresourceid")]
        public int AsmResourceId { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public int Bid { get; set; }

        /// <summary>
        /// 原文出处
        /// </summary>
        [JsonProperty(PropertyName = "description_source")]
        public string DescriptionSource { get; set; }

        /// <summary>
        /// 原文出处
        /// </summary>
        [JsonProperty(PropertyName = "is_oa")]
        public string IsOA { get; set; }
    }
}
