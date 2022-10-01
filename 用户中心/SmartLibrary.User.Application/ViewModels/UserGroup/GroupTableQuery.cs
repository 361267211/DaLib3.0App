/*********************************************************
* 名    称：GroupTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户组查询
    /// </summary>
    public class GroupTableQuery : TableQueryBase
    {
        public DateTime? CreateStartTime { get; set; }
        public DateTime? CreateEndTime { get; set; }
        public DateTime? CreateEndCompareTime
        {
            get
            {
                if (CreateEndTime != null)
                {
                    return CreateEndTime.Value.AddDays(1);
                }
                return null;
            }
        }
    }
}
