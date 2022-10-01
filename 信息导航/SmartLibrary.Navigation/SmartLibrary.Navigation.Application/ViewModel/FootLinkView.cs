/*********************************************************
 * 名    称：FootLinkView
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.ViewModel
{
    /// <summary>
    /// 底部链接视图
    /// </summary>
   public class FootLinkView
    {
        /// <summary>
        /// 智图联盟链接
        /// </summary>
        public string AllianceCertifyUrl { get; set; }

        /// <summary>
        /// 快速链接列表
        /// </summary>
        public IList<ContentInfoDto> FastLink { get; set; }

        /// <summary>
        /// 专题链接列表
        /// </summary>
        public IList<ContentInfoDto> AssemblyLink { get; set; }
    }
}
