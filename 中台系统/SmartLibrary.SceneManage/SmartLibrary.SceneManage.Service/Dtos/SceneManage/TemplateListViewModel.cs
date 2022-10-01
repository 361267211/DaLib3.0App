/*********************************************************
 * 名    称：TemplateListViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/11/8 21:12:17
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Service.Dtos.SceneManage
{
    /// <summary>
    /// 场景模板视图模型
    /// </summary>
    public class TemplateListViewModel
    {
        /// <summary>
        /// 模板Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 屏数
        /// </summary>
        public int ScreenCount { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }

        /// <summary>
        /// 宽高比
        /// </summary>
        public string AspectRatio { get; set; }

        /// <summary>
        /// 模板路由
        /// </summary>
        public string Router { get; set; }

        /// <summary>
        /// 模板编码
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 适用布局标识
        /// </summary>
        public int LayoutId { get; set; }

        /// <summary>
        /// 模板类型 1-场景 2-头部 3-底部
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 默认头部模板
        /// </summary>
        public TemplateListViewModel DefaultHeaderTemplate { get; set; }

        /// <summary>
        /// 默认底部模板
        /// </summary>
        public TemplateListViewModel DefaultFooterTemplate { get; set; }

        /// <summary>
        /// 富文本内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// JS文件的路径
        /// </summary>
        public string JsPath { get; set; }

        /// <summary>
        /// logo图标的路径
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 展示的导航栏目id
        /// </summary>
        public string DisplayNavColumn { get; set; }

    }
}
