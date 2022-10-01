/*********************************************************
 * 名    称：BaseInfo
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/12/3 17:54:54
 * 描    述：当前用户和机构的基础信息
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.SceneManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.BaseInfo
{
    /// <summary>
    /// 当前用户和机构的基础信息
    /// </summary>
    public class BaseInfo
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 机构信息
        /// </summary>
        public OrgInfo OrgInfo { get; set; }

        /// <summary>
        /// 头部底部信息
        /// </summary>
        public HeaderFooterReply HeaderFooterInfo { get; set; }

        /// <summary>
        /// URL信息
        /// </summary>
        public List<UrlInfo> UrlInfo { get; set; }
    }
}
