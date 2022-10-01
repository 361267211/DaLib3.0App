/*********************************************************
* 名    称：GoodsOrderDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：商品订单管理
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 商品订单管理
    /// </summary>
    public class GoodsOrderDto
    {
        /// <summary>
        /// 操作识别码
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public Guid GoodsID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 接收地址
        /// </summary>
        public string RecieveAddress { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
    }
}
