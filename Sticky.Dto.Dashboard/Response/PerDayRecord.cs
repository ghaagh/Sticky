using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class PerDayRecord
    {
        public List<long> Values { get; set; }
        public List<string> Labels { get; set; }

        public PerDayRecord()
        {
            Values = new List<long>();
            Labels = new List<string>();
        }

    }
}
