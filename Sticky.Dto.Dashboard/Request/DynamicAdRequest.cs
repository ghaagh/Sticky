using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class DynamicAdRequest
    {
        [Required]
        public string Email { get; set; }
        public int HostId { get; set; }
        public List<DynamicAdItems> HtmlBody { get; set; } = new List<DynamicAdItems>();
    }
}
