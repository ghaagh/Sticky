using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class SegmentStaticNatives
    {
        public int Id { get; set; }
        public int SegmentId { get; set; }
        public string NativeText { get; set; }
        public string NativeLogoAddress { get; set; }
        public string NativeLogoOtherData { get; set; }
    }
}
