/*********************************************************
* 名    称：UserChangeLogTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户信息变更查询条件
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户信息变更查询
    /// </summary>
    public class UserChangeLogTableQuery : BaseChangeLogTableQuery
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

    }

    public class EncodeUserChangeLogTableQuery: UserChangeLogTableQuery
    {

    }
}
