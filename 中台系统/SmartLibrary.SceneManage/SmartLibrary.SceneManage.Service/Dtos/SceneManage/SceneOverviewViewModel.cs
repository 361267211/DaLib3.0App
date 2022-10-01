/*********************************************************
 * 名    称：SceneOverviewViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/20 20:12:48
 * 描    述：场景总览视图模型
 *
 * 更新历史：
 *
 * *******************************************************/

using System.Collections.Generic;

namespace SmartLibrary.SceneManage.Service.Dtos.SceneManage
{
    public class SceneOverviewViewModel
    {
        /// <summary>
        /// 终端名称
        /// </summary>
        public string TerminalName { get; set; }

        /// <summary>
        /// 终端标识
        /// </summary>
        public string TerminalId { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        public int TerminalType { get; set; }

        /// <summary>
        /// 场景列表
        /// </summary>
        public IEnumerable<SceneListViewModel> SceneList { get; set; }
    }
}
