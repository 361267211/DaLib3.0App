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
    public class TemplateListQuery: TableQueryBase
    {
        /// <summary>
        /// 布局标识 
        /// </summary>
        public int? LayoutId { get; set; }

        /// <summary>
        /// 模板类型 1-场景 2-头部 3-底部
        /// </summary>
        public int? Type { get; set; }

    }
}
