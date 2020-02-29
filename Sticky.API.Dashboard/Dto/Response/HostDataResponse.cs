using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class HostDataResponse : EmptyResponse
    {
        public HostDataResult Result { get; set; } = new HostDataResult();
    }
}
