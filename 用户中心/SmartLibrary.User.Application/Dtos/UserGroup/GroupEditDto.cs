/*********************************************************
* 名    称：GroupEditDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组数据
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;
namespace SmartLibrary.User.Application.Dtos.UserGroup
{
    /// <summary>
    /// 用户组数据
    /// </summary>
    public class GroupEditDto
    {
        public GroupEditDto()
        {
            Rules = new List<PropertyGroupRuleDto>();
            UserIds = new List<Guid>();
        }
        /// <summary>
        /// 用户组ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 数据来源 0:规则创建 1:手动创建
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 创建规则
        /// </summary>
        public List<PropertyGroupRuleDto> Rules { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public List<Guid> UserIds { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public Guid CreateUserID { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }
    }
}
