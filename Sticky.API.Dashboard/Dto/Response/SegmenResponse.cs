using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class SegmentResponse : EmptyResponse
    {
        public List<SegmentResult> Result { get; set; } = new List<SegmentResult>();
    }
}
