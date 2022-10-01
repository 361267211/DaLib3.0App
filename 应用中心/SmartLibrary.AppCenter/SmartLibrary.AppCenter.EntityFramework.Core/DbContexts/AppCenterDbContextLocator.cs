/*********************************************************
 * 名    称：AssetDbContextLocator
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：数据库上下文选择器。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;

namespace SmartLibrary.AppCenter.EntityFramework.Core
{
    /// <summary>
    /// DbContext上下文选择器，仅有一个DbContext时，可以省略
    /// </summary>
    public sealed class AppCenterDbContextLocator : IDbContextLocator
    {
    }
}
