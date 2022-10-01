/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.DatabaseTerrace.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Application.Services
{
    public class CustomGrpcTargetResolver : IGrpcTargetAddressResolver
    {
        public string GetGrpcTargetAddress(string orgCode)
        {
            //返回测试地址fabio网关，实际应该查询开放平台获取对应地址
            return $"http://{SiteGlobalConfig.GrpcRegist.CloudUrl}:{ SiteGlobalConfig.GrpcRegist.CloudPort}";
        }
    }
}
