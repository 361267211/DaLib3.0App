using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 名    称：NavigationLableInfo
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 16:16:22
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationLableInfo: Entity<string>
    {
        ///<summary>
        ///内容标题
        ///</summary>
        [StringLength(20), Required]
        public string Title { get; set; }

        ///<summary>
        ///删除标识
        ///</summary>
        public bool DeleteFlag { get; set; }
    }
}
