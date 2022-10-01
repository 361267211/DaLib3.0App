/*********************************************************
* 名    称：UserRegisterTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：UserRegisterTableQuery
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户注册申请查询
    /// </summary>
    public class UserRegisterTableQuery : TableQueryBase
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 注册开始日期
        /// </summary>
        public DateTime? RegisterStartTime { get; set; }
        /// <summary>
        /// 注册截止日期
        /// </summary>
        public DateTime? RegisterEndTime { get; set; }
        /// <summary>
        /// 实际比较日期
        /// </summary>
        public DateTime? RegisterEndCompareTime
        {
            get
            {
                if (RegisterEndTime.HasValue)
                {
                    return RegisterEndTime.Value.AddDays(1);
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

    public class UserRegisterEncodeTableQuery : UserRegisterTableQuery
    {

    }
}
