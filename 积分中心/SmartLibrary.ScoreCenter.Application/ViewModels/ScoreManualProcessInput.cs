/*********************************************************
* 名    称：ScoreManualProcessInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分奖惩
* 更新历史：
*
* *******************************************************/
using SmartLibrary.ScoreCenter.Application.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 客户手动操作
    /// </summary>
    public class ScoreManualProcessInput
    {
        public ScoreManualProcessInput()
        {
            Users = new List<ScoreRecieveUserDto>();
            Rules = new List<ScoreRecieveRuleDto>();
        }
        /// <summary>
        /// 描述
        /// </summary>
        [Required(ErrorMessage = "请填写描述")]
        [MaxLength(150, ErrorMessage = "描述信息最多输入150个字符")]
        public string Desc { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 积分有效期
        /// </summary>
        public int ValidTerm { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 执行状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 用户来源
        /// </summary>
        public int SourceFrom { get; set; }

        public List<ScoreRecieveUserDto> Users { get; set; }
        public List<ScoreRecieveRuleDto> Rules { get; set; }
    }
}
