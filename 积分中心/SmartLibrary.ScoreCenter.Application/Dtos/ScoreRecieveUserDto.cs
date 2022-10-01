/*********************************************************
* 名    称：ScoreRecieveUserDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分奖惩读者关联
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 积分奖惩读者关联
    /// </summary>
    public class ScoreRecieveUserDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid ProcessID { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        [Required(ErrorMessage = "请输入用户标识")]
        public string UserKey { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public int SourceFrom { get; set; }
    }
}
