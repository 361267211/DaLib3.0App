/*********************************************************
 * 名    称：SceneApp
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 15:47:05
 * 描    述：场景分屏
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using SmartLibrary.SceneManage.Service.Dtos.SceneManage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.SceneManage.Service.Dtos
{
    /// <summary>
    /// 场景分屏
    /// </summary>
    public class SceneScreenDto
    {

        /// <summary>
        /// 主键标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 场景标识
        /// </summary>
        public string SceneId { get; set; }

        /// <summary>
        /// 分屏名称
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 场景内的应用
        /// </summary>
        public IEnumerable<SceneAppDto> SceneApps { get; set; }
    }
}
