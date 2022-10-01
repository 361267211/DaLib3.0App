/*********************************************************
* 名    称：AppGatewayResult.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：应用网关查询结果
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 应用网关查询
    /// </summary>
    public class AppGatewayResult
    {
        /// <summary>
        /// 是否在使用
        /// </summary>
        public bool InUse { get; set; }
        /// <summary>
        /// 网关地址
        /// </summary>
        public string Gateway { get; set; }
    }
}
