using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class SegmentFilter
    {

        public int Id { get; set; }
        public string Value { get; set; }
        public int TypeId { get; set; }
        public bool IsNegative { get; set; }

    }
}
