/*********************************************************
 * 名    称：DatabaseTerraceDto
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：DatabaseTerrace的Dto层
 *
 * 更新历史：
 *
 * *******************************************************/

using SmartLibrary.DatabaseTerrace.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos
{
    /// <summary>
    /// 数据库平台的Dto数据
    /// </summary>
    public class DatabaseTerraceDto : BaseDto
    {
        /// <summary>
        /// 平台/数据库名称
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 平台/数据库名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 简称/别名
        /// </summary>
        public string Abbreviation { get; set; }
        /// <summary>
        /// 数据库供应商ID
        /// </summary>
        public string DatabaseProviderID { get; set; }
        /// <summary>
        /// 首字母
        /// </summary>
        public string Initials { get; set; }
        /// <summary>
        /// 文献类型 1：图书；3：期刊文献；4：学位论文；5：标准
        /// </summary>
        public List<string> ArticleTypes { get; set; }

        /// <summary>
        /// 中图分类
        /// </summary>
        public string DomainClcs { get; set; }
        /// <summary>
        /// 学科分类
        /// </summary>
        public List<string> DomainEscs { get; set; }
        /// <summary>
        /// 采购类型
        /// </summary>
        public string PurchaseType { get; set; }
        /// <summary>
        /// 主要语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 自定义标签id
        /// </summary>
        public List<string> Label { get; set; }
        /// <summary>
        /// 界面展示用的标签
        /// </summary>
        public List<string> DisplayLabel { get; set; }
        /// <summary>
        /// 效期开始时间
        /// </summary>
        public DateTime ExpiryBeginTime { get; set; }
        /// <summary>
        /// 效期失效时间
        /// </summary>
        public DateTime ExpiryEndTime { get; set; }
        /// <summary>
        /// 封面图标
        /// </summary>
        public string Cover { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 直接访问的列表
        /// </summary>
        public List<DatabaseAcessUrlDto> DirectUrls { get; set; }
        /// <summary>
        /// 间接访问
        /// </summary>
        public string IndirectUrl { get; set; }
        /// <summary>
        /// 限定访问用户名单
        /// </summary>
        public string WhiteList { get; set; }
        /// <summary>
        /// 数据库简介
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 详细介绍
        /// </summary>
        public string Information { get; set; }
        /// <summary>
        /// 详细介绍编辑器
        /// </summary>
        public int InformationEditor { get; set; }
        /// <summary>
        /// 使用指南
        /// </summary>
        public string UseGuide { get; set; }
        /// 使用指南编辑器
        /// </summary>
        public int UseGuideEditor { get; set; }
        /// <summary>
        /// 删除标志
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 状态 1-正常，2-推荐
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 导航显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 当月点击数量
        /// </summary>
        public decimal MonthClickNum { get; set; }
        /// <summary>
        /// 总点击数量
        /// </summary>
        public int TotalClickNum { get; set; }
        /// <summary>
        /// 读者评分
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public decimal OrderIndex { get; set; }
        /// <summary>
        /// 是否已收藏
        /// </summary>
        public bool IsCollected { get; set; }

        /// <summary>
        /// 门户调用的路由
        /// </summary>
        public string PortalUrl { get; set; }

        /// <summary>
        /// 资源统计项
        /// </summary>
        public string ResourceStatistics { get; set; }
        /// <summary>
        /// 资源统计编辑器
        /// </summary>
        public int ResourceStatisticsEditor { get; set; }

        public List<string> UserTypes { get; set; }
        public List<string> UserGroups { get; set; }


        /// <summary>
        /// 详细介绍文本内容
        /// </summary>
        public string InformationText { get; set; }

        /// <summary>
        /// 使用指南文本内容
        /// </summary>
        public string UseGuideText { get; set; }

        /// <summary>
        /// 资源统计文本内容
        /// </summary>
        public string ResourceStatisticsText { get; set; }

        /// <summary>
        /// 资源供应源标识
        /// </summary>
        public string ResourceProvider { get; set; }
    }
}
