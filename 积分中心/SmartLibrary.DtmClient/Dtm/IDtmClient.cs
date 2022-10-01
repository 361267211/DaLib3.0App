/*********************************************************
* 名    称：IDtmClient.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：DtmClient服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.DtmClient.Dtm.Tcc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// DtmClient服务
    /// </summary>
    public interface IDtmClient:IDisposable
    {
        Task<bool> RegisterTccBranch(RegisterTccBranch registerTcc, CancellationToken cancellationToken);

        Task<bool> TccPrepare(TccBody tccBody, CancellationToken cancellationToken);


        Task<bool> TccSubmit(TccBody tccBody, CancellationToken cancellationToken);


        Task<bool> TccAbort(TccBody tccBody, CancellationToken cancellationToken);


        Task<string> GenGid(CancellationToken cancellationToken);
    }
}
