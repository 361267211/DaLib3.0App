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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.EntityFramework.Core.Entitys.Assembly
{
    /// 汇编专题的基本信息
    public class AssemblyBaseInfo:Entity<Guid>
    {
        /// <summary>
        /// 专题名称
        /// </summary>
        [StringLength(48), Required]
        public string AssemblyName { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        [StringLength(40), Required]
        public Guid ColumnId { get; set; }

        /// <summary>
        /// 简要介绍
        /// </summary>
        [StringLength(500), Required]
        public string Description { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        [StringLength(200), Required]
        public string KeyWords { get; set; }

        /// <summary>
        /// 分类标签
        /// </summary>
        [StringLength(200), Required]
        public string Label { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        [StringLength(200), Required]
        public string Template { get; set; }

        /// <summary>
        /// 维护人的id
        /// </summary>

        public string MaintainerUserKeys { get; set; }



        /// <summary>
        /// 创建人的id
        /// </summary>

        public string CreaterUserKey { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        [StringLength(200), Required]
        public string Cover { get; set; }

        /// <summary>
        /// 专题来源
        /// </summary>

        public int Source { get; set; }

        /// <summary>
        /// 关注人数量
        /// </summary>

        public int FocusCounts { get; set; }

        /// <summary>
        /// 状态 1-推荐，2-正常，3-屏蔽
        /// </summary>

        public int Status { get; set; }

        /// <summary>
        /// 联盟服务类型 1-设置共享，2-已共享，3-已获取
        /// </summary>

        public int UnionServiceType { get; set; }


        /// <summary>
        /// 联盟共享Id，仅当联盟服务类型为已获取时有此id
        /// </summary>
        [StringLength(40), Required]
        public Guid SharedId { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>

        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 审核状态 1-待审核 ，2已审核，3-未通过
        /// </summary>

        public int AuditStatus { get; set; }

        /// <summary>
        /// 共享次数
        /// </summary>
        public int SharedCount { get; set; }

        /// <summary>
        /// 共享时间
        /// </summary>
        public DateTime SharedTime { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        [StringLength(200)]
        public string RejectionReason { get; set; }

        /// <summary>
        /// 所属组织标识
        /// </summary>
        [StringLength(20), Required]
        public string OrgCode { get; set; }

        /// <summary>
        /// 推荐类型 
        /// </summary>
        [StringLength(40)]
        public string RecommendType { get; set; }

        /// <summary>
        /// 初始的创建者姓名
        /// </summary>
        public string OrginCreaterName { get; set; }

        /// <summary>
        /// 初始的持有图书馆名
        /// </summary>
        public string OrginOwnerName { get; set; }

    }
}
