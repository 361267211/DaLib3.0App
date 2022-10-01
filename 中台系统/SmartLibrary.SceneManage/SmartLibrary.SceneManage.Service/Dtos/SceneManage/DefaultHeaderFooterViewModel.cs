/*********************************************************
 * 名    称：SceneViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/20 21:31:22
 * 描    述：场景视图模型
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
    public class DefaultHeaderFooterViewModel
    {
        /// <summary>
        /// 头部模板
        /// </summary>
        public TemplateListViewModel HeaderTemplate { get; set; }

        /// <summary>
        /// 底部模板
        /// </summary>
        public TemplateListViewModel FooterTemplate { get; set; }

        /// <summary>
        /// api路由
        /// </summary>
        public string ApiRouter { get; set; }

        /// <summary>
        /// 主题颜色
        /// </summary>
        public string ThemeColor { get; set; }

        /// <summary>
        /// 用户中心场景名称
        /// </summary>
        public string UserCenterName { get; set; }
    }
}
