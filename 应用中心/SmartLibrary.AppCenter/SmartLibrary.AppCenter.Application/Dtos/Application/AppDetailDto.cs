using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 应用详情
    /// </summary>
    public class AppDetailDto
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用新名称
        /// </summary>
        public string AppNewName { get; set; }

        /// <summary>
        /// 当前版本号
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// 用途类型
        /// </summary>
        public string AppType { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public string ShowStatus { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 是否是三方应用
        /// </summary>
        public bool IsThirdApp { get; set; }

        /// <summary>
        /// 场景类型
        /// </summary>
        public string SceneType { get; set; }

        /// <summary>
        /// 支持终端
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// 开发商
        /// </summary>
        public string Developer { get; set; }

        /// <summary>
        /// 建议售价
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 采购类型 1正式，2试用
        /// </summary>
        public string PurchaseType { get; set; }

        /// <summary>
        /// 显示采购类型 正式，试用
        /// </summary>
        public string ShowPurchaseType { get; set; }

        /// <summary>
        /// 到期日期
        /// </summary>
        public string ExpireDate { get; set; }

        /// <summary>
        /// 应用介绍
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 应用说明
        /// </summary>
        public string AppExplain { get; set; }

        /// <summary>
        /// 前台地址
        /// </summary>
        public string FrontUrl { get; set; }

        /// <summary>
        /// 管理地址
        /// </summary>
        public string BackendUrl { get; set; }

        /// <summary>
        /// 应用路由编号
        /// </summary>
        public string RouteCode { get; set; }

        /// <summary>
        /// 管理权限
        /// </summary>
        public List<AuthInfo> ManagerAuth { get; set; }

        /// <summary>
        /// 使用权限
        /// </summary>
        public List<AuthInfo> UseAuth { get; set; }
    }

    /// <summary>
    /// 授权信息
    /// </summary>
    public class AuthInfo
    {
        /// <summary>
        /// 授权类型 1=管理角色权限，2=使用读者群体权限
        /// </summary>
        public int AuthType { get; set; }

        /// <summary>
        /// 权限名称  角色名或读者群体
        /// </summary>
        public string AuthName { get; set; }
    }
}
