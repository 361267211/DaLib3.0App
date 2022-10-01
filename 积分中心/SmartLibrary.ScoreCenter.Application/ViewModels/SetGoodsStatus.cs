/*********************************************************
* 名    称：SetGoodsStatus.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：批量设置商品状态
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 设置商品状态
    /// </summary>
    public class SetGoodsStatus
    {
        /// <summary>
        /// 标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
    /// <summary>
    /// 批量设置商品状态
    /// </summary>
    public class BatchSetGoodsStatus
    {
        /// <summary>
        /// 标识
        /// </summary>
        public List<Guid> IdList { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
