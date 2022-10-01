/*********************************************************
* 名    称：GroupEditInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户组
    /// </summary>
    public class GroupEditInput
    {
        public GroupEditInput()
        {
            Rules = new List<PropertyGroupRuleInput>();
            UserIds = new List<Guid>();
        }
        /// <summary>
        /// 用户组ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户组名称
        /// </summary>
        [Required(ErrorMessage = "请输入用户组名称")]
        [MaxLength(10, ErrorMessage = "用户名称长度不能大于10")]
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "备注长度不能大于200")]
        public string Desc { get; set; }
        /// <summary>
        /// 数据来源 0:规则创建 1:手动创建
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 创建规则
        /// </summary>
        public List<PropertyGroupRuleInput> Rules { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public List<Guid> UserIds { get; set; }

    }
}
