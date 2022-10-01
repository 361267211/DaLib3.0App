using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.Services.Dtos
{
    public class AppBranchEntryPointDto
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 应用类型ID
        /// </summary>
        [Required(ErrorMessage = "请选择应用")]
        public string AppId { get; set; }
        /// <summary>
        /// 应用分支ID
        /// </summary>
        [Required(ErrorMessage = "请选择应用分支")]
        public string AppBranchId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "请输入编码")]
        [MaxLength(20, ErrorMessage = "编码最多输入20个字符")]
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "请输入名称")]
        [MaxLength(20, ErrorMessage = "名称最多输入20个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [Required(ErrorMessage = "请输入路径")]
        [MaxLength(20, ErrorMessage = "路径最多输入20个字符")]
        public string VisitUrl { get; set; }
    }
}
