using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class Segments
    {
        public Segments()
        {
            ProductTextTemplates = new HashSet<ProductTextTemplates>();
        }

        public int Id { get; set; }
        public int HostId { get; set; }
        public string SegmentName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatorId { get; set; }
        public int AudienceId { get; set; }
        public int ActionId { get; set; }
        public string AudienceExtra { get; set; }
        public string ActionExtra { get; set; }
        public bool Paused { get; set; }
        public bool? IsPublic { get; set; }
        public long? AudienceNumber { get; set; }
        public virtual ActionTypes Action { get; set; }
        public virtual AudienceTypes Audience { get; set; }
        public virtual Hosts Host { get; set; }
        public virtual AspNetUsers Creator { get; set; }
        public virtual ICollection<ProductTextTemplates> ProductTextTemplates { get; set; }
    }
}
