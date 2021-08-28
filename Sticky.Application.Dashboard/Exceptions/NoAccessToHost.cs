using System;

namespace Sticky.Application.Dashboard.Exceptions
{
    public class NoAccessToHostException : Exception
    {
        public NoAccessToHostException() : base("User has no acess to specified host")
        {

        }
    }
}
