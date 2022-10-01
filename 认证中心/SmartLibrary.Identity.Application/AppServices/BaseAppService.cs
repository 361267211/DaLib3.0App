using Furion;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartLibrary.Identity.Application.Dtos;
using SmartLibrary.Identity.Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.AppServices
{
    [Route("api/[controller]")]
    public class BaseAppService : IDynamicApiController
    {
        private readonly ILogger<BaseAppService> _logger;
        protected readonly IUserPermissionService _userPermissionService;
        /// <summary>
        /// 初始化
        /// </summary>
        public BaseAppService()
        {
            _userPermissionService = App.GetService<IUserPermissionService>();
            _logger = App.GetService<ILogger<BaseAppService>>();
        }

        /// <summary>
        /// 当前登录人信息
        /// </summary>
        public AppUserInfo CurrentUser
        {
            get
            {
                try
                {
                    var userKey = App.User.FindFirst("UserKey") != null ? App.User.FindFirst("UserKey").Value : "";
                    if (string.IsNullOrWhiteSpace(userKey))
                    {
                        return null;
                    }
                    var userInfo = _userPermissionService.GetUserInfo(userKey).Result;
                    return userInfo;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"获取用户信息失败:{ex.Message}");
                    throw Oops.Oh("获取用户信息失败");
                }

            }
        }

        /// <summary>
        /// 当前登录人权限
        /// </summary>
        public AppUserPermission CurUserPermission
        {
            get
            {
                try
                {
                    var userKey = App.User.FindFirst("UserKey") != null ? App.User.FindFirst("UserKey").Value : "";
                    if (string.IsNullOrWhiteSpace(userKey))
                    {
                        return null;
                    }
                    var userInfo = _userPermissionService.GetUserPermission(userKey).Result;
                    return userInfo;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"获取用户信息失败:{ex.Message}");
                    throw Oops.Oh("获取用户权限失败");
                }
            }
        }
    }
}
