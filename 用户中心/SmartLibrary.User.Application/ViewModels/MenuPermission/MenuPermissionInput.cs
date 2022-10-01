/*********************************************************
* 名    称：MenuPermissionInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：菜单权限输入
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 菜单权限输入
    /// </summary>
    public class MenuPermissionInput
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid ParentID { get; set; }
        /// <summary>
        /// 节点值
        /// </summary>
        public int Path { get; set; }
        /// <summary>
        /// 全路径
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 类型，1：目录 2：菜单 3：按钮 4：api地址
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Router { get; set; }
        /// <summary>
        /// 组件地址
        /// </summary>
        public string Component { get; set; }
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Permission { get; set; }
        /// <summary>
        /// 所属类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 打开方式 1：系统内部打开 2：外部链接
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
        public string Remark { get; set; }
        /// <summary>
        /// 是否系统管理菜单
        /// </summary>
        public bool IsSysMenu { get; set; }
        /// <summary>
        /// 行为类型
        /// </summary>
        public int ActionType { get; set; }
    }
}
