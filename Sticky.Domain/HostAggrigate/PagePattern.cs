using System;
using System.Collections.Generic;

namespace Sticky.Domain.SegmentAggrigate
{
    public class PagePattern: BaseEntity
    {
        public PagePattern(string name, string pattern)
        {
            Name = name;
            Pattern = pattern;
        }
        public string Name { get; private set; }
        public string Pattern { get; private set; }
    }
}
