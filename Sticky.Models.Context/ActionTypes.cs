using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class ActionTypes
    {
        public ActionTypes()
        {
            Segments = new HashSet<Segments>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Segments> Segments { get; set; }
    }
}
