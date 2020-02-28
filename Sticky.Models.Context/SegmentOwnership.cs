using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class SegmentOwnership
    {
        public int Id { get; set; }
        public int SegmentId { get; set; }
        public string UserId { get; set; }
        public int HostId { get; set; }
        public string SubFilter { get; set; }

        public virtual Hosts Host { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
