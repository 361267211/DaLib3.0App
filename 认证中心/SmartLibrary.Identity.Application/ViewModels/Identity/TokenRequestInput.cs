using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.ViewModels
{
    public class TokenRequestInput
    {
        public string OrgId { get; set; }
        public string OrgSecret { get; set; }
        public string OrgCode { get; set; }
        public string UserKey { get; set; }
    }
}
