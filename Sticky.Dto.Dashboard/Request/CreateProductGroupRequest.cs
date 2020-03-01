using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class CreateProductGroupRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int HostId { get; set; }
        public List<ProductGroupItem> Filters { get; set; } = new List<ProductGroupItem>();
    }
}
