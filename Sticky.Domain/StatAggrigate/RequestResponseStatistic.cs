using Sticky.Domain.PartnerAggrigate;

namespace Sticky.Domain.StatAggrigate
{
    public partial class RequestResponseStatistic : StatisticEntity
    {

        public int PartnerId { get; set; }
        public int Response { get; set; }
        public double DayCost { get; set; }
        public virtual Partner Partner { get; set; }
    }
}
