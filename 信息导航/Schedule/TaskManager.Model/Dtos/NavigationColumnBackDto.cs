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

namespace TaskManager.Model.Entities
{
    public class NavigationColumnBackDto  
    {
        public string Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }

        ///<summary>
        ///状态
        ///</summary>
        public int Status { get; set; }

        ///<summary>
        ///访问地址
        ///</summary>
        public string LinkUrl { get; set; }

        ///<summary>
        ///默认模板
        ///</summary>
        public string DefaultTemplate { get; set; }

        ///<summary>
        ///栏目图标
        ///</summary>
        public string ColumnIcon { get; set; }

        ///<summary>
        ///侧边列表
        ///</summary>
        public int SideList { get; set; }

        ///<summary>
        ///展示的系统信息列表
        ///</summary>
        public string SysMesList { get; set; }

        ///<summary>
        ///目录封面高度
        ///</summary>
        public int CoverHeight { get; set; }

        ///<summary>
        ///目录封面宽度
        ///</summary>
        public int CoverWidth { get; set; }

        ///<summary>
        ///是否登录可用
        ///</summary>
        public bool IsLoginAcess { get; set; }

        ///<summary>
        ///授权访问的用户分组
        ///</summary>
        public string UserGroups { get; set; }

        ///<summary>
        ///授权访问的用户类型
        ///</summary>
        public string UserTypes { get; set; }

        ///<summary>
        ///是否启用意见反馈
        ///</summary>
        public bool IsOpenFeedback { get; set; }

        ///<summary>
        ///删除标志
        ///</summary>
        public bool DeleteFlag { get; set; }

        ///<summary>
        ///头部模板
        ///</summary>
        public string HeadTemplate { get; set; }

        ///<summary>
        ///尾部模板
        ///</summary>
        public string FootTemplate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset? UpdatedTime { get; set; }
        /// <summary>
        /// 2.2的原始id
        /// </summary>
        public int OldId { get; set; }
    }
}
