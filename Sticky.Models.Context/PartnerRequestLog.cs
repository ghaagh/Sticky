using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class PartnerRequestLog
    {
        public long Id { get; set; }
        public DateTime LogDate { get; set; }
        public int PartnerId { get; set; }
        public int TotalRequestsCounter { get; set; }
        public int TotalResponse { get; set; }
        public double DayCost { get; set; }

        public virtual Partner Partner { get; set; }
    }
}
