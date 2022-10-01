using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.ViewModels
{
    public class TokenResultOutput
    {
        public string StatusCode { get; set; }
        public TokenInfoOutput Data { get; set; }
        public string Token_Type { get; set; }
    }
    public class TokenInfoOutput
    {
        public string Token { get; set; }
        public DateTime TokenExpiredAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireAt { get; set; }
    }
}
