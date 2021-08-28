using Sticky.Domain.CategoryAggrigate;

namespace Sticky.Domain.StatAggrigate
{
    public class CategoryStatistic: StatisticEntity
    {
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
