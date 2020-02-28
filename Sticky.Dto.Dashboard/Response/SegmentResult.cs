using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class SegmentResult
    {
        public long Id { get; set; }
        public int ActionId { get; set; }
        public int AudienceId { get; set; }
        public string AudieceExtra { get; set; }
        public string ActionExtra { get; set; }
        public string Name { get; set; }
        public HostData Host { get; set; } = new HostData();
        public string Owner { get; set; }
        public bool IsPublic { get; set; }
        public bool IsPaused { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UserCount { get; set; }

    }
    
    public class HostData
    {
        public int Id { get; set; }
        public string Address { get; set; }
    }
}
