/*********************************************************
* 名    称：UserAuthAppListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户授权App查询
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户授权App查询
    /// </summary>
    public class UserAuthAppListItemOutput
    {
        /// <summary>
        /// 类型，前台/后台
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
    }
}
