using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Web.Jwt
{
    public class PortalColumnPremissionRequirement: IAuthorizationRequirement
    {
        public PortalColumnPremissionRequirement(int age)
        {
            MinimumAge = age;
        }

        protected int MinimumAge { get; set; }

    }
}
