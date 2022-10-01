using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.News.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Services
{
    /// <summary>
    /// 名    称：CustomGrpcTargetResolver
    /// 作    者：张泽军
    /// 创建时间：2021/10/19 10:53:26
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class CustomGrpcTargetResolver : IGrpcTargetAddressResolver
    {
        public string GetGrpcTargetAddress(string orgCode)
        {
            //返回测试地址fabio网关，实际应该查询开放平台获取对应地址
            return $"http://{SiteGlobalConfig.GrpcRegist.CloudUrl}:{ SiteGlobalConfig.GrpcRegist.CloudPort}";
        }
    }
}
