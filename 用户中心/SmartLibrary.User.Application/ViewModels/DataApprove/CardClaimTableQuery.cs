/*********************************************************
* 名    称：CardClaimTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者领卡查询条件
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者领卡查询条件
    /// </summary>
    public class CardClaimTableQuery : TableQueryBase
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 申请开始日期
        /// </summary>
        public DateTime? ApplyStartTime { get; set; }
        /// <summary>
        /// 申请截止日期
        /// </summary>
        public DateTime? ApplyEndTime { get; set; }
        /// <summary>
        /// 申请比较日期
        /// </summary>
        public DateTime? ApplyEndCompareTime
        {
            get
            {
                if (ApplyEndTime.HasValue)
                {
                    return ApplyEndTime.Value.AddDays(1);
                }
                return null;
            }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string UserPhone { get; set; }
    }

    public class CardClaimEncodeTableQuery : CardClaimTableQuery
    {

    }
}
