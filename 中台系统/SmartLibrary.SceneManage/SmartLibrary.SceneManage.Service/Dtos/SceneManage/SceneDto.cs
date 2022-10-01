/*********************************************************
 * 名    称：SceneViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/20 21:31:22
 * 描    述：场景视图模型
 *
 * 更新历史：
 *
 * *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Service.Dtos.SceneManage
{
    public class SceneDto
    {
        /// <summary>
        /// 场景Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 场景名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 终端实例标识
        /// </summary>
        public string TerminalInstanceId { get; set; }

        /// <summary>
        /// 终端实例名称
        /// </summary>
        public string TerminalInstanceName { get; set; }

        /// <summary>
        /// 布局标识
        /// </summary>
        public int LayoutId { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public TemplateListViewModel Template { get; set; }

        /// <summary>
        /// 头部模板
        /// </summary>
        public TemplateListViewModel HeaderTemplate { get; set; }

        /// <summary>
        /// 底部模板
        /// </summary>
        public TemplateListViewModel FooterTemplate { get; set; }


        /// <summary>
        /// 主题颜色
        /// </summary>
        public string ThemeColor { get; set; }

        /// <summary>
        /// 场景封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 启用状态。0-启用，1-禁用
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// 权限控制类型。0-禁用，1-登录认证，2-按学院，3-按用户类型，4-按用户标签
        /// </summary>
        public int VisitorLimitType { get; set; }


        /// <summary>
        /// 访问地址
        /// </summary>
        public string VisitUrl { get; set; }


        /// <summary>
        /// 是否系统默认场景 0-否  1-是
        /// </summary>
        public bool IsSystemScene { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>

        public string UserKey { get; set; }

        /// <summary>
        /// 是否个人默认首页
        /// </summary>
        public bool IsPersonalIndex { get; set; }

        /// <summary>
        /// 场景分屏
        /// </summary>
        public IEnumerable<SceneScreenDto> SceneScreens { get; set; }


        /// <summary>
        /// 场景用户
        /// </summary>
        public IEnumerable<SceneUserDto> SceneUsers { get; set; }
    }
}
