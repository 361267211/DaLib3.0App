/*********************************************************
 * 名    称：TerminalViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/20 22:11:17
 * 描    述：
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
    public class TerminalInstanceDto
    {
        /// <summary>
        /// 终端实例标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 终端实例名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 终端类型 1-PC端、2-APP端、3-小程序端、4-自适应移动端、5-显示屏
        /// </summary>
        public int TerminalType { get; set; }

        /// <summary>
        /// 关键词，多个逗号分隔
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Logo的Url
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 图标的Url
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 访问路径
        /// </summary>
        public string VisitUrl { get; set; }

        /// <summary>
        /// 状态  0-下线  1-正常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否系统默认实例 0-否  1-是
        /// </summary>
        public bool IsSystemInstance { get; set; }

        /// <summary>
        /// 场景数量
        /// </summary>
        public int SceneCount { get; set; }
    }
}
