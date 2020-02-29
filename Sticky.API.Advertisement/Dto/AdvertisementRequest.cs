using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.API.Advertisement.Dto
{
    public class AdvertisementRequest
    {
        public long? StickyUserId { get; set; }
        public string PartnerUserId { get; set; }
        public string Size { get; set; }
        public string PartnerHashCode { get; set; }
        public int Type { get; set; } = 1;
    }
}
