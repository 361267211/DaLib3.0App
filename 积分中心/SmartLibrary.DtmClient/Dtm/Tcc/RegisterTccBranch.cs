/*********************************************************
* 名    称：RegisterTccBranch.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：用于注册Tcc事务分支
* 更新历史：
*
* *******************************************************/
using System.Text.Json.Serialization;

namespace SmartLibrary.DtmClient.Dtm.Tcc
{
    /// <summary>
    /// 注册Tcc请求分支
    /// </summary>
    public class RegisterTccBranch
    {
        /// <summary>
        /// 全局事务Id
        /// </summary>
        [JsonPropertyName("gid")]
        public string Gid { get; set; }
        /// <summary>
        /// 分支Id
        /// </summary>
        [JsonPropertyName("branch_id")]
        public string Branch_id { get; set; }
        /// <summary>
        /// 事务类型
        /// </summary>
        [JsonPropertyName("trans_type")]
        public string Trans_type { get; set; } = "tcc";
        /// <summary>
        /// 状态
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = "prepared";
        /// <summary>
        /// 附加数据
        /// </summary>
        [JsonPropertyName("data")]
        public string Data { get; set; }
        /// <summary>
        /// 尝试Api请求地址
        /// </summary>
        [JsonPropertyName("try")]
        public string Try { get; set; }
        /// <summary>
        /// 确认Api请求地址
        /// </summary>
        [JsonPropertyName("confirm")]
        public string Confirm { get; set; }
        /// <summary>
        /// 撤销Api请求地址
        /// </summary>
        [JsonPropertyName("cancel")]
        public string Cancel { get; set; }
    }
}
