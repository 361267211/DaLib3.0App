/*********************************************************
* 名    称：AuthHandler.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：授权实现
* 更新历史：
*
* *******************************************************/
using Microsoft.AspNetCore.Authorization;

namespace SmartLibrary.User.Application.AuthHandler
{
    public class TokenAuthRequirement : IAuthorizationRequirement
    {
    }
    public class StaffAuthRequirement : IAuthorizationRequirement
    {
    }
    public class ReaderAuthRequirement : IAuthorizationRequirement
    {
    }

    public class PermitReaderAuthRequirement : IAuthorizationRequirement
    {
    }
}
