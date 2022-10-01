/*********************************************************
* 名    称：EnumProperty.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性相关枚举
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Services.Enum
{
    public enum EnumLogDiffType
    {
        新增 = 0,
        修改 = 1,
        删除 = 2
    }
    public enum EnumPropertyType
    {
        文本 = 0,
        数值 = 1,
        时间 = 2,
        是非 = 3,
        属性组 = 4,
        地址 = 5,
        附件 = 6
    }

    public enum EnumPropertyDataStatus
    {
        未激活 = 0,
        正常 = 1,
    }

    public enum EnumPropertyApproveStatus
    {
        待审批 = 0,
        正常 = 1,
    }

    public enum EnumPropertyLogStatus
    {
        驳回 = -1,
        待审批 = 0,
        通过 = 1,
        无需审批 = 2,
    }

    public enum EnumPropertyLogType
    {
        新增 = 1,
        修改 = 2,
        删除 = 3,
        批量修改 = 4,
    }

    public enum EnumLogPropertyType
    {
        属性 = 0,
        属性组 = 1,
    }

}
