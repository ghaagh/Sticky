using System;

namespace Sticky.Domain.StatAggrigate
{
    public abstract class StatisticEntity : BaseEntity
    {
        public StatisticEntity() { }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public long Count { get; set; }
    }
}
