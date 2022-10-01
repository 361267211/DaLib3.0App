/*********************************************************
* 名    称：PropertyShowSetInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：可列表展示属性设置
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 可列表展示属性设置
    /// </summary>
    public class PropertyShowSetInput
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 是否在用户列表展示
        /// </summary>
        public bool ShowOnTable { get; set; }
    }
}
