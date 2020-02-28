using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class AudienceType
    {
        public AudienceType()
        {
            Segments = new HashSet<Segment>();
        }

        public int Id { get; set; }
        public string AudienceTypeName { get; set; }

        public virtual ICollection<Segment> Segments { get; set; }
    }
}
