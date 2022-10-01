/*********************************************************
* 名    称：ReaderGoodsTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：用户积分商城查询
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 用户积分商城查询
    /// </summary>
    public class ReaderGoodsTableQuery : TableQueryBase
    {
        /// <summary>
        /// 热门兑换
        /// </summary>
        public bool HotExchange { get; set; }
        /// <summary>
        /// 最近上新
        /// </summary>
        public bool LatestTime { get; set; }
        /// <summary>
        /// 奖品类型
        /// </summary>
        public int? GoodsType { get; set; }
        /// <summary>
        /// 积分范围
        /// </summary>
        public int? ScoreRange { get; set; }
        /// <summary>
        /// 我喜欢的
        /// </summary>
        public bool Like { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserKey { get; set; }
    }
}
