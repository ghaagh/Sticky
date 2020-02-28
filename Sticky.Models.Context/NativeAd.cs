using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Models.Context
{
    public class NativeDetails
    {
        public string AdId { get; set; }
        public string ProductId { get; set; }
        public string Image { get; set; }
        public string UrlAddress { get; set; }
        public int? Price { get; set; }
        public int? OldPrice { get; set; }
        public string ProductName { get; set; }
        public string OriginalProductName { get; set; }

    }

}
