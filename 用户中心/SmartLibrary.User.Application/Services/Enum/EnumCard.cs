/*********************************************************
* 名    称：EnumCard.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡枚举定义
* 更新历史：
*
* *******************************************************/


namespace SmartLibrary.User.Application.Services.Enum
{
    public enum EnumCardStatus
    {
        正常 = 1,
        挂失 = 2,
        停用 = 3,
    }

    public enum EnumCardUsage
    {
        无特殊用途 = 0,
        临时馆员登陆 = 1
    }

    public enum EnumCardClaimStatus
    {
        取消 = -2,
        驳回 = -1,
        待审批 = 0,
        通过 = 1,
    }

    public enum EnumCardClaimWay
    {
        提交审核 = 0,
        直接领卡 = 1,
    }

    public enum EnumSyncCardStatus
    {
        运行 = 0,
        暂停 = 1,
        不触发 = 2,
    }

    public enum EnumSyncCardLogStatus
    {
        失败 = -1,
        执行中 = 0,
        成功 = 1,
        部分成功 = 2
    }

    public enum EnumSyncCardType
    {
        增量同步 = 1,
        全量同步 = 2,
    }
}
