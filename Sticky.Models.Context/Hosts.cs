using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class Hosts
    {
        public Hosts()
        {
            CreatedSegments = new HashSet<CreatedSegments>();
            DruidSegmentOwnership = new HashSet<SegmentOwnership>();
            DynamicAdHtml = new HashSet<DynamicAdHtml>();
            RecordedCategories = new HashSet<RecordedCategories>();
            SegmentPagePattern = new HashSet<SegmentPagePattern>();
            UsersHostAccess = new HashSet<UsersHostAccess>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Host { get; set; }
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

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<SegmentOwnership> DruidSegmentOwnership { get; set; }
        public virtual ICollection<DynamicAdHtml> DynamicAdHtml { get; set; }
        public virtual ICollection<RecordedCategories> RecordedCategories { get; set; }
        public virtual ICollection<SegmentPagePattern> SegmentPagePattern { get; set; }
        public virtual ICollection<UsersHostAccess> UsersHostAccess { get; set; }
    }
}
