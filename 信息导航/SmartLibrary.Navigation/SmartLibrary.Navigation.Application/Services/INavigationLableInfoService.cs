using SmartLibrary.Navigation.Application.PageParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    /// <summary>
    /// 名    称：INavigationLableInfoService
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 16:32:57
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public interface INavigationLableInfoService
    {
        Task<List<LableUpdateParm>> GetLableList();
        Task<ApiResultInfoModel> SaveLableInfo(List<LableUpdateParm> updateParmList);
        Task<ApiResultInfoModel> DeleteLableInfo(string lableID);
        Task<string> ProcessLablesFromLableStr(string labels);
    }
}
