using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class RecordedCategory
    {
        public RecordedCategory()
        {
            CategoryStats = new HashSet<CategoryStat>();
        }

        public int Id { get; set; }
        public int HostId { get; set; }
        public string CategoryName { get; set; }
        public long Counter { get; set; }

        public virtual Host Host { get; set; }
        public virtual ICollection<CategoryStat> CategoryStats { get; set; }
    }
}
