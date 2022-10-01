/*********************************************************
* 名    称：InfoPermitStaffTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：修改信息用户条件查询
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 修改信息用户查询
    /// </summary>
    public class InfoPermitStaffTableQuery 
    {
        public InfoPermitStaffTableQuery() 
        {
            Conditions = new List<UserTableQueryItem>();
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        public List<UserTableQueryItem> Conditions { get; set; }
    }
}
