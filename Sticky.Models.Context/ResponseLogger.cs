using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class ResponseLogger
    {
        public long Id { get; set; }
        public int PartnerId { get; set; }
        public DateTime Date { get; set; }
        public int Counter { get; set; }
    }
}
