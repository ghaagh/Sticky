using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class FiltersForSegments
    {
        public int Id { get; set; }
        public int SegmentId { get; set; }
        public int FilterId { get; set; }
        public string FilterValue { get; set; }
        public bool IsNegative { get; set; }

        public virtual Filters Filter { get; set; }
    }
}
