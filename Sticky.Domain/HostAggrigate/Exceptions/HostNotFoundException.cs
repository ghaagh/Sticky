using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Domain.HostAggrigate.Exceptions
{
    public class HostNotFoundException : Exception
    {
        public HostNotFoundException() : base("Host not found")
        {

        }
    }
}
