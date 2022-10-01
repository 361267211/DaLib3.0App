/*********************************************************
* 名    称：CardClaimDetailOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者领卡详情
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者领卡详情
    /// </summary>
    public class CardClaimDetailOutput
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 条码号
        /// </summary>
        public string BarCode { get; set; }
        /// <summary>
        /// 物理码
        /// </summary>
        public string PhysicCode { get; set; }
    }
}
