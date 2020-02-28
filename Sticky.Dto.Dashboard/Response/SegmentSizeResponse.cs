using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class SegmentSizeResponse : EmptyResponse
    {
        public List<SegmentTagsResult> Result { get; set; } = new List<SegmentTagsResult>();
    }
}
