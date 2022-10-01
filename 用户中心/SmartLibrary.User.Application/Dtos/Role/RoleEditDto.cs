/*********************************************************
* 名    称：RoleEditDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：角色编辑
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos.Role
{
    /// <summary>
    /// 角色编辑
    /// </summary>
    public class RoleEditDto
    {
        public RoleEditDto()
        {
            MenuIds = new List<Guid>();
        }
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 菜单权限
        /// </summary>
        public List<Guid> MenuIds { get; set; }
    }
}
