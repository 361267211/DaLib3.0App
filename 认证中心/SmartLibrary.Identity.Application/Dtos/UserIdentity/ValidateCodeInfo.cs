using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Dtos.UserIdentity
{
    public class ValidateCodeInfoDto
    {
        public string ValidateKey { get; set; }
        public string ImgFile { get; set; }
    }

    public class PhoneVerifyDto
    {
        public string Phone { get; set; }
        public string ValidateKey { get; set; }
        public string ValidateCode { get; set; }
    }

    public class PhoneVerifyForgetDto
    {
        public string Phone { get; set; }
        public string OperateKey { get; set; }

    }
}
