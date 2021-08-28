using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Domain.HostAggrigate.Exceptions
{
    public class UserAccessNotFoundException : Exception
    {
        public UserAccessNotFoundException() : base("User has no access to this host")
        {

        }
    }
}
