using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Application.Script.Controllers.Dto
{
    public class ProductRequest
    {
        public long UserId { get; set; }
        public string PageAddress { get; set; }
        public List<ProductData> Products { get; set; }
    }
    public class ProductData
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool Available { get; set; }
        public bool Added { get; set; }
        public string PageAddress { get; set; }
        public string ImageAddress { get; set; }
    }


}
