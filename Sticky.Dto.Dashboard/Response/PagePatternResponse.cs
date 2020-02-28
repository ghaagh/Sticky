using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class PagePatternsReponse : EmptyResponse
    {
        public List<PagePatternResult> Result { get; set; } = new List<PagePatternResult>();
    }
}
