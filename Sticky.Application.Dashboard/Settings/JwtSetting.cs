using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Settings
{
    public class JwtSetting
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public int ExpirationInMinutes { get; set; }
    }
}
