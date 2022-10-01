/*********************************************************
 * 名    称：PersonalAppCenterViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/12/6 17:20:56
 * 描    述：个人应用中心视图模型
 *
 * 更新历史：
 *
 * *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 个人应用中心
    /// </summary>
    public class PersonalAppCenterViewModel
    {
        /// <summary>
        /// 应用中心路由
        /// </summary>
        public string AppCenterRouteCode { get; set; }

        /// <summary>
        /// 我的应用更多链接
        /// </summary>
        public string MyAppRouteCode { get; set; }

        /// <summary>
        /// 应用列表
        /// </summary>
        public List<AppMenuListDto> AppList { get; set; }
    }
}
