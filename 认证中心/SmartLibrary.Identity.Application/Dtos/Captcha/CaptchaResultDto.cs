using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Dtos.Captcha
{
    public class CaptchaResultDto
    {
        public string Code { get; set; }
        public string ImageBase64 { get; set; }
    }
}
