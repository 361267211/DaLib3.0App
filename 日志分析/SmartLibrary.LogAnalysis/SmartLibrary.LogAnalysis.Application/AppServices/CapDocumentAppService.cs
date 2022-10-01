/*********************************************************
* 名    称：CapDocumentAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20210831
* 描    述：当前应用Cap发布事件描述信息获取服务
* 更新历史：
*
* *******************************************************/
using Furion.DynamicApiController;
using SmartLibrary.Core.Cap;
using System.Collections.Generic;

namespace SmartLibrary.LogAnalysis.Application.AppServices
{
    /// <summary>
    /// Cap文档描述
    /// </summary>
    public class CapDocumentAppService : IDynamicApiController
    {

        /// <summary>
        /// 获取当前服务发布事件描述文档数据
        /// </summary>
        /// <returns></returns>
        public List<SmartCapEventIntroduction> GetCapPublishEventDoc()
        {
            return SmartCapExtensions.CollectCapPublishEventIntroduction();
        }
    }
}
