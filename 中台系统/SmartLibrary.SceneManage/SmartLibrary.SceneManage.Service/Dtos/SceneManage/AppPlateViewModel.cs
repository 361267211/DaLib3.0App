/*********************************************************
 * 名    称：AppPlateViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/20 22:07:24
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.SceneManage.Service.Dtos.SceneManage
{
    public class AppPlateViewModel
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 应用栏目
        /// </summary>
        public List<AppPlateItem> PlateList { get; set; }
    }
    public class AppPlateItem
    {
        /// <summary>
        /// 应用栏目标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用栏目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级场景应用标识（用于个人中心）
        /// </summary>
        public string ParentSceneAppId { get; set; }

        /// <summary>
        /// 应用
        /// </summary>
        public string VisitUrl { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 数据条数
        /// </summary>
        public int? TopCount { get; set; }


        /// <summary>
        /// 排序方式 1-创建时间倒序  2-访问量倒序
        /// </summary>
        public string SortType { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int OrderIndex { get; set; }
    }
}
