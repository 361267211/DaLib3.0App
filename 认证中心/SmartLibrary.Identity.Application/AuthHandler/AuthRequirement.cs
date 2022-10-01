using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.AuthHandler
{
    public class TokenAuthRequirement : IAuthorizationRequirement
    {
    }
    public class StaffAuthRequirement : IAuthorizationRequirement
    {
    }
    public class ReaderAuthRequirement : IAuthorizationRequirement
    {
    }
}
