using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class HostResult
    {
        public int Id { get; set; }
        public bool AlreadyExists { get; set; }
        public string TrackerAddress { get; set; }
        public string HostAddress { get; set; }
        public int? ProductValidityType { get; set; }
        public int? UserValidityType { get; set; }
        public string Owner { get; set; }
        public string LogoAddress { get; set; }
        public string LogoOtherData { get; set; }
        public string FinalizePage { get; set; }
        public string AdToCartId { get; set; }
        public SegmentCreationAccess SegmentCreationAccess { get; set; } = new SegmentCreationAccess();

    }
    public class SegmentCreationAccess
    {
        public bool Page { get; set; }
        public bool ProductVisit { get; set; }
        public bool AddToCart { get; set; }
        public bool Buy { get; set; }
        public bool Fav { get; set; }
    }
}
