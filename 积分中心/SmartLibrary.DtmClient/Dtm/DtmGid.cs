/*********************************************************
* 名    称：DtmGid.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：全局事务Id
* 更新历史：
*
* *******************************************************/
using System.Text.Json.Serialization;

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// Dtm Global Transaction Id
    /// </summary>
    public class DtmGid
    {
        [JsonPropertyName("gid")]
        public string Gid { get; set; }

        [JsonPropertyName("dtm_result")]
        public string Dtm_Result { get; set; }
    }
}
