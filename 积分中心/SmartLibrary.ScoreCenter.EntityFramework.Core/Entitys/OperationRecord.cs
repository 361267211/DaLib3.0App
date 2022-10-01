using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class OperationRecord : BaseEntity<Guid>
    {
        /// <summary>
        /// 操作标识
        /// </summary>
        [StringLength(200)]
        public string OperateKey { get; set; }
    }
}
