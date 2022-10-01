/*********************************************************
* 名    称：PropertyGroupSelectDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：属性组可选项
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos.PropertyGroup
{
    /// <summary>
    /// 属性组可选项
    /// </summary>
    public class PropertyGroupSelectDto
    {
        public PropertyGroupSelectDto()
        {
            GroupItems = new List<DictItem<string, string>>();
        }
        /// <summary>
        /// 属性组编码
        /// </summary>
        public string GroupCode { get; set; }
        /// <summary>
        /// 可选项
        /// </summary>
        public List<DictItem<string, string>> GroupItems { get; set; }
    }
}
