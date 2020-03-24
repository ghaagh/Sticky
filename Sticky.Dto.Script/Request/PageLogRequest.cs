using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Script.Request
{
    public class PageLogRequest
    {
        public int HostId { get; set; }
        public string Address { get; set; }
        public long UserId { get; set; }
    }
}
