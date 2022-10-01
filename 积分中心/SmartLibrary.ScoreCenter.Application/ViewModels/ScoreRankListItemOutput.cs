/*********************************************************
* 名    称：ScoreRankListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分排行列表
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分排行列表
    /// </summary>
    public class ScoreRankListItemOutput
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 读者标识
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 学院
        /// </summary>
        public string UserCollege { get; set; }
        /// <summary>
        /// 系
        /// </summary>
        public string UserCollegeDepart { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
    }
}
