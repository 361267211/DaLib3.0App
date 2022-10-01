/*********************************************************
* 名    称：EsHttp.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210831
* 描    述：http请求代理示例
* 更新历史：
*
* *******************************************************/
using Furion.RemoteRequest;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Application.Services.RemoteProxy
{
    /// <summary>
    /// 请求代理，具体使用方式参考furion框架
    /// </summary>
    public interface EsHttp : IHttpDispatchProxy
    {
        /// <summary>
        /// 转发到Es查询服务，方法vipSearchByObjectV2
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [Post("vipSearchByObjectV2"), Client("Es")]
        Task<string> PostVipSearchByObjectV2Async([Body] dynamic condition);
    }
}
