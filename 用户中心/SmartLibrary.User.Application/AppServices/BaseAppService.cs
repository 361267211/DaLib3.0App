/*********************************************************
* 名    称：AppServices.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：AppService基类，用于提供基础数据获取功能
* 更新历史：
*
* *******************************************************/
using Furion;
using Furion.DynamicApiController;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Services.Interface;
using System;

namespace SmartLibrary.User.Application.AppServices
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
