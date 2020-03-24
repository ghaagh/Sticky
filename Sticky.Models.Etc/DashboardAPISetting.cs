using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Models.Etc
{
    public class DashboardAPISetting
    {
        public string JWTSecret { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireDays { get; set; }
        public string ConnectionString { get; set; }
        public string ScriptLocation { get; set; }
        public int UserDataExpirationCache { get; set; }
        public string ScriptAPIUrlBase { get; set; }
    }
}
