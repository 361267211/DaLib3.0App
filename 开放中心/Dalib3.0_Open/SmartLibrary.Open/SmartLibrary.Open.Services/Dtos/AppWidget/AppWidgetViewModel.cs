/*********************************************************
* 名    称：AppWidgetViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210913
* 描    述：应用组件视图模型
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用组件视图模型
    /// </summary>
    public class AppWidgetViewModel
    {
        /// <summary>
        /// 应用组件ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 组件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 组件描述信息
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 组件内容地址
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 可用配置 1-栏目 2-数据条数 3-排序字段
        /// 逗号分隔
        /// </summary>
        public string AvailableConfig { get; set; }

        /// <summary>
        /// 最大数据条数
        /// </summary>
        public int MaxTopCount { get; set; }

        /// <summary>
        /// 数据条数间隔
        /// </summary>
        public int TopCountInterval { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset? UpdateTime { get; set; }

        /// <summary>
        /// 默认宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 默认高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 特殊场景标识 ，1-通用 2-个人中心
        /// </summary>
        public int SceneType { get; set; }
    }
}
