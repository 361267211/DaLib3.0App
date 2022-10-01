﻿/*********************************************************
* 名    称：ScoreConsumeListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分消费列表数据
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 积分消费列表数据
    /// </summary>
    public class ScoreConsumeListItemDto
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 消费积分
        /// </summary>
        public int ConsumeScore { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTimeOffset CreateTime { get; set; }
    }
}
