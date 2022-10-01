/*********************************************************
* 名    称：AppListViewModel
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210912
* 描    述：应用列表视图模型
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用列表视图模型
    /// </summary>
    public class AppListViewModel
    {
        /// <summary>
        /// 应用分支ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 部署环境
        /// </summary>
        public string DeployeeID { get; set; }
        /// <summary>
        /// 分支名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 当前版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 是否主分支
        /// </summary>
        public bool IsMaster { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 网关地址
        /// </summary>
        public string ApiHost { get; set; }

        /// <summary>
        /// 前台入口
        /// </summary>
        public string FrontUrl { get; set; }

        /// <summary>
        /// 后台入口
        /// </summary>
        public string BackendUrl { get; set; }

        /// <summary>
        /// 开发者名称
        /// </summary>
        public string DeveloperName { get; set; }

        /// <summary>
        /// 授权类型 1、正式授权 2、试用授权
        /// </summary>
        public string PurcaseType { get; set; }

        /// <summary>
        /// 采购类型名称
        /// </summary>
        public string PurcaseTypeName { get; set; }

        /// <summary>
        /// 服务类型 1、基础应用 2、资源服务 3、学术与情报 4、阅读推广 5、空间服务 6、管理与馆务 7、分析决策
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// 服务类型名称
        /// </summary>
        public string ServiceTypeName { get; set; }

        /// <summary>
        /// 使用场景  1-前台 2-后台 3-通用
        /// </summary>
        public string UseScene { get; set; }

        /// <summary>
        /// 适用场景类型名称
        /// </summary>
        public string SceneTypeName { get; set; }

        /// <summary>
        /// 适用终端 1-PC端、2-APP端、3-小程序端、4-自适应移动端、5-显示屏
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// 路由编码
        /// </summary>
        public string RouteCode { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        public string TerminalName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }


        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 建议价
        /// </summary>
        public decimal  AdvisePrice { get; set; }

        /// <summary>
        /// 应用入口
        /// </summary>
        public IEnumerable<AppBranchEntryPointViewModel> AppEntrances { get; set; }

        /// <summary>
        /// 应用组件
        /// </summary>
        public IEnumerable<AppWidgetViewModel> AppWidgets { get; set; }

        /// <summary>
        /// 应用允许的排序字段
        /// </summary>
        public IEnumerable<AppAvailibleSortFieldViewModel> AppAvailibleSortFields { get; set; }
    }
}
