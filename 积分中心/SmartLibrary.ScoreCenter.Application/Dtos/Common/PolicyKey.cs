/*********************************************************
* 名    称：PolicyKey.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：授权校验策略key值
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.ScoreCenter.Application.Dtos.Common
{
    /// <summary>
    /// 缓存key
    /// </summary>
    public class PolicyKey
    {
        public const string TokenAuth = "TokenAuth";
        public const string StaffAuth = "StaffAuth";
        public const string ReaderAuth = "ReaderAuth";
        public const string PermitReaderAuth = "PermitReaderAuth";
        public const string UnAuthKey = "UnAuth";
    }
}
