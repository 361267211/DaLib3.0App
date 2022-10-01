/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.ViewModels
{
    public class MyFavoriteDatabasesReply
    {
        public string MoreUrl { get; set; }

        public List<DatabaseItemViewModel> Databases { get; set; }
    }

    public class DatabaseItemViewModel
    {
        /// <summary>
        /// 平台/数据库名称
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 平台/数据库名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 直接访问的列表
        /// </summary>
        public List<DatabaseAcessUrlDto> DirectUrls { get; set; }
        /// <summary>
        /// 间接访问
        /// </summary>
        public string IndirectUrl { get; set; }

        /// <summary>
        /// 门户调用的路由
        /// </summary>
        public string PortalUrl { get; set; }
        /// <summary>
        /// 来源   1-用户订阅/2-行为日志推荐
        /// </summary>
        public int Type { get; set; }
    }
}
