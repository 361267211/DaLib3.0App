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

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Dtos
{
    public class SysMenuPermissionDto
    {
        public SysMenuPermissionDto()
        {
            PermissionNodes = new List<SysMenuPermissionDto>();
        }
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// 节点路径
        /// </summary>

        public string Path { get; set; }

        /// <summary>
        /// 全路径
        /// </summary>

        public string FullPath { get; set; }

        /// <summary>
        /// 名称
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>

        public int Type { get; set; }

        /// <summary>
        /// 图标
        /// </summary>

        public string Icon { get; set; }

        /// <summary>
        /// 路由
        /// </summary>

        public string Router { get; set; }

        /// <summary>
        /// 组件
        /// </summary>

        public string Component { get; set; }

        /// <summary>
        /// 权限标识:接口的Action
        /// </summary>

        public string Permission { get; set; }

        ///// <summary>
        ///// 分组标识
        ///// </summary>

        //public string Category { get; set; }

        /// <summary>
        /// 打开方式
        /// </summary>

        public int OpenWay { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>

        public bool Visible { get; set; }

        /// <summary>
        /// 排序
        /// </summary>

        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>

        public string Remark { get; set; }

        /// <summary>
        /// 是否应用系统菜单
        /// </summary>

        public bool IsSysMenu { get; set; }
        /// <summary>
        /// 下级Permission
        /// </summary>
        public IEnumerable<SysMenuPermissionDto> PermissionNodes { get; set; }


    }
}
