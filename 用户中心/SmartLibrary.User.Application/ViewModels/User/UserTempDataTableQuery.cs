/*********************************************************
* 名    称：UserTempDataTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者导入数据查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者导入数据查询
    /// </summary>
    public class UserTempDataTableQuery : TableQueryBase
    {
        /// <summary>
        /// 批处理Id
        /// </summary>
        public Guid BatchId { get; set; }
        /// <summary>
        /// 是否存在错误的数据
        /// </summary>
        public bool? IsError { get; set; }
    }
}
