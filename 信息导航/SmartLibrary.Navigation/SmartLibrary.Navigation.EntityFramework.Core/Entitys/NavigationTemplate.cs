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
    /// 名    称：NavigationTemplate
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 15:22:33
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationTemplate: Entity<string>
    {
        ///<summary>
        ///名称
        ///</summary>
        [StringLength(20), Required]
        public string Name { get; set; }

        ///<summary>
        ///头部模板
        ///</summary>
        [StringLength(64), Required]
        public string HeadTemplate { get; set; }

        ///<summary>
        ///尾部模板
        ///</summary>
        [StringLength(64), Required]
        public string FootTemplate { get; set; }

        ///<summary>
        ///描述
        ///</summary>
        [StringLength(300)]
        public string Note { get; set; }

        ///<summary>
        ///预览图
        ///</summary>
        [StringLength(100), Required]
        public string PreviewPic { get; set; }

        ///<summary>
        ///删除标志
        ///</summary>
        public bool DeleteFlag { get; set; }
    }
}
