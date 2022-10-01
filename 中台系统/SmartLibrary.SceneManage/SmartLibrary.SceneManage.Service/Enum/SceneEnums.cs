/*********************************************************
 * 名    称：SceneEnums
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/22 16:54:52
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.SceneManage.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Service.Enum
{
    /// <summary>
    /// 终端状态枚举
    /// </summary>
    public enum TerminalStatusEnum
    {
        [Enum("正常")]
        Normal = 0,

        [Enum("下线")]
        Disabled = 1
    }



    /// <summary>
    /// 场景状态枚举
    /// </summary>
    public enum SceneStatusEnum
    {
        [Enum("启用")]
        Normal = 0,

        [Enum("禁用")]
        Disabled = 1
    }



    /// <summary>
    /// 适用终端 1-PC端、2-APP端、3-小程序端、4-自适应移动端、5-显示屏
    /// 这里只支持3种
    /// </summary>
    public enum AppTerminalTypeEnum
    {
        [Enum("PC")]
        Pc = 1,

        [Enum("移动端")]
        H5 = 4,

        [Enum("显示屏")]
        OtherScreen = 5
    }


    /// <summary>
    /// 权限控制 0-禁用、1-必须登录认证、2-按学院、3-按用户类型、4-按用户分组
    /// </summary>
    public enum VisitorLimitTypeEnum
    {
        [Enum("禁用")]
        None = 0,

        [Enum("必须登录认证")]
        NeedLogin = 1,

        [Enum("按学院")]
        ByDepartment = 2,

        [Enum("按用户类型")]
        ByUserType = 3,

        [Enum("按用户分组")]
        ByUserGroup = 4
    }


    /// <summary>
    /// 适用终端 0-默认、1-添加时间倒序、2-访问量倒序
    /// </summary>
    public enum AppPlateSortTypeEnum
    {
        [Enum("默认")]
        Default = 0,

        [Enum("添加时间倒序")]
        CreateTime = 1,

        [Enum("访问量倒序")]
        VisitCount = 2
    }


    /// <summary>
    /// 布局类型
    /// </summary>
    public enum SceneLayoutTypeEnum
    {

        [Enum("通屏")]
        NeedLogin = 1,

        [Enum("分屏")]
        ByDepartment = 2,

        [Enum("通屏定宽")]
        ByUserType = 3,

        [Enum("分屏定宽")]
        ByUserGroup = 4
    }
}
