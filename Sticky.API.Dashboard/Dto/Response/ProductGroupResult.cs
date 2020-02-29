using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class ProductGorupResult
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public string Name { get; set; }
        public List<ProductGroupDetails> Filters { get; set; } = new List<ProductGroupDetails>();
        public string Owner { get; set; }
    }
}
