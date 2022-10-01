/*********************************************************
 * 名    称：AppRouteService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/15 15:53:56
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.Services.Dtos.AppRoute;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.AppManagement
{
    public interface IAppRouteService
    {
        Task<List<AppRouteViewModel>> GetAppRouteList(AppRouteQuery queryFilter);
    }
}