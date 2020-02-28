using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class UserCountPerDayAggrigated
    {
        public long UserCount { get; set; }
        public DateTime Date { get; set; }
    }
}
