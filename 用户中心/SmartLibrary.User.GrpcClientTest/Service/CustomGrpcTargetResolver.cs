/*********************************************************
* 名    称：CustomGrpcTargetResolver.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20210827
* 描    述：获取Grpc服务端地址，通常为服务端网关地址，这里只是写了一个样例，需要自己实现替换
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Core.GrpcClientHelper;
using System;

namespace SmartLibrary.User.GrpcClientTest.Service
{
    public class CustomGrpcTargetResolver : IGrpcTargetAddressResolver
    {
        public string GetGrpcTargetAddress(string orgCode)
        {
            //返回测试地址fabio网关，实际应该查询开放平台获取对应地址
            return "http://192.168.21.71:9999";
        }
    }
}
