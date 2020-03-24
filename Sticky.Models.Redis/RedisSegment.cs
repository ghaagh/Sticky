using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Models.Redis
{
    public class RedisSegment
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public int AudienceId { get; set; }
        public int ActionId { get; set; }
        public string AudienceExtra { get; set; }
        public string ActionExtra { get; set; }
    }
}
