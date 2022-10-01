/*********************************************************
* 名    称：CardBatchEditInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡批量修改输入
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者卡批量修改输入
    /// </summary>
    public class CardBatchEditInput
    {
        public CardBatchEditInput()
        {
            CardIDList = new List<Guid>();
            Fields = new List<string>();
            //Properties = new List<CardBatchEditItemInput>();
        }
        public List<Guid> CardIDList { get; set; }
        public List<string> Fields { get; set; }

        /// <summary>
        /// 卡类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 卡类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 发证时间
        /// </summary>
        public DateTime? IssueDate { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpireDate { get; set; }
        //public List<CardBatchEditItemInput> Properties { get; set; }
    }
}
