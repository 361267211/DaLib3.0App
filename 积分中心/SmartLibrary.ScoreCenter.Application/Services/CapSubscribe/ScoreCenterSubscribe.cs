/*********************************************************
* 名    称：UserServiceSubscribe.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20210831
* 描    述：事件消费服务,实现消费逻辑
* 更新历史：
*
* *******************************************************/
using DotNetCore.CAP;
using Furion;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using SmartLibrary.ScoreCenter.Application.Dtos.Cap;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Common.Utils;

namespace SmartLibrary.ScoreCenter.Application.Services.CapSubscribe
{
    //针对不同服务投递的消息实现不同的消息订阅，命名约定消息投递服务名称+Subscribe
    //此处示例代码为监听自己服务投递的消息所以是UserService+Subscribe
    //eg:OrderServiceSubscribe

    public class ScoreCenterSubscribe : ICapSubscribe, IScoped
    {
        //Arg1：订阅事件名称，Group为订阅组，同一组相同事件订阅只会消费一次，所以原则上需要定义为自己服务名称，达到自己服务消费一次的目的
        //如果不定义Group名称，在多个服务都需要监听同一事件的情况下，只会被其中一个服务消费
        [CapSubscribe(SubscribeEvent.ActionEvent)]
        public void OnRecieveActionEvent(SubscribeEvent.ActionEventMsg msg, [FromCap] CapHeader capHeader)
        {
            //检查当前数据库是否存在该租户，存在则继续处理，不存在在抛弃
            if (string.IsNullOrWhiteSpace(msg.TenantName))
            {
                return;
            }
            //pgSql的检查语句，如果后续需要扩展其他数据库，需要进行配置
            var schemaCheckSql = "SELECT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = @TenantName)";
            var db = SqlSugarHelper.GetTenantDb(msg.TenantName);
            var existSchema = (bool)db.Ado.GetScalar(schemaCheckSql, new { TenantName = msg.TenantName });
            if (!existSchema)
            {
                //如果不存在该租户数据，直接返回不处理
                db.Close();
                return;
            }
            var actionEventService = App.GetRequiredService<IReaderActionEventService>();
            var result = actionEventService.ProcessReaderActionEvent(msg, capHeader).Result;
            if (!result)
            {
                throw Oops.Oh("积分失败");
            }
        }


    }
}
