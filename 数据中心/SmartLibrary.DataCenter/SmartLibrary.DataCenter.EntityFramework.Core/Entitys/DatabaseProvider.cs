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
    /// <summary>
    /// 数据库商表
    /// </summary>
    public class DatabaseProvider:Entity<Guid>
    {
        /// <summary>
        /// 数据库标识
        /// </summary>
        public int DatabaseCode { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 供应商地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 收款信息
        /// </summary>
        public string Gathering { get; set; }

        /// <summary>
        /// 联系人信息
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 所属地区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContractsTel { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public int Country { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Introduction { get; set; }
    }
}
