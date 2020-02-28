using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class ActionType
    {
        public ActionType()
        {
            Segments = new HashSet<Segment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Segment> Segments { get; set; }
    }
}
