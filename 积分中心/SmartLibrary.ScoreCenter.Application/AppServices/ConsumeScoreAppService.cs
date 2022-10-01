/*********************************************************
* 名    称：ConsumeScoreAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分消费接口，注意返回结果添加[NonUnify]标记，不对数据进行二次封装
* 更新历史：
*
* *******************************************************/
using Furion.ClayObject.Extensions;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.DtmClient.Dtm;
using SmartLibrary.DtmClient.Dtm.Tcc;
using SmartLibrary.ScoreCenter.Application.Dtos.Common;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.Common.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.AppServices
{
    /// <summary>
    /// 积分接口
    /// </summary>
    [Authorize(Policy = PolicyKey.ReaderAuth)]
    public class ConsumeScoreAppService : BaseAppService
    {
        private readonly IConsumeScoreService _consumeScoreService;
        private readonly BranchBarrier _branchBarrier;
        public ConsumeScoreAppService(IConsumeScoreService consumeScoreService,
            BranchBarrier branchBarrier)
        {
            _consumeScoreService = consumeScoreService;
            _branchBarrier = branchBarrier;
        }
        /// <summary>
        /// 锁定扣费积分
        /// </summary>
        /// <param name="dtmReq"></param>
        /// <param name="dataInput"></param>
        /// <returns></returns>
        [NonUnify]
        public async Task<object> Try([FromQuery] TccQueryRequest dtmReq, [FromBody] ConsumeScoreInput dataInput)
        {
            var dtmReqDict = dtmReq.ToDictionary();
            var db = SqlSugarHelper.GetTenantDb(dataInput.Tenant);
            _branchBarrier.InitConfig(dtmReqDict);
            await _branchBarrier.Call(db, async branchBarrier =>
              {
                  var result = await _consumeScoreService.ConsumeTry(db, dataInput);
                  if (!result)
                  {
                      throw Oops.Oh("积分不足");
                  }
              });
            return new { Result = "SUCCESS" };
        }

        /// <summary>
        /// 扣减锁定积分
        /// </summary>
        /// <param name="dtmReq"></param>
        /// <param name="dataInput"></param>
        /// <returns></returns>
        [NonUnify]
        public async Task<object> Confirm([FromQuery] TccQueryRequest dtmReq, [FromBody] ConsumeScoreInput dataInput)
        {
            var dtmReqDict = dtmReq.ToDictionary();
            var db = SqlSugarHelper.GetTenantDb(dataInput.Tenant);
            _branchBarrier.InitConfig(dtmReqDict);
            await _branchBarrier.Call(db, async branchBarrier =>
            {
                var result = await _consumeScoreService.ConsumeConfirm(db, dataInput);
                if (!result)
                {
                    throw Oops.Oh("Confirm:锁定积分不足");
                }
            });
            return new { Result = "SUCCESS" };
        }

        /// <summary>
        /// 退回锁定积分
        /// </summary>
        /// <param name="dtmReq"></param>
        /// <param name="dataInput"></param>
        /// <returns></returns>
        [NonUnify]
        public async Task<object> Cancel([FromQuery] TccQueryRequest dtmReq, [FromBody] ConsumeScoreInput dataInput)
        {
            var dtmReqDict = dtmReq.ToDictionary();
            var db = SqlSugarHelper.GetTenantDb(dataInput.Tenant);
            _branchBarrier.InitConfig(dtmReqDict);
            await _branchBarrier.Call(db, async branchBarrier =>
            {
                var result = await _consumeScoreService.ConsumeCancel(db, dataInput);
                if (!result)
                {
                    throw Oops.Oh("Cancel:锁定积分不足");
                }
            });
            return new { Result = "SUCCESS" };
        }

    }
}
