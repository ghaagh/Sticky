using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class SegmentTextTemplates
    {
        public long Id { get; set; }
        public int SegmentId { get; set; }
        public string Template { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }

    }
}
