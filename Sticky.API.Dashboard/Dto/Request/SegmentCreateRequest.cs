using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class SegmentCreateRequest
    {
        public List<SegmentTag> Tags { get; set; } = new List<SegmentTag>();
        public string Name { get; set; }
        public bool? DiscountIsPercent { get; set; }
        public int DiscountValue { get; set; }
        public string ExtraQuery { get; set; }
        public List<int> RemovedSegments { get; set; }
        public int ActionId { get; set; }
        public string ResultValue { get; set; }
        public string NativeTitle { get; set; }
        public string NativeDescription { get; set; }
        public string NativeLogoAddress { get; set; }
        public string NativeLogoOtherData { get; set; }
        public bool IsPublic { get; set; }
        public List<SegmentFilter> Filters { get; set; } = new List<SegmentFilter>();
        [Required]
        public string Email { get; set; }
        [Required]
        public int HostId { get; set; }
    }
}
