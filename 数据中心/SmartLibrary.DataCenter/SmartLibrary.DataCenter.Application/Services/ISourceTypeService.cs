using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SmartLibrary.DataCenter.EntityFramework.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application.Services
{
    public interface ISourceTypeService
    {
        /// <summary>
        /// 获取资源类型字典
        /// </summary>
        /// <returns></returns>
        Task<List<SourceTypeDto>> GetSourceTypes();
        /// <summary>
        /// 添加资源类型字典
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        Task<SourceTypeDto> AddSourceType(SourceTypeDto sourceType);
    }
}
