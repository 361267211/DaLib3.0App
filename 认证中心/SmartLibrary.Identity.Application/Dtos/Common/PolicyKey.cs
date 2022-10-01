using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Dtos.Common
{
    /// <summary>
    /// 缓存key
    /// </summary>
    public class PolicyKey
    {
        public const string TokenAuth = "TokenAuth";
        public const string StaffAuth = "StaffAuth";
        public const string ReaderAuth = "ReaderAuth";
        public const string UnAuthKey = "UnAuth";
    }
}
