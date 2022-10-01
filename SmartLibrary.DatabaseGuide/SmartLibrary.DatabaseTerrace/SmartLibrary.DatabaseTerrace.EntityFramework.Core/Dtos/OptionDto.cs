/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos
{
    public class OptionDto
    {
        /// <summary>
        /// 键名
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 键值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 子级 无层级关系的键值项目勿用
        /// </summary>
        public List<OptionDto> Child { get; set; }
    }
}
