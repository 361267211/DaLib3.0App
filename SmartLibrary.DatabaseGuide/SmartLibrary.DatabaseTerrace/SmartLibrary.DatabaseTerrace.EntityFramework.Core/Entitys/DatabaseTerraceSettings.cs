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

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys
{
    /// 数据库平台应用设置
    public class DatabaseTerraceSettings : Entity<Guid>
    {

        /// <summary>
        /// 详情其他介绍内容
        /// </summary>
        [StringLength(maximumLength: 50, MinimumLength = 0), Required]
        public string Introduce { get; set; }

        /// <summary>
        /// 必须登陆访问
        /// </summary>
        public bool IsLoginAcess { get; set; }

        /// <summary>
        /// 是否开启打分评价
        /// </summary>
        public bool IsOpenComment { get; set; }

        /// <summary>
        /// 意见反馈
        /// </summary>
        public bool IsOpenFeedback { get; set; }

        /// <summary>
        /// 支持用户筛选自定义标签
        /// </summary>
        public bool CanFilterCustomLabel { get; set; }

        /// <summary>
        /// 集成图书馆VPN
        /// </summary>
        public bool IsOpenVpn { get; set; }

        /// <summary>
        /// 开启校外独立链接
        /// </summary>
        public bool IsOpenExternalUrl { get; set; }

        /// <summary>
        /// 列表模板
        /// </summary>
        [StringLength(maximumLength: 50, MinimumLength = 0), Required]
        public string Template { get; set; }

        /// <summary>
        /// 头部列表模板
        /// </summary>
        [StringLength(maximumLength: 50, MinimumLength = 0)]
        public string HeadTemplate { get; set; }

        /// <summary>
        /// 尾部列表模板
        /// </summary>
        [StringLength(maximumLength: 50, MinimumLength = 0)]
        public string FootTemplate { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public bool DeleteFlag { get; set; }


        /// <summary>
        /// 默认的排序规则 1-推荐，2-访问量 ，3-首字母
        /// </summary>
        public int DefaultSortRule { get; set; }

    }
}
