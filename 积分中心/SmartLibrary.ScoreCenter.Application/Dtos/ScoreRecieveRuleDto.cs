/*********************************************************
* 名    称：ScoreRecieveRuleDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：获取积分读者规则
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 获取积分读者规则
    /// </summary>
    public class ScoreRecieveRuleDto
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid ProcessID { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public int RuleType { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        public string PropertyCode { get; set; }
        /// <summary>
        /// 操作符
        /// </summary>
        public int OperateType { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string PropertyValue { get; set; }
    }
}
