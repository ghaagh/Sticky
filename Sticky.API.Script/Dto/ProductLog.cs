using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.API.Script.Dto
{
    public class ProductLog
    {

        public long UserId { get; set; }
        public int HostId { get; set; }
        public List<ProductData> ProductData { get; set; } = new List<ProductData>();
        public string PageAddress { get; set; }

    }
}
