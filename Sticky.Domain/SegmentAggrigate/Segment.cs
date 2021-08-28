using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Domain.SegmentAggrigate
{
    public class Segment : BaseEntity, IAggrigateRoot
    {
        public Segment(string name, ActivityTypeEnum activity, ActionTypeEnum action,string activityExtra="", string actionExtra="")
        {
            Name = name;
            Activity = activity;
            Action = action;
            ActivityExtra = activityExtra;
            ActionExtra = actionExtra;
        }
        public string Name { get;private set; }
        public ActivityTypeEnum Activity { get; private set; }
        public ActionTypeEnum Action { get; private set; }
        public long HostId { get; set; }
        public virtual HostAggrigate.Host Host { get; private set; }
        public string ActionExtra { get; private set; }
        public string ActivityExtra { get; private set; }
        public bool Paused { get; private set; }
        public bool IsPublic { get; private set; }
        public long? TotalAudienceCount { get; private set; }
        private List<ProductTextTemplate> _productTextTemplates;
        public IReadOnlyCollection<ProductTextTemplate> ProductTextTemplates() => _productTextTemplates;

        public void ToogleStart() => Paused = !Paused;

        public void TogglePublic() => IsPublic = !IsPublic;

        public void AddTemplate(string template, int? minPrice = null, int? maxPrice = null)
        {
            var templateOjb = new ProductTextTemplate(template, minPrice, maxPrice);
            _productTextTemplates.Add(templateOjb);

        }
    }
}
