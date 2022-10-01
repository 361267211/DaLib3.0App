/*********************************************************
* 名    称：数据库类型枚举.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：数据库类型枚举
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// 数据库类型枚举，同SqlSugar数据库类型枚举
    /// </summary>
    public enum EnumDbType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3,
        PostgreSQL = 4,
        Dm = 5,
        Kdbndp = 6,
        Oscar = 7
    }
}
