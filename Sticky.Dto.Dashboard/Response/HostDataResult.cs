using DataAccess.Mongo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class HostDataResult
    {

        public List<KeyValuePair<string, bool>> Features { get; set; } = new List<KeyValuePair<string, bool>>();
        public int OveralComplition { get; set; }
        public HostProducts LastProduct { get; set; } = new HostProducts();
        public PerDayRecord VisitedPagesPerDay { get; set; } = new PerDayRecord();
        public PerDayRecord Top20Categories { get; set; } = new PerDayRecord();

    }
}
