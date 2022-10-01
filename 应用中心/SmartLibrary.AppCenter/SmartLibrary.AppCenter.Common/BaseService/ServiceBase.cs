using Furion;
using Furion.FriendlyException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Common.BaseService
{
    /// <summary>
    /// service基类
    /// </summary>
    public class ServiceBase
    {
        /// <summary>
        /// 当前登录机构
        /// </summary>
        public static string Owner
        {
            get
            {
                return App.User?.FindFirstValue("OrgCode");
            }
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static string UserKey
        {
            get
            {
                return App.User?.FindFirstValue("UserKey");
            }
        }

        /// <summary>
        /// 检测是否登录
        /// </summary>
        public static void CheckIsLogin()
        {
            if (string.IsNullOrWhiteSpace(UserKey))
            {
                App.HttpContext.Response.Headers.Add("UnAuth", "1");
                App.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                throw Oops.Oh("请登录").StatusCode(403);
            }
        }
    }
}
