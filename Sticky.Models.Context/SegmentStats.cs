using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class SegmentStats
    {
        public int Id { get; set; }
        public int SegmentId { get; set; }
        public int? TextTemplateId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
    }
}
