using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services.Enum
{
    public enum EnumLoginType
    {
        微信登录 = 1,
        QQ登录 = 2,
        短信验证登录 = 3,
        身份证登录 = 4,
        学校统一认证 = 5,
        读者证密码登录 = 6
    }

    public enum EnumRegisterType
    {
        手机验证码 = 1,
    }

    public enum EnumRegisterFlow
    {
        账号认证 = 1,
        馆员审核 = 2,
    }
}
