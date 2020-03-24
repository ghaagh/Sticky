using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Advertisement.Response
{
    public class MembershipResponse : GeneralResponse
    {
        public long UserId { get; set; }
        public List<MemberShipResult> Result { get; set; } = new List<MemberShipResult>();
    }
}
