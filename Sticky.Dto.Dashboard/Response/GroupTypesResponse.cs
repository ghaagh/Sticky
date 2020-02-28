using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class GroupTypesResponse : EmptyResponse
    {
        public List<GroupTypesResult> Result { get; set; } = new List<GroupTypesResult>();
    }
}
