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
using Furion.DependencyInjection;
using SmartLibrary.Core.Cap;
using SmartLibrary.SceneManage.Application.Dtos.Cap;

namespace SmartLibrary.SceneManage.Application.Services.CapSubscribe
{
    //针对不同服务投递的消息实现不同的消息订阅，命名约定消息投递服务名称+Subscribe
    //此处示例代码为监听自己服务投递的消息所以是UserService+Subscribe
    //eg:OrderServiceSubscribe

    public class UserServiceSubscribe : ICapSubscribe, IScoped
    {
        private ICapPublisher _capPublisher;
        public UserServiceSubscribe(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        //Arg1：订阅事件名称，Group为订阅组，同一组相同事件订阅只会消费一次，所以原则上需要定义为自己服务名称，达到自己服务消费一次的目的
        //如果不定义Group名称，在多个服务都需要监听同一事件的情况下，只会被其中一个服务消费
        [SmartCapSubscribe(UserServiceSubscribeEvent.NewPerson, Group = "UserService")]
        public void OnNewPerson(UserServiceSubscribeEvent.NewPersonMsg msg)
        {
            //由于相同服务会在云端和本地同时部署，多租户环境下可能出现本地部署服务监听到消息但不能处理其他租户数据的情况
            //所以所有消息需要投递为公有云事件，然后消费者监听到后首先检查是否能处理（是否能访问租户数据）
            //如果不能处理，则将消息事件重新投递为本地部署的特定租户事件，由本地租户服务监听处理
            var tenantName = msg.TenantName;
            var tenantExist = false;
            if (tenantExist)
            {
                var transferTopic = UserServicePublishEvent.NewPerson.ReplaceTenant(tenantName);
                _capPublisher.Publish(transferTopic, msg);
            }
            else
            {
                //添加自己的实现逻辑
                var pId = msg.PersonId;
            }
        }
    }
}
