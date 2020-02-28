using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class Partners
    {
        public Partners()
        {
            PartnerRequestLogs = new HashSet<PartnerRequestLogs>();
        }

        public int Id { get; set; }
        public string PartnerName { get; set; }
        public string ParnerHash { get; set; }
        public string Domain { get; set; }
        public string CookieSyncAddress { get; set; }
        public bool? Verified { get; set; }

        public virtual ICollection<PartnerRequestLogs> PartnerRequestLogs { get; set; }
    }
}
