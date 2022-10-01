/*********************************************************
* 名    称：ScoreCenterSceneOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分中心场景信息
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分中心场景数据
    /// </summary>
    public class ScoreCenterSceneOutput
    {
        public ScoreCenterSceneOutput()
        {
            ScoreTasks = new List<ReaderScoreObtainTask>();
        }
        /// <summary>
        /// 跳转连接
        /// </summary>

        public string LinkUrl { get; set; }
        /// <summary>
        /// 我的积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 勋章个数
        /// </summary>
        public int MedalCount { get; set; }
        /// <summary>
        /// 积分获取任务
        /// </summary>
        public List<ReaderScoreObtainTask> ScoreTasks { get; set; }
    }
}
