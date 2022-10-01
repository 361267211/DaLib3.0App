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

using Furion.RemoteRequest;
using SmartLibrary.Navigation.Application.ViewModel;
using SmartLibrary.Navigation.Common.Dtos;
using SmartLibrary.Navigation.EntityFramework.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services.RemoteProxy
{
    public interface IHttp : IHttpDispatchProxy
    {
        [Get("http://localhost:5555/api/v1.0/GetPlatList?plateSign={plateSign}&count={count}&itemType={itemType}")]
        Task<ApiResult<List<ContentInfoDto>>> GetPlatListAsync(string plateSign, int count, int itemType);

        [Get("http://localhost:5555/api/v1.0/GetAllianceCertifyUrl")]
        Task<ApiResult<string>> GetAllianceCertifyUrlAsync();
    }
}
