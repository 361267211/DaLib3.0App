/*********************************************************
* 名    称：EnumPropertyGroupItem.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性组相关枚举
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Services.Enum
{

    public enum EnumGroupItemDataStatus
    {
        未激活 = 0,
        正常 = 1
    }
    public enum EnumGroupItemApproveStatus
    {
        待审批 = 0,
        正常 = 1
    }

    public enum EnumGroupType
    {
        内置 = 0,
        扩展 = 1
    }

    public enum EnumGroupLogStatus
    {
        驳回 = -1,
        待审批 = 0,
        通过 = 1,
        无需审批 = 2,
    }

    public enum EnumGroupLogType
    {
        新增 = 1,
        修改 = 2,
        删除 = 3,
    }
}
