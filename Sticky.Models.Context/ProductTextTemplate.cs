using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class ProductTextTemplate
    {
        public ProductTextTemplate()
        {
            Segment =new  Segment();
        }
        public int Id { get; set; }
        public int SegmentId { get; set; }
        public string Template { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }

        public virtual Segment Segment { get; set; }
    }
}
