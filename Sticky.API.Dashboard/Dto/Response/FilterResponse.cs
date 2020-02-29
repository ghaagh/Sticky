using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class FilterResponse : EmptyResponse
    {
        public List<FilterResult> Result { get; set; } = new List<FilterResult>();
    }
}
