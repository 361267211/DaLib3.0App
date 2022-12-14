/*********************************************************
* 名    称：Id生成扩展.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：2021
* 描    述：******
* 更新历史：
*
* *******************************************************/
using Furion.DistributedIDGenerator;
using System;

namespace SmartLibrary.ScoreCenter.Application.Services.Extensions
{
    /// <summary>
    /// Id生成扩展
    /// </summary>
    public static class IDGeneratorExtensions
    {
        public static Guid CreateGuid(this IDistributedIDGenerator idGenerator, Guid? fixedGuid = null)
        {
            if (fixedGuid == null || fixedGuid == Guid.Empty)
            {
                return new Guid(idGenerator.Create().ToString());
            }
            return fixedGuid.Value;
        }
    }
}
