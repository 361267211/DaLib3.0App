/*********************************************************
* 名    称：ScoreLevelInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分等级信息
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分等级信息
    /// </summary>
    public class ScoreLevelInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "请输入等级名称")]
        [MaxLength(100, ErrorMessage = "等级名称最多输入100个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 达成积分
        /// </summary>
        public int ArchiveScore { get; set; }
    }
}
