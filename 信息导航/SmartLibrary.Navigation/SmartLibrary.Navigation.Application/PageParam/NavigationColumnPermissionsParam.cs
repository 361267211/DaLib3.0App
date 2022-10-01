using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.PageParam
{
    /// <summary>
    /// 名    称：NewsColumnPermissionsParam
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 11:20:37
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationColumnPermissionsParam
    {
        /// <summary>
        /// 栏目ID
        /// </summary>
        [Required]
        public string ColumnID { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        [Required]
        public int Permission { get; set; }

        /// <summary>
        /// 管理员列表
        /// </summary>
        public List<ManagerParam> ManagerList { get; set; }
    }

    public class ManagerParam
    {
        /// <summary>
        /// 管理员ID
        /// </summary>
        [Required]
        public string ManagerID { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        [StringLength(50), Required]
        public string Manager { get; set; }
    }
}
