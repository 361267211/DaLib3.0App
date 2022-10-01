/*********************************************************
* 名    称：CacheKey.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：用户中心缓存Key值
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Dtos.Common
{
    /// <summary>
    /// 缓存key前缀
    /// </summary>
    public class CacheKey
    {
        public const string UserBaseInfo = "UserBaseInfo_";
        public const string UserPermisInfo = "UserPermisInfo_";
    }
}
