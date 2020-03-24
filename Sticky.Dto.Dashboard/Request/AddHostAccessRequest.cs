using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class AdHostAccessRequest
    {
        [Required]
        public string TargetEmail { get; set; }
        [Required]
        public int? HostId { get; set; }
    }
}
