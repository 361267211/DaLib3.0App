/*********************************************************
* 名    称：ChangeGoodsInventory.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：商品库存变更
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 商品库存变更
    /// </summary>
    public class ChangeGoodsInventory
    {
        /// <summary>
        /// 防重复提交验证码
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int OperateType { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public int Count { get; set; }
    }
}
