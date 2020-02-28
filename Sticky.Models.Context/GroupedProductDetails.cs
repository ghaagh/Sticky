using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class GroupedProductDetails
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public int? GroupTypeId { get; set; }
        public string Value { get; set; }

        public virtual ProductGroupings Group { get; set; }
        public virtual ProductGroupingTypes GroupType { get; set; }
    }
}
