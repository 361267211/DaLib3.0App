/*********************************************************
 * 名    称：BaseAppService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/2 14:15:27
 * 描    述：聚合服务基类
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;

namespace SmartLibrary.SceneManage.Common.AssemblyBase
{
    [Route("api/[controller]")]
    public class BaseAppService : IDynamicApiController
    {
    }
}
