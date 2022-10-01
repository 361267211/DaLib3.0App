/*********************************************************
 * 名    称：AppRouteViewModel
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/15 17:40:18
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using System.Collections.Generic;

namespace SmartLibrary.Open.Services.Dtos.AppRoute
{


    public class AppRouteViewModel
    {
        /// <summary>
        /// 租户标识
        /// </summary>
        public string TenantCode { get; set; }

        /// <summary>
        /// 应用路由
        /// </summary>
        public IEnumerable<AppRouter> AppRouters {get;set;}
    }

    public class AppRouter
    {
        /// <summary>
        /// 应用路由
        /// </summary>
        public string AppRouteCode { get; set; }

        /// <summary>
        /// RestApi的Consul服务标识
        /// </summary>
        public string RestApiServerName { get; set; }

        /// <summary>
        /// GrpcApi的Consul服务标识
        /// </summary>
        public string GrpcApiServerName { get; set; }
    }
}
