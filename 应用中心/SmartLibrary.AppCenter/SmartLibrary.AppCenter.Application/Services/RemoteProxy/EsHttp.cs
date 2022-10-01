/*********************************************************
* 名    称：EsHttp.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20210831
* 描    述：http请求代理示例
* 更新历史：
*
* *******************************************************/
using Furion.RemoteRequest;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.RemoteProxy
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
