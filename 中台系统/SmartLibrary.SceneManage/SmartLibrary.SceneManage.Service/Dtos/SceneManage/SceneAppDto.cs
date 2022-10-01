/*********************************************************
 * 名    称：SceneAppViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/20 21:42:40
 * 描    述：场景内应用视图模型
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
    public class SceneAppDto
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 场景标识
        /// </summary>
        public string SceneId { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 父级场景应用标识（用于个人中心）
        /// </summary>
        public string ParentSceneAppId { get; set; }


        /// <summary>
        /// 应用组件(模板)
        /// </summary>
        public AppWidgetListViewModel AppWidget { get; set; }

        /// <summary>
        /// 应用模板
        /// </summary>
        public List<AppPlateItem> AppPlateItems { get; set; }


        /// <summary>
        /// 分屏Id
        /// </summary>
        public string SceneScreenId { get; set; }

        /// <summary>
        /// X轴位置
        /// </summary>
        public int XIndex { get; set; }

        /// <summary>
        /// Y轴位置
        /// </summary>
        public int YIndex { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
    }
}
