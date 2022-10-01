using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 读者可编辑属性配置
    /// </summary>
    public class ReaderEditProperty : BaseEntity<Guid>
    {
        public string PropertyCode { get; set; }
        public bool IsEnable { get; set; }
        public bool IsCheck { get; set; }
    }
}
