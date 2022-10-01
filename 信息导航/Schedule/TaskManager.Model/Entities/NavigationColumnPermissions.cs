
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 名    称：NavigationColumnPermissions
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 15:37:33
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationColumnPermissions 
    {
        public string Id { get; set; }
        /// <summary>
        /// 栏目ID
        /// </summary>
        public string ColumnID { get; set; }

        /// <summary>
        /// 管理员ID
        /// </summary>
        public string ManagerID { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public int Permission { get; set; }

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset? UpdatedTime { get; set; }
    }
}
