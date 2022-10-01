/*********************************************************
* 名    称：DtmExecuteResult.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Dtm执行结果
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// Dtm Try结果
    /// </summary>
    public class DtmExecuteResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}
