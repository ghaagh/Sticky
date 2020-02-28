using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class PagePatternResult
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Pattern { get; set; }
        public HostData Host { get; set; }
        public string Owner { get; set; }
    }
}
