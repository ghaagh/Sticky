using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Script.Request
{
    public class AddToCartLog
    {

        public long UserId { get; set; }
        public int HostId { get; set; }
        public List<ProductEdit> ProductData { get; set; } = new List<ProductEdit>();
        public string PageAddress { get; set; }

    }
}
