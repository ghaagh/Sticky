using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class Host
    {
        public Host()
        {
            RecordedCategories = new HashSet<RecordedCategory>();
            SegmentPagePattern = new HashSet<SegmentPagePattern>();
            UsersHostAccess = new HashSet<UsersHostAccess>();
            Segments = new HashSet<Segment>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string HostAddress { get; set; }
        public string HashCode { get; set; }
        public bool HostValidated { get; set; }
        public bool PageValidated { get; set; }
        public bool ProductValidated { get; set; }
        public bool CategoryValidated { get; set; }
        public bool AddToCardValidated { get; set; }
        public bool FinalizeValidated { get; set; }
        public string ValidatingHtmlAddress { get; set; }
        public bool? FavValidated { get; set; }
        public string AddToCardId { get; set; }
        public string FinalizePage { get; set; }
        public int? UserValidityId { get; set; }
        public int? ProductValidityId { get; set; }
        public bool? HostPriority { get; set; }
        public string LogoAddress { get; set; }
        public string LogoOtherData { get; set; }
        public int? ProductImageWidth { get; set; }
        public int? ProductImageHeight { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<RecordedCategory> RecordedCategories { get; set; }
        public virtual ICollection<Segment> Segments { get; set; }
        public virtual ICollection<SegmentPagePattern> SegmentPagePattern { get; set; }
        public virtual ICollection<UsersHostAccess> UsersHostAccess { get; set; }
    }
}
