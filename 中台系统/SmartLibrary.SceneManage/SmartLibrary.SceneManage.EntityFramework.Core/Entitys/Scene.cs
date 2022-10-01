/*********************************************************
 * 名    称：Scene
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 15:47:05
 * 描    述：场景
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DatabaseAccessor;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.SceneManage.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 场景
    /// </summary>
    public class Scene:Entity<Guid>
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        [StringLength(50), Required]
        public string Name { get; set; }

        /// <summary>
        /// 终端实例标识
        /// </summary>
        [StringLength(48), Required]
        public string TerminalInstanceId { get; set; }

        /// <summary>
        /// 布局标识
        /// </summary>
        [StringLength(48), Required]
        public string LayoutId { get; set; }

        /// <summary>
        /// 模板标识
        /// </summary>
        [StringLength(48), Required]
        public string TemplateId { get; set; }

        /// <summary>
        /// 头部模板标识
        /// </summary>
        [StringLength(48)]
        public string HeaderTemplateId { get; set; }

        /// <summary>
        /// 底部模板标识
        /// </summary>
        [StringLength(48)]
        public string FooterTemplateId { get; set; }

        /// <summary>
        /// 主题颜色
        /// </summary>
        [StringLength(48), Required]
        public string ThemeColor { get; set; }

        /// <summary>
        /// 场景封面
        /// </summary>
        [StringLength(200)]
        public string Cover { get; set; }

        /// <summary>
        /// 启用状态。0-启用，1-禁用
        /// </summary>
        [Required]
        public int Status { get; set; }


        /// <summary>
        /// 权限控制类型。0-禁用，1-登录认证，2-按学院，3-按用户类型，4-按用户标签
        /// </summary>
        [Required]
        public int VisitorLimitType { get; set; }


        /// <summary>
        /// 访问地址
        /// </summary>
        [StringLength(200), Required]
        public string VisitUrl { get; set; }


        /// <summary>
        /// 是否系统默认场景 0-否  1-是
        /// </summary>
        [Required]
        public bool IsSystemScene { get; set; }

        /// <summary>
        /// 场景类型。0-普通，1-门户首页，2-个人中心
        /// </summary>
        public int SceneType { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>

        [StringLength(50)]
        public string UserKey { get; set; }

        /// <summary>
        /// 是否个人默认首页
        /// </summary>
        [Required]
        public bool IsPersonalIndex { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Required]
        public bool DeleteFlag { get; set; }
    }
}
