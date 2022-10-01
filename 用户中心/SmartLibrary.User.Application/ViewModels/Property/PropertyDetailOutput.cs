/*********************************************************
* 名    称：PropertyDetailOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性详情
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性详情
    /// </summary>
    public class PropertyDetailOutput
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用于描述读者
        /// </summary>
        public bool ForReader { get; set; }
        /// <summary>
        /// 用于描述卡
        /// </summary>
        public bool ForCard { get; set; }
        /// <summary>
        /// 属性标识
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 是否可检索
        /// </summary>
        public bool CanSearch { get; set; }
        /// <summary>
        /// 是否列表显示
        /// </summary>
        public bool ShowOnTable { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool Unique { get; set; }

    }
}
