using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Dtos.UserIdentity
{
    public class LoginReaderInfoDto
    {
        public string UserKey { get; set; }
        public string user_key { get; set; }
        public string reader_name { get; set; }
        public string login_name { get; set; }
        public string reader_id { get; set; }
        public string reader_number { get; set; }
        public string reader_type { get; set; }
        public string reader_title { get; set; }
        public string department_name { get; set; }
        public string specialty_name { get; set; }
        public string reader_gender { get; set; }
        public string reader_phone { get; set; }
        public string reader_mail { get; set; }
        public string reader_address { get; set; }
        public string edu_background { get; set; }
        public short is_active { get; set; }
        public int status { get; set; }
        public DateTime create_time { get; set; }
        public DateTime stop_time { get; set; }
        public DateTime update { get; set; }
        /// <summary>
        /// 此字段是为了维普中刊对接cas所加
        /// </summary>
        public string username { get; set; }
        public string Nickname { get; set; }
        public string GradeName { get; set; }
        public int Backreadertype { get; set; }
        public string reader_iccode { get; set; }
        public string access_token { get; set; }
    }
}
