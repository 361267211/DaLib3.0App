/*********************************************************
 * 名    称：ScenenListViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/20 21:26:13
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
    public class SceneListViewModel
    {
        /// <summary>
        /// 场景标识 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 场景名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 场景封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 启用状态。0-停用，1-启用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否系统默认场景 0-否  1-是
        /// </summary>
        public bool IsSystemScene { get; set; }

        /// <summary>
        /// 预览地址
        /// </summary>
        public string VisitUrl { get; set; }
    }
}
