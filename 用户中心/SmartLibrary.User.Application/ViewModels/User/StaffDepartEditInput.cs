/*********************************************************
* 名    称：StaffDepartEditInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：馆员部门设置
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 馆员部门设置
    /// </summary>
    public class StaffDepartEditInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>

        public List<Guid> UserIds { get; set; }
        /// <summary>
        /// 部门信息
        /// </summary>
        public string Depart { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
    }
}
