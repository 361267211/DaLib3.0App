using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Service
{
    public static class SqlSugarExtensions
    {
        public static void InitTables(this ICodeFirst codeFirst, Type entityType, string sql)
        {
            var entityInfo = codeFirst.Context.EntityMaintenance.GetEntityInfo(entityType);
            var tableName = codeFirst.Context.EntityMaintenance.GetTableName(entityInfo.EntityName);
            var isAny = codeFirst.Context.DbMaintenance.IsAnyTable(tableName);
            if (!isAny)
            {
                codeFirst.Context.Ado.ExecuteCommand(sql);
            }
        }
    }
}
