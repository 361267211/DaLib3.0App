using Furion;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SmartLibrary.AppCenter.Common.BaseService
{
    /// <summary>
    /// API基类
    /// </summary>
    [Route("api/[controller]")]
    public class BaseAppService : IDynamicApiController
    {
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
    }
}
