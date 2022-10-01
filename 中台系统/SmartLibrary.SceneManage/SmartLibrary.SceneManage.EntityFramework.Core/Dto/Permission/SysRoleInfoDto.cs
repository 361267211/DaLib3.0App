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

namespace SmartLibrary.SceneManage.EntityFramework.Core.Dtos
{
    public class SysRoleInfoDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>

        public string Code { get; set; }

        /// <summary>
        /// 备注
        /// </summary>

        public string Remark { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>

        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 权限id
        /// </summary>
        public List<SysRoleMenuDto> Permissions { get; set; }



    }
}
