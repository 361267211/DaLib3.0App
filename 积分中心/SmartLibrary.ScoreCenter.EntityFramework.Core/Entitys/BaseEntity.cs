using Furion.DatabaseAccessor;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class BaseEntity<T> : IPrivateEntity
    {
        public BaseEntity()
        {
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
        }
        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID")]
        [SugarColumn(ColumnName = "ID")]
        public T Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        [StringLength(100)]
        public string TenantId { get; set; }

    }
}
