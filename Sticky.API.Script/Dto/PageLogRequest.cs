using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.API.Script.Dto
{
    public class PageLogRequest
    {
        public int HostId { get; set; }
        public string Address { get; set; }
        public long UserId { get; set; }
    }
}
