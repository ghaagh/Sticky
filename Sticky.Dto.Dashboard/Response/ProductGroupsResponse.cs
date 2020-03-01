using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class ProductGroupsResponse : EmptyResponse
    {
        public List<ProductGorupResult> Result { get; set; } = new List<ProductGorupResult>();
    }
}
