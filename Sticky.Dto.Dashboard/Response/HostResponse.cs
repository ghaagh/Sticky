using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class HostReponse : EmptyResponse
    {
        public IEnumerable<HostResult> Result { get; set; }
    }

}
