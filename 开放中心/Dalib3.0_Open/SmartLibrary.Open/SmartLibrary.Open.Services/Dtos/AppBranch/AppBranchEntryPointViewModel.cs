using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos
{
    public class AppBranchEntryPointViewModel
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 应用分支ID
        /// </summary>
        public string AppBranchId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string VisitUrl { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 使用场景 1-前台  2-后台
        /// </summary>
        public int UseScene { get; set; }

        /// <summary>
        /// 是否内置，内置不能删除，不能修改编码
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 是默认入口
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string AppEvenName { get; set; }

        /// <summary>
        /// 事件编号
        /// </summary>
        public string AppEvenCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 应用事件
        /// </summary>
        public List<AppEventViewModel> AppEvents { get; set; }
    }
}
