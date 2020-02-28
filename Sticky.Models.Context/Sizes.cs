using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class Sizes
    {
        public Sizes()
        {
        }
        public int Id { get; set; }
        public string AdSize { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

    }
}
