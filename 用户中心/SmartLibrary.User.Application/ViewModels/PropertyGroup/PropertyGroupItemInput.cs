/*********************************************************
* 名    称：PropertyGroupItemInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性组选项输入
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性组选项输入
    /// </summary>
    public class PropertyGroupItemInput
    {
        /// <summary>
        /// 属性项ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 属性组ID
        /// </summary>
        public Guid GroupID { get; set; }
        /// <summary>
        /// 属性组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        [Required(ErrorMessage = "请输入分组名称")]
        [StringLength(20, ErrorMessage = "分组名称最多输入20个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}
