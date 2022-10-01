using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.Services.Dtos
{
    public class AppBranchDto
    {
        /// <summary>
        /// 应用分支ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        [Required(ErrorMessage = "请选择应用")]
        public Guid AppId { get; set; }
        /// <summary>
        /// 分支名称
        /// </summary>
        [Required(ErrorMessage = "请输入名称")]
        [MaxLength(50, ErrorMessage = "名称最多输入20个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Required(ErrorMessage = "请选择或上传图标")]
        [MaxLength(50, ErrorMessage = "图标最多输入50个字符")]
        public string Icon { get; set; }

        /// <summary>
        /// 部署环境
        /// </summary>
        [Required(ErrorMessage = "请选择部署环境")]
        public Guid DeployeeId { get; set; }

        /// <summary>
        /// 分支版本
        /// </summary>
        [Required(ErrorMessage = "请输入版本号")]
        [MaxLength(20, ErrorMessage = "版本号最多输入20个字符")]
        public string Version { get; set; }

        /// <summary>
        /// 应用备注
        /// </summary>
        [MinLength(0, ErrorMessage = "应用分支备注请输入0-500个字符")]
        [MaxLength(500, ErrorMessage = "应用分支备注请输入0-500个字符")]
        public string Remark { get; set; }


        /// <summary>
        /// 是否主分支
        /// </summary>
        [Required(ErrorMessage = "请设置是否主分支")]
        public bool IsMaster { get; set; }
    }
}
