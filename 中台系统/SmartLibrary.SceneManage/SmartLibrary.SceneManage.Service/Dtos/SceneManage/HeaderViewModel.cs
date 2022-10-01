/*********************************************************
 * 名    称：HeaderViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/20 21:57:50
 * 描    述：头部视图模型
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
    public class HeaderViewModel
    {
        /// <summary>
        /// Logo
        /// </summary>
        public string SiteLogo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 访问链接
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// 主导航列表
        /// </summary>
        public List<HeaderNavigationViewModel> MainNavigationList { get; set; }

        /// <summary>
        /// 栏目集合
        /// </summary>
        public List<List<HeaderNavigationViewModel>> NavigationColumnList { get; set; }

        /// <summary>
        /// 个人书斋
        /// </summary>
        public HeaderNavigationViewModel PersonalLibrary { get; set; }

        /// <summary>
        /// 登录
        /// </summary>
        public HeaderNavigationViewModel LogOn { get; set; }

        /// <summary>
        /// 帮助
        /// </summary>
        public HeaderNavigationViewModel HelpInfo { get; set; }


        /// <summary>
        /// 登录
        /// </summary>
        public HeaderNavigationViewModel OldSite { get; set; }

        /// <summary>
        /// 帮助
        /// </summary>
        public HeaderNavigationViewModel EnglishSite { get; set; }
    }

    public class HeaderNavigationViewModel
    {

        /// <summary>
        /// 名称
        /// </summary>
        public string NavigationName { get; set; }


        /// <summary>
        /// 图标
        /// </summary>
        public string NavigationIcon { get; set; }

        /// <summary>
        /// 访问链接
        /// </summary>
        public string NavigationUrl { get; set; }

        /// <summary>
        /// 是否打开新窗口
        /// </summary>
        public bool IsOpenNewWindow { get; set; }
    }
}
