/*********************************************************
* 名    称：CardPropertyInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡属性输入
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者卡属性输入
    /// </summary>
    public class CardPropertyInput
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid PropertyID { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        [StringLength(250)]
        public string PropertyValue { get; set; }
    }
}
