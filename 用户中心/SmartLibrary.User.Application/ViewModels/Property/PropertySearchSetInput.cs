/*********************************************************
* 名    称：PropertySearchSetInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：可查询属性配置
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 可查询属性配置
    /// </summary>
    public class PropertySearchSetInput
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 是否支持检索
        /// </summary>
        public bool CanSearch { get; set; }
    }
}
