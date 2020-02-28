using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class Clicks
    {
        public long Id { get; set; }
        public int SegmentId { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public long Count { get; set; }

    }
}
