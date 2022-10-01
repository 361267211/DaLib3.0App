/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Dtos
{
    /// <summary>
    /// 返回结果
    /// </summary>
    [Serializable]
    public class ApiResult<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string info { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public T result { get; set; }
        /// <summary>
        /// 匹配结果总数
        /// </summary>
        public int count { get; set; }
    }
    /// <summary>
    /// 结果封装类，所有对外提供的接口全部通过此类封装返回
    /// </summary>
    [Serializable]
    public class Result<T>
    {

        /// <summary>
        /// 结果状态
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; } 
        /// <summary>
        /// 处理消息
        /// 注意不要把异常信息写到这里!!!
        /// </summary>
        [JsonProperty(PropertyName = "info")]
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        //[JsonProperty(PropertyName = "result")]
        public T result { get; set; }
        /// <summary>
        /// 当前条件匹配到的数据的总数
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int Total { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public int TotalPage { get; set; }

        /// <summary>
        /// 操作日志对象
        /// </summary>
        [JsonProperty(PropertyName = "logInfo")]
        public OperateLogInfo LogInfo { get; set; }


        /// <summary>
        /// 操作日志所需信息
        /// </summary>
        public class OperateLogInfo
        {
            /// <summary>
            /// 操作类型，1添加，2编辑，3删除
            /// </summary>
            [JsonProperty(PropertyName = "type")]
            public int Type { get; set; }
            /// <summary>
            /// 操作对象状态
            /// </summary>
            [JsonProperty(PropertyName = "contentStatus")]
            public int ContentStatus { get; set; }
            /// <summary>
            /// MenuId
            /// </summary>
            [JsonProperty(PropertyName = "menuId")]
            public int MenuId { get; set; }
            /// <summary>
            /// 操作日志备注信息
            /// </summary>
            [JsonProperty(PropertyName = "remark")]
            public string Remark { get; set; }
            /// <summary>
            /// 操作对象父级ID
            /// </summary>
            [JsonProperty(PropertyName = "objectParentId")]
            public int ObjectParentId { get; set; }

            /// <summary>
            /// 操作对象ID，多个用逗号分隔
            /// </summary>
            [JsonProperty(PropertyName = "objectIds")]
            public string ObjectIds { get; set; }

            /// <summary>
            /// 操作对象名称，多个用逗号分隔
            /// </summary>
            [JsonProperty(PropertyName = "objectNames")]
            public string ObjectNames { get; set; }

            /// <summary>
            /// 指定操作者userkey，多个用逗号分隔
            /// </summary>
            [JsonProperty(PropertyName = "operatorIds")]
            public string OperatorIds { get; set; }
        }
    }
}
