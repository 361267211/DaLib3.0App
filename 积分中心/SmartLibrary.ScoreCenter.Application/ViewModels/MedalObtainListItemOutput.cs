/*********************************************************
* 名    称：MedalObtainListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：勋章管理列表
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 勋章管理列表
    /// </summary>
    public class MedalObtainListItemOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 勋章图片
        /// </summary>
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 任务有效期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
    }
}
