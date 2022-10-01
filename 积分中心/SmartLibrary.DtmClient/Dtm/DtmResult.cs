/*********************************************************
* 名    称：DtmResult.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Dtm请求结果
* 更新历史：
*
* *******************************************************/
using System.Text.Json.Serialization;

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// Dtm请求结果
    /// </summary>
    public class DtmResult
    {
        [JsonPropertyName("dtm_result")]
        public string Dtm_Result { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        public bool Success
        {
            get
            {
                return Dtm_Result.ToUpper() == "SUCCESS";
            }
        }
    }
}
