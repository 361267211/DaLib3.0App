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

using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.EntityFramework.Core.Entitys
{
    /// 用户角色关系表
    public class SysUserRole : Entity<Guid>
    {

        /// <summary>
        /// 用户ID
        /// </summary>

        public Guid UserID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>

        public Guid RoleID { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>

        public bool DeleteFlag { get; set; }


    }
}
