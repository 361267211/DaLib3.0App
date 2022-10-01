/*********************************************************
* 名    称：BaseChangeLogTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：变更日志查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 变更日志查询
    /// </summary>
    public class BaseChangeLogTableQuery:TableQueryBase
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 修改开始时间
        /// </summary>
        public DateTime? ChangeStartTime { get; set; }
        /// <summary>
        /// 修改结束时间
        /// </summary>
        public DateTime? ChangeEndTime { get; set; }
        /// <summary>
        /// 修改结束时间
        /// </summary>
        public DateTime? ChangeEndCompareTime
        {
            get
            {
                if (ChangeEndTime.HasValue)
                {
                    return ChangeEndTime.Value.AddDays(1);
                }
                return null;
            }
        }
        /// <summary>
        /// 操作人
        /// </summary>
        public string ChangeUserName { get; set; }
        /// <summary>
        /// 操作人电话
        /// </summary>
        public string ChangeUserPhone { get; set; }
    }
}
