using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class UserTotalVisit
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public DateTime LogDate { get; set; }
        public int Count { get; set; }
    }
}
