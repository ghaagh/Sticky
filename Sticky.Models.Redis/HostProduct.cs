using System;
using System.Collections.Generic;
using System.Text;
using ZeroFormatter;

namespace Sticky.Models.Redis
{
    [ZeroFormattable]
    public class HostProduct
    {
        [Index(0)]
        public virtual string Id { get; set; }
        [Index(1)]
        public virtual string ProductName { get; set; }
        [Index(2)]
        public virtual int? Price { get; set; }
        [Index(3)]
        public virtual string ImageAddress { get; set; }
        [Index(4)]
        public virtual DateTime UpdateDate { get; set; }
        [Index(5)]
        public virtual bool IsAvailable { get; set; }
        [Index(6)]
        public virtual string CategoryName { get; set; }
        [Index(7)]
        public virtual string Url { get; set; }
        [Index(8)]
        public virtual string Description { get; set; }
    }
}
