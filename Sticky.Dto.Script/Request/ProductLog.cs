using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Script.Request
{
    public class ProductLog
    {

        public long UserId { get; set; }
        public int HostId { get; set; }
        public List<ProductData> ProductData { get; set; } = new List<ProductData>();
        public string PageAddress { get; set; }

    }
}
