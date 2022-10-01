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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.EntityFramework.Core.Entitys
{
    public class ProviderResource:Entity<Guid>
    {
        /// <summary>
        /// 数据库中心站标识编码
        /// </summary>
        public int DatabaseCode { get; set; }
        /// <summary>
        /// 供应标识 
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 链接名称 
        /// </summary>
        public string LinkTitle { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 数据库全称 
        /// </summary>
        public string TerraceFullName { get; set; }

        /// <summary>
        /// 数据库简称  
        /// </summary>
        public string TerraceShortName { get; set; }

        /// <summary>
        /// 元数据来源  0暂无数据，1网络自动抓取，2元数据人工导入
        /// </summary>
        public int ServiceType { get; set; }

        /// <summary>
        /// 类型 
        /// </summary>
        public int Type { get; set; }


        /// <summary>
        /// 载体  1-电子，2-纸本
        /// </summary>
        public int Medium { get; set; }

        /// <summary>
        /// 语言：0=中文 ：外文
        /// </summary>
        public int LanguageKind { get; set; }

        /// <summary>
        ///  是否常用数据库  
        /// </summary>
        public bool FrequentUsedFlag { get; set; }

        /// <summary>
        /// 供应类型 1-公共  2-已购买的数据库或者学校的纸本  
        /// </summary>
        public int ProviderType { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 数据库介绍
        /// </summary>
        public string Introduction { get; set; }


        /// <summary>
        /// 子库名称  
        /// </summary>
        public string ChildDatabaseName { get; set; }
    }
}
