/*********************************************************
* 名    称：ReaderScoreInfoOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者积分信息
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 读者积分信息
    /// </summary>
    public class ReaderScoreInfoOutput
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 我的积分
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 已消耗积分
        /// </summary>
        public int ConsumeScore { get; set; }

        /// <summary>
        /// 当前等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 我的勋章
        /// </summary>
        public int MedalCount { get; set; }

        /// <summary>
        /// 我的兑换
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 商品兑换数量
        /// </summary>
        public int GoodsCount { get; set; }
    }
}
