using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;

namespace SmartLibrary.FileServer.Application
{
    /// <summary>
    /// WebApi心跳检查
    /// </summary>
    public class HealthAppService : IDynamicApiController
    {
        /// <summary>
        /// WebApi心跳检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int Index()
        {
            return 1;
        }

    }
}
