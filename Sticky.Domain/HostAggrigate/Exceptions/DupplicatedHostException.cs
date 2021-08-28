using System;

namespace Sticky.Domain.HostAggrigate.Exceptions
{
    public class DuplicatedHostException : Exception
    {
        public DuplicatedHostException() : base("Host already exists")
        {

        }
    }
}
