/*********************************************************
* 名    称：ScoreRankListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分排行列表数据
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 积分排行列表数据
    /// </summary>
    public class ScoreRankListItemDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户学院
        /// </summary>
        public string UserCollege { get; set; }
        /// <summary>
        /// 用户系 
        /// </summary>
        public string UserCollegeDepart { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
    }
}
