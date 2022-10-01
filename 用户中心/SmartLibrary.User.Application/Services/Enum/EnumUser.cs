/*********************************************************
* 名    称：EnumUser.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户相关枚举
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Services.Enum
{
    public enum EnumUserApproveStatus
    {
        待审批 = 0,
        正常 = 1,
    }

    public enum EnumUserSourceFrom
    {
        后台新增 = 0,
        用户注册 = 1,
        后台导入 = 2,
        数据同步 = 3
    }

    public enum EnumUserStatus
    {
        未激活 = 0,
        正常 = 1,
        禁用 = 2,
        注销 = 3,
    }

    public enum EnumStaffStatus
    {
        正式 = 1,
        临时 = 0,
    }

    public enum EnumUserLogType
    {
        新增 = 1,
        修改 = 2,
        删除 = 3,
        批量修改 = 4,
    }

    public enum EnumUserLogStatus
    {
        驳回 = -1,
        待审批 = 0,
        通过 = 1,
        无需审批 = 2,
    }

    public enum EnumUserPropertyType
    {
        字段 = 0,
        属性 = 1
    }


    public enum EnumUserAuthAppType
    {
        应用授权 = 0,
        馆员授权 = 1,
    }

    public enum EnumUserRegisterStatus
    {
        驳回 = -1,
        待审批 = 0,
        通过 = 1,
    }
}
