using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Models.Mongo
{
    public class HostProduct
    {
        public virtual string Id { get; set; }
        public virtual string ProductName { get; set; }
        public virtual int? Price { get; set; }
        public virtual string ImageAddress { get; set; }
        public virtual DateTime UpdateDate { get; set; }
        public virtual bool IsAvailable { get; set; }
        public virtual string CategoryName { get; set; }
        public virtual string Url { get; set; }
        public virtual string Description { get; set; }
    }
}
