/*********************************************************
* 名    称：ReaderScoreLevelOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者积分等级信息
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 读者积分等级信息
    /// </summary>
    public class ReaderScoreLevelOutput
    {
        public ReaderScoreLevelOutput()
        {
            Levels = new List<ScoreLevelListItemOutput>();
        }
        /// <summary>
        /// 用户积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 当前等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 当前等级名称
        /// </summary>
        public string LevelName { get; set; }
        /// <summary>
        /// 下个等级所需积分
        /// </summary>
        public int NextLevelScore { get; set; }
        /// <summary>
        /// 积分等级
        /// </summary>

        public List<ScoreLevelListItemOutput> Levels { get; set; }
    }

}
