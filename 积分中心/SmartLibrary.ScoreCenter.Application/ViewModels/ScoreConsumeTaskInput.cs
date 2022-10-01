/*********************************************************
* 名    称：ScoreConsumeTaskInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分消费获取
* 更新历史：
*
* *******************************************************/
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分消费获取
    /// </summary>
    public class ScoreConsumeTaskInput
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [MaxLength(20,ErrorMessage ="任务名称最多输入20个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        [StringLength(200,ErrorMessage ="任务描述最多输入200个字符")]
        public string Desc { get; set; }
        /// <summary>
        /// 应用编码
        /// </summary>
        [Required(ErrorMessage ="请选择应用")]
        [StringLength(20,ErrorMessage ="应用编码最多输入20个字符")]
        public string AppCode { get; set; }
        /// <summary>
        /// 触发事件编码
        /// </summary>
        [Required(ErrorMessage ="请选择事件")]
        [StringLength(50,ErrorMessage ="事件编码最多输入50个字符")]
        public string EventCode { get; set; }
        /// <summary>
        /// 消耗积分
        /// </summary>
        public int ConsumeScore { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
