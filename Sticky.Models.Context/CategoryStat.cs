using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class CategoryStat
    {
        public long Id { get; set; }
        public int CategoryId { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Counter { get; set; }

        public virtual RecordedCategory Category { get; set; }
    }
}
