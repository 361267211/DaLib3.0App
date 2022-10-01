/*********************************************************
* 名    称：MyStudyInfoOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：我的书斋场景输出
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 我的书斋场景字段
    /// </summary>
    public class MyStudyInfoOutput
    {
        public MyStudyInfoOutput()
        {
            Items = new List<MyStudyItemOutput>();
            Recommends = new List<MyStudyRecommendItem>();
        }
        /// <summary>
        /// 首页 当前借阅等信息
        /// </summary>
        public List<MyStudyItemOutput> Items { get; set; }

        /// <summary>
        /// 我的成果信息
        /// </summary>
        public List<MyStudyRecommendItem> Recommends { get; set; }
    }

    /// <summary>
    /// 我的书斋跳转信息
    /// </summary>
    public class MyStudyItemOutput
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int? Count { get; set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 跳转链接
        /// </summary>
        public string LinkUrl { get; set; }
    }

    /// <summary>
    /// 我的书斋猜你喜欢
    /// </summary>
    public class MyStudyRecommendItem
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public int Date { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 文献类型
        /// </summary>
        public int Type { get; set; }
        public string Creator_Institution { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }
        public string Identifier_Pissn { get; set; }
        /// <summary>
        /// 中图分类
        /// </summary>
        public string Subject_Clc { get; set; }
        /// <summary>
        /// 是否存在封面
        /// </summary>
        public bool Exist_Cover { get; set; }
        public string Creator_en { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 页
        /// </summary>
        public string Page { get; set; }
        /// <summary>
        /// 载体
        /// </summary>
        public string Medium { get; set; }
        /// <summary>
        /// 卷
        /// </summary>
        public string Volume { get; set; }
        /// <summary>
        /// 核心收录
        /// </summary>
        public string Description_Core { get; set; }
        /// <summary>
        /// 文献ID
        /// </summary>
        public string _id { get; set; }
        /// <summary>
        /// 详情链接
        /// </summary>
        public string DetailLink { get; set; }

        /// <summary>
        /// 成果类型 1=我的成果,2=待领成果,3=本院成果,4=本校成果
        /// </summary>
        public int AchievementType { get; set; } = 1;
    }

    /// <summary>
    /// 应用信息
    /// </summary>
    public class AppInfo
    {
        public string AppId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string RouteCode { get; set; }
        public string FrontUrl { get; set; }
        public string BackUrl { get; set; }
    }
}
