/*********************************************************
* 名    称：InfoPermitStaffOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：可修改信息权限用户配置
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 可修改信息权限用户配置
    /// </summary>
    public class InfoPermitStaffOutput
    {
        public InfoPermitStaffOutput()
        {
            Properties = new List<UserPropertyItemOutput>();
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 是否已选择
        /// </summary>
        public bool Checked { get; set; }
        /// <summary>
        /// 用户属性
        /// </summary>
        public List<UserPropertyItemOutput> Properties { get; set; }
    }
}
