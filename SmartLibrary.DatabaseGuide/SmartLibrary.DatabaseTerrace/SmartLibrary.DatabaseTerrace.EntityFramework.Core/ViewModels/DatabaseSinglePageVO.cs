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

using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.ViewModels
{
  public  class DatabaseSinglePageVO
    {
        public List<DatabaseTerraceDto> ChineseDatabases { get; set; }
        public List<DatabaseTerraceDto> ForeignDatabases { get; set; }
        public List<DatabaseTerraceDto> ProbationDatabase { get; set; }
        public List<DatabaseTerraceDto> SelfBuiltDatabases { get; set; }
        public List<DatabaseTerraceDto> OtherDatabases { get; set; }
    }
}
