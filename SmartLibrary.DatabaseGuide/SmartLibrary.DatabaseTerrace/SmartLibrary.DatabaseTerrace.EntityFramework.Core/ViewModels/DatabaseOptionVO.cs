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

using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.ViewModels
{
    public class DatabaseOptionVO
    {
        /// <summary>
        /// 中心站所有的数据库
        /// </summary>
        public List<OptionDto> AllDatabase { get; set; }
        /// <summary>
        /// 选中了的数据库
        /// </summary>
        public List<OptionDto> SelectedDatabase { get; set; }
    }
}
