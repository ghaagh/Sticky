using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.API.Script.Dto
{
    public abstract class ProductIdLog
    {
        public ProductIdLog()
        {
            ProductData = new List<ProductIdData>();
        }

        public long UserId { get; set; }
        public int HostId { get; set; }
        public List<ProductIdData> ProductData { get; set; }
        public string PageAddress { get; set; }


    }
}
