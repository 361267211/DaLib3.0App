using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Reader
{
    public class ReaderValidateInfo
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string UserKey { get; set; }
        public string StudentNo { get; set; }
        public string Phone { get; set; }
        public string IdCard { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CardValidateInfo
    {
        public Guid UserID { get; set; }
        public string No { get; set; }
        public string BarCode { get; set; }
        public string PhysicNo { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
    }


}
