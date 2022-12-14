using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Web.Jwt
{
    public class StaffPremissionRequirement : IAuthorizationRequirement
    {
        public StaffPremissionRequirement(int age)
        {
            MinimumAge = age;
        }

        protected int MinimumAge { get; set; }
    }
}
