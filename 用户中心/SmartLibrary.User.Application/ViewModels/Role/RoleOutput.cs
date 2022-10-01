/*********************************************************
* 名    称：RoleOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：角色输出
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 角色输出
    /// </summary>
    public class RoleOutput
    {
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
        /// 馆员人数
        /// </summary>
        public int StaffCount { get; set; }
        /// <summary>
        /// 系统内置
        /// </summary>
        public bool SysBuildIn { get; set; }
    }

    public class RoleMenuOutput
    {
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public Guid MenuPermissionID { get; set; }
        public string MenuName { get; set; }
    }
}
