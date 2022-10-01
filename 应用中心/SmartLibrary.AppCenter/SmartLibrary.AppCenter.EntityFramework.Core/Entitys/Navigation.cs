/*********************************************************
 * 名    称：Navigation
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/22 18:53:52
 * 描    述：应用导航
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 导航栏目
    /// </summary>
    public class Navigation : Entity<Guid>
    {
        /// <summary>
        /// 栏目名称
        /// </summary>
        [StringLength(50), Required]
        public string Name { get; set; }

        /// <summary>
        /// 是否个人应用优先
        /// </summary>
        [Required]
        public bool PrivateFirst { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
