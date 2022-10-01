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
   public class DatabaseProviderDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 数据库名称
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
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public string DeleteFlag { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Adress { get; set; }

        /// <summary>
        /// 收款信息
        /// </summary>
        public string Gathering { get; set; }

        /// <summary>
        /// 联系人信息
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 所属地区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 公司电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContractsTel { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
