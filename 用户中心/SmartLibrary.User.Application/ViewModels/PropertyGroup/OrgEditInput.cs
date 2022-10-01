/*********************************************************
* 名    称：OrgEditInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：组织机构管理
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 组织机构编辑
    /// </summary>
    public class OrgEditInput
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid? ID { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public string PId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        [Required(ErrorMessage ="请输入机构名称")]
        [MaxLength(200,ErrorMessage ="机构名称不能大于200")]
        public string Name { get; set; }
        [MaxLength(50, ErrorMessage = "机构编码不能大于50")]
        public string Code { get; set; }
        [MaxLength(200, ErrorMessage = "机构备注不能大于200")]
        public string Remark { get; set; }
    }
}
