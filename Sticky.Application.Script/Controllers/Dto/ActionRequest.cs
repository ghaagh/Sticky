using Sticky.Domain.SegmentAggrigate;
using System.Collections.Generic;

namespace Sticky.Application.Script.Controllers.Dto
{
    public class ActionRequest
    {


        public long UserId { get; set; }
        public List<string> ProductId { get; set; }
        public string PageAddress { get; set; }
        public ActivityTypeEnum StatType { get; set; }


    }
}
