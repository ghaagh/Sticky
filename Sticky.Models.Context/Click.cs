using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class Click
    {
        public Click()
        {
            Segment = new Segment();
        }
        public long Id { get; set; }
        public int SegmentId { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public long Count { get; set; }

        public virtual Segment Segment { get; set; } 
    }
}
