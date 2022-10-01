/*********************************************************
* 名    称：ReaderScoreTaskTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者积分任务查询
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 读者积分任务查询
    /// </summary>
    public class ReaderScoreTaskTableQuery : TableQueryBase
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 终端类型
        /// </summary>
        public int? EndPointType { get; set; }
    }
}
