using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Dtos
{
    public class AppUserInfo
    {
        public string UserKey { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public string UserIdCard { get; set; }
        public int Status { get; set; }
        public bool IsStaff { get; set; }
    }

    public class AppUserPermission
    {
        public string UserKey { get; set; }
        public AppUserPermission()
        {
            RoleCodes = new List<string>();
            PermissionList = new List<string>();
        }
        public List<string> RoleCodes { get; set; }
        public List<string> PermissionList { get; set; }
    }
}
