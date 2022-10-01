using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SmartLibrary.News.Utility.EnumTool;
using SmartLibrary.News.Application.Enums;
using SmartLibrary.News.Utility;
using SmartLibrary.News.Application.ViewModel;

namespace SmartLibrary.News.Application.Dto
{
    /// <summary>
    /// 名    称：NewsColumnDto
    /// 作    者：张泽军
    /// 创建时间：2021/9/8 16:27:03
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsColumnDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, StringLength(64)]
        public string Id { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [StringLength(maximumLength: 50, MinimumLength = 2), Required]
        public string Title { get; set; }

        /// <summary>
        /// 栏目别名
        /// </summary>
        [StringLength(50)]
        public string Alias { get; set; }

        /// <summary>
        /// 自定义标签
        /// </summary>
        [StringLength(500)]
        public string Label { get; set; }

        /// <summary>
        /// 自定义标签键值对
        /// </summary>
        public List<KeyValuePair<string, string>> LabelKV { get; set; }

        /// <summary>
        /// 是否多终端同步
        /// </summary>
        [Required]
        public int Terminals { get; set; }

        /// <summary>
        /// 状态 1:启用，2:下架
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 内容扩展项
        /// </summary>
        [StringLength(300), Required]
        public string Extension { get; set; }

        /// <summary>
        /// 内容扩展项键值对
        /// </summary>
        public List<KeyValuePair<string, string>> ExtensionKV
        {
            get
            {
                if (string.IsNullOrEmpty(Extension))
                    return null;
                List<KeyValuePair<string, string>> listKV = new List<KeyValuePair<string, string>>();
                foreach (var ext in Extension.Split(';'))
                {
                    listKV.Add(new KeyValuePair<string, string>(ext.Split('-')[0], ext.Split('-')[1]));
                }
                return listKV;
            }
        }


        /// <summary>
        /// 栏目地址
        /// </summary>
        [StringLength(500)]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 默认模板
        /// </summary>
        [StringLength(64), Required]
        public string DefaultTemplate { get; set; }

        /// <summary>
        /// 头部模板
        /// </summary>
        [StringLength(64)]
        public string HeadTemplate { get; set; }

        /// <summary>
        /// 尾部模板
        /// </summary>
        [StringLength(64)]
        public string FootTemplate { get; set; }

        /// <summary>
        /// 侧边列表
        /// </summary>
        [StringLength(100)]
        public string SideList { get; set; }

        /// <summary>
        /// 侧边列表的键值对
        /// </summary>
        public List<KeyValuePair<int, string>> SideListKV
        {
            get
            {
                return EnumUtils.GetValueName(typeof(SideListEnum), SideList);
            }
        }

        /// <summary>
        /// 展示的系统信息列表
        /// </summary>
        [StringLength(100)]
        public string SysMesList { get; set; }

        /// <summary>
        /// 展示的系统信息列表的键值对
        /// </summary>
        public List<KeyValuePair<int, string>> SysMesListKV
        {
            get
            {
                return EnumUtils.GetValueName(typeof(SysMesListEnum), SysMesList);
            }
        }

        ///<summary>
        ///启用封面
        ///</summary>
        [Required]
        public int IsOpenCover { get; set; }



        /// <summary>
        /// 是否登录可用
        /// </summary>
        [Required]
        public int IsLoginAcess { get; set; }

        /// <summary>
        /// 授权访问列表字符串
        /// </summary>
        [StringLength(500)]
        public string VisitingList { get; set; }

        /// <summary>
        /// 授权访问列表字符串对象化
        /// </summary>
        public VisitingListModel VisitingListModel
        {
            get
            {
                if (string.IsNullOrEmpty(VisitingList) || VisitingList == "-1")
                    return null;
                else
                {
                    var type = VisitingList.Split('|')[0];
                    var visitList = VisitingList.Split('|')[1].Split(';');
                    List<KeyValuePair<string, string>> vList = new List<KeyValuePair<string, string>>();
                    foreach (var item in visitList)
                    {
                        vList.Add(new KeyValuePair<string, string>(item.Split('-')[0], item.Split('-')[1]));
                    }
                    return new VisitingListModel
                    {
                        Type = Converter.ObjectToInt(type),
                        VisitList = vList
                    };
                }
            }
        }

        /// <summary>
        /// 是否开启评论
        /// </summary>
        [Required]
        public int IsOpenComment { get; set; }

        ///// <summary>
        ///// 是否开启留言
        ///// </summary>
        //[Required]
        //public int IsOpenLeaveMes { get; set; }

        /// <summary>
        /// 是否开启审核流程
        /// </summary>
        [Required]
        public int IsOpenAudit { get; set; }

        /// <summary>
        /// 审核流程
        /// </summary>
        [StringLength(50)]
        public string AuditFlow { get; set; }

        /// <summary>
        /// 审核流程键值对
        /// </summary>
        public List<KeyValuePair<int, string>> AuditFlowKV
        {
            get
            {
                return EnumUtils.GetValueName(typeof(AuditProcessEnum), AuditFlow);
            }
        }

        /// <summary>
        /// 栏目下添加新闻的默认审核状态
        /// </summary>
        public List<KeyValuePair<int, string>> ContentDefaultAuditStatusKV { get; set; }

        ///// <summary>
        ///// 子类 (标签）
        ///// </summary>
        //public string[] ChildClass { get; set; }

        /// <summary>
        /// 封面宽
        /// </summary>
        public int CoverWidth { get; set; }

        /// <summary>
        /// 封面高
        /// </summary>
        public int CoverHeight { get; set; }


        /// <summary>
        /// 允许所有用户访问
        /// </summary>
        public bool AcessAll { get; set; }

        /// <summary>
        /// 栏目地址
        /// </summary>
        public string ColumnUrl { get; set; }
    }
}
