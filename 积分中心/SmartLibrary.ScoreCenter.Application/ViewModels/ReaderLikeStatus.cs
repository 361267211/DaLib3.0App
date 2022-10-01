/*********************************************************
* 名    称：ReaderLikeStatus.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者标记喜欢状态
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 读者标记喜欢状态
    /// </summary>
    public class ReaderLikeStatus
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public Guid GoodsID { get; set; }
        /// <summary>
        /// 是否喜欢
        /// </summary>
        public bool Like { get; set; }
    }
}
