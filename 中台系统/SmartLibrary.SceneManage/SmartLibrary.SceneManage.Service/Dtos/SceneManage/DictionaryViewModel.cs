/*********************************************************
 * 名    称：DictionaryViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/22 15:51:02
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.SceneManage.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Service.Dtos.SceneManage
{
    public class DictionaryViewModel
    {
        /// <summary>
        /// 服务状态
        /// </summary>
        public List<SysDictModel<int>> SceneStatus { get; set; }

        /// <summary>
        /// 权限控制
        /// </summary>
        public List<SysDictModel<int>> VisitorLimitType { get; set; }

        /// <summary>
        /// 场景布局
        /// </summary>
        public List<SysDictModel<int>> SceneLayout { get; set; }

        /// <summary>
        /// 场景模板
        /// </summary>
        public List<SysDictModel<string>> SceneTemplate { get; set; }

        /// <summary>
        /// 场景主题色
        /// </summary>
        public List<SysDictModel<string>> SceneThemeColor { get; set; }

        /// <summary>
        /// 场景内App排序规则
        /// </summary>
        public List<SysDictModel<int>> AppPlateSortType { get; set; }

        /// <summary>
        /// 应用服务类型
        /// </summary>
        public List<SysDictModel<string>> AppServiceType { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        public List<SysDictModel<int>> AppTerminalType { get; set; }

        /// <summary>
        /// 终端状态
        /// </summary>
        public List<SysDictModel<int>> TerminalStatus { get; set; }

        /// <summary>
        /// 终端备选图表
        /// </summary>
        public List<SysDictModel<string>> TerminalIcon { get; set; }
    }
}
