using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class AudienceTypes
    {
        public AudienceTypes()
        {
            Segments = new HashSet<Segments>();
        }

        public int Id { get; set; }
        public string AudienceTypeName { get; set; }

        public virtual ICollection<Segments> Segments { get; set; }
    }
}
