/*********************************************************
 * 名    称：SceneOverviewQuery
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/20 20:01:29
 * 描    述：场景总览请求参数
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.SceneManage.Common.AssemblyBase;

namespace SmartLibrary.SceneManage.Service.Dtos.SceneManage
{
    public class SceneListQuery: TableQueryBase
    {
        /// <summary>
        /// 终端标识 
        /// </summary>
        public string TerminalId { get; set; }

        /// <summary>
        /// 是否系统默认场景 0-否  1-是
        /// </summary>
        public int? IsSystemScene { get; set; }

        /// <summary>
        /// 启用状态。0-停用，1-启用
        /// </summary>
        public int? Status { get; set; }
    }
}
