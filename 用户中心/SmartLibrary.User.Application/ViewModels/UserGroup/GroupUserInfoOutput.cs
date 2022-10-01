/*********************************************************
* 名    称：GroupUserInfoOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组读者关系
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户组读者关系表
    /// </summary>
    public class GroupUserInfoOutput
    {
        public Guid ID { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int SourceFrom { get; set; }
        public string Phone { get; set; }
    }
    /// <summary>
    /// 用户组读者查询条件
    /// </summary>

    public class GroupUserImportSearchInput
    {
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
    }
}
