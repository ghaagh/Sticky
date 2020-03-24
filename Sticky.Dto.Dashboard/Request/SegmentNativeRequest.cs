using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class SegmentNativeRequest
    {
        public string NativeText { get; set; }
        public string NativeLogoAddress { get; set; }
        public string NativeLogoOtherData { get; set; }
        public int SegmentId { get; set; }

    }
}
