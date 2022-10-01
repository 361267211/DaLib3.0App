/*********************************************************
 * 名    称：MgrMenu
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2022/3/30 13:39:35
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.AppCenter.Application.Dtos.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.BaseInfo
{
    /// <summary>
    /// 后台顶部对象
    /// </summary>
    public class MgrMenu
    {
        /// <summary>
        /// 顶部导航菜单
        /// </summary>
        public List<AppMenuListDto> AppMenuList { get; set; }

        /// <summary>
        /// Logo地址
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 简版Logo地址
        /// </summary>
        public string SimpleLogoUrl { get; set; }
    }
}
