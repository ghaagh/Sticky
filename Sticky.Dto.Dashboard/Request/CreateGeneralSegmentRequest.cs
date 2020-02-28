using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class CreateGeneralSegmentRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public int HostId { get; set; }
    }
}
