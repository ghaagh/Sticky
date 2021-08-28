using Sticky.Domain.HostAggrigate;

namespace Sticky.Domain.CategoryAggrigate
{
    public class Category : BaseEntity
    {
        private Category()
        {

        }
        public Category(string name)
        {
            Name = name;
            Counter = 0;
        }
        public long HostId { get; set; }
        public virtual Host Host { get; set; }
        public string Name { get;private set; }
        public long Counter { get; set; }

    }
}
