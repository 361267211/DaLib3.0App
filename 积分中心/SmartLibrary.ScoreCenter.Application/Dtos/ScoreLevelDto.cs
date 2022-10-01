/*********************************************************
* 名    称：ScoreLevelDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分等级信息
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 积分等级信息
    /// </summary>
    public class ScoreLevelDto
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 达成积分
        /// </summary>
        public int ArchiveScore { get; set; }
    }
}
