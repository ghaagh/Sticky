using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Request
{
   public class UpdateTextTemplateRequest
    {
        public int SegmentId { get; set; }
        public List<string> Templates { get; set; } = new List<string>();
    }
    public class UpdateTextTemplateRequestV2
    {
        public int SegmentId { get; set; }
        public List<TemplateItem> Templates { get; set; } = new List<TemplateItem>();
    }
    public class TemplateItem
    {
        public string Body { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}
