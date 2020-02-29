using Sticky.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.API.Advertisement.Dto
{
    public class AdvertisementResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<MemberShipResult> Result { get; set; }
    }
    public class MemberShipResult
    {
        public MemberShipResult()
        {
        }
        public long SegmentId { get; set; }
        public int HostId { get; set; }
        public int Priority { get; set; }
        public List<NativeDetails> Products { get; set; } = new List<NativeDetails>();
    }
}
