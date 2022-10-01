using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.ViewModel
{
    /// <summary>
    /// 名    称：FrontContentView
    /// 作    者：张泽军
    /// 创建时间：2021/10/22 18:07:27
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class FrontContentView: ContentDto
    {
        public bool IsShowPublishDate { get; set; }
        public bool IsShowHitCount { get; set; }
    }
}
