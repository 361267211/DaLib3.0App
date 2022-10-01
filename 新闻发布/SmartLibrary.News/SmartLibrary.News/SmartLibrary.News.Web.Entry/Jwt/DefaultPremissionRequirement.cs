using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.News.Web.Jwt
{
    public class DefaultPremissionRequirement : IAuthorizationRequirement
    {
        public DefaultPremissionRequirement(int age)
        {
            MinimumAge = age;
        }

        protected int MinimumAge { get; set; }
    }
}
