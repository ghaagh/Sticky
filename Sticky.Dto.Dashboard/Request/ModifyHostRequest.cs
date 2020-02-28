using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
    public class ModifyHostRequest
    {
        public int ProductValidityId { get; set; }
        public int UserValidityId { get; set; }
        public string LogoAddress { get; set; }
        public string LogoOtherData { get; set; }
        public string FinalizeAddress { get; set; }
        public string AdToCartElementId { get; set; }
    }
}
