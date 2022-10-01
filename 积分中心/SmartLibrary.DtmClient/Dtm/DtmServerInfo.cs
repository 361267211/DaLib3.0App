/*********************************************************
* 名    称：DtmServerInfo.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Dtm服务端请求api地址
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// Dtm服务端请求api地址
    /// </summary>
    public class DtmServerInfo
    {
        /// <summary>
        /// 分支注册
        /// </summary>
        public const string Register_Tcc_Branch = "/api/dtmsvr/registerTccBranch";
        /// <summary>
        /// 设置就绪状态
        /// </summary>
        public const string Prepare = "/api/dtmsvr/prepare";
        /// <summary>
        /// 提交
        /// </summary>
        public const string Submit = "/api/dtmsvr/submit";
        /// <summary>
        /// 撤销
        /// </summary>
        public const string Abort = "/api/dtmsvr/abort";
        /// <summary>
        /// 获取全局事务Id
        /// </summary>
        public const string New_Gid = "/api/dtmsvr/newGid";
    }
}
