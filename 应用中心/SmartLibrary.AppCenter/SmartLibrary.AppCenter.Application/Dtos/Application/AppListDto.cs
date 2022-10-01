using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 应用检索结果
    /// </summary>
    public class AppListDto
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用改名之后的名称，如未改名，则等于原名称
        /// </summary>
        public string AppNewName { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 前台地址
        /// </summary>
        public string FrontUrl { get; set; }

        /// <summary>
        /// 后台地址
        /// </summary>
        public string BackendUrl { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public string ExpireDate { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 应用类型,逗号分隔
        /// </summary>
        public string AppType { get; set; }

        /// <summary>
        /// 采购类型 1正式，2试用
        /// </summary>
        public string PurchaseType { get; set; }

        /// <summary>
        /// 采购类型名称
        /// </summary>
        public string PurchaseTypeName { get; set; }

        /// <summary>
        /// 当前版本号
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public string ShowStatus { get; set; }

        /// <summary>
        /// 应用状态
        /// </summary>
        public string Status { get; set; }

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
        /// 应用介绍
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 应用说明
        /// </summary>
        public string AppExplain { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiHost { get; set; }

        /// <summary>
        /// 路由编码
        /// </summary>
        public string RouteCode { get; set; }

        /// <summary>
        /// 是否三方app
        /// </summary>
        public bool IsThirdApp { get; set; }

        /// <summary>
        /// 入口列表
        /// </summary>
        public List<AppEntrance> AppEntranceList { get; set; }

        /// <summary>
        /// 应用组件列表
        /// </summary>
        public List<AppWidget> AppWidgetList { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public List<AppAvailibleSortField> AppAvailibleSortFieldList { get; set; }
    }

    /// <summary>
    /// 应用菜单模型
    /// </summary>
    public class AppMenuListDto
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 前台地址
        /// </summary>
        public string FrontUrl { get; set; }

        /// <summary>
        /// 后台地址
        /// </summary>
        public string BackendUrl { get; set; }
    }

    /// <summary>
    /// 应用入口信息
    /// </summary>
    public class AppEntrance
    {
        /// <summary>
        /// 入口ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 入口名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 入口编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 使用场景 1=前台，2=后台
        /// </summary>
        public int UseScene { get; set; }

        /// <summary>
        /// 访问路径
        /// </summary>
        public string VisitUrl { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 是否内置入口
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 是否默认入口
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 应用事件
        /// </summary>
        public List<AppEvent> AppEventList { get; set; }
    }

    /// <summary>
    /// 应用事件
    /// </summary>
    public class AppEvent
    {
        /// <summary>
        /// 事件编码
        /// </summary>
        public string EventCode { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 事件类型，支持多选，逗号分隔
        /// 1 运行日志，2 操作日志 ，3 积分获取，4 积分消费，5 待办项
        /// </summary>
        public string EventType { get; set; }
    }

    /// <summary>
    /// 应用组件
    /// </summary>
    public class AppWidget
    {
        /// <summary>
        /// 应用组件ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组件内容地址
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 可用配置 1-栏目 2-数据条数 3-排序字段 逗号分隔
        /// </summary>
        public string AvailableConfig { get; set; }

        /// <summary>
        /// 最大数据条数
        /// </summary>
        public int MaxTopCount { get; set; }

        /// <summary>
        /// 数据条数间隔
        /// </summary>
        public int TopCountInterval { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 默认宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 默认高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 特殊场景标识 ，0-全部 1-通用 2-个人中心
        /// </summary>
        public int SceneType { get; set; }

        /// <summary>
        /// 组件标识
        /// </summary>
        public string WidgetCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime { get; set; }
    }

    /// <summary>
    /// 排序字段
    /// </summary>
    public class AppAvailibleSortField
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 排序字段名称
        /// </summary>
        public string SortFieldName { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string SortFieldValue { get; set; }
    }
}
