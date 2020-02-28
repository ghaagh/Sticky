using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class CreatePatternRequest
    {
        [Required]
        public int HostId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Pattern { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
