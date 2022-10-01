/*********************************************************
* 名    称：AppBranchViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210912
* 描    述：应用组件视图模型
* 更新历史：
*
* *******************************************************/
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using System;
using System.Collections.Generic;

namespace SmartLibrary.SceneManage.Service.Dtos
{
    /// <summary>
    /// 应用组件视图模型
    /// </summary>
    public class AppWidgetListViewModel
    {
        /// <summary>
        /// 应用组件ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 组件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组件标识
        /// </summary>
        public string WidgetCode { get; set; }

        /// <summary>
        /// 组件内容地址
        /// </summary>
        public string Target { get; set; }


        /// <summary>
        /// 可用配置项 ，1-栏目 2-条数 3-排序字段，多个逗号分隔
        /// </summary>
        public string AvailableConfig { get; set; }


        /// <summary>
        /// 组件封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 默认宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 默认高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }


        /// <summary>
        /// 特殊场景标识 ，1-通用 2-个人中心
        /// </summary>
        public int SceneType { get; set; }

        /// <summary>
        /// 当前模板条数选项
        /// </summary>
        public List<SysDictModel<int>> TopCountList { get;set;}

        /// <summary>
        /// 当前模板排序选项
        /// </summary>
        public List<SysDictModel<string>> SortList { get; set; }

        /// <summary>
        /// 当前模板的栏目
        /// </summary>
        public AppPlateItem AppColumn { get; set; }
    }
}
