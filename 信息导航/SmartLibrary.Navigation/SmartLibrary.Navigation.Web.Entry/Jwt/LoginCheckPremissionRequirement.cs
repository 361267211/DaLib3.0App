using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Web.Jwt
{
    public class LoginCheckPremissionRequirement : IAuthorizationRequirement
    {
        public LoginCheckPremissionRequirement(int age)
        {
            MinimumAge = age;
        }

        protected int MinimumAge { get; set; }
    }


}
