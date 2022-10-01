/*********************************************************
* 名    称：PropertyGroupListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性组列表输出
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性组列表输出
    /// </summary>
    public class PropertyGroupListItemOutput
    {
        /// <summary>
        /// 属性组ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 属性组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性组编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 分组数量
        /// </summary>
        public int Count { get; set; }

    }
}
