/*********************************************************
* 名    称：BasicConfigEditData.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分规则数据
* 更新历史：
*
* *******************************************************/
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分规则数据
    /// </summary>
    public class BasicConfigEditData
    {
        /// <summary>
        /// 是否显示规则
        /// </summary>
        public bool ShowRule { get; set; }
        /// <summary>
        /// 积分规则
        /// </summary>
        [MaxLength(8000, ErrorMessage = "积分规则最多输入8000个字符")]
        public string RuleContent { get; set; }
    }
}
