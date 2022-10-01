/*********************************************************
* 名    称：ReaderEditPropertyOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者可编辑属性输出
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者可编辑属性输出
    /// </summary>
    public class ReaderEditPropertyOutput
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 属性编码
        /// </summary>
        public string PropertyCode { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool IsUnique { get; set; }
    }
}
