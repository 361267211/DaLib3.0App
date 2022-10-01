using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Web.Jwt
{
    public class CustomPremissionRequirement : IAuthorizationRequirement
    {
        public CustomPremissionRequirement(int age)
        {
            MinimumAge = age;
        }

        protected int MinimumAge { get; set; }
    }


}
