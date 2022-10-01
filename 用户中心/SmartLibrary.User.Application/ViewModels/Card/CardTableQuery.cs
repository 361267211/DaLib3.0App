/*********************************************************
* 名    称：CardTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者卡查询
    /// </summary>
    public class CardTableQuery : TableQueryBase
    {
        public CardTableQuery() : base()
        {
            //Conditions = new List<CardTableQueryItem>();
        }
        //public List<CardTableQueryItem> Conditions { get; set; }
        /// <summary>
        /// 是否主卡
        /// </summary>
        public bool? IsPrincipal { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public int? CardStatus { get; set; }
        /// <summary>
        /// 发卡时间
        /// </summary>
        public DateTime? CardIssueStartTime { get; set; }
        /// <summary>
        /// 发卡时间
        /// </summary>
        public DateTime? CardIssueEndTime { get; set; }
        /// <summary>
        /// 发卡时间
        /// </summary>
        public DateTime? CardIssueEndCompareTime
        {
            get
            {
                if (CardIssueEndTime.HasValue)
                {
                    return CardIssueEndTime.Value.AddDays(1);
                }
                return null;
            }
        }
        /// <summary>
        /// 读者名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 学工号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 物理卡号
        /// </summary>
        public string PhysicNo { get; set; }
    }

    public class CardEncodeTableQuery : CardTableQuery
    {

    }
}
