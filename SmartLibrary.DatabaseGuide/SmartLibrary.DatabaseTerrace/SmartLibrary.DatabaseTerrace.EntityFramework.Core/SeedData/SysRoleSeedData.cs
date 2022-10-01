/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.DbContexts;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.SeedData
{
    /// <summary>
    /// 系统角色表种子数据
    /// </summary>
    public class SysRoleSeedData : IEntitySeedData<SysRole>
    {
        /// <summary>
        /// 种子数据
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        /// <returns></returns>
        public IEnumerable<SysRole> HasData(DbContext dbContext, Type dbContextLocator)
        {
            return new[]
            {
                new SysRole{ Name="管理员",Remark="管理员",Id=Guid.NewGuid(),Code="sys_manager_role" },
                new SysRole{ Name="操作员",Remark="操作员",Id=Guid.NewGuid(),Code="sys_operator_role" },
                new SysRole{ Name="使用者",Remark="使用者",Id=Guid.NewGuid(),Code="sys_user_role" },
            };
        }

    }
}
