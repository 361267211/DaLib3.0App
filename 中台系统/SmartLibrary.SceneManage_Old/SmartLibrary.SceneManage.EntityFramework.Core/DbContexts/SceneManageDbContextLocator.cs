/*********************************************************
 * 名    称：SceneManageDbContextLocator
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 15:47:05
 * 描    述：场景
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;

namespace SmartLibrary.SceneManage.EntityFramework.Core
{
    /// <summary>
    /// DbContext上下文选择器，仅有一个DbContext时，可以省略
    /// </summary>
    public sealed class SceneManageDbContextLocator : IDbContextLocator
    {
    }
}
