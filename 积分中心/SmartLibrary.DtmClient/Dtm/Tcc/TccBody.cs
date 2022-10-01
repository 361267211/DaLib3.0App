/*********************************************************
* 名    称：TccBody.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Tcc请求消息体
* 更新历史：
*
* *******************************************************/
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SmartLibrary.DtmClient.Dtm.Tcc
{
    /// <summary>
    /// Tcc请求数据
    /// </summary>
    public class TccBody
    {
        /// <summary>
        /// 全局事务Id
        /// </summary>
        [JsonPropertyName("gid")]
        public string Gid { get; set; }
        /// <summary>
        /// 事务类型
        /// </summary>
        [JsonPropertyName("trans_type")]
        public string Trans_Type { get; set; } = "tcc";
        /// <summary>
        /// 是否需要等待结果立即返回
        /// </summary>
        [JsonPropertyName("wait_result")]
        public bool Wait_Result { get; set; } = false;
        /// <summary>
        /// 重试周期
        /// </summary>
        [JsonPropertyName("retry_interval")]
        public int Retry_Interval { get; set; } = 15;
        /// <summary>
        /// 请求超时时间
        /// </summary>
        [JsonPropertyName("timeout_to_fail")]
        public int Timeout_To_Fail { get; set; } = 60;
        /// <summary>
        /// 请求头部信息，事务服务器会在调用注册的Api时附加上
        /// </summary>
        [JsonPropertyName("branch_headers")]
        public Dictionary<string, string> Branch_Headers { get; set; }
    }

}
