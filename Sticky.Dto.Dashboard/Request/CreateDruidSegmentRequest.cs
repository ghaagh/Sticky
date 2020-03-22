using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class CreateSegmentRequest
    {

        public string SegmentName { get; set; }
        public int ActionId { get; set; }
        public int AudienceId { get; set; }
        public string ActionExtra { get; set; }
        public string AudienceExtra { get; set; }
        [Required]
        public int HostId { get; set; }
    }
}
