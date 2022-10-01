using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos
{
    public class CustomerAppUsageDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        [Required(ErrorMessage = "请选择应用")]
        public Guid AppID { get; set; }
        /// <summary>
        /// 应用分支ID
        /// </summary>
        [Required(ErrorMessage = "请选择应用分支")]
        public Guid AppBranchID { get; set; }
        /// <summary>
        //客户ID
        /// </summary>
        [Required(ErrorMessage = "请选择客户")]
        public Guid CustomerID { get; set; }

    }
}
