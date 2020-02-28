using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class RecordedCategories
    {
        public RecordedCategories()
        {
            CategoryStats = new HashSet<CategoryStats>();
        }

        public int Id { get; set; }
        public int HostId { get; set; }
        public string CategoryName { get; set; }
        public long Counter { get; set; }

        public virtual Hosts Host { get; set; }
        public virtual ICollection<CategoryStats> CategoryStats { get; set; }
    }
}
