/*********************************************************
* 名    称：OrderDetailOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：订单详情信息
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 订单详情信息
    /// </summary>
    public class OrderDetailOutput
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 商品标识
        /// </summary>
        public Guid GoodsID { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string GoodsIntroPicUrl { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        public int GoodsType { get; set; }
        /// <summary>
        /// 兑换码
        /// </summary>
        public string ExchangeCode { get; set; }
        /// <summary>
        /// 消耗积分
        /// </summary>
        public int ExchangeScore { get; set; }
        /// <summary>
        /// 兑换数量
        /// </summary>
        public int ExchangeCount { get; set; }
        /// <summary>
        /// 兑换账号
        /// </summary>
        public string ExchangeUserKey { get; set; }
        /// <summary>
        /// 兑换名称
        /// </summary>
        public string ExchangeName { get; set; }
        /// <summary>
        /// 读者号
        /// </summary>
        public string ExchangeStudentNo { get; set; }
        /// <summary>
        /// 读者学院
        /// </summary>
        public string ExchangeCollegeName { get; set; }
        /// <summary>
        /// 读者系
        /// </summary>
        public string ExchangeCollegeDepartName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 快递名称
        /// </summary>
        public string Express { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }
        /// <summary>
        /// 兑换方式
        /// </summary>
        public int ObtainWay { get; set; }
        /// <summary>
        /// 兑换时间
        /// </summary>
        public string ObtainTime { get; set; }
        /// <summary>
        /// 兑换时间
        /// </summary>
        public DateTime ExchangeTime { get; set; }
    }
}
