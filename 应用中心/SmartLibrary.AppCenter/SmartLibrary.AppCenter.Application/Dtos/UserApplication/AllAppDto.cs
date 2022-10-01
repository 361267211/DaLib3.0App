﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.UserApplication
{
    /// <summary>
    /// 个人应用中心所有应用
    /// </summary>
    public class AllAppDto
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用类型(资源服务/学术服务..)，用于筛选数据
        /// </summary>
        public string AppType { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 应用简介
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 前台访问地址
        /// </summary>
        public string FrontUrl { get; set; }

        /// <summary>
        /// 是否已经收藏
        /// </summary>
        public bool IsCollection { get; set; }

        /// <summary>
        /// 应用编码
        /// </summary>
        public string RouteCode { get; set; }

    }
}
