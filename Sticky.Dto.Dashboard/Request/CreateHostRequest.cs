using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class CreateHostRequest
    {

        [Required]
        public string HostAddress { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
