/*********************************************************
* 名    称：SysRoleDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：2022032
* 描    述：系统角色
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.Permission
{
    /// <summary>
    /// 系统角色
    /// </summary>
    public class SysRoleDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>

        public string Code { get; set; }

        /// <summary>
        /// 备注
        /// </summary>

        public string Remark { get; set; }
    }
}
