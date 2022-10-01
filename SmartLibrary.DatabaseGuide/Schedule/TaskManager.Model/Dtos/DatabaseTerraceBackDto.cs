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

namespace TaskManager.Model.Dtos
{
    public class DatabaseTerraceBackDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? UpdatedTime { get; set; }

        public   string CustomUrl { get; set; }
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
        public string ArticleTypes { get; set; }



        /// <summary>
        /// 中图分类
        /// </summary>
        public string DomainClcs { get; set; }

        /// <summary>
        /// 学科分类
        /// </summary>

        public string DomainEscs { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>

        public string PurchaseType { get; set; }

        /// <summary>
        /// 主要语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 自定义标签
        /// </summary>
        public string Label { get; set; }

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
        /// 使用指南
        /// </summary>

        public string UseGuide { get; set; }

        /// <summary>
        /// 资源统计
        /// </summary>

        public string ResourceStatistics { get; set; }


        /// <summary>
        /// 排序序号
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 状态 1-正常，2-推荐
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 导航显示
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 当月点击数量
        /// </summary>
        public Int64 MonthClickNum { get; set; }
        /// <summary>
        /// 总点击数量
        /// </summary>
        public Int64 TotalClickNum { get; set; }
        /// <summary>
        /// 可访问的用户类型
        /// </summary>
        public string UserTypes { get; set; }
        /// <summary>
        /// 可访问的用户分组
        /// </summary>
        public string UserGroups { get; set; }


        /// <summary>
        /// 2.2的原始id
        /// </summary>
        public int OldId { get; set; }


        /// <summary>
        /// 资源统计编辑器
        /// </summary>
        public int ResourceStatisticsEditor { get; set; }

        /// <summary>
        /// 资源统计文本内容
        /// </summary>
        public string ResourceStatisticsText { get; set; }

        /// <summary>
        /// 使用指南编辑器
        /// </summary>
        public int UseGuideEditor { get; set; }

        /// <summary>
        /// 使用指南文本内容
        /// </summary>
        public string UseGuideText { get; set; }

        /// <summary>
        /// 详细介绍编辑器
        /// </summary>
        public int InformationEditor { get; set; }

        /// <summary>
        /// 详细介绍文本内容
        /// </summary>
        public string InformationText { get; set; }
    }
}
