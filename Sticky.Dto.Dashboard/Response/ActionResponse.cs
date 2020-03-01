using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class ActionResponse : EmptyResponse
    {
        public List<ActionResult> Result { get; set; } = new List<ActionResult>();
    }
    public class ActionResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AudienceTypeResponse : EmptyResponse
    {
        public List<AudienceTypeResult> Result { get; set; } = new List<AudienceTypeResult>();
    }
    public class AudienceTypeResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
