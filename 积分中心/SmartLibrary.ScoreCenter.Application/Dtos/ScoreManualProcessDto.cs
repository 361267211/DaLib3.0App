/*********************************************************
* 名    称：ScoreManualProcessDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分奖惩任务
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 积分奖惩任务
    /// </summary>
    public class ScoreManualProcessDto
    {
        public Guid ID { get; set; }
        public ScoreManualProcessDto()
        {
            Users = new List<ScoreRecieveUserDto>();
            Rules = new List<ScoreRecieveRuleDto>();
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 积分有效期
        /// </summary>
        public int ValidTerm { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 用户来源
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 操作者
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 操作者Userkey
        /// </summary>
        public string OperatorUserKey { get; set; }

        public List<ScoreRecieveUserDto> Users { get; set; }
        public List<ScoreRecieveRuleDto> Rules { get; set; }
    }
}
