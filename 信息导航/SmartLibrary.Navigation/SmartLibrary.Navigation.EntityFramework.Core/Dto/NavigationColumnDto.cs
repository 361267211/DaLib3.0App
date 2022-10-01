using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.EntityFramework.Core.Dto
{
    /// <summary>
    /// 名    称：NavigationColumnDto
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 16:22:52
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationColumnDto
    {
        ///<summary>
        ///主键
        ///</summary>
        [Key, StringLength(64)]
        public string Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(50), Required]
        public string Title { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [StringLength(50)]
        public string Label { get; set; }

        /// <summary>
        /// 自定义标签键值对
        /// </summary>
        public List<KeyValuePair<string, string>> LabelKV { get; set; }

        ///<summary>
        ///状态
        ///</summary>
        [Required]
        public int Status { get; set; }

        ///<summary>
        ///访问地址
        ///</summary>
        [StringLength(100)]
        public string LinkUrl { get; set; }

        ///<summary>
        ///默认模板
        ///</summary>
        [StringLength(64), Required]
        public string DefaultTemplate { get; set; }

        ///<summary>
        ///栏目图标
        ///</summary>
        [StringLength(100)]
        public string ColumnIcon { get; set; }

        ///<summary>
        ///侧边列表
        ///</summary>
        [Required]
        public int SideList { get; set; }

        ///<summary>
        ///展示的系统信息列表
        ///</summary>
        [StringLength(100)]
        public string SysMesList { get; set; }

        /// <summary>
        /// 展示的系统信息列表的键值对
        /// </summary>
        public List<KeyValuePair<int, string>> SysMesListKV { get; set; }

        ///<summary>
        ///目录封面高度
        ///</summary>
        [Required]
        public int CoverHeight { get; set; }

        ///<summary>
        ///目录封面宽度
        ///</summary>
        [Required]
        public int CoverWidth { get; set; }

        ///<summary>
        ///是否登录可用
        ///</summary>
        [Required]
        public bool IsLoginAcess { get; set; }

        ///<summary>
        ///授权访问的用户分组
        ///</summary>
        [StringLength(500)]
        public List<string> UserGroups { get; set; }

        ///<summary>
        ///授权访问的用户类型
        ///</summary>
        [StringLength(500)]
        public List<string> UserTypes { get; set; }

        ///<summary>
        ///是否启用意见反馈
        ///</summary>
        [Required]
        public bool IsOpenFeedback { get; set; }

        ///<summary>
        ///头部模板
        ///</summary>
        public string HeadTemplate { get; set; }

        ///<summary>
        ///尾部模板
        ///</summary>
        public string FootTemplate { get; set; }
    }
}
