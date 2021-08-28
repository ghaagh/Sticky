using Sticky.Domain.SegmentAggrigate;

namespace Sticky.Domain.SegmentAggrigate
{
    public class ProductTextTemplate : BaseEntity
    {
        private ProductTextTemplate() { }
        public ProductTextTemplate(string template, int? minPrice = null, int? maxPrice = null)
        {
            Template = template;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }

        public Segment Segment { get; private set; }
        public string Template { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }


    }
}
