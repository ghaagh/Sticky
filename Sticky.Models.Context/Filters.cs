using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class Filters
    {
        public Filters()
        {
            FiltersForSegments = new HashSet<FiltersForSegments>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleFa { get; set; }
        public int? ResultTypePower { get; set; }

        public virtual ICollection<FiltersForSegments> FiltersForSegments { get; set; }
    }
}
