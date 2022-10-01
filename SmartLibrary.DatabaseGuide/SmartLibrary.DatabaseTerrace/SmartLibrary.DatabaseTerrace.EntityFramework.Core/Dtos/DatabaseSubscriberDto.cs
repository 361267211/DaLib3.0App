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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos
{
    public class DatabaseSubscriberDto : BaseDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 数据库ID
        /// </summary>

        public Guid DatabaseID { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>

        public string Title { get; set; }


        /// <summary>
        /// 数据库类型  我的收藏
        /// </summary>

        public string TypeName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>

        public string UserKey { get; set; }

        /// <summary>
        /// 封面地址
        /// </summary>

        public string Cover { get; set; }

        /// <summary>
        /// 简介
        /// </summary>

        public string Introduction { get; set; }

        /// <summary>
        /// 总访问量
        /// </summary>

        public Int64 TotalCount { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>

        public bool DeleteFlag { get; set; }


        /// <summary>
        /// 数据库的创建时间
        /// </summary>

        public DateTimeOffset DatabaseCreatedTime { get; set; }


        /// <summary>
        /// 直接访问的列表
        /// </summary>
        public List<DatabaseAcessUrlDto> DirectUrls { get; set; }
    }
}
