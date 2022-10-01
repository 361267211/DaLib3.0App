using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    public class PropertyChangeItem : BaseEntity<Guid>
    {

        public Guid LogID { get; set; }
        public Guid PropertyID { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string FieldCode { get; set; }
    }
}
